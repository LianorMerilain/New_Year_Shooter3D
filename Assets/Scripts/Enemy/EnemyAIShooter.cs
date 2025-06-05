using UnityEngine;
using UnityEngine.AI;

public class EnemyAIShooter : MonoBehaviour
{
    [SerializeField] private Transform _player; //префаб игрока -  теперь сериализованное поле
    [SerializeField] private GameObject projectilePrefab; //префаб снаряда - сериализованное поле
    [SerializeField] private Transform firePoint; // точки, из которой враг будет стрелять - сериализованное поле

    [SerializeField] private float enragedTime = 4f; // Время "взбешенности"
    [SerializeField] private float projectileSpeed = 20f; // Скорость снаряда

    [SerializeField] private float fireRate = 1f; // Частота стрельбы

    [SerializeField] private float range = 15f; // Радиус, на котором враг идёт на игрока и стреляет - сериализованное поле
    [SerializeField] private float fleeDistance = 5f; // Радиус, на котором враг начинает убегать - сериализованное поле
    [SerializeField] private float attackRadius = 7f; // Радиус, на котором враг стоит и стреляет - сериализованное поле
    [SerializeField] private float fleeSpeedMultiplier = 2f; // Умножитель скорости при бегстве - сериализованное поле

    private NavMeshAgent _agent; // используется для навигации врага по NavMesh - убрано подчеркивание
    private HealthEnemy _healthEnemy;

    private float _enragedTimer; // Таймер для состояния "взбешен"

    private bool _isEnraged = false; // Флаг, указывающий, что враг взбешен

    private float nextFireTime; // Время следующего выстрела


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _healthEnemy = GetComponent<HealthEnemy>();
        _healthEnemy.OnHealthChangedEnemy += HandleHealthChanged;
        _agent.updateRotation = false; //убираю встроеное вращение 
    }

    private void Update()
    {
        if (_player == null || firePoint == null || projectilePrefab == null || _agent == null || _healthEnemy == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // Управление состоянием "взбешенности"
        if (_isEnraged)
        {
            _enragedTimer -= Time.deltaTime;
            if (_enragedTimer <= 0)
            {
                _isEnraged = false;
            }
        }

        // Выбор поведения в зависимости от расстояния до игрока
        if (_isEnraged || distanceToPlayer <= range)
        {
            
            
            Attack();
            // Вычисляем направление к игроку
            Vector3 direction = _player.position - transform.position;
            direction.y = 0f; // Убираем вращение по оси Y (если нужно)
                              // Поворачиваем противника в сторону игрока
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 5f это скорость поворота
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
            _agent.SetDestination(transform.position); // Останавливаемся
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
        _agent.SetDestination(_player.position); // Преследуем игрока
    }

    private void Flee()
    {
        Vector3 directionToPlayer = transform.position - _player.position;
        _agent.SetDestination(transform.position + directionToPlayer.normalized * fleeDistance);
        _agent.speed = _agent.speed * fleeSpeedMultiplier; // Увеличиваем скорость при бегстве
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