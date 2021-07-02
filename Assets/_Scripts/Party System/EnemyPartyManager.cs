using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartyManager : MonoBehaviour
{
    private int MAX_MEMBERS = 4;
    private int currentPartyCount = 2;

    [SerializeField]
    private List<Character> currentParty;

    [SerializeField]
    private List<GameObject> characterObjects;

    public GameObject FindCharacterGameObject(string _name)
    {
        // TODO: Add guard rails eventually
        return characterObjects.Find(x => x.name == _name);
    }

    public List<Character> GetCurrentParty() => currentParty;
    public int GetPartyMembersCount() => currentPartyCount;
}
