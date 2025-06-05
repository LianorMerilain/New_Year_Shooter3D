using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DeerKiller : MonoBehaviour
{

    [SerializeField] private string _killTag2 = "Player";     // Тег объектов, которые нужно уничтожить
    [SerializeField] private float _damageInterval = 1f;    // Интервал нанесения урона в секундах

    private Dictionary<GameObject, float> _lastDamageTime = new Dictionary<GameObject, float>(); // Время последнего нанесения урона
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null)
        {
            Debug.LogError("Collider component not found!");
            enabled = false;
        }
    }
  

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(_killTag2))
        {
            ApplyDamage(other.gameObject, _killTag2);
        }
    }


    // Метод для нанесения урона объекту.
    private void ApplyDamage(GameObject obj, string tag)
    {
        float currentTime = Time.time;

        if (!_lastDamageTime.ContainsKey(obj) || currentTime - _lastDamageTime[obj] >= _damageInterval)
        {
        if (tag == _killTag2)
            {
                HealthPlayer healthPlayer = obj.GetComponent<HealthPlayer>();
                if (healthPlayer != null)
                {
                    healthPlayer.TakeDamage(100);
                    _lastDamageTime[obj] = currentTime;
                }
                else
                {
                    Debug.LogWarning($"У объекта {obj.name} с тегом {tag} не найден компонент HealthPlayer");
                }
            }
        }

    }

    // Очищаем словарь при выходе объекта
    private void OnTriggerExit(Collider other)
    {
        if (_lastDamageTime.ContainsKey(other.gameObject))
        {
            _lastDamageTime.Remove(other.gameObject);
        }
    }
}