using UnityEngine;
public class HealOnTouch : MonoBehaviour
{
    [SerializeField] private int healAmount = 20; // ���������� ��������, ������� ����� �������������
    [SerializeField] private string playerTag = "Player"; // ��� ������
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(playerTag))
        {
            // �������� ��������� Health � ������ (��������������, ��� � ������ ���� ����� ���������)
            HealthPlayer playerHealth = other.GetComponent<HealthPlayer>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount); // ����� ������
                Destroy(gameObject); // ���������� ������
            }
            else
            {
                Debug.LogWarning("����� �� ����� ���������� Health");
            }
        }
    }
}