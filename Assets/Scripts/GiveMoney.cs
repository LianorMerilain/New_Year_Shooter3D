using UnityEngine;

public class GiveMoney : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player"; // ��� ������
    [SerializeField] private RaidenQuest _raidenQuest;
    [SerializeField] private RaidenDialog _dialogueTrigger;
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������������ ��������� � ������
        if (other.CompareTag(_playerTag))
        {
            // ���� ������
            GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
            if (player != null)
            {
                _raidenQuest.CompleteCurrentObjective();
                _dialogueTrigger.SetCanStartFirstDialogue(true);
                Destroy(gameObject); // ���������� ������
            }
        }
    }
}
