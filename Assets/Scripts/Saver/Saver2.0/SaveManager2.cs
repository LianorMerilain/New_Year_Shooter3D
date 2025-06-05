using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class SaveManager2 : MonoBehaviour
{
    private string _savePath;

    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/gamesave.sav";
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        // ���������� ������ �� ������
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            saveData.PlayerPositionX = player.transform.position.x;
            saveData.PlayerPositionY = player.transform.position.y;
            saveData.PlayerPositionZ = player.transform.position.z;
            HealthPlayer healthPlayer = player.GetComponent<HealthPlayer>();
            if (healthPlayer != null)
            {
                saveData.PlayerHealth = healthPlayer.CurrentHealth;
            }
        }

        // ���������� ������ � ������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyData enemyData = new EnemyData();
            enemyData.PositionX = enemy.transform.position.x;
            enemyData.PositionY = enemy.transform.position.y;
            enemyData.PositionZ = enemy.transform.position.z;
            HealthEnemy healthEnemy = enemy.GetComponent<HealthEnemy>();
            if (healthEnemy != null)
            {
                enemyData.Health = healthEnemy.CurrentHealthEnemy;
            }
            enemyData.Id = enemy.GetInstanceID();
            saveData.EnemyData.Add(enemyData);
        }

        saveData.CurrentSceneName = SceneManager.GetActiveScene().name;

        // ���������� ������ �� QuestManager
        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager != null)
        {
            saveData.QuestManagerCurrentObjectiveIndex = questManager._currentObjectiveIndex;
        }
        // ���������� ������ �� RaidenQuest
        RaidenQuest raidenQuest = FindObjectOfType<RaidenQuest>();
        if (raidenQuest != null)
        {
            saveData.RaidenQuestCurrentObjectiveIndex = raidenQuest._currentObjectiveIndex;
        }

        // ���������� ������ �� DialogueTrigger
        DialogueTrigger dialogueTrigger = FindObjectOfType<DialogueTrigger>();
        if (dialogueTrigger != null)
        {
            saveData.DialogueTriggerCanStartFirstDialogue = dialogueTrigger._canStartFirstDialogue;
            saveData.DialogueTriggerCanStartSecondDialogue = dialogueTrigger._canStartSecondDialogue;
        }

        // ���������� ������
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_savePath, FileMode.Create);
        formatter.Serialize(stream, saveData);
        stream.Close();
        Debug.Log("Game Saved!");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1; // ������������ �����
        Debug.Log("Game Reseted!");
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1; // ������������ �����
        Debug.Log("Go to Menu!");
    }

    private IEnumerator LoadGameCoroutine()
    {
        Time.timeScale = 1; // ������������ �����
        yield return null; // ���� ���� ����

        if (File.Exists(_savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_savePath, FileMode.Open);
            SaveData saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            // �������� ������ � �����
            if (saveData != null)
            {
                if (SceneManager.GetActiveScene().name != saveData.CurrentSceneName)
                {
                    SceneManager.LoadScene(saveData.CurrentSceneName);
                    yield break;
                }

                // �������� ������ �� ������
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = new Vector3(saveData.PlayerPositionX, saveData.PlayerPositionY, saveData.PlayerPositionZ);
                    HealthPlayer healthPlayer = player.GetComponent<HealthPlayer>();
                    if (healthPlayer != null)
                    {
                        healthPlayer.CurrentHealth = saveData.PlayerHealth;
                    }
                }

                // �������� ������ � ������
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    HealthEnemy healthEnemy = enemy.GetComponent<HealthEnemy>();
                    if (healthEnemy != null)
                    {
                        EnemyData enemyData = saveData.EnemyData.FirstOrDefault(e => e.Id == enemy.GetInstanceID());
                        if (enemyData != null)
                        {
                            enemy.transform.position = new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ);
                            healthEnemy.CurrentHealthEnemy = enemyData.Health;
                        }
                    }
                }
                // �������� ������ �� QuestManager
                QuestManager questManager = FindObjectOfType<QuestManager>();
                if (questManager != null)
                {
                    questManager._currentObjectiveIndex = saveData.QuestManagerCurrentObjectiveIndex;
                }

                // �������� ������ �� RaidenQuest
                RaidenQuest raidenQuest = FindObjectOfType<RaidenQuest>();
                if (raidenQuest != null)
                {
                    raidenQuest._currentObjectiveIndex = saveData.RaidenQuestCurrentObjectiveIndex;
                }

                // �������� ������ �� DialogueTrigger
                DialogueTrigger dialogueTrigger = FindObjectOfType<DialogueTrigger>();
                if (dialogueTrigger != null)
                {
                    dialogueTrigger._canStartFirstDialogue = saveData.DialogueTriggerCanStartFirstDialogue;
                    dialogueTrigger._canStartSecondDialogue = saveData.DialogueTriggerCanStartSecondDialogue;
                }
                Debug.Log("Game Loaded!");
            }
            else
            {
                Debug.LogError("SaveData is null! May be error in deserialization");
            }
        }
        else
        {
            Debug.Log("Save file not found!");
        }
    }

    public void LoadGame()
    {
        if (Time.timeScale == 0)
        {
            StartCoroutine(LoadGameCoroutine());
            return;
        }
        if (File.Exists(_savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_savePath, FileMode.Open);
            SaveData saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            // �������� ������ � �����
            if (saveData != null)
            {
                if (SceneManager.GetActiveScene().name != saveData.CurrentSceneName)
                {
                    SceneManager.LoadScene(saveData.CurrentSceneName);
                    return;
                }
                // �������� ������ �� ������
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = new Vector3(saveData.PlayerPositionX, saveData.PlayerPositionY, saveData.PlayerPositionZ);
                    HealthPlayer healthPlayer = player.GetComponent<HealthPlayer>();
                    if (healthPlayer != null)
                    {
                        healthPlayer.CurrentHealth = saveData.PlayerHealth;
                    }
                }
                // �������� ������ � ������
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    HealthEnemy healthEnemy = enemy.GetComponent<HealthEnemy>();
                    if (healthEnemy != null)
                    {
                        EnemyData enemyData = saveData.EnemyData.FirstOrDefault(e => e.Id == enemy.GetInstanceID());
                        if (enemyData != null)
                        {
                            enemy.transform.position = new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ);
                            healthEnemy.CurrentHealthEnemy = enemyData.Health;
                        }
                    }
                }
                // �������� ������ �� QuestManager
                QuestManager questManager = FindObjectOfType<QuestManager>();
                if (questManager != null)
                {
                    questManager._currentObjectiveIndex = saveData.QuestManagerCurrentObjectiveIndex;
                }
                // �������� ������ �� RaidenQuest
                RaidenQuest raidenQuest = FindObjectOfType<RaidenQuest>();
                if (raidenQuest != null)
                {
                    raidenQuest._currentObjectiveIndex = saveData.RaidenQuestCurrentObjectiveIndex;
                }
                // �������� ������ �� DialogueTrigger
                DialogueTrigger dialogueTrigger = FindObjectOfType<DialogueTrigger>();
                if (dialogueTrigger != null)
                {
                    dialogueTrigger._canStartFirstDialogue = saveData.DialogueTriggerCanStartFirstDialogue;
                    dialogueTrigger._canStartSecondDialogue = saveData.DialogueTriggerCanStartSecondDialogue;
                }
                Debug.Log("Game Loaded!");
            }
            else
            {
                Debug.Log("Save file not found!");
            }
        }
    }
    public bool HasSave()
    {
        return File.Exists(_savePath);
    }
}