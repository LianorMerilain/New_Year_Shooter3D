using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /*public static GameManager Instance { get; private set; }
    private SaveManager _saveManager;
    private PauseMenu _pauseMenu;
    private DeadWindow _deadWindow;
    private bool _isInitialized = false;
    private bool _firstLoad = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _saveManager = FindObjectOfType<SaveManager>();
        if (_saveManager == null)
            Debug.Log("Error no SaveManager script");
        _pauseMenu = FindObjectOfType<PauseMenu>();
        if (_pauseMenu == null)
            Debug.Log("Error no PauseMenu script");
        _deadWindow = FindObjectOfType<DeadWindow>();
        if (_deadWindow == null)
            Debug.Log("Error no DeadWindow script");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            if (_deadWindow != null)
                _deadWindow.Initialize();
        }
    }


    public void RestartLevel()
    {
        if (_saveManager != null)
            StartCoroutine(RestartLevelCoroutine());
    }

    private IEnumerator RestartLevelCoroutine()
    {
        Time.timeScale = 1;
        yield return null;
        if (_saveManager != null)
        {
            _saveManager.LoadGame(0);
            StartCoroutine(ResumeAfterLoad());
        }
    }

    private IEnumerator ResumeAfterLoad()
    {
        yield return new WaitForSeconds(0.1f); // Даем время на загрузку
        if (_pauseMenu != null)
            _pauseMenu.Resume();
        if (_deadWindow != null)
            _deadWindow.Initialize();
    }


    public void SelectSlot(int slot)
    {
        if (_saveManager != null)
        {
            _saveManager.SelectedSlot = slot;
            Debug.Log("Selected save slot: " + slot);
            if (_saveManager._saveUIManager != null)
                _saveManager._saveUIManager.SetMessage("Выбран слот сохранения: " + slot);
        }
    }
    public void SaveGame()
    {
        if (_saveManager != null)
            _saveManager.SaveGame(_saveManager.SelectedSlot);
    }
    public void SaveGameZero()
    {
        if (_saveManager != null)
            _saveManager.SaveGame(0);
    }
    public void LoadGame()
    {
        if (_saveManager != null)
        {
            StartCoroutine(LoadGameCoroutine());
        }
    }
    private IEnumerator LoadGameCoroutine()
    {
        Time.timeScale = 1;
        yield return null;
        if (_saveManager != null)
            _saveManager.LoadGame(_saveManager.SelectedSlot);
        StartCoroutine(ResumeAfterLoad());
    }
    public void DeleteSave()
    {
        if (_saveManager != null)
            _saveManager.DeleteSave();
    }*/
}