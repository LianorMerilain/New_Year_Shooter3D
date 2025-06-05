using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private bool _isEnraged;            // ���� "������������"
    [SerializeField] private float _enragedTime = 2f;     // ����� "������������"
    [SerializeField] private float _damage = 10f;         // ����
    [SerializeField] private float _speed = 3.5f;        // ��������
    [SerializeField] private float _chaseRadius = 10f;    // ������ �������������
    [SerializeField] private float _stopRadius = 1.12f;   // ������ ���������
    [SerializeField] private Transform _player;          // ����� (������ �� Transform)

    private NavMeshAgent _agent;                         // ����� ���������
    private HealthEnemy _healthEnemy;                     // �������� �����
    private float _enragedTimer;                          // ������ "������������"


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _healthEnemy = GetComponent<HealthEnemy>();
        _healthEnemy.OnHealthChangedEnemy += HandleHealthChanged;

        _agent.updateRotation = false; //������ ��������� �������� 
    }

    private void Update()
    {
        

        if (_player == null || _healthEnemy == null) return;
        
        float distance = Vector3.Distance(transform.position, _player.position);
        // ���������� ���������� "������������"
        if (_isEnraged)
        {
            _enragedTimer -= Time.deltaTime;
            if (_enragedTimer <= 0)
            {
                _isEnraged = false;
            }
        }
        // ����� ��������� � ����������� �� ���������� � ���������
        if (_isEnraged || distance <= _chaseRadius)
        {
            // ��������� ����������� � ������
            Vector3 direction = _player.position - transform.position;
            direction.y = 0f; // ������� �������� �� ��� Y (���� �����)
            if (direction != Vector3.zero)
              {
                // ������������ ���������� � ������� ������
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 5f ��� �������� ��������
             }
            if (distance <= _stopRadius)
             {
                Attack();
             }
            else
            {
                Chase();
            }
        }
        else
        {
            Idle();
        }
    }
    private void Attack()
    {
        // ��������� ����� ������
        HealthPlayer health = _player.GetComponent<HealthPlayer>();
        if (health != null)
        {
            health.TakeDamage(_damage);
        }
        _agent.SetDestination(transform.position); // ���������������
    }
    private void Chase()
    {
        _agent.SetDestination(_player.position); // ���������� ������
    }
    private void Idle()
    {
        _agent.SetDestination(transform.position); // ���������������
    }
    private void HandleHealthChanged(float newHealth)
    {
        if (newHealth < _healthEnemy.MaxHealthEnemy)
        {
            _isEnraged = true;
            _enragedTimer = _enragedTime;
        }
    }
    private void OnDisable()
    {
        if (_healthEnemy != null)
        {
            _healthEnemy.OnHealthChangedEnemy -= HandleHealthChanged;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _stopRadius);
    }
}