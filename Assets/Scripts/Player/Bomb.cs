using UnityEngine;
using System.Collections.Generic; // Для использования List<string>
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 100f;
    [SerializeField] private float _explosionDamage = 50f;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _fuseTime = 1f;
    [SerializeField] private List<string> _targetTags; // Список тегов, на которые будет реагировать бомба
    private bool _exploded = false;

    private void Start()
    {
        Invoke(nameof(ExplodeByTimer), _fuseTime); // Запускаем взрыв по таймеру
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_exploded && IsValidTarget(collision.gameObject))
        {
            Explode(); // Вызываем взрыв при столкновении с врагом
        }
    }

    private bool IsValidTarget(GameObject targetObject)
    {
        if (_targetTags == null || _targetTags.Count == 0) return false; // Если список тегов пуст, то ничего не возвращаем.
        foreach (string tag in _targetTags)
        {
            if (targetObject.CompareTag(tag)) return true;
        }
        return false;
    }
    private void ExplodeByTimer()
    {
        Explode();
    }
    private void Explode()
    {
        if (_exploded) return;
        _exploded = true;

        // Эффект взрыва
        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, transform.position, transform.rotation);
        }
        // Урон и отбрасывание объектов 3D
        Collider[] colliders3D = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider hit in colliders3D)
        {
            if (IsValidTarget(hit.gameObject))
            {
                HealthPlayer healthPlayer = hit.GetComponent<HealthPlayer>();
                if (healthPlayer != null)
                {
                    healthPlayer.TakeDamage(_explosionDamage / 2);
                }
                HealthEnemy healthEnemy = hit.GetComponent<HealthEnemy>();
                if (healthEnemy != null)
                {
                    healthEnemy.TakeDamageEnemy(_explosionDamage, 0);
                }
                Vector3 direction = (hit.transform.position - transform.position).normalized;
                hit.transform.position += direction * _explosionForce * Time.deltaTime;
            }
        }
        Destroy(gameObject);
    }
}