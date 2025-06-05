using UnityEngine;
public class HealOnTouch : MonoBehaviour
{
    [SerializeField] private int healAmount = 20; // Количество здоровья, которое будет восстановлено
    [SerializeField] private string playerTag = "Player"; // Тэг игрока
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(playerTag))
        {
            // Получаем компонент Health у игрока (предполагается, что у игрока есть такой компонент)
            HealthPlayer playerHealth = other.GetComponent<HealthPlayer>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount); // Лечим игрока
                Destroy(gameObject); // Уничтожаем объект
            }
            else
            {
                Debug.LogWarning("Игрок не имеет компонента Health");
            }
        }
    }
}