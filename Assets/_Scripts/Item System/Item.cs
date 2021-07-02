using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item System/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDescription;
    public ItemDetails itemDetails;
}
