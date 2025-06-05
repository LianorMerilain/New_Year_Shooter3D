using System;
using UnityEngine;

[Serializable]
public class SaveSlotData
{
    public int SlotNumber { get; set; } // Номер слота
    public string SavePath { get; set; } // Путь к файлу сохранения
    public DateTime SaveDateTime { get; set; } // Дата и время сохранения
    public bool IsEmpty { get; set; } // Пустой ли слот
    public string SceneName { get; set; }

    public SaveSlotData(int slotNumber)
    {
        SlotNumber = slotNumber;
        SavePath = "";
        SaveDateTime = DateTime.MinValue;
        IsEmpty = true;
    }
}