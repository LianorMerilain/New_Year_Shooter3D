using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    /*    public float PlayerPositionX { get; set; }
        public float PlayerPositionY { get; set; }
        public float PlayerPositionZ { get; set; }
        public float PlayerHealth { get; set; }
        public List<EnemyData> EnemyData { get; set; }
        public List<int> DeadEnemies { get; set; } // Список ID мертвых врагов
        public string CurrentSceneName { get; set; }

        public SaveData()
        {
            EnemyData = new List<EnemyData>();
            DeadEnemies = new List<int>(); // Инициализация списка мертвых врагов
        }
    }
    [Serializable]
    public class EnemyData
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float Health { get; set; }
        public int Id { get; set; }
        public bool IsDeadEnemy { get; set; }*/
    public float PlayerPositionX { get; set; }
    public float PlayerPositionY { get; set; }
    public float PlayerPositionZ { get; set; }
    public float PlayerHealth { get; set; }
    public List<EnemyData> EnemyData { get; set; }
    public string CurrentSceneName { get; set; }

    public int QuestManagerCurrentObjectiveIndex { get; set; }
    public int RaidenQuestCurrentObjectiveIndex { get; set; }
    public bool DialogueTriggerCanStartFirstDialogue { get; set; }
    public bool DialogueTriggerCanStartSecondDialogue { get; set; }
    public SaveData()
    {
        EnemyData = new List<EnemyData>();
    }
}
[Serializable]
public class EnemyData
{
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }
    public float Health { get; set; }
    public int Id { get; set; }
}