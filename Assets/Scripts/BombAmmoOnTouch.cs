using UnityEngine;
public class BombAmmoOnTouch : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player"; // ��� ������
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������������ ��������� � ������
        if (other.CompareTag(_playerTag))
        {

            // ���� ������
            GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
            if (player != null)
            {

                // �������� ��������� PlayerController � ������ (��������������, ��� � ������ ���� ����� ���������)
                ShootingPlayer playerController = player.GetComponent<ShootingPlayer>();
                if (playerController != null)
                {
                    GiveBombAmmo(playerController);
                    Destroy(gameObject); // ���������� ������
                }
            }
        }
    }
    private void GiveBombAmmo(ShootingPlayer playerController)
    {
        playerController.AddBombAmmo();
    }
}
