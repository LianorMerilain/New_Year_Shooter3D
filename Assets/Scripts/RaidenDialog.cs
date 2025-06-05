using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaidenDialog : MonoBehaviour
{
    [Header("Dialogue settings")]
    [SerializeField] private float _activationDistance = 3f;    // ��������� ��� ��������� �������
    [SerializeField] private float _activationAngle = 30f; // ���� ��� ��������� �������

    [Header("Main Dialogue")]
    [SerializeField] private List<string> _mainDialogueLines;    // ������ ����� ��������� �������
    private int _currentMainLineIndex = 0;         // ������� ������ ������ ��������� �������

    [Header("First Additional Dialogue")]
    [SerializeField] private bool _canStartFirstDialogue = false; // ���� ��� ��������� ������� ���. �������
    [SerializeField] private List<string> _firstDialogueLines;    // ������ ����� ������� ���. �������
    private int _currentFirstLineIndex = 0;        // ������� ������ ������ ������� ���. �������


    [Header("UI settings")]
    [SerializeField] private GameObject _canvas;          // ������ �� Canvas
    [SerializeField] private TextMeshProUGUI _dialogueText; // ������ �� TextMeshPro
    [SerializeField] private GameObject _textPressF;

    private Transform _playerTransform;        // Transform ������
    private bool _isDialogueActive = false;    // ����, ������� �� ������
    private enum DialogueState { Main, First, Second, None }
    private DialogueState _currentDialogueState = DialogueState.None; // ���� ��� ������ �������� �������
    private bool _startQuest;
    [SerializeField] private RaidenQuest _raidenQuest;
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // ������� ������
        _canvas.SetActive(false); // �������� Canvas
    }
    private void Update()
    {
        // ��������, ���������� �� ������ ����� � ������� �� �� ������
        bool isPlayerClose = CheckProximity();
        bool isPlayerLooking = CheckViewAngle();

        // ���� ������ F, �� ���� �������, ���� ����������� ������
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
        // ��������� ���������� ����� ������� � ��������.
        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        return distance <= _activationDistance; // ���������, ���������� �� ������
    }
    private bool CheckViewAngle()
    {
        // ��������� ����������� �� ������ �� �������
        Vector3 directionToTarget = transform.position - Camera.main.transform.position;
        // ��������� ���� ����� �������� ������ � ������������ �� ������
        float angle = Vector3.Angle(Camera.main.transform.forward, directionToTarget);
        return angle <= _activationAngle; // ���������, ������� �� ����� �� ������
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
                _currentMainLineIndex++;  // ��������� � ��������� ������

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
                _currentFirstLineIndex++; // ��������� � ��������� ������
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
        _canvas.SetActive(false); // �������� Canvas
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
    // ������ ��� ���������� ������� (����� �������� �� ������ ��������)
    public void SetCanStartFirstDialogue(bool value)
    {
        _canStartFirstDialogue = value;
    }
}