using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance = 3f;   // ��������� ��������������
    [SerializeField] private float _interactionAngle = 45f;    // ���� ������
    [SerializeField] private KeyCode _interactionKey = KeyCode.F;  // ������� ��������������
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    [SerializeField] private GameObject _textPressF;

    private Transform _playerTransform;  // ��������� ������
    private Camera _mainCamera;          // ������� ������

    public delegate void InteractionAction();   // ������� ��� �������
    private bool active = true;
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        // �������� ���� ������� ��� ��������������
        if (CheckProximity() && CheckViewAngle() && Input.GetKeyDown(_interactionKey) && active == true && _dialogueTrigger._currentFirstLineIndex >= 5)
        {
            _dialogueTrigger.SetCanStartSecondDialogue(true);
            active = false;
        }
        if (CheckProximity() && CheckViewAngle() && active ==true && _dialogueTrigger._currentFirstLineIndex >= 5)
        {
           _textPressF.SetActive(true);
        }
        else _textPressF.SetActive(false);
    }

    private bool CheckProximity()
    {
        // ��������� ���������� ����� ������� � ��������.
        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        return distance <= _interactionDistance;  // �������� ���������
    }

    private bool CheckViewAngle()
    {
        // ��������� ����������� �� ������ �� �������
        Vector3 directionToTarget = transform.position - _mainCamera.transform.position;
        // ��������� ���� ����� �������� ������ � ������������ �� ������
        float angle = Vector3.Angle(_mainCamera.transform.forward, directionToTarget);
        return angle <= _interactionAngle;  // �������� ����
    }
    private void OnValidate()
    {
        if (_interactionDistance <= 0)
        {
            _interactionDistance = 0.01f;
        }
        if (_interactionAngle <= 0)
        {
            _interactionAngle--;
        }
    }
}