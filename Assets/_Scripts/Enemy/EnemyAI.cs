using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// For now, to keep things simple let's architect this in an easy way...
    /// 
    /// Enemies will contain 1 maybe 2 actions with the potential to scale up.
    /// The Character class already contains battle choices, which we can specify for each enemy.
    /// 
    /// We will generate a random integer in order to yank an option out of their battle options
    /// This will fire off
    /// 
    /// </summary>


    private Character characterBase;

    public static event Action<string> OnEnemyActionCompleted;

    private void Start()
    {
        characterBase = GetComponent<Character>();
    }

    //private void Update()
    //{
    //    if(BattleManager.instance.GetBattleState() == BattleState.ENEMY_TURN)
    //    {
    //        if(BattleManager.instance.GetActiveCharacter().ID == characterBase.GetID())
    //        {
    //            DecideAction();
    //        }
    //    }
    //}

    private int CreateRandomInteger()
    {
        return UnityEngine.Random.Range(0, characterBase.GetBattleChoices().Count);
    }

    //public void DecideAction()
    //{
    //    int i = CreateRandomInteger();
    //    string action = characterBase.GetBattleChoices()[i];

    //    int selectedPlayerIndex = BattleManager.


    //    OnEnemyActionCompleted?.Invoke(action);
    //}
}
