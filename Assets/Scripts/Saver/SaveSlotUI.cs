using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    /*public int SlotNumber;
    public Button SlotButton;
    public TextMeshProUGUI SaveTimeText;
    [SerializeField] private TextMeshProUGUI _tMPro;
    private SaveManager _saveManager;


    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("Error no GameManager script");
            enabled = false;
        }
        SlotButton.onClick.AddListener(OnSlotSelect);
    }
    public void UpdateSlotUI(SaveSlotData slotData)
    {
        if (SaveTimeText == null)
        {
            Debug.LogError("TextMeshPro text component is null on " + gameObject.name);
            return;
        }
        if (slotData == null)
        {
            SaveTimeText.text = "������ ����";
            return;
        }
        if (!slotData.IsEmpty)
        {
            SaveTimeText.text = "����������: " + slotData.SaveDateTime.ToString("g") + "\n�������: " + slotData.SceneName;
        }
        else
        {
            SaveTimeText.text = "������ ����";
        }
    }

    public void OnSlotSelect()
    {
        GameManager.Instance.SelectSlot(SlotNumber);
    }*/
}