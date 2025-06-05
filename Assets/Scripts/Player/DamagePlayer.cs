using UnityEngine;
public class DamagePlayer : MonoBehaviour
{
    public float damage = 10f;          // Урон
    public float GetDamage() { return damage; }
    [SerializeField] private float _lifespawn = 1f;          // Время жизни
    [SerializeField] private int weaponType = 0;          // Тип оружия
    [SerializeField] private string enemyTag = "Enemy";    // Тег врага
    [SerializeField] private string groundTag = "Ground";  // Тег земли

    private void Start()
    {
        Invoke("SelfDestruct", 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            ApplyDamage(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag(groundTag))
        {
            Destroy(gameObject, _lifespawn);
        }
    }

    private void ApplyDamage(GameObject target)
    {
        HealthEnemy healthEnemy = target.GetComponent<HealthEnemy>();
        if (healthEnemy != null)
        {
            healthEnemy.TakeDamageEnemy(damage, weaponType);
        }
        Destroy(gameObject);
    }
}