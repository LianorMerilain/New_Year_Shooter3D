using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ���������� ��� Slider � Toggle

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; //�������������� �� ���� ��� ���
    public GameObject pauseMenuUI; //����� ��� ������� ���� �����
    public GameObject settingsMenuUI; //����� ��� ������� ��������

    private CameraFollow _cameraFollow;
    private MusicLevelOne _musicLevelOne;

    //�������� ���������� �����������
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private TextMeshProUGUI _bombText;
    [SerializeField] private TextMeshProUGUI _healthText; // ��������� ������ �� TextMeshPro


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
        settingsMenuUI.SetActive(false); // �������������� ���� ��������, ���� ��� �������
        Time.timeScale = 1f;
        GameIsPaused = false;
        if (_cameraFollow != null) _cameraFollow.enabled = true; // ��������
        if (_bombText != null) _bombText.enabled = true; // ��������
        if (_healthText != null) _healthText.enabled = true; // ��������
        if (_musicLevelOne != null) _musicLevelOne.PlayMusic();
    }
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        if (_cameraFollow != null) _cameraFollow.enabled = false; // ���������
        if (_bombText != null) _bombText.enabled = false; // ���������
        if (_healthText != null) _healthText.enabled = false; // ���������
        if (_musicLevelOne != null) _musicLevelOne.PauseMusic();
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    // ���������� �������� (������ ����������)
    public void CloseSettings()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
    public void SaveSettings()
    {
        //PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value); // ��������� �������� ����������������
        _musicLevelOne.ChangeVolume(volumeSlider.value);
        Debug.Log("��������� ���������!");
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
            sensitivitySlider.value = 5; // �������� �� ���������
        }
    }
}