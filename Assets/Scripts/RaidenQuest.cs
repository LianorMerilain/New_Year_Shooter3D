using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaidenQuest : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private bool _questActive = false; // Булевая переменная для активации квеста
    [SerializeField] private List<string> _questObjectives; // Список целей квеста
   public int _currentObjectiveIndex = 0; // Текущий индекс цели

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI _objectiveText; // Ссылка на TextMeshPro для вывода цели
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _questFinished;
    [SerializeField] private DamagePlayer _damagePlayer;
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
    // Метод для запуска квеста
    public void StartQuest()
    {
        if (_questObjectives.Count == 0)
        {
            Debug.LogWarning("Нет целей квеста");
            return;
        }
        _questActive = true;
        _currentObjectiveIndex = 0;
        UpdateObjectiveText();
    }

    // Метод для выполнения промежуточного задания
    public void CompleteCurrentObjective()
    {
        _currentObjectiveIndex++; // Переходим к следующей цели
        if (_currentObjectiveIndex >= _questObjectives.Count)
        {
            FinishQuest();
            return;
        }
        UpdateObjectiveText();
    }
    public void CompleteCurrentObjectiveOne()
    {
        _currentObjectiveIndex = 2; // Переходим к следующей цели
        if (_currentObjectiveIndex >= _questObjectives.Count)
        {
            FinishQuest();
            return;
        }
        UpdateObjectiveText();
    }
    // Метод для завершения квеста
    private void FinishQuest()
    {
        _questActive = false;
        if (_canvas != null)
            _canvas.SetActive(false);
        if (_questFinished != null)
            _questFinished.SetActive(true);
        _damagePlayer.damage = 20;
        Debug.Log("Квест завершен!");
    }

    // Метод для обновления текста текущей цели
    private void UpdateObjectiveText()
    {
        if (_objectiveText != null && _currentObjectiveIndex < _questObjectives.Count)
        {
            _objectiveText.text = _questObjectives[_currentObjectiveIndex];
        }
    }
    // Для других скриптов, чтобы управлять квестом
    public void SetQuestActive(bool value)
    {
        _questActive = value;
    }
}

