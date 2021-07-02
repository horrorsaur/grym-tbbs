using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entry point for animatable characters.
/// 
/// Animatable characters will have certain "base" animations:
/// 1. Attack
/// 2. Defend
/// 3. Item
/// 
/// Ability will constantly changed, so we need some way to dynamically import it based on the attack the base
/// character used.
/// </summary>
public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField]
    private string attackAnimationName;
    [SerializeField]
    private string abilityAnimationName;
    [SerializeField]
    private string defendAnimationName;
    [SerializeField]
    private string itemAnimationName;

    private int attackHash;
    private int abilityHash;
    private int defendHash;
    private int itemHash;

    private void Start()
    {
        animator = GetComponent<Animator>();

        attackHash = Animator.StringToHash(attackAnimationName);
        abilityHash = Animator.StringToHash(abilityAnimationName);
        defendHash = Animator.StringToHash(defendAnimationName);
        itemHash = Animator.StringToHash(itemAnimationName);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
        }
    }

    private void MapStringToAnimationHash()
    {

    }

    public void UpdateAbilityHash(string _value) => abilityAnimationName = _value;

}
