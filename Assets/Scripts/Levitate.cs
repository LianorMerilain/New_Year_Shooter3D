using UnityEngine;

public class Levitate : MonoBehaviour
{
    [SerializeField] private float floatHeight = 1f;      // Высота левитации
    [SerializeField] private float floatSpeed = 1f;      // Скорость левитации
    [SerializeField] private float startOffset = 0f;      // Начальное смещение по фазе
    [SerializeField] private float rotationSpeed = 30f; // Скорость вращения объекта в градусах в секунду.

    private Vector3 _startPosition;      // Начальная позиция объекта
    private float _currentFloat;     // Текущая позиция по фазе

    void Start()
    {
        _startPosition = transform.position;
        _currentFloat = startOffset; // Устанавливаем начальное смещение.
    }

    void Update()
    {
        // Рассчитываем текущую позицию по фазе
        _currentFloat += Time.deltaTime * floatSpeed;

        // Расчитываем вертикальное смещение с использованием синуса, для плавности
        float verticalOffset = Mathf.Sin(_currentFloat) * floatHeight;

        // Устанавливаем новую позицию, добавляя вертикальное смещение к начальной позиции
        transform.position = _startPosition + Vector3.up * verticalOffset;

        // Вращение объекта вокруг оси Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}