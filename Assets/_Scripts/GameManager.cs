using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private List<Character> unlockedCharacters = new List<Character>();
    [SerializeField]
    private List<Character> allCharacters = new List<Character>();
    [SerializeField]
    private List<GameObject> allCharacterGameObjects;

    public List<string> unlockedCharacterIds { get; private set; } = new List<string>();
    public List<string> allCharacterIds { get; private set; } = new List<string>();

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

        GetUnlockedCharacterIds();
    }

    /// <summary>
    /// Finds a GameObject matching the name provided in the parameter within currentParty
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public GameObject FindCharacterGameObject(string _name)
    {
        // TODO: Add guard rails eventually
        return allCharacterGameObjects.Find(x => x.name == _name);
    }

    public List<string> GetUnlockedCharacterIds()
    {
        foreach (var c in unlockedCharacters)
        {
            if(unlockedCharacterIds.Contains(c.GetID())) 
            {
                Debug.LogError($"The ID {c.GetID()} is already within the unlockedCharacterIds - Skipping...");
            } else
            {
                unlockedCharacterIds.Add(c.GetID());
            }
        }

        return unlockedCharacterIds;
    }
    public List<string> GetAllCharacterIds()
    {
        foreach (var c in allCharacters)
        {
            if (allCharacterIds.Contains(c.GetID()))
            {
                Debug.LogError($"The ID {c.GetID()} is already within the allCharacterIds - Skipping...");
            }
            else
            {
                allCharacterIds.Add(c.GetID());
            }
        }

        return allCharacterIds;
    }
    public List<Character> GetAllUnlockedCharacters() => unlockedCharacters;

}
