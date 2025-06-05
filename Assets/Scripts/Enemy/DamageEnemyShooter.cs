using UnityEngine;

public class DamageEnemyShooter : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;       // Урон
    [SerializeField] private float _lifespan = 1f;       // Время жизни
    [SerializeField] private string _playerTag = "Player"; // Тег игрока
    [SerializeField] private string _groundTag = "Ground"; // Тег земли
    private void Start()
    {
        Invoke("SelfDestruct", 10f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_playerTag))
        {
            ApplyDamage(collision.gameObject);
        }
        //Проверка на null не нужна, так как проверяется тег
        if (collision.gameObject.CompareTag(_groundTag))
        {
            Destroy(gameObject, _lifespan);
        }
    }
    private void ApplyDamage(GameObject target)
    {
        HealthPlayer health = target.GetComponent<HealthPlayer>();
        if (health != null)
        {
            health.TakeDamage(_damage);
        }
        Destroy(gameObject); // Уничтожаем снежок после попадания
    }
}