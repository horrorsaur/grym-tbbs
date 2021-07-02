using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability System/New Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public AttackType abilityType;
    [TextArea]
    public string abilityDescription;

    public string GetTooltipDescription()
    {
        return $"{abilityDescription}";
    }
}
