using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance = 3f;   // Дистанция взаимодействия
    [SerializeField] private float _interactionAngle = 45f;    // Угол обзора
    [SerializeField] private KeyCode _interactionKey = KeyCode.F;  // Клавиша взаимодействия
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    [SerializeField] private GameObject _textPressF;

    private Transform _playerTransform;  // Трансформ игрока
    private Camera _mainCamera;          // Главная камера

    public delegate void InteractionAction();   // Делегат для функции
    private bool active = true;
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        // Проверка всех условий для взаимодействия
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
        // Вычисляем расстояние между игроком и объектом.
        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        return distance <= _interactionDistance;  // Проверка дистанции
    }

    private bool CheckViewAngle()
    {
        // Вычисляем направление от камеры до объекта
        Vector3 directionToTarget = transform.position - _mainCamera.transform.position;
        // Вычисляем угол между взглядом камеры и направлением на объект
        float angle = Vector3.Angle(_mainCamera.transform.forward, directionToTarget);
        return angle <= _interactionAngle;  // Проверка угла
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