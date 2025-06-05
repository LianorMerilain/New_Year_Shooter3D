using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SaveUIManager : MonoBehaviour
{
/*    [SerializeField] private Transform _slotsParent;
    [SerializeField] private TextMeshProUGUI _tMPro;

    private void Start()
    {
        UpdateAllSlotsUI();
        if (SaveManager.Instance != null)
            SaveManager.Instance.OnUIInitialized();
    }
    public void UpdateAllSlotsUI()
    {
        if (_slotsParent == null) return;
        for (int i = 0; i < _slotsParent.childCount; i++)
        {
            Transform slotTransform = _slotsParent.GetChild(i);
            SaveSlotUI slotUI = slotTransform.GetComponent<SaveSlotUI>();
            if (slotUI != null)
            {
                slotUI.UpdateSlotUI(SaveManager.Instance.GetSaveSlots().FirstOrDefault(s => s.SlotNumber == slotUI.SlotNumber));
                if (slotUI.GetComponent<LayoutElement>() != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(slotUI.transform as RectTransform);
            }
        }
    }
    public void UpdateSlotUI(int slotNumber)
    {
        if (_slotsParent == null) return;
        SaveSlotUI slotUI = GetSlotUI(slotNumber);
        if (slotUI != null)
        {
            slotUI.UpdateSlotUI(SaveManager.Instance.GetSaveSlots().FirstOrDefault(s => s.SlotNumber == slotNumber));
            if (slotUI.GetComponent<LayoutElement>() != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(slotUI.transform as RectTransform);
        }
    }
    private SaveSlotUI GetSlotUI(int slotNumber)
    {
        if (_slotsParent == null) return null;
        GameObject slotObject = null;
        for (int i = 0; i < SaveManager.Instance.GetSaveSlots().Count; i++)
        {
            if (SaveManager.Instance.GetSaveSlots()[i].SlotNumber == slotNumber)
            {
                slotObject = GameObject.Find("Слот " + SaveManager.Instance.GetSaveSlots()[i].SlotNumber);
                if (slotObject != null) return slotObject.GetComponent<SaveSlotUI>();
            }
        }
        return null;
    }
    public void SetMessage(string message)
    {
        if (_tMPro != null)
            _tMPro.text = message;
    }*/
}