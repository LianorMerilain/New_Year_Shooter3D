using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Необходимо для Slider и Toggle

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; //приостановлена ли игра или нет
    public GameObject pauseMenuUI; //показ или скрытие меню паузы
    public GameObject settingsMenuUI; //показ или скрытие настроек

    private CameraFollow _cameraFollow;
    private MusicLevelOne _musicLevelOne;

    //элементы управления настройками
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private TextMeshProUGUI _bombText;
    [SerializeField] private TextMeshProUGUI _healthText; // Добавляем ссылку на TextMeshPro


    private void Start()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        _cameraFollow = FindObjectOfType<CameraFollow>();
        _musicLevelOne = FindAnyObjectByType<MusicLevelOne>();
        LoadSettings();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false); // Деактивировать меню настроек, если оно активно
        Time.timeScale = 1f;
        GameIsPaused = false;
        if (_cameraFollow != null) _cameraFollow.enabled = true; // Включить
        if (_bombText != null) _bombText.enabled = true; // включить
        if (_healthText != null) _healthText.enabled = true; // включить
        if (_musicLevelOne != null) _musicLevelOne.PlayMusic();
    }
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        if (_cameraFollow != null) _cameraFollow.enabled = false; // Отключить
        if (_bombText != null) _bombText.enabled = false; // Отключить
        if (_healthText != null) _healthText.enabled = false; // Отключить
        if (_musicLevelOne != null) _musicLevelOne.PauseMusic();
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    // Сохранение настроек (пример реализации)
    public void CloseSettings()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
    public void SaveSettings()
    {
        //PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value); // Сохраняем значение чувствительности
        _musicLevelOne.ChangeVolume(volumeSlider.value);
        Debug.Log("Настройки сохранены!");
        CloseSettings();
    }
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
        }
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            sensitivitySlider.value = 5; // Значение по умолчанию
        }
    }
}