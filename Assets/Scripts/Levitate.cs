using UnityEngine;

public class Levitate : MonoBehaviour
{
    [SerializeField] private float floatHeight = 1f;      // ������ ���������
    [SerializeField] private float floatSpeed = 1f;      // �������� ���������
    [SerializeField] private float startOffset = 0f;      // ��������� �������� �� ����
    [SerializeField] private float rotationSpeed = 30f; // �������� �������� ������� � �������� � �������.

    private Vector3 _startPosition;      // ��������� ������� �������
    private float _currentFloat;     // ������� ������� �� ����

    void Start()
    {
        _startPosition = transform.position;
        _currentFloat = startOffset; // ������������� ��������� ��������.
    }

    void Update()
    {
        // ������������ ������� ������� �� ����
        _currentFloat += Time.deltaTime * floatSpeed;

        // ����������� ������������ �������� � �������������� ������, ��� ���������
        float verticalOffset = Mathf.Sin(_currentFloat) * floatHeight;

        // ������������� ����� �������, �������� ������������ �������� � ��������� �������
        transform.position = _startPosition + Vector3.up * verticalOffset;

        // �������� ������� ������ ��� Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}