using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gridSlots;
    [SerializeField] private TMP_Text itemInfo;
    
    private void OnEnable()
    {
        foreach (Transform slotItem in gridSlots)
        {
            if (slotItem.TryGetComponent<Slot>(out var slot))
            {
                slot.OnInfoSelected += OnSlotHover;
            }
        }
    }

    private void OnSlotHover(string itemName, string itemDescription)
    {
        itemInfo.text = $"<color=yellow>Nombre:</color> {itemName} | <color=yellow>Descripción:</color> {itemDescription}";
    }

    private void OnDisable()
    {
        foreach (Transform slotItem in gridSlots)
        {
            if (slotItem.TryGetComponent<Slot>(out var slot))
            {
                slot.OnInfoSelected -= OnSlotHover;
            }
        }
    }
}
