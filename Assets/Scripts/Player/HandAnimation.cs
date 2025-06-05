using UnityEngine;
using System.Collections;

public class HandAnimation : MonoBehaviour
{
    public Vector3 DownRotation = new Vector3(-30f, 0f, 0f); // Вектор вращения вниз
    public float RotationSpeed = 10f; // Скорость вращения
    public float ReloadTime = 1f; // Время перезарядки
    private Quaternion _initialRotation;
    private bool _isReloading = false;
    void Start()
    {
        _initialRotation = transform.localRotation; // Сохраняем начальное вращение
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isReloading)
        {
            StartCoroutine(RotateAndReload());
        }
    }

    IEnumerator RotateAndReload()
    {
        _isReloading = true;

        // Запускаем корутину для вращения, а затем ожидаем
        yield return StartCoroutine(RotateHand(DownRotation, _initialRotation));

        _isReloading = false;
    }
    IEnumerator RotateHand(Vector3 downRotation, Quaternion initialRotation)
    {
        Quaternion targetRotation = transform.localRotation * Quaternion.Euler(downRotation); // Целевое вращение
        float elapsedTime = 0;

        while (elapsedTime < 1f) // 1 секунда вращения
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * RotationSpeed; // скорость вращения
            yield return null;
        }

        // Выполнить вращение вверх
        elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, elapsedTime);
            elapsedTime += Time.deltaTime * RotationSpeed;
            yield return null;
        }
    }
}