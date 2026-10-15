using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    [SerializeField] private List<ItemData.ItemType> dropTypes;
    
    private Image _backgroundColor;
    private Color _originalColor;

    private bool _isItemUsable = false;

    private void Awake()
    {
        _backgroundColor = GetComponent<Image>();
        _originalColor = _backgroundColor.color;
    }

    public bool Check(ItemData.ItemType itemType)
    {
        foreach (var tp in dropTypes)
        {
            if (itemType == tp)
            {
                ResetZoneColor();
                return true;
            }
            
            _isItemUsable = false;
            _backgroundColor.color = Color.red;
        }

        return false;
    }

    public void ResetZoneColor()
    {
        _isItemUsable = true;
        _backgroundColor.color = _originalColor;
    }
    
    public List<ItemData.ItemType> GetDropTypes() => dropTypes;
}
