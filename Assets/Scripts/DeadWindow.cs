/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class DeadWindow : MonoBehaviour
{
    private HealthPlayer _healthPlayer;
    public GameObject deathPanel;
    private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _deathText;
    private Vector3 _initialScale;
    [SerializeField] private Image _fadeImage;
    private CameraFollow _cameraFollow;
    private ShootingPlayer _shootingPlayer;
    private PauseMenu _pauseMenu;
    private MusicLevelOne _musicLevelOne;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _deadSound;
    private bool _isInitialized = false;


    private void Awake()
    {

    }
    public void Initialize()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _audioSource = gameObject.AddComponent<AudioSource>();
        _cameraFollow = FindObjectOfType<CameraFollow>();
        _healthPlayer = FindObjectOfType<HealthPlayer>();
        _shootingPlayer = FindObjectOfType<ShootingPlayer>();
        _pauseMenu = FindObjectOfType<PauseMenu>();
        _musicLevelOne = FindObjectOfType<MusicLevelOne>();
        _canvasGroup = deathPanel.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _initialScale = _deathText.transform.localScale;
        _deathText.transform.localScale = Vector3.zero;
        // Изначально делаем fadeImage прозрачным
        Color imageColor = _fadeImage.color;
        imageColor.a = 0f;
        _fadeImage.color = imageColor;
        _fadeImage.gameObject.SetActive(false);
        deathPanel.SetActive(false);
        if (_healthPlayer != null)
            _healthPlayer.OnDeath += HandlePlayerDeath;
        if (_musicLevelOne != null) _musicLevelOne.enabled = true;
        if (_cameraFollow != null) _cameraFollow.enabled = true;
        if (_shootingPlayer != null) _shootingPlayer.enabled = true;
        if (_pauseMenu != null) _pauseMenu.enabled = true;
    }
    private void HandlePlayerDeath()
    {
        Time.timeScale = 0f;
        if (_musicLevelOne != null) _musicLevelOne.StopMusic();
        if (_audioSource != null && _deadSound != null)
        {
            _audioSource.clip = _deadSound;
            _audioSource.Play();
        }

        if (_musicLevelOne != null) _musicLevelOne.enabled = false;
        if (_cameraFollow != null) _cameraFollow.enabled = false;
        if (_shootingPlayer != null) _shootingPlayer.enabled = false;
        if (_pauseMenu != null) _pauseMenu.enabled = false;

        deathPanel.SetActive(true);
        _fadeImage.gameObject.SetActive(true);
        StartCoroutine(AnimateDeathPanel());
    }

    private IEnumerator AnimateDeathPanel()
    {
        float elapsedTime = 0;
        float duration = 1.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            _deathText.transform.localScale = Vector3.Lerp(Vector3.zero, _initialScale, elapsedTime / duration);
            yield return null;
        }
        _canvasGroup.alpha = 1;
        _deathText.transform.localScale = _initialScale;
        StartCoroutine(FadeInImageAndRestart());
    }

    private IEnumerator FadeInImageAndRestart()
    {
        _fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        float duration = 2f;
        Color startColor = _fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }
        _fadeImage.color = targetColor;
        if (GameManager.Instance != null)
            GameManager.Instance.RestartLevel();

        _fadeImage.gameObject.SetActive(false);
    }

}*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class DeadWindow : MonoBehaviour
{
    private HealthPlayer _healthPlayer;
    public GameObject deathPanel;
    private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _deathText;
    private Vector3 _initialScale;
    [SerializeField] private Image _fadeImage;
    private CameraFollow _cameraFollow;
    private ShootingPlayer _shootingPlayer;
    private PauseMenu _pauseMenu;
    private MusicLevelOne _musicLevelOne;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _deadSound;
    private EnemyAI _enemyAi;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _cameraFollow = FindObjectOfType<CameraFollow>();
        _healthPlayer = FindObjectOfType<HealthPlayer>();
        _shootingPlayer = FindObjectOfType<ShootingPlayer>();
        _pauseMenu = FindObjectOfType<PauseMenu>();
        _musicLevelOne = FindObjectOfType<MusicLevelOne>();
        _enemyAi = FindObjectOfType<EnemyAI>();
        if (deathPanel == null)
        {
            Debug.LogError("не назначена панель смерти!");
            enabled = false;
            return;
        }
        _canvasGroup = deathPanel.GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            Debug.LogError("не найден компонент CanvasGroup!");
            enabled = false;
            return;
        }
        _canvasGroup.alpha = 0;

        if (_deathText == null)
        {
            Debug.LogError("не назначен текст смерти!");
            enabled = false;
            return;
        }
        _initialScale = _deathText.transform.localScale;
        _deathText.transform.localScale = Vector3.zero;
        deathPanel.SetActive(false);


        if (_fadeImage == null)
        {
            Debug.LogError("не назначено затемнение!");
            enabled = false;
            return;
        }
        // Изначально делаем fadeImage прозрачным
        Color imageColor = _fadeImage.color;
        imageColor.a = 0f;
        _fadeImage.color = imageColor;
        _fadeImage.gameObject.SetActive(false);

        if (_healthPlayer == null)
        {
            Debug.LogError("не найден компонент HealthPlayer!");
            enabled = false;
            return;
        }

        _healthPlayer.OnDeath += HandlePlayerDeath;
    }
    private void OnDestroy()
    {
        if (_healthPlayer != null)
        {
            _healthPlayer.OnDeath -= HandlePlayerDeath;
        }
    }
    private void HandlePlayerDeath()
    {
        ShowDeathPanel();
    }
    public void ShowDeathPanel()
    {
        _musicLevelOne.StopMusic();
        _audioSource.PlayOneShot(_deadSound);
        if (_musicLevelOne != null) _musicLevelOne.enabled = false;
        if (_cameraFollow != null) _cameraFollow.enabled = false;
        if (_shootingPlayer != null) _shootingPlayer.enabled = false;
        if (_pauseMenu != null) _pauseMenu.enabled = false;
        if (_healthPlayer != null) _healthPlayer.enabled = false;
        if (_enemyAi != null)  _enemyAi.enabled = false;
        deathPanel.SetActive(true);
        StartCoroutine(AnimateDeathPanel());
        Time.timeScale = 0f;
    }

    private IEnumerator AnimateDeathPanel()
    {
        float elapsedTime = 0;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            _deathText.transform.localScale = Vector3.Lerp(Vector3.zero, _initialScale, elapsedTime / duration);
            yield return null;
        }
        _canvasGroup.alpha = 1;
        _deathText.transform.localScale = _initialScale;
        StartCoroutine(FadeInImageAndRestart());
    }

    private IEnumerator FadeInImageAndRestart()
    {
        _fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        float duration = 0.5f;
        Color startColor = _fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }
        _fadeImage.color = targetColor;
        RestartLevel();
    }
    public void RestartLevel()
    {
        if (_musicLevelOne != null) _musicLevelOne.enabled = true;
        if (_cameraFollow != null) _cameraFollow.enabled = true;
        if (_shootingPlayer != null) _shootingPlayer.enabled = true;
        if (_pauseMenu != null) _pauseMenu.enabled = true;
        if (_healthPlayer != null) _healthPlayer.enabled = true;
        if (_enemyAi != null) _enemyAi.enabled = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}