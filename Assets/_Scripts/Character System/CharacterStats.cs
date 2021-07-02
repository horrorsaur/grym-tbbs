using UnityEngine;

[System.Serializable]
public struct CharacterStats
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxMana;
    [SerializeField] private int maxStrength;
    [SerializeField] private int maxDefense;
    [SerializeField] private int maxAgility;

    private int currentHealth;
    private int currentMana;
    private int currentStrength;
    private int currentDefense;
    private int currentAgility;

    public void InitializeStats()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStrength = maxStrength;
        currentDefense = maxDefense;
        currentAgility = maxAgility;
    }

    #region Getters
    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => currentHealth;

    public int GetMaxMana() => maxMana;
    public int GetCurrentMana() => currentMana;

    public int GetMaxStrength() => maxStrength;
    public int GetCurrentStrength() => currentStrength;

    public int GetMaxDefense() => maxDefense;
    public int GetCurrentDefense() => currentDefense;

    public int GetMaxAgility() => maxAgility;
    public int GetCurrentAgility() => currentAgility;
    #endregion

    #region Setters
    public int SetMaxHealth(int _value) => maxHealth = _value;
    public int SetCurrentHealth(int _value) => currentHealth = _value;

    public int SetMaxMana(int _value) => maxMana = _value;
    public int SetCurrentMana(int _value) => currentMana = _value;

    public int SetMaxStrength(int _value) => maxStrength = _value;
    public int SetCurrentStrength(int _value) => currentStrength = _value;

    public int SetMaxDefense(int _value) => maxDefense = _value;
    public int SetCurrentDefense(int _value) => currentDefense = _value;

    public int SetMaxAgility(int _value) => maxAgility = _value;
    public int SetCurrentAgility(int _value) => currentAgility = _value;
    #endregion
}
