using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour
{
    [Header("Character")] 
    [SerializeField] private Image hatImage;
    [SerializeField] private Image rightHandImage;
    [SerializeField] private Image leftHandImage;

    private ItemData _hatItem;
    private ItemData _leftHandItem;
    private ItemData _rightHandItem;
    
    public void EquipItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemData.ItemType.Hat:
                _hatItem = item;
                break;
            
            case ItemData.ItemType.Gun:
            case ItemData.ItemType.Sword:
                _rightHandItem = item;
                break;
            
            case ItemData.ItemType.Shield:
                _leftHandItem = item;
                break;
        }

        if (_hatItem)
        {
            hatImage.sprite = _hatItem.icon;
            hatImage.gameObject.SetActive(true);
        }
        
        if (_leftHandItem)
        {
            leftHandImage.sprite = _leftHandItem.icon;
            leftHandImage.gameObject.SetActive(true);
        }

        if (_rightHandItem)
        {
            rightHandImage.sprite = _rightHandItem.icon;
            rightHandImage.gameObject.SetActive(true);
        }
    
    }

    public void UnequipItem(ItemData.ItemType item)
    {
        switch (item)
        {
            case ItemData.ItemType.Hat:
                hatImage.sprite = null;
                hatImage.gameObject.SetActive(false);
                break;
            
            case ItemData.ItemType.Gun:
            case ItemData.ItemType.Sword:
                rightHandImage.sprite = null;
                rightHandImage.gameObject.SetActive(false);
                break;
            
            case ItemData.ItemType.Shield:
                leftHandImage.sprite = null;
                leftHandImage.gameObject.SetActive(false);
                
                break;
        }
    }
}