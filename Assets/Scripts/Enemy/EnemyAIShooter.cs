using UnityEngine;
using UnityEngine.AI;

public class EnemyAIShooter : MonoBehaviour
{
    [SerializeField] private Transform _player; //������ ������ -  ������ ��������������� ����
    [SerializeField] private GameObject projectilePrefab; //������ ������� - ��������������� ����
    [SerializeField] private Transform firePoint; // �����, �� ������� ���� ����� �������� - ��������������� ����

    [SerializeField] private float enragedTime = 4f; // ����� "������������"
    [SerializeField] private float projectileSpeed = 20f; // �������� �������

    [SerializeField] private float fireRate = 1f; // ������� ��������

    [SerializeField] private float range = 15f; // ������, �� ������� ���� ��� �� ������ � �������� - ��������������� ����
    [SerializeField] private float fleeDistance = 5f; // ������, �� ������� ���� �������� ������� - ��������������� ����
    [SerializeField] private float attackRadius = 7f; // ������, �� ������� ���� ����� � �������� - ��������������� ����
    [SerializeField] private float fleeSpeedMultiplier = 2f; // ���������� �������� ��� ������� - ��������������� ����

    private NavMeshAgent _agent; // ������������ ��� ��������� ����� �� NavMesh - ������ �������������
    private HealthEnemy _healthEnemy;

    private float _enragedTimer; // ������ ��� ��������� "�������"

    private bool _isEnraged = false; // ����, �����������, ��� ���� �������

    private float nextFireTime; // ����� ���������� ��������


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _healthEnemy = GetComponent<HealthEnemy>();
        _healthEnemy.OnHealthChangedEnemy += HandleHealthChanged;
        _agent.updateRotation = false; //������ ��������� �������� 
    }

    private void Update()
    {
        if (_player == null || firePoint == null || projectilePrefab == null || _agent == null || _healthEnemy == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // ���������� ���������� "������������"
        if (_isEnraged)
        {
            _enragedTimer -= Time.deltaTime;
            if (_enragedTimer <= 0)
            {
                _isEnraged = false;
            }
        }

        // ����� ��������� � ����������� �� ���������� �� ������
        if (_isEnraged || distanceToPlayer <= range)
        {
            
            
            Attack();
            // ��������� ����������� � ������
            Vector3 direction = _player.position - transform.position;
            direction.y = 0f; // ������� �������� �� ��� Y (���� �����)
                              // ������������ ���������� � ������� ������
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 5f ��� �������� ��������
            if (distanceToPlayer <= fleeDistance)
            {
                Flee();
            }
            else if (distanceToPlayer <= attackRadius)
            {
                _agent.SetDestination(transform.position);
            }
            else
            {
                Chase();
            }
        }
        else
        {
            _agent.SetDestination(transform.position); // ���������������
        }
    }

    private void Attack()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void Chase()
    {
        _agent.SetDestination(_player.position); // ���������� ������
    }

    private void Flee()
    {
        Vector3 directionToPlayer = transform.position - _player.position;
        _agent.SetDestination(transform.position + directionToPlayer.normalized * fleeDistance);
        _agent.speed = _agent.speed * fleeSpeedMultiplier; // ����������� �������� ��� �������
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = (_player.position - firePoint.position).normalized * projectileSpeed;
        }
    }

    private void HandleHealthChanged(float newHealth)
    {
        if (newHealth < _healthEnemy.MaxHealthEnemy)
        {
            _isEnraged = true;
            _enragedTimer = enragedTime;
        }
    }

    private void OnDisable()
    {
        if (_healthEnemy != null) _healthEnemy.OnHealthChangedEnemy -= HandleHealthChanged;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fleeDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}