using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Sword,
        Shield,
        Bow,
        Hammer,
        Gun,
        Hat
    }
    
    public Sprite icon;
    public string itemName;
    public ItemType itemType;
    public string description;
    public int quantity;
    public bool collected;
}
