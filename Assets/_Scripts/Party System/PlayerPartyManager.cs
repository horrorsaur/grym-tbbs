using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyManager : MonoBehaviour
{
    private int MAX_REQUIRED_MEMBERS = 4;

    private int currentPartyCount = 2;

    [SerializeField]
    private List<Character> currentParty = new List<Character>();

    [SerializeField]
    private List<GameObject> characterObjects;

    public GameObject FindCharacterGameObject(string _name)
    {
        // TODO: Add guard rails eventually
        return characterObjects.Find(x => x.name == _name);
    }

    #region Party Composition
    private void AddMember(string _id)
    {
        var c = GameManager.instance.GetAllUnlockedCharacters().Find(x => x.GetID() == _id);
        if (!currentParty.Contains(c))
        {
            currentParty.Add(c);
        }
    }

    private void RemoveMember(string _id)
    {
        try
        {
            currentParty.RemoveAll(n => n.GetID() == _id);
        }
        catch
        {
            Debug.LogWarning($"Encountered error while removing party member");
        }
    }

    private void SetPartyLeader() { }
    private void SetRole() { }
    #endregion

    #region Getters
    public List<Character> GetCurrentParty() => currentParty;
    public int GetPartyMembersCount() => currentPartyCount;
    #endregion

}
