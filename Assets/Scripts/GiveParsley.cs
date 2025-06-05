using UnityEngine;
public class GiveParsley : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player"; // ��� ������
    [SerializeField] private QuestManager _questManager;
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������������ ��������� � ������
        if (other.CompareTag(_playerTag))
        {
            // ���� ������
            GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
            if (player != null)
            {
                _questManager.CompleteCurrentObjective();
                _dialogueTrigger.SetCanStartFirstDialogue(true);
                Destroy(gameObject); // ���������� ������
            }
        }
    }
}
