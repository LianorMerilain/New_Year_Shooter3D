using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private bool _isEnraged;            // Флаг "взбешенности"
    [SerializeField] private float _enragedTime = 2f;     // Время "взбешенности"
    [SerializeField] private float _damage = 10f;         // Урон
    [SerializeField] private float _speed = 3.5f;        // Скорость
    [SerializeField] private float _chaseRadius = 10f;    // Радиус преследования
    [SerializeField] private float _stopRadius = 1.12f;   // Радиус остановки
    [SerializeField] private Transform _player;          // Игрок (ссылка на Transform)

    private NavMeshAgent _agent;                         // Агент навигации
    private HealthEnemy _healthEnemy;                     // Здоровье врага
    private float _enragedTimer;                          // Таймер "взбешенности"


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _healthEnemy = GetComponent<HealthEnemy>();
        _healthEnemy.OnHealthChangedEnemy += HandleHealthChanged;

        _agent.updateRotation = false; //убираю встроеное вращение 
    }

    private void Update()
    {
        

        if (_player == null || _healthEnemy == null) return;
        
        float distance = Vector3.Distance(transform.position, _player.position);
        // Управление состоянием "взбешенности"
        if (_isEnraged)
        {
            _enragedTimer -= Time.deltaTime;
            if (_enragedTimer <= 0)
            {
                _isEnraged = false;
            }
        }
        // Выбор поведения в зависимости от расстояния и состояния
        if (_isEnraged || distance <= _chaseRadius)
        {
            // Вычисляем направление к игроку
            Vector3 direction = _player.position - transform.position;
            direction.y = 0f; // Убираем вращение по оси Y (если нужно)
            if (direction != Vector3.zero)
              {
                // Поворачиваем противника в сторону игрока
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 5f это скорость поворота
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
        // Нанесение урона игроку
        HealthPlayer health = _player.GetComponent<HealthPlayer>();
        if (health != null)
        {
            health.TakeDamage(_damage);
        }
        _agent.SetDestination(transform.position); // Останавливаемся
    }
    private void Chase()
    {
        _agent.SetDestination(_player.position); // Преследуем игрока
    }
    private void Idle()
    {
        _agent.SetDestination(transform.position); // Останавливаемся
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