using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;

public class SaveManager : MonoBehaviour
{
  /*  public static SaveManager Instance { get; private set; }
    private string _saveDirectory;
    private List<SaveSlotData> _saveSlots;
    public SaveUIManager _saveUIManager;

    [SerializeField] private PlayerInitialData _playerInitialData;
    private int _selectedSlot = -1;


    public int SelectedSlot { get => _selectedSlot; set => _selectedSlot = value; }
    private List<HealthEnemy> _allEnemies = new List<HealthEnemy>(); // Список всех врагов
    private void Start()
    {

        GetAllEnemies();

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            _saveUIManager = FindObjectOfType<SaveUIManager>();
            if (_saveUIManager == null)
                Debug.Log("Error no SaveUIManager script");
            LoadGameOnStart();
        }
    }
    private void GetAllEnemies()
    {
        _allEnemies.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            HealthEnemy healthEnemy = enemy.GetComponent<HealthEnemy>();
            if (healthEnemy != null)
                _allEnemies.Add(healthEnemy);

        }
    }
    public void OnUIInitialized()
    {
        LoadGameOnStart();
    }

    public void LoadGameOnStart()
    {
        if (HasSave(0))
        {
            LoadGame(0);
        }
    }
    public void CreateInitialSave()
    {
        SaveGame(0);
    }
    public void ResetLevel()
    {
        if (Instance != null)
            Destroy(gameObject);
        SceneManager.LoadScene(1);

    }
    private void Awake()
    {

        _saveDirectory = Application.persistentDataPath + "/saves/";
        _saveSlots = new List<SaveSlotData>();
        for (int i = 0; i < 3; i++)
        {
            _saveSlots.Add(new SaveSlotData(i));
        }

        if (!Directory.Exists(_saveDirectory))
            Directory.CreateDirectory(_saveDirectory);

        LoadSlotData();
    }
    public void SaveGame(int slot)
    {
        string savePath = _saveDirectory + "slot_" + slot + ".sav";
        if (slot == -1)
        {
            Debug.Log("No save slot selected!");
            if (_saveUIManager != null)
                _saveUIManager.SetMessage("Не выбран слот сохранения!");
            return;
        }

        SaveData2 saveData = new();
        // Получаем данные об игроке
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

        // Получаем данные о врагах
        saveData.DeadEnemies.Clear();
        saveData.EnemyData.Clear();
        foreach (HealthEnemy enemy in _allEnemies)
        {
            EnemyData enemyData = new EnemyData();
            enemyData.PositionX = enemy.transform.position.x;
            enemyData.PositionY = enemy.transform.position.y;
            enemyData.PositionZ = enemy.transform.position.z;
            if (enemy != null)
            {
                enemyData.Health = enemy.CurrentHealthEnemy;
                enemyData.IsDeadEnemy = enemy.IsDeadEnemy;
                if (enemy.IsDeadEnemy)
                    saveData.DeadEnemies.Add(enemy.GetInstanceID());
            }
            enemyData.Id = enemy.GetInstanceID();
            saveData.EnemyData.Add(enemyData);
        }
        saveData.CurrentSceneName = SceneManager.GetActiveScene().name;

        // Сохраняем данные
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);
        formatter.Serialize(stream, saveData);
        stream.Close();

        UpdateSlotData(savePath);
        if (_saveUIManager != null)
            _saveUIManager.UpdateSlotUI(slot);
        if (_saveUIManager != null)
            _saveUIManager.SetMessage("Игра сохранена в слот " + slot);
        Debug.Log("Game Saved to slot: " + slot);
    }
    public void LoadGame(int slot)
    {
        string savePath = GetSavePath(slot);

        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            SaveData2 saveData = formatter.Deserialize(stream) as SaveData2;
            stream.Close();
            // Загружаем данные в сцену
            if (saveData != null)
            {
                if (SceneManager.GetActiveScene().name != saveData.CurrentSceneName)
                {
                    SceneManager.LoadScene(saveData.CurrentSceneName);
                    return;
                }
                // Загружаем данные об игроке
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
                // Получаем данные о врагах
                foreach (HealthEnemy enemy in _allEnemies)
                {
                    if (enemy != null)
                    {
                        EnemyData enemyData = saveData.EnemyData.FirstOrDefault(e => e.Id == enemy.GetInstanceID());
                        if (enemyData != null)
                        {
                            enemy.transform.position = new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ);
                            enemy.CurrentHealthEnemy = enemyData.Health;
                            enemy.IsDeadEnemy = enemyData.IsDeadEnemy;
                        }
                        if (saveData.DeadEnemies.Contains(enemy.GetInstanceID()))
                        {
                            Renderer renderer = enemy.GetComponentInChildren<Renderer>();
                            if (renderer != null)
                                renderer.enabled = false;
                            MonoBehaviour[] movement = enemy.GetComponents<MonoBehaviour>();
                            if (movement != null)
                                foreach (var item in movement)
                                    if (item != this)
                                        item.enabled = false;

                        }
                        else
                        {
                            Renderer renderer = enemy.GetComponentInChildren<Renderer>();
                            if (renderer != null)
                                renderer.enabled = true;
                            MonoBehaviour[] movement = enemy.GetComponents<MonoBehaviour>();
                            if (movement != null)
                                foreach (var item in movement)
                                    if (item != this)
                                        item.enabled = true;
                        }
                    }
                }
            }
            HealthPlayer _healthPlayer = FindObjectOfType<HealthPlayer>();
            _healthPlayer.UpdateHealthText();
            if (_saveUIManager != null)
                _saveUIManager.SetMessage("Игра загружена из слота: " + slot);
            Debug.Log("Game Loaded from slot: " + slot);
        }
        else
        {
            Debug.Log("Save file not found!");
            if (_saveUIManager != null)
                _saveUIManager.SetMessage("Файл сохранения не найден");
        }
    }
    public void LoadGame()
    {
        if (SelectedSlot == -1)
        {
            if (_saveUIManager != null)
                _saveUIManager.SetMessage("Не выбран слот сохранения!");
            Debug.Log("No save slot selected!");
            return;
        }
        if (Time.timeScale == 0)
        {
            StartCoroutine(LoadGameCoroutine());
            return;
        }
        LoadGame(SelectedSlot);

    }
    private IEnumerator LoadGameCoroutine()
    {
        Time.timeScale = 1;
        yield return null;
        LoadGame(SelectedSlot);
    }

    public bool HasSave(int slot)
    {
        return !string.IsNullOrEmpty(GetSavePath(slot)) && File.Exists(GetSavePath(slot));
    }

    private string GetSavePath(int slot)
    {
        if (slot >= 0 && slot < _saveSlots.Count)
            return _saveSlots[slot].SavePath;
        return "";
    }
    // Метод для обновления данных слота
    private void UpdateSlotData(string savePath)
    {
        SaveSlotData slot = _saveSlots.FirstOrDefault(x => x.SlotNumber == SelectedSlot);
        if (slot != null)
        {
            slot.SavePath = savePath;
            slot.SaveDateTime = DateTime.Now;
            slot.IsEmpty = false;
            slot.SceneName = SceneManager.GetActiveScene().name;
            SaveSlotData();
        }
    }
    // Метод для загрузки данных слотов при старте игры
    private void LoadSlotData()
    {
        if (File.Exists(_saveDirectory + "slotdata.sav"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_saveDirectory + "slotdata.sav", FileMode.Open);

            List<SaveSlotData> saveData = formatter.Deserialize(stream) as List<SaveSlotData>;
            stream.Close();

            if (saveData != null)
            {
                _saveSlots = saveData;
            }
        }
    }
    private void SaveSlotData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_saveDirectory + "slotdata.sav", FileMode.Create);

        formatter.Serialize(stream, _saveSlots);
        stream.Close();
    }
    public List<SaveSlotData> GetSaveSlots()
    {
        return _saveSlots;
    }
    // Функция удаления сохранения из выбранного слота
    public void DeleteSave()
    {
        if (SelectedSlot == -1) return;
        string savePath = GetSavePath(SelectedSlot);
        if (!string.IsNullOrEmpty(savePath) && File.Exists(savePath))
        {
            File.Delete(savePath);
            if (_saveUIManager != null)
                _saveUIManager.SetMessage("Сохранение в слоте: " + SelectedSlot + " было удалено");
            Debug.Log("Save in slot: " + SelectedSlot + " was deleted");
            SaveSlotData slot = _saveSlots.FirstOrDefault(x => x.SlotNumber == SelectedSlot);
            if (slot != null)
            {
                slot.SavePath = "";
                slot.SaveDateTime = DateTime.MinValue;
                slot.IsEmpty = true;
                slot.SceneName = "";
                SaveSlotData();
                if (_saveUIManager != null)
                    _saveUIManager.UpdateSlotUI(SelectedSlot);
            }
        }
        else
        {
            Debug.LogWarning("Save was not found");
            if (_saveUIManager != null)
                _saveUIManager.SetMessage("Сохранение не найдено");
        }
    }
    private GameObject GetEnemyById(int enemyId)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.FirstOrDefault(e => e.GetInstanceID() == enemyId);
    }*/
}