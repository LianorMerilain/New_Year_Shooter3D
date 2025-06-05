using System;
using UnityEngine;

[Serializable]
public class SaveSlotData
{
    public int SlotNumber { get; set; } // ����� �����
    public string SavePath { get; set; } // ���� � ����� ����������
    public DateTime SaveDateTime { get; set; } // ���� � ����� ����������
    public bool IsEmpty { get; set; } // ������ �� ����
    public string SceneName { get; set; }

    public SaveSlotData(int slotNumber)
    {
        SlotNumber = slotNumber;
        SavePath = "";
        SaveDateTime = DateTime.MinValue;
        IsEmpty = true;
    }
}