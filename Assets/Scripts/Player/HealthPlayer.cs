using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class HealthPlayer : MonoBehaviour
{
    public float MaxHealth = 200f;
    public float CurrentHealth;
    public bool IsDead = false;
    public event Action OnDeath; // �������, ������������� ��� ������
    public event Action<float> OnHealthChanged; // �������, ������������� ��� ��������� ��������
    [SerializeField] private TextMeshProUGUI _healthText; // ��������� ������ �� TextMeshPro
    [SerializeField] private Image _damageFlash; // ������ �� UI Image ��� ����������� ������

    void Start()
    {
        CurrentHealth = MaxHealth;
        UpdateHealthText();
        _damageFlash.color = new Color(_damageFlash.color.r, _damageFlash.color.g, _damageFlash.color.b, 0);
    }

    public void TakeDamage(float damage)
    {

        if (!PauseMenu.GameIsPaused)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
            UpdateHealthText();
            OnHealthChanged?.Invoke(CurrentHealth);
            StartCoroutine(DamageFlashEffect());
        }
    }

    public void Heal(float healAmount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Min(CurrentHealth + healAmount, MaxHealth);
        UpdateHealthText();
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    protected virtual void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

    private IEnumerator DamageFlashEffect()
    {
        _damageFlash.color = new Color(_damageFlash.color.r, _damageFlash.color.g, _damageFlash.color.b, 0.4f);
        yield return new WaitForSeconds(0.2f);
        _damageFlash.color = new Color(_damageFlash.color.r, _damageFlash.color.g, _damageFlash.color.b, 0);
    }

    public void UpdateHealthText()
    {
        if (_healthText != null)
        {
            _healthText.text = $"��������: {CurrentHealth.ToString("F0")}/{MaxHealth}";
        }
    }
}