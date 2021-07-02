using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PAUSED,
    IN_BATTLE
}

public enum BattleHUDState
{
    DISPLAY_MAIN,
    SELECT_ENEMY,
    DISPLAY_ABILITIES,
    DISPLAY_ITEMS
}

public enum BattleState
{
    START,
    PLAYER_TURN,
    ENEMY_TURN,
    ENEMY_TURN_IN_PROGRESS,
    SELECT_ENEMY,
    SELECT_ABILITY,
    SELECT_ITEM,
    ENEMY_SELECTED,
    END_TURN,
    WON,
    LOST
}

public enum CharacterState
{
    IDLE,
    MOVING,
    ATTACKING,
    DEFENDING,
    USING_ITEM,
    DEAD
}

public enum StatusEffect
{
    NONE,
    BLEEDING,
    POISONED,
    PARALYZED,
    SLOW,
}

public enum CharacterType 
{ 
    PLAYER, 
    ENEMY 
}

public enum CharacterClass
{
    WARRIOR,
    MAGE,
}

public enum SelectedBattleOption
{
    ATTACK,
    ARTES,
    SPELLS,
    DEFEND,
    ITEM
}

public enum AttackType
{
    MELEE,
    RANGE,
    MAGIC
}

public enum ItemType
{
    RESTORATIVE,
    DETRIMENTAL,
    KEY,
}

class SortBattleParticipantsDescending : IComparer<PartyMember>
{
    int IComparer<PartyMember>.Compare(PartyMember a, PartyMember b)
    {
        if (a.currentAgility > b.currentAgility)
            return -1;
        if (a.currentAgility < b.currentAgility)
            return 1;
        else
            return 0;
    }
}

[System.Serializable]
public struct PartyMember
{
    public Character baseCharacter { get; private set; }

    public string characterName;
    public CharacterType type;
    public CharacterClass characterClass;
    public StatusEffect statusEffect;

    public string ID;
    public int currentAgility;

    public List<Ability> abilities;
    public List<string> battleChoices;

    public Transform characterPosition;

    public PartyMember(
            Character _baseCharacter,
            string _charName,
            CharacterType _charType, 
            string _id, 
            int _agility, 
            List<Ability> _abilities, 
            List<string> _choices,
            CharacterClass _class,
            StatusEffect _statusEffect,
            Transform _position
        )
    {
        baseCharacter = _baseCharacter;
        characterName = _charName;
        type = _charType;
        ID = _id;
        currentAgility = _agility;
        abilities = _abilities;
        battleChoices = _choices;
        characterClass = _class;
        statusEffect = _statusEffect;
        characterPosition = _position;
    }
}


[System.Serializable]
public struct ItemDetails
{
    public int goldCost;
    public ItemType itemType;
    [Range(0,1)] public float restorePercentage;
}