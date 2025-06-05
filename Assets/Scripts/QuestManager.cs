using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private bool _questActive = false; // ������� ���������� ��� ��������� ������
    [SerializeField] private List<string> _questObjectives; // ������ ����� ������
    public int _currentObjectiveIndex = 0; // ������� ������ ����

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI _objectiveText; // ������ �� TextMeshPro ��� ������ ����
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _questFinished;
    [SerializeField] private HealthPlayer _healthPlayer;
    void Start()
    {
        if (_canvas != null)
            _canvas.SetActive(false);

        if (_questFinished != null)
            _questFinished.SetActive(false);
    }
    void Update()
    {
        if (_questActive)
        {
            if (_canvas != null)
                _canvas.SetActive(true);
            UpdateObjectiveText();
        }
        else
        {
            if (_canvas != null)
                _canvas.SetActive(false);
        }
    }
    // ����� ��� ������� ������
    public void StartQuest()
    {
        if (_questObjectives.Count == 0)
        {
            Debug.LogWarning("��� ����� ������");
            return;
        }
        _questActive = true;
        _currentObjectiveIndex = 0;
        UpdateObjectiveText();
    }

    // ����� ��� ���������� �������������� �������
    public void CompleteCurrentObjective()
    {
        _currentObjectiveIndex++; // ��������� � ��������� ����
        if (_currentObjectiveIndex >= _questObjectives.Count)
        {
            FinishQuest();
            return;
        }
        UpdateObjectiveText();
    }
    public void CompleteCurrentObjectiveOne()
    {
        _currentObjectiveIndex=2; // ��������� � ��������� ����
        if (_currentObjectiveIndex >= _questObjectives.Count)
        {
            FinishQuest();
            return;
        }
        UpdateObjectiveText();
    }
    // ����� ��� ���������� ������
    private void FinishQuest()
    {
        _questActive = false;
        if (_canvas != null)
            _canvas.SetActive(false);
        if (_questFinished != null)
            _questFinished.SetActive(true);
        _healthPlayer.MaxHealth = 200;
        _healthPlayer.Heal(200);
        _healthPlayer.UpdateHealthText();
        Debug.Log("����� ��������!");
    }

    // ����� ��� ���������� ������ ������� ����
    private void UpdateObjectiveText()
    {
        if (_objectiveText != null && _currentObjectiveIndex < _questObjectives.Count)
        {
            _objectiveText.text = _questObjectives[_currentObjectiveIndex];
        }
    }
    // ��� ������ ��������, ����� ��������� �������
    public void SetQuestActive(bool value)
    {
        _questActive = value;
    }
}
