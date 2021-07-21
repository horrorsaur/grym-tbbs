using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    private PlayerInput controls;

    private BattleHUDState hudState;

    [Header("Parent References")]
    [SerializeField] private GameObject hudParent;
    [SerializeField] private GameObject battleChoicesParent;
    [SerializeField] private GameObject abilityChoicesParent;
    [SerializeField] private GameObject enemySelectionParent;

    [SerializeField] private GameObject actionButtonPrefab;

    [Header("Tooltip References")]
    [SerializeField] private Text toolTipDescription;
    [SerializeField] private Text battleHelperTooltip;


    private List<GameObject> battleChoices = new List<GameObject>();
    private List<GameObject> abilityChoices = new List<GameObject>();
    public List<GameObject> enemyChoices = new List<GameObject>();

    private GameObject defaultButtonSelection;

    #region Broadcasting Events
    public static event Action OnReceiveAttack;
    public static event Action OnListAbilities;
    public static event Action<Ability> OnReceiveAbility;
    public static event Action OnReceiveDefend;
    public static event Action<string> OnReceiveItem;
    public static event Action OnBackButtonPress;
    public static event Action<PartyMember> OnSelectEnemy;
    #endregion

    #region OnEnable & OnDisable
    private void OnEnable()
    {
        controls.Battle.Enable();

        BattleManager.OnBattleStateUpdate += SetOptionHeaderText;
        BattleManager.OnBattleStateUpdate += HandleHUDState;
        BattleManager.OnActiveCharacterChange += InstantiateBattleOptions;
        BattleManager.OnActiveCharacterChange += InstantiateAbilityOptions;
        BattleManager.OnLoadPlayerList += InstantiateEnemySelectionOptions;
        BattleManager.OnActiveCharacterChange += SetBattleChoiceParentPosition;
        BattleManager.EnemyRemoved += InstantiateEnemySelectionOptions;
    }

    private void OnDisable()
    {
        controls.Battle.Disable();

        BattleManager.OnBattleStateUpdate -= SetOptionHeaderText;
        BattleManager.OnBattleStateUpdate -= HandleHUDState;
        BattleManager.OnActiveCharacterChange -= InstantiateBattleOptions;
        BattleManager.OnActiveCharacterChange -= InstantiateAbilityOptions;
        BattleManager.OnLoadPlayerList -= InstantiateEnemySelectionOptions;
        BattleManager.OnActiveCharacterChange -= SetBattleChoiceParentPosition;
        BattleManager.EnemyRemoved -= InstantiateEnemySelectionOptions;
    }
    #endregion

    #region Unity Methods
    private void Awake() 
    {
        controls = new PlayerInput(); 
    }

    private void Start()
    {
        controls.Battle.Submit.performed += _ => HandleUIInput();
        controls.Battle.Back.performed += _ => InvokeOnBackButtonPress();
    }

    private void Update()
    {
        StartCoroutine(SetSelectedOptionTooltip());
    }
    #endregion

    #region State Management
    /// <summary>
    /// Method to fire off whenever our listener picks up on BattleManager.OnBattleStateUpdate
    /// </summary>
    /// <param name="_state"></param>
    private void HandleHUDState(BattleState _state)
    {
        if (_state == BattleState.SELECT_ENEMY)
        {
            battleChoicesParent.SetActive(false);
            enemySelectionParent.SetActive(true);

            StartCoroutine(SelectButton(enemyChoices[0], 0.1f));
        }

        switch (_state)
        {
            case BattleState.PLAYER_TURN:
                battleChoicesParent.SetActive(true);
                abilityChoicesParent.SetActive(false);
                enemySelectionParent.SetActive(false);
                break;
            case BattleState.ENEMY_TURN:
                battleChoicesParent.SetActive(false);
                break;
            case BattleState.SELECT_ABILITY:
                battleChoicesParent.SetActive(false);
                abilityChoicesParent.SetActive(true);
                StartCoroutine(SelectButton(abilityChoices[0], 0.1f));
                break;
        }
    }

    private void UpdateHUDState(BattleHUDState _state) => hudState = _state;
    #endregion

    #region Insantiating Buttons
    /// <summary>
    /// Set the battle options based on the active character
    /// </summary>
    /// <param name="_member"></param>
    private void InstantiateBattleOptions(PartyMember _member)
    {
        battleChoices.Clear();

        foreach (Transform child in battleChoicesParent.transform)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _member.battleChoices.Count; i++)
        {
            GameObject go = Instantiate(actionButtonPrefab);
            go.name = _member.battleChoices[i];
            Text t = go.GetComponentInChildren<Text>();
            Button b = go.GetComponent<Button>();

            battleChoices.Add(go);

            go.transform.SetParent(battleChoicesParent.transform, false);

            t.text = _member.battleChoices[i];
            b.onClick.AddListener(delegate { SetInvokeParameter(t.text); });

            if (i == 0)
            {
                StartCoroutine(SelectButton(go, 0.1f));
            }
        }
    }


    /// <summary>
    /// Check the active character abilities and instantiate buttons based on available choices
    /// </summary>
    private void InstantiateAbilityOptions(PartyMember _member)
    {
        abilityChoices.Clear();

        foreach (Transform child in abilityChoicesParent.transform)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Destroy(child.gameObject);
        }

        foreach (Ability a in _member.abilities)
        {
            GameObject go = Instantiate(actionButtonPrefab);
            Text t = go.GetComponentInChildren<Text>();
            Button b = go.GetComponent<Button>();

            go.GetComponent<ButtonDetails>().ability = a;

            go.name = a.abilityName;
            go.transform.SetParent(abilityChoicesParent.transform, false);
            t.text = a.abilityName;
            b.onClick.AddListener(delegate { InvokeOnReceiveAbility(a); });

            abilityChoices.Add(go);
        }
    }

    private void InstantiateEnemySelectionOptions(List<PartyMember> _members)
    {
        enemyChoices.ForEach(x => Destroy(x)); // this is hacky wacky
        enemyChoices.Clear();


        foreach (PartyMember member in _members)
        {
            if (member.type == CharacterType.ENEMY)
            {
                GameObject go = Instantiate(actionButtonPrefab);
                Text t = go.GetComponentInChildren<Text>();
                Button b = go.GetComponent<Button>();

                go.name = member.characterName;
                go.transform.SetParent(enemySelectionParent.transform, false);
                t.text = member.characterName;
                b.onClick.AddListener(delegate { InvokeSelectEnemy(member); });

                enemyChoices.Add(go);
            }
        }
    }

    // Keep here JIC
    private void InstantiateBackButton()
    {
        GameObject backButtonGO = Instantiate(actionButtonPrefab);
        backButtonGO.name = "BackButton";
        backButtonGO.transform.SetParent(abilityChoicesParent.transform, false);
        Text backText = backButtonGO.GetComponentInChildren<Text>();
        backText.text = "Back";
        Button backButton = backButtonGO.GetComponent<Button>();
        backButton.onClick.AddListener(delegate { InvokeOnBackButtonPress(); });
    }
    #endregion

    /// <summary>
    /// Handles invoking the correct event based on the _action parameter that is passed in.
    /// </summary>
    /// <param name="_action"></param>
    private void SetInvokeParameter(string _action)
    {
        switch (_action)
        {
            case "Attack":
                InvokeOnReceiveAttack();
                break;
            case "Arte":
                InvokeOnListAbilities();
                break;
            case "Spells":
                InvokeOnListAbilities();
                break;
            case "Defend":
                InvokeOnReceiveDefend();
                break;
            case "Item":
                InvokeOnReceiveItem("Potion");
                break;

        }
    }

    /// <summary>
    /// Callback for controls.Battle.Submit.performed on a battle element
    /// </summary>
    private void HandleUIInput()
    {
        Button currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        currentButton.onClick.Invoke();
    }


    #region UI Selection
    /// <summary>
    /// Set EventSystem.current to passed in parameter
    /// </summary>
    /// <param name="_selected"></param>
    /// <param name="_waitTime"></param>
    /// <returns></returns>
    public virtual IEnumerator SelectButton(GameObject _selected, float _waitTime)
    {
        defaultButtonSelection = EventSystem.current.currentSelectedGameObject;

        yield return new WaitForSeconds(_waitTime);
        EventSystem.current.SetSelectedGameObject(_selected);
    }

    public virtual IEnumerator SelectButton(Button _button, float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        _button.Select();
    }
    #endregion

    #region Invoking Battle Actions
    public void InvokeOnReceiveAttack()
    {
        OnReceiveAttack?.Invoke();
    }

    public void InvokeOnListAbilities()
    {
        OnListAbilities?.Invoke();
    }

    public void InvokeOnReceiveAbility(Ability _ability)
    {
        OnReceiveAbility?.Invoke(_ability);
    }

    public void InvokeOnReceiveDefend()
    {
        OnReceiveDefend?.Invoke();
    }

    public void InvokeOnReceiveItem(string _name)
    {
        OnReceiveItem?.Invoke(_name);
    }

    public void InvokeOnBackButtonPress()
    {
        // need to check previous state here i think
        OnBackButtonPress?.Invoke();

        // TODO: check battle state to decide which list to choose
        EventSystem.current.SetSelectedGameObject(battleChoices[0]);
    }

    private void InvokeSelectEnemy(PartyMember _member)
    {
        OnSelectEnemy?.Invoke(_member);
    }
    #endregion

    #region Tooltips
    private void SetOptionHeaderText(BattleState _state)
    {
        string tooltip = "";
        switch (_state)
        {
            case BattleState.START:
                break;
            case BattleState.PLAYER_TURN:
                tooltip = "Player's turn!";
                break;
            case BattleState.ENEMY_TURN:
                tooltip = "Enemy's turn!";
                break;
            case BattleState.ENEMY_TURN_IN_PROGRESS:
                break;
            case BattleState.SELECT_ENEMY:
                tooltip = "Select an enemy!";
                break;
            case BattleState.SELECT_ABILITY:
                tooltip = "Select an ability!";
                break;
            case BattleState.SELECT_ITEM:
                tooltip = "Select an item!";
                break;
            case BattleState.ENEMY_SELECTED:
                break;
            case BattleState.END_TURN:
                break;
            case BattleState.WON:
                tooltip = "You won!";
                break;
            case BattleState.LOST:
                tooltip = "You f****** lost...";
                break;
        }

        battleHelperTooltip.text = tooltip;
    }

    private IEnumerator SetSelectedOptionTooltip()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject _selected = EventSystem.current.currentSelectedGameObject;

        if(_selected != null)
        {
            ButtonDetails details = _selected.GetComponent<ButtonDetails>();

            try
            {
                toolTipDescription.text = details.ability.GetTooltipDescription();
            }
            catch
            {
                toolTipDescription.text = "";
            }
        }
    }

    #endregion

    private void SetBattleChoiceParentPosition(PartyMember _member)
    {
        //Vector3 offset = new Vector2(-1, 0);
        ////hudParent.transform.position += _member.characterPosition.position + offset;
        //hudParent.transform.Translate(_member.characterPosition.position + offset, Space.World);
    }

    private void ListItems()
    {
        // First, we need to get all available items and their quantity from the list of available items
        // Then iterate and display an option for each item

        // this function should feed into corresponding "SetButtonOption#" methods...
    }
}
