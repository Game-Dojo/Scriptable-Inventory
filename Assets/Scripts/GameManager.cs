using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private enum QuantityDivisionType
    {
        One,
        Half,
        All
    }

    [Header("Slots")]
    [SerializeField] private Transform gridSlots;
    [SerializeField] private Transform dropSlots;
    [SerializeField] private Transform targetSlots;
    
    [Header("Visuals")]
    [SerializeField] private RectTransform cursorSlot;
    [SerializeField] private TMP_Text itemInfo;

    [Header("Prefabs")]
    [SerializeField] private Slot slotPrefab;
    
    [Header("Character")]
    [SerializeField] private CharacterCustomization character;
    
    [Header("Inventory")]
    [SerializeField] private List<ItemData> items;
    
    [Header("Input Actions")] [SerializeField]
    private InputActionReference allItemsAction;
    [SerializeField]
    private InputActionReference halfItemsAction;
    [SerializeField]
    private InputActionReference oneItemAction;

    private Slot _currentSlot;
    private DropZone _currentZone;
    private QuantityDivisionType _quantityDivisionType = QuantityDivisionType.All;
    
    #region Unity Methods
    private void OnEnable()
    {
        cursorSlot.gameObject.SetActive(false);
        
        foreach (var item in items)
        {
            CreateItem(item, gridSlots);
        }
        
        foreach (Transform slotItem in gridSlots)
        {
            if (slotItem.TryGetComponent<Slot>(out var slot))
            {
                slot.OnInfoSelected += OnSlotHover;
                slot.OnSlotClicked += OnSlotClicked;
                slot.OnSlotReleased += OnSlotReleased;
            }
        }
        
        foreach (Transform targetSlotItem in targetSlots)
        {
            if (targetSlotItem.TryGetComponent<Slot>(out var slotItem))
            {
                slotItem.OnInfoSelected += OnSlotHover;
                slotItem.OnSlotClicked += OnSlotClicked;
                slotItem.OnSlotReleased += OnSlotReleased;
            }
        }

        allItemsAction.action.performed += OnAllItemsAction;
        halfItemsAction.action.performed += OnHalfItemsAction;
        oneItemAction.action.performed += OnOneItemAction;
    }
    
    private void Update()
    {
        //if (!_currentSlot) return;
        var mousePosition = Mouse.current.position.ReadValue();
        cursorSlot.position = mousePosition;
        
        //RectTransformUtility.ScreenPointToLocalPointInRectangle();
        //rectTransform.localPosition = localPoint;
    }
    
    private void OnDisable()
    {
        foreach (Transform slotItem in gridSlots)
        {
            if (slotItem && slotItem.TryGetComponent<Slot>(out var slot))
            {
                slot.OnInfoSelected -= OnSlotHover;
                slot.OnSlotClicked -= OnSlotClicked;
                slot.OnSlotReleased -= OnSlotReleased;
            }
        }
        
        allItemsAction.action.performed -= OnAllItemsAction;
        halfItemsAction.action.performed -= OnHalfItemsAction;
        oneItemAction.action.performed -= OnOneItemAction;
    }
    #endregion
    
    #region Set Quantity Type
    private void OnOneItemAction(InputAction.CallbackContext obj)
    {
        _quantityDivisionType = QuantityDivisionType.One;
    }

    private void OnHalfItemsAction(InputAction.CallbackContext obj)
    {
        _quantityDivisionType = QuantityDivisionType.Half;
    }

    private void OnAllItemsAction(InputAction.CallbackContext obj)
    {
        _quantityDivisionType = QuantityDivisionType.All;
    }
    #endregion
    
    #region Item Creation
    private void ProcessItem(RectTransform rect, Transform drop)
    {
        var slotData = _currentSlot.GetData();
        var slotItems = _currentSlot.GetQuantity();
                
        if (slotItems > 1)
        {
            Slot item;
            
            switch (_quantityDivisionType)
            {
                case QuantityDivisionType.One:
                    _currentSlot.ModifyQuantityByOne();
                    _currentSlot.UpdateQuantity();
                    
                    item = CreateItem(slotData, drop);
                    if (item)
                    {
                        if (item.TryGetComponent<Slot>(out var slot))
                        {
                            slot.ShowOne();
                            
                            slot.OnInfoSelected += OnSlotHover;
                            slot.OnSlotClicked += OnSlotClicked;
                            slot.OnSlotReleased += OnSlotReleased;
                        }
                    }
                    break;
                
                case QuantityDivisionType.Half:
                    var itemsRest = _currentSlot.ModifyQuantityByHalf();
                    _currentSlot.UpdateQuantity();
                    
                    var currentQuantity = _currentSlot.GetQuantity() - itemsRest;
                    
                    item = CreateItem(slotData, drop);
                    if (item)
                    {
                        if (item.TryGetComponent<Slot>(out var slot))
                        {
                            slot.SetQuantity(currentQuantity);
                            slot.UpdateQuantity();
                            
                            slot.OnInfoSelected += OnSlotHover;
                            slot.OnSlotClicked += OnSlotClicked;
                            slot.OnSlotReleased += OnSlotReleased;
                        }
                    }
                    break;
                
                case QuantityDivisionType.All:
                    _currentSlot.transform.SetParent(rect);
                    break;
            }

            return;
        }
        
        _currentSlot.transform.SetParent(rect);
    }

    private void ProcessStackableItem(RectTransform rect, Transform drop)
    {
        var slotData = _currentSlot.GetData();
        var slotType = slotData.itemType;
        var slotItems = _currentSlot.GetQuantity();

        if (rect.childCount < 1) return;
        var currentItemInSlot = rect.GetChild(0);
        if (!currentItemInSlot) return;
        if (currentItemInSlot.TryGetComponent<Slot>(out var actualSlot))
        {
            if (actualSlot.GetData().itemType == slotType)
            {
                print("Same Type");
            }
        }
        
        //Destroy(_currentSlot.gameObject);
    }

    private Slot CreateItem(ItemData data, Transform parentObject)
    {
        var newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity, parentObject);
        if (newSlot)
        {
            newSlot.SetData(data);
        }
        
        return newSlot;
    }
    #endregion
    
    #region Slot Actions
    private void OnSlotHover(string itemName, string itemDescription)
    {
        itemInfo.text = $"<color=yellow>Nombre:</color> {itemName} | <color=yellow>Descripción:</color> {itemDescription}";
    }

    private void OnSlotClicked(Slot clickedSlot, Sprite iconSprite)
    {
        cursorSlot.gameObject.SetActive(true);
        
        _currentSlot = clickedSlot;
        
        if (clickedSlot.gameObject.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.alpha = 0.2f;
        }
        
        if (cursorSlot.TryGetComponent<Slot>(out var slot))
        {
            slot.SetIcon(iconSprite);
        }

        if (_currentZone)
        {
            character.UnequipItem(_currentSlot.GetItemType());
            _currentZone.ResetZoneColor();
            _currentZone = null;
        }
    }
    
    private void OnSlotReleased()
    {
        cursorSlot.gameObject.SetActive(false);
        
        if (_currentSlot.gameObject.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.alpha = 1.0f;
        }

        foreach (Transform drop in dropSlots)
        {
            var rect = drop.GetComponent<RectTransform>();
            var hasChildren = drop.childCount > 0;

            if (!rect ||
                !RectTransformUtility.RectangleContainsScreenPoint(rect, cursorSlot.position)) continue;
            
            if (hasChildren)
                ProcessStackableItem(rect, drop);
            else
                ProcessItem(rect, drop);
        }
        
        /* TODO: Cambiar esto a una función */
        foreach (Transform drop in targetSlots)
        {
            var rect = drop.GetComponent<RectTransform>();
            var hasChildren = drop.childCount > 0;

            if (!rect || hasChildren ||
                !RectTransformUtility.RectangleContainsScreenPoint(rect, cursorSlot.position)) continue;
            
            if (rect.TryGetComponent<DropZone>(out var zone))
            {
                if (zone.Check(_currentSlot.GetItemType()))
                {
                    character.EquipItem(_currentSlot.GetData());
                }
                _currentZone = zone;
            }
            
            _currentSlot.transform.SetParent(rect);
        }

        _currentSlot = null;
    }
    #endregion
}
