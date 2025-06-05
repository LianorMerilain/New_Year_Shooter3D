using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinWindows : MonoBehaviour
{
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
    bool already = false;

    [SerializeField] private float _targetY = 5f;   // Целевая высота по Y
    [SerializeField] private float _moveSpeed = 2f; // Скорость перемещения

    private Vector3 _startPosition;          // Стартовая позиция объекта
    private bool _isMoving = false;      // Флаг, движется ли объект


    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _cameraFollow = FindObjectOfType<CameraFollow>();
        _shootingPlayer = FindObjectOfType<ShootingPlayer>();
        _pauseMenu = FindObjectOfType<PauseMenu>();
        _musicLevelOne = FindObjectOfType<MusicLevelOne>();
        _startPosition = _fadeImage.transform.position; // Сохраняем стартовую позицию

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

    }
    public void HandleBossDead()
    {
        ShowWinPanel();
    }
    public void ShowWinPanel()
    {
        _musicLevelOne.StopMusic();
        _audioSource.PlayOneShot(_deadSound);
        if (_musicLevelOne != null) _musicLevelOne.enabled = false;
        if (_cameraFollow != null) _cameraFollow.enabled = false;
        if (_shootingPlayer != null) _shootingPlayer.enabled = false;
        if (_pauseMenu != null) _pauseMenu.enabled = false;
        deathPanel.SetActive(true);
        StartCoroutine(AnimatePanel());
        Time.timeScale = 0f;
    }
    private IEnumerator AnimatePanel()
    {
        float elapsedTime = 0;
        float duration = 10f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            _deathText.transform.localScale = Vector3.Lerp(Vector3.zero, _initialScale, elapsedTime / duration);
            yield return null;
        }
        _canvasGroup.alpha = 1;
        _deathText.transform.localScale = _initialScale;
        StartCoroutine(FadeInImageAndWin());
    }
    private IEnumerator FadeInImageAndWin()
    {
        MoveUp();
        _fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        float duration = 1f;
        Color startColor = _fadeImage.color;
        Color targetColor = new(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }
        _fadeImage.color = targetColor;
        already = true;
    }
    private void Update()
    {
        if (_isMoving)
        {
            // Вычисляем новую позицию по оси Y
            float newY = Mathf.Lerp(_fadeImage.transform.position.y, _startPosition.y + _targetY, _moveSpeed * Time.deltaTime);

            // Применяем новую позицию
            _fadeImage.transform.position = new Vector3(_fadeImage.transform.position.x, newY, _fadeImage.transform.position.z);

            // Проверяем, достигли ли мы целевой высоты
            if (Mathf.Abs(_fadeImage.transform.position.y - (_startPosition.y + _targetY)) < 0.01f)
            {
                _isMoving = false;  // Останавливаем движение
                _fadeImage.transform.position = new Vector3(_fadeImage.transform.position.x, _startPosition.y + _targetY, _fadeImage.transform.position.z); // Достигли, устанавливаем точную позицию
            }
        }
        if (Input.GetKeyDown(KeyCode.F)&&already==true)
        {
            WinLevel();
        }
    }
    public void WinLevel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (_musicLevelOne != null) _musicLevelOne.enabled = true;
        if (_cameraFollow != null) _cameraFollow.enabled = true;
        if (_shootingPlayer != null) _shootingPlayer.enabled = true;
        if (_pauseMenu != null) _pauseMenu.enabled = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void MoveUp()
    {
        _startPosition = _fadeImage.transform.position; // Обновляем стартовую позицию
        _isMoving = true;
    }
}
