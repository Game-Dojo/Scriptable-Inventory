using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public string description;
    public int quantity;
    public bool collected;
    
    
}
