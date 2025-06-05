using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class HealthEnemy : MonoBehaviour
{
    public float MaxHealthEnemy;
    public float CurrentHealthEnemy;
    public bool IsDeadEnemy;
    public UnityAction OnDeathEnemy;
    public UnityAction OnHitEnemy;
    public event Action<float> OnHealthChangedEnemy;
    [SerializeField] private float _deadTime = 2;
    [SerializeField] public Renderer _enemyRenderer;
    [SerializeField] public MonoBehaviour _enemyAI;
    private BoxCollider _boxCollider;
    public Transform targetPoint;
    public Transform _targetObgect;
    public float rotationDuration = 0.5f;
    [SerializeField] protected Color _flashColor;
    protected Material _originalMaterial;
    [SerializeField] private float damageMultiplierWeapon1 = 1f;
    [SerializeField] private float damageMultiplierWeapon2 = 2f;
    public TextMeshPro _textPro;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _DamageSound;
    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _boxCollider = gameObject.GetComponent<BoxCollider>();
        if(_boxCollider == null)
            Debug.Log("Error no BoxCollider script");
        _originalMaterial = _enemyRenderer.material;

        _enemyAI = GetComponent<EnemyAI>() as MonoBehaviour;
        if (_enemyAI == null)
        {
            _enemyAI = GetComponent<EnemyAIShooter>() as MonoBehaviour;
            if (_enemyAI == null)
                Debug.Log("Error no EnemyAI script");
        }
        if (_enemyRenderer == null)
            Debug.Log("Error no Renderer script");

    }
    public void Awake()
    {
        CurrentHealthEnemy = MaxHealthEnemy;
        if (_enemyRenderer == null)
            _enemyRenderer = GetComponentInChildren<Renderer>();
        if (_enemyAI == null)
            _enemyAI = GetComponent<MonoBehaviour>();

        IsDeadEnemy = false;
    }
    private void OnEnable()
    {

    }
    public void TakeDamageEnemy(float damage, int weaponType)
    {
        StartCoroutine(FlashBlue());
        if (IsDeadEnemy) return;

        float damageMultiplier;
        switch (weaponType)
        {
            case 0:
                damageMultiplier = damageMultiplierWeapon1;
                _audioSource.PlayOneShot(_DamageSound);
                break;
            case 1:
                damageMultiplier = damageMultiplierWeapon2;
                _audioSource.PlayOneShot(_DamageSound);
                break;
            default:
                damageMultiplier = 1f;
                break;
        }

        float totalDamage = damage * damageMultiplier; // Вычисляем totalDamage
        CurrentHealthEnemy -= totalDamage; // Используем totalDamage для вычитания урона
        if (CurrentHealthEnemy <= 0)
        {
            CurrentHealthEnemy = 0;
            DieEnemy();
        }
        OnHealthChangedEnemy?.Invoke(CurrentHealthEnemy);
    }
    protected virtual void DieEnemy()
    {
        IsDeadEnemy = true;
        OnDeathEnemy?.Invoke();
        StartCoroutine(SmoothRotateCoroutine());
        StartCoroutine(DelayedDestroy());
        Debug.Log("Die Called");
    }
    private IEnumerator FlashBlue() //Изменение цвета врага 
    {
        if (_enemyRenderer != null && _originalMaterial != null)
        {
            Material tempMat = new Material(_originalMaterial); //Создаем копию исходного материала
            tempMat.color = _flashColor;
            _enemyRenderer.material = tempMat;
            yield return new WaitForSeconds(0.1f); // Ожидание одной секунды.
            _enemyRenderer.material = _originalMaterial; // Возвращаемся к исходному материалу.
            Destroy(tempMat);
            UpdateHealthText();
        }
    }
    private IEnumerator SmoothRotateCoroutine()
    {
        Vector3 direction = targetPoint.position - _targetObgect.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion initialRotation = _targetObgect.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle + 90f); // Вращение относительно нижней точки

        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            _targetObgect.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _targetObgect.rotation = targetRotation; // Установить точное целевое вращение в конце
    }
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f); // Ждём завершения вращения
        Destroy(gameObject);
    }
    public void ReviveEnemy(float health, Vector3 position)
    {
        CurrentHealthEnemy = health;
        IsDeadEnemy = false;
        transform.position = position;
        if (_enemyAI != null)
            _enemyAI.enabled = true;
        if (_enemyRenderer != null)
            _enemyRenderer.enabled = true;
        if (_boxCollider != null)
            _boxCollider.enabled = true;
        OnHealthChangedEnemy?.Invoke(CurrentHealthEnemy);
        StartCoroutine(FlashBlue());
        Debug.Log("ReviveEnemy Called");
    }
    private void UpdateHealthText()
    {
        if (_textPro == null)
            return;
        _textPro.text = CurrentHealthEnemy.ToString();
    }
}