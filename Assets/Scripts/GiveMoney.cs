using UnityEngine;

public class GiveMoney : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player"; // Тэг игрока
    [SerializeField] private RaidenQuest _raidenQuest;
    [SerializeField] private RaidenDialog _dialogueTrigger;
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что столкновение произошло с врагом
        if (other.CompareTag(_playerTag))
        {
            // Ищем игрока
            GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
            if (player != null)
            {
                _raidenQuest.CompleteCurrentObjective();
                _dialogueTrigger.SetCanStartFirstDialogue(true);
                Destroy(gameObject); // Уничтожаем объект
            }
        }
    }
}
