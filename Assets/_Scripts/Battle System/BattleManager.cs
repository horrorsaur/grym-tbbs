using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// best post I could find that explains new unity action input
/// https://gamedevbeginner.com/input-in-unity-made-easy-complete-guide-to-the-new-system/#input_system
/// 
/// 
/// @TODO: Don't forget to turn off old input system once the new one is implemented
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public BattleState battleState { get; private set; }
    public BattleState previousBattleState { get; private set; }
    public SelectedBattleOption selectedBattleOption { get; private set; }
    public PartyMember activeCharacterTurn { get; private set; }
    public PartyMember selectedEnemy { get; private set; }
    public List<PartyMember> battleParticipants { get; private set; } = new List<PartyMember>();

    private Queue<PartyMember> turnQueue = new Queue<PartyMember>();
    private bool generateTurnQueue = true;
    private PartyMember latestDefeatedEnemy;

    [SerializeField] private PlayerPartyManager playerPartyManager;
    [SerializeField] private EnemyPartyManager enemyPartyManager;

    public Transform pfDamagePopup;


    #region Broadcasting Events
    public static event Action<PartyMember> OnActiveCharacterChange;
    public static event Action<List<PartyMember>> OnLoadPlayerList;
    public static event Action<List<PartyMember>> OnLoadEnemyList;
    public static event Action<string> OnPlayerTurn;
    public static event Action<string> OnEnemyTurn;
    public static event Action<BattleState> OnBattleStateUpdate;
    public static event Action<string, int, string> OnDamageCharacter;
    #endregion

    #region OnEnable & OnDisable
    private void OnEnable()
    {

        BattleHUD.OnReceiveAttack += AttackAction;
        BattleHUD.OnReceiveAbility += AbilityAction;
        BattleHUD.OnReceiveDefend += DefendAction;
        BattleHUD.OnReceiveItem += ItemAction;
        BattleHUD.OnBackButtonPress += BackAction;
        BattleHUD.OnListAbilities += ListAbilities;

        BattleHUD.OnSelectEnemy += SetSelectedEnemy;

        CharacterBase.OnCharacterDeath += RemoveFromBattle;
    }

    private void OnDisable()
    {
        BattleHUD.OnReceiveAttack -= AttackAction;
        BattleHUD.OnReceiveAbility -= AbilityAction;
        BattleHUD.OnReceiveDefend -= DefendAction;
        BattleHUD.OnReceiveItem -= ItemAction;
        BattleHUD.OnBackButtonPress -= BackAction;
        BattleHUD.OnListAbilities -= ListAbilities;

        BattleHUD.OnSelectEnemy -= SetSelectedEnemy;

        CharacterBase.OnCharacterDeath -= RemoveFromBattle;
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetTurnQueue();
        ResetBattleParticipants();

        ParseBattleParticipants();
        CreateTurnQueue();

        NextParticipant();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DamagePopup.Create(Vector3.zero, 300);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            NextParticipant();
        }

        if (generateTurnQueue && turnQueue.Count <= 0)
        {
            CreateTurnQueue();
        }

        switch (battleState)
        {
            case BattleState.ENEMY_TURN:
                EnemyTurnSetup();
                break;
            case BattleState.WON:
                break;
        }
    }
    #endregion

    #region TEMP - Enemy Turn
    private void EnemyTurnSetup()
    {
        battleState = BattleState.ENEMY_TURN_IN_PROGRESS;
        int battleChoiceIndex = UnityEngine.Random.Range(0, activeCharacterTurn.battleChoices.Count);
        int playerIndex = UnityEngine.Random.Range(0, playerPartyManager.GetCurrentParty().Count);
        string action = activeCharacterTurn.battleChoices[battleChoiceIndex];

        List<PartyMember> availablePlayerTargets = new List<PartyMember>();
        if (battleParticipants.Count != 0)
        {
            for (int i = 0; i < battleParticipants.Count; i++)
            {
                if (battleParticipants[i].type == CharacterType.PLAYER)
                {
                    availablePlayerTargets.Add(battleParticipants[i]);
                }
            }

            PartyMember target = availablePlayerTargets[playerIndex];

            Debug.Log($"{activeCharacterTurn.characterName} selected {target.characterName} as their target.");

            switch (action)
            {
                case "Attack":
                    EnemyAttack(target);
                    break;
            }
        }
    }

    private void EnemyAttack(PartyMember _target)
    {
        selectedEnemy = _target;

        OnDamageCharacter?.Invoke(selectedEnemy.ID, activeCharacterTurn.baseCharacter.CurrentStrength, activeCharacterTurn.characterName);
        NextParticipant();
    }
    #endregion

    #region State Methods
    private void UpdateBattleState(BattleState _state)
    {
        previousBattleState = battleState;
        battleState = _state;
        OnBattleStateUpdate?.Invoke(battleState);
    }

    private void UpdateBattleAction(SelectedBattleOption _state)
    {
        selectedBattleOption = _state;
    }
    #endregion

    /// <summary>
    /// Method for building out and sorting the battleParticipants by descending agility.
    /// </summary>
    private void ParseBattleParticipants()
    {
        foreach (var c in playerPartyManager.GetCurrentParty())
        {
            PartyMember m = new PartyMember(
                c,
                c.GetCharacterName(),
                c.GetCharacterType(),
                c.GetID(),
                c.GetCurrentAgility(),
                c.GetAbilities(),
                c.GetBattleChoices(),
                c.GetCharacterClass(),
                c.GetStatusEffect(),
                c.gameObject.transform
            );

            battleParticipants.Add(m);
        }

        foreach (var c in enemyPartyManager.GetCurrentParty())
        {
            PartyMember m = new PartyMember(
                c,
                c.GetCharacterName(),
                c.GetCharacterType(),
                c.GetID(),
                c.GetCurrentAgility(),
                c.GetAbilities(),
                c.GetBattleChoices(),
                c.GetCharacterClass(),
                c.GetStatusEffect(),
                c.gameObject.transform
            );

            battleParticipants.Add(m);
        }

        battleParticipants.Sort(new SortBattleParticipantsDescending());
        OnLoadPlayerList?.Invoke(battleParticipants);
    }

    private void RemoveFromBattle(GameObject _go)
    {
        Character c = _go.GetComponent<Character>();
        for (int i = 0; i < battleParticipants.Count; i++)
        {
            if (battleParticipants[i].ID == c.GetID())
            {
                if (battleParticipants[i].type == CharacterType.ENEMY)
                {
                    Debug.Log($"Removed {battleParticipants[i].characterName} from battle");
                    latestDefeatedEnemy = battleParticipants[i];
                    battleParticipants.RemoveAt(i);
                    RegenerateTurnQueue();
                }
            }
        }

        if (!battleParticipants.Any(x => x.type == CharacterType.ENEMY))
        {
            UpdateBattleState(BattleState.WON);
            generateTurnQueue = false;
            battleParticipants.Clear();
            Debug.Log($"All enemies have been defeated!");
        }
    }

    #region Turn Queue Logic
    /// <summary>
    /// Loop through battleParticipants to add PartyMember types to turnQueue
    /// </summary>
    private void CreateTurnQueue()
    {
        if (battleParticipants.Count <= 0)
        {
            Debug.LogWarning($"battleParticipants was 0. Calling ParseBattleParticipants()");
            ParseBattleParticipants();
        }

        foreach (var p in battleParticipants)
        {
            turnQueue.Enqueue(p);
        }
    }

    /// <summary>
    /// Dequeue's the turnQueue to retrieve the next participant in the collection
    /// </summary>
    private void NextParticipant()
    {
        activeCharacterTurn = turnQueue.Dequeue();

        if (activeCharacterTurn.type == CharacterType.PLAYER)
        {
            OnActiveCharacterChange?.Invoke(activeCharacterTurn);
            OnPlayerTurn?.Invoke(activeCharacterTurn.characterName);
            UpdateBattleState(BattleState.PLAYER_TURN);
        }
        else
        {
            OnEnemyTurn?.Invoke(activeCharacterTurn.characterName);
            UpdateBattleState(BattleState.ENEMY_TURN);
        }
    }

    private void RegenerateTurnQueue()
    {
        Debug.Log($"Regenerating turn queue...");
        turnQueue.Clear();

        CreateTurnQueue();
    }
    #endregion

    #region Reset Methods
    private void ResetTurnQueue() => turnQueue.Clear();
    private void ResetBattleParticipants() => battleParticipants.Clear();
    #endregion

    #region Enemy Selection

    public void SetSelectedEnemy(PartyMember _selection)
    {
        selectedEnemy = _selection;
        UpdateBattleState(BattleState.ENEMY_SELECTED);

        HandleAttackPhase();

        OnDamageCharacter?.Invoke(selectedEnemy.ID, activeCharacterTurn.baseCharacter.CurrentStrength, activeCharacterTurn.characterName);
        NextParticipant();
    }
    #endregion

    #region Physical Attack Phase
    /// <summary>
    /// First, we need to get the position of the currently selected target
    /// Then, we need to save our current position
    /// Then, we need to calculate the distance
    /// Then we need to navigate the distance
    /// after navigating to the target, it's time to play the associated attack animation
    /// this step will require some kind of delegate action event logic that can fire whenever an animation is done for e.g
    /// </summary>
    /// 

    private void HandleAttackPhase()
    {
        CalculateDistance();

    }

    private void CalculateDistance()
    {
        var previousPosition = activeCharacterTurn.characterPosition;
        float distanceToMove = Vector2.Distance(activeCharacterTurn.characterPosition.position, selectedEnemy.characterPosition.position);
        Vector3 m = new Vector3(distanceToMove, 0f, 0f);
        GameObject go;

        if (activeCharacterTurn.type == CharacterType.PLAYER)
            go = GameManager.instance.FindCharacterGameObject(activeCharacterTurn.characterName);
        else
            go = enemyPartyManager.FindCharacterGameObject(activeCharacterTurn.characterName);

        //go.transform.position += m;
        go.transform.position = selectedEnemy.characterPosition.position;
    }

    private void MoveCharacterToEnemy(Action _onMoveComplete)
    {

    }

    private void MoveCharacterToOriginalPosition(Action _onMoveComplete)
    {

    }

    private void PlayAnimation(Action _onAnimationComplete)
    {

    }




    #endregion

    #region Battle Actions
    public void AttackAction()
    {
        UpdateBattleAction(SelectedBattleOption.ATTACK);

        UpdateBattleState(BattleState.SELECT_ENEMY);
    }

    public void AbilityAction(Ability _ability)
    {
        Debug.Log($"{_ability.abilityName}");
        Debug.Log($"{_ability.abilityDescription}");
        Debug.Log($"{_ability.abilityType}");
        Debug.Log($"{activeCharacterTurn.characterName} used {_ability.abilityName}");
    }

    public void DefendAction()
    {
        UpdateBattleAction(SelectedBattleOption.DEFEND);
        GameObject go = GameManager.instance.FindCharacterGameObject(activeCharacterTurn.characterName);
        CharacterBase t = go.GetComponent<CharacterBase>();
        t.PlayAnimation("Defend");
        NextParticipant();
    }

    private void ItemAction(string _name)
    {
        // OR ID??????
        Debug.Log($"{activeCharacterTurn.characterName} used a {_name}");
    }

    public void BackAction()
    {
        UpdateBattleState(previousBattleState);
    }

    public void ListAbilities()
    {
        UpdateBattleState(BattleState.SELECT_ABILITY);
    }



    #endregion

    #region Getters
    public BattleState GetBattleState() => battleState;
    public PartyMember GetActiveCharacter() => activeCharacterTurn;
    public List<string> GetActiveCharacterChoices() => activeCharacterTurn.battleChoices;
    #endregion
}
