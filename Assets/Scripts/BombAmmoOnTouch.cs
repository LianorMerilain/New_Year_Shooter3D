using UnityEngine;
public class BombAmmoOnTouch : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player"; // Тэг игрока
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что столкновение произошло с врагом
        if (other.CompareTag(_playerTag))
        {

            // Ищем игрока
            GameObject player = GameObject.FindGameObjectWithTag(_playerTag);
            if (player != null)
            {

                // Получаем компонент PlayerController у игрока (предполагается, что у игрока есть такой компонент)
                ShootingPlayer playerController = player.GetComponent<ShootingPlayer>();
                if (playerController != null)
                {
                    GiveBombAmmo(playerController);
                    Destroy(gameObject); // Уничтожаем объект
                }
            }
        }
    }
    private void GiveBombAmmo(ShootingPlayer playerController)
    {
        playerController.AddBombAmmo();
    }
}
