using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public float Sensitivity = 2.0f; // ���������������� ����
    public float MaxYAngle = 80.0f; // ������������ ���� �������� �� ���������

    private float _rotationX = 0.0f;
    private void Update()
    {
        // ���������, ���� �� � PlayerPrefs �������� ����������������, ���� ����, �� �������� ���.
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            Sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        // �������� ���� �� ����
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ������� ��������� � �������������� ���������
        transform.parent.Rotate(Vector3.up * mouseX * Sensitivity);

        // ������� ������ � ������������ ���������
        _rotationX -= mouseY * Sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -MaxYAngle, MaxYAngle);
        transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
    }
}

