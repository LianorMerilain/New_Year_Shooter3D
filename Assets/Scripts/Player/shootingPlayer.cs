using System.Collections;
using UnityEngine;
using TMPro;

public class ShootingPlayer : MonoBehaviour
{
    [Header("Snowball Settings")]
    [Tooltip("Префаб первого снаряда")] [SerializeField] private GameObject _firstShell;
    [Tooltip("Префаб второго снаряда")] [SerializeField] private GameObject _secondShell;
    [Tooltip("Сила броска")] [SerializeField] private float _throwingPower = 15f;
    [Tooltip("Точка вылета снаряда")] [SerializeField] private Transform _firePoint;
    [Tooltip("Префаб руки")] [SerializeField] private Transform _hand;
    [Tooltip("Вращение руки")] [SerializeField] private Vector3 _handRotation = new Vector3(7f, 4f, 1f);
    [Tooltip("Скорость вращения руки")] [SerializeField] private float _handRotationSpeed = 10f;
    [Tooltip("Время за которое вращается рука")] [SerializeField] private float _handReloadTime = 0.5f;
    [Tooltip("Выбранный снаряд")] [SerializeField] private int _selectedSnowball = 0;
    private bool _isHandReloading;
    private Quaternion _initialHandRotation;

    [Header("Bomb Settings")]
    [Tooltip("Префаб бомбы")] [SerializeField] private GameObject _bombPrefab;
    [Tooltip("Максимальное колличество бомб")] [SerializeField] private int _maxBombs = 30;
    [Tooltip("Сила броска бомбы")] [SerializeField] private float _bombLaunchForce = 10f;
    [Tooltip("Текущее колличество бомб")] [SerializeField] private int _currentBombs;

    [SerializeField] private TextMeshProUGUI _bombText;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _snowballThrowSound; // Звук броска снежка
    [SerializeField] private AudioClip _bombThrowSound; // Звук броска бомбы
    public int CurrentBombs => _currentBombs;
    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _initialHandRotation = _hand.localRotation;
        UpdateBombText();
    }

    private void Update()
    {
        if (!PauseMenu.GameIsPaused) // Проверка, приостановлена ли игра
        {
            // Логика для снежков
            if (Input.GetButtonDown("Fire1") && !_isHandReloading)
            {
                StartCoroutine(ShootAndReloadSnowball());
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _selectedSnowball = (_selectedSnowball == 0) ? 1 : 0;
            }
            // Логика для бомб
            if (Input.GetMouseButtonDown(1) && _currentBombs > 0 && !_isHandReloading)
            {
                StartCoroutine(ShootAndReloadlBomb());
            }
        }
       
    }
    // Логика для снежков
    private IEnumerator ShootAndReloadSnowball()
    {
        _isHandReloading = true;
        StartCoroutine(RotateHand());
        ShootSnowball();
        _audioSource.PlayOneShot(_snowballThrowSound); // Воспроизведение звука
        yield return new WaitForSeconds(_handReloadTime);
        _isHandReloading = false;
    }
    private IEnumerator ShootAndReloadlBomb()
    {
        _isHandReloading = true;
        StartCoroutine(RotateHand());
        LaunchBomb();
        _audioSource.PlayOneShot(_bombThrowSound); // Воспроизведение звука
        yield return new WaitForSeconds(_handReloadTime);
        _isHandReloading = false;
    }
    private IEnumerator RotateHand()
    {
        Quaternion targetRotation = _hand.localRotation * Quaternion.Euler(_handRotation);
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            _hand.localRotation = Quaternion.Slerp(_hand.localRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * _handRotationSpeed;
            yield return null;
        }
        elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            _hand.localRotation = Quaternion.Slerp(_hand.localRotation, _initialHandRotation, elapsedTime);
            elapsedTime += Time.deltaTime * _handRotationSpeed;
            yield return null;
        }
    }
    private void ShootSnowball()
    {
        GameObject snowballPrefab = (_selectedSnowball == 0) ? _firstShell : _secondShell;
        if (snowballPrefab != null && _firePoint != null)
        {
            GameObject snowball = Instantiate(snowballPrefab, _firePoint.position, _firePoint.rotation);
            Rigidbody rb = snowball.GetComponent<Rigidbody>();
            DamagePlayer snowballScript = snowball.GetComponent<DamagePlayer>(); //Добавляем SnowballDamage
            if (rb != null && snowballScript != null)
            {
                rb.velocity = _firePoint.forward * _throwingPower;
                snowballScript.GetDamage();
            }
        }
    }
    // Логика для бомб
    private void LaunchBomb()
    {
        GameObject bomb = Instantiate(_bombPrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody bombRb3D = bomb.GetComponent<Rigidbody>();
        bombRb3D.AddForce(_firePoint.forward * _bombLaunchForce, ForceMode.Impulse);
        _currentBombs--;
        UpdateBombText();
    }
    public void AddBombAmmo()
    {
        if (_currentBombs < _maxBombs)
        {
            _currentBombs++;
            UpdateBombText();
        }
    }
    private void UpdateBombText()
    {
        if (_bombText != null)
        {
            _bombText.text = $"Бомб: {_currentBombs}/{_maxBombs}";
        }
    }
    // Методы для доступа к переменным
    public int GetCurrentBombs()
    {
        return _currentBombs;
    }
    public int GetMaxBombs()
    {
        return _maxBombs;
    }
}