using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public float Sensitivity = 2.0f; // Чувствительность мыши
    public float MaxYAngle = 80.0f; // Максимальный угол вращения по вертикали

    private float _rotationX = 0.0f;
    private void Update()
    {
        // Проверяем, есть ли в PlayerPrefs значение чувствительности, если есть, то получаем его.
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            Sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        // Получаем ввод от мыши
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Вращаем персонажа в горизонтальной плоскости
        transform.parent.Rotate(Vector3.up * mouseX * Sensitivity);

        // Вращаем камеру в вертикальной плоскости
        _rotationX -= mouseY * Sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -MaxYAngle, MaxYAngle);
        transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
    }
}

