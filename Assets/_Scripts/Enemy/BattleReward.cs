using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReward : MonoBehaviour
{
    [SerializeField] 
    private int baseExperiencePoints;
    [Range(0f,1f)]
    [SerializeField] 
    private float itemDropPercentage;
    [SerializeField] 
    private List<Item> itemDrops;

    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    #region Getters
    public int ExperiencePoints => baseExperiencePoints;
    public float ItemDropPercentage => itemDropPercentage;
    public List<Item> ItemDrops => itemDrops;
    #endregion
}
