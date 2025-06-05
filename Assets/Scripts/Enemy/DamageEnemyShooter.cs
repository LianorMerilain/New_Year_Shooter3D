using UnityEngine;

public class DamageEnemyShooter : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;       // ����
    [SerializeField] private float _lifespan = 1f;       // ����� �����
    [SerializeField] private string _playerTag = "Player"; // ��� ������
    [SerializeField] private string _groundTag = "Ground"; // ��� �����
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
        //�������� �� null �� �����, ��� ��� ����������� ���
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
        Destroy(gameObject); // ���������� ������ ����� ���������
    }
}