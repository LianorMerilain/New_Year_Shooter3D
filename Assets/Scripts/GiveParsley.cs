using UnityEngine;
public class GiveParsley : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player"; // Тэг игрока
    [SerializeField] private QuestManager _questManager;
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что столкновение произошло с врагом
        if (other.CompareTag(_playerTag))
        {
            // Ищем игрока
            GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
            if (player != null)
            {
                _questManager.CompleteCurrentObjective();
                _dialogueTrigger.SetCanStartFirstDialogue(true);
                Destroy(gameObject); // Уничтожаем объект
            }
        }
    }
}
