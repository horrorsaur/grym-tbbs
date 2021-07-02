using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private string characterName;
    [SerializeField] private int level;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private CharacterClass characterClass;

    [SerializeField]
    protected CharacterStats stats = new CharacterStats();

    [SerializeField]
    protected List<Ability> abilities = new List<Ability>();

    [SerializeField]
    protected List<string> battleChoices = new List<string>();

    public Healthbar healthBar;

    protected StatusEffect statusEffect { get; private set; }
    protected CharacterState characterState { get; private set; }
    protected Animator characterAnimator { get; private set; }
    protected bool isDead { get; private set; }

    #region Broadcasting Events
    public static Action<GameObject> OnCharacterDeath;
    #endregion

    private void OnEnable()
    {
        BattleManager.OnDamageCharacter += TakeDamage;
    }

    private void OnDisable()
    {
        BattleManager.OnDamageCharacter -= TakeDamage;
    }

    private void Awake()
    {
        stats.InitializeStats();
        healthBar?.SetMaxHealth(stats.GetMaxHealth());
    }

    public virtual void Start()
    {
        characterAnimator = GetComponent<Animator>();
        SetStatusEffect(StatusEffect.NONE);
    }

    protected virtual void TakeDamage(string _id, int _amount, string _name)
    {
        if (_id != ID)
            return;

        int current = stats.GetCurrentHealth();
        int healthRemaining = current - _amount;

        stats.SetCurrentHealth(healthRemaining);
        healthBar?.SetHealth(healthRemaining);
        Debug.Log($"{characterName} took {_amount} damage from {_name}");

        if (stats.GetCurrentHealth() <= 0)
        {
            DeathState();
        }
    }

    protected virtual void DeathState()
    {
        isDead = true;
        Debug.Log($"{characterName} was defeated!");
        Destroy(gameObject);
        //gameObject.SetActive(false);

        OnCharacterDeath?.Invoke(gameObject);
    }

    public void PlayAnimation(string _name)
    {
        int hash = Animator.StringToHash(_name);
        characterAnimator.Play(hash);
    }

    #region Setters
    public void SetStatusEffect(StatusEffect _state) => statusEffect = _state;
    public void SetCharacterState(CharacterState _state) => characterState = _state;
    #endregion

    #region Getters
    public int Level => level;
    public string GetCharacterName() => characterName;
    public string GetID() => ID;
    public List<Ability> GetAbilities() => abilities;
    public List<string> GetBattleChoices() => battleChoices;
    public int GetCurrentAgility() => stats.GetCurrentAgility();
    public StatusEffect GetStatusEffect() => statusEffect;
    public CharacterType GetCharacterType() => characterType;
    public CharacterClass GetCharacterClass() => characterClass;
    #endregion

    #region Stat Getters
    public int CurrentHealth => stats.GetCurrentHealth();
    public int CurrentMana => stats.GetCurrentMana();
    public int CurrentStrength => stats.GetCurrentStrength();
    #endregion
}
