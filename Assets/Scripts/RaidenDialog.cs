using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaidenDialog : MonoBehaviour
{
    [Header("Dialogue settings")]
    [SerializeField] private float _activationDistance = 3f;    // Дистанция для активации диалога
    [SerializeField] private float _activationAngle = 30f; // Угол для активации диалога

    [Header("Main Dialogue")]
    [SerializeField] private List<string> _mainDialogueLines;    // Список строк основного диалога
    private int _currentMainLineIndex = 0;         // Текущий индекс строки основного диалога

    [Header("First Additional Dialogue")]
    [SerializeField] private bool _canStartFirstDialogue = false; // Флаг для активации первого доп. диалога
    [SerializeField] private List<string> _firstDialogueLines;    // Список строк первого доп. диалога
    private int _currentFirstLineIndex = 0;        // Текущий индекс строки первого доп. диалога


    [Header("UI settings")]
    [SerializeField] private GameObject _canvas;          // Ссылка на Canvas
    [SerializeField] private TextMeshProUGUI _dialogueText; // Ссылка на TextMeshPro
    [SerializeField] private GameObject _textPressF;

    private Transform _playerTransform;        // Transform игрока
    private bool _isDialogueActive = false;    // Флаг, активен ли диалог
    private enum DialogueState { Main, First, Second, None }
    private DialogueState _currentDialogueState = DialogueState.None; // Флаг для выбора текущего диалога
    private bool _startQuest;
    [SerializeField] private RaidenQuest _raidenQuest;
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Находим игрока
        _canvas.SetActive(false); // Скрываем Canvas
    }
    private void Update()
    {
        // Проверка, достаточно ли близко игрок и смотрит ли на объект
        bool isPlayerClose = CheckProximity();
        bool isPlayerLooking = CheckViewAngle();

        // Если нажать F, то либо открыть, либо переключить диалог
        if (Input.GetKeyDown(KeyCode.F) && isPlayerClose && isPlayerLooking)
        {
            if (!_isDialogueActive)
            {
                _isDialogueActive = true;
                _canvas.SetActive(true);
                if (_currentDialogueState == DialogueState.None)
                {
                    _currentDialogueState = DialogueState.Main;
                    ShowCurrentDialogueLine();
                }
            }
            else
            {
                NextDialogueLine();
            }
        }
        if (isPlayerClose && isPlayerLooking)
        {
            if (!_isDialogueActive) _textPressF.SetActive(true);
            else _textPressF.SetActive(false);
        }
        else _textPressF.SetActive(false);


    }
    private bool CheckProximity()
    {
        // Вычисляем расстояние между игроком и объектом.
        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        return distance <= _activationDistance; // Проверяем, достаточно ли близко
    }
    private bool CheckViewAngle()
    {
        // Вычисляем направление от камеры до объекта
        Vector3 directionToTarget = transform.position - Camera.main.transform.position;
        // Вычисляем угол между взглядом камеры и направлением на объект
        float angle = Vector3.Angle(Camera.main.transform.forward, directionToTarget);
        return angle <= _activationAngle; // Проверяем, смотрит ли игрок на объект
    }
    private void ShowCurrentDialogueLine()
    {
        if (_dialogueText != null)
        {
            switch (_currentDialogueState)
            {
                case DialogueState.Main:
                    if (_mainDialogueLines.Count > 0)
                    {
                        _dialogueText.text = _mainDialogueLines[_currentMainLineIndex];
                    }
                    break;
                case DialogueState.First:
                    if (_firstDialogueLines.Count > 0)
                    {
                        _dialogueText.text = _firstDialogueLines[_currentFirstLineIndex];
                    }
                    break;
            }
        }
    }
    private void NextDialogueLine()
    {
        switch (_currentDialogueState)
        {
            case DialogueState.Main:
                _currentMainLineIndex++;  // Переходим к следующей строке

                if (_currentMainLineIndex >= _mainDialogueLines.Count)
                {
                    _currentMainLineIndex--;
                    _currentDialogueState = DialogueState.None;
                    if (_canStartFirstDialogue)
                    {
                        _currentDialogueState = DialogueState.First;
                    }
                    if (!_startQuest)
                    {
                        _raidenQuest.StartQuest();
                        _startQuest = true;
                    }
                    EndDialogue();
                    return;
                }
                break;
            case DialogueState.First:
                _currentFirstLineIndex++; // Переходим к следующей строке
                if (_currentFirstLineIndex >= _firstDialogueLines.Count)
                {
                    _currentFirstLineIndex--;
                    _currentDialogueState = DialogueState.None;
                    _raidenQuest.CompleteCurrentObjectiveOne();
                    EndDialogue();
                    return;
                }
                break;
        }
        ShowCurrentDialogueLine();
    }
    private void EndDialogue()
    {
        _isDialogueActive = false;
        _canvas.SetActive(false); // Скрываем Canvas
    }
    private void OnValidate()
    {
        if (_activationDistance <= 0)
        {
            _activationDistance = 0.01f;
        }
        if (_activationAngle <= 0)
        {
            _activationAngle--;
        }
    }
    // Методы для управления флагами (можно вызывать из других скриптов)
    public void SetCanStartFirstDialogue(bool value)
    {
        _canStartFirstDialogue = value;
    }
}