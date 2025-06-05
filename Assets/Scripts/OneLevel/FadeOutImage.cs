/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeOutImage : MonoBehaviour
{
    [SerializeField] private Image _fadeImage; // Ссылка на Image
    [SerializeField] private float _duration = 0.5f; // Длительность анимации

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        _fadeImage.gameObject.SetActive(true);
        // Устанавливаем изображение полностью непрозрачным в начале.
        Color imageColor = _fadeImage.color;
        imageColor.a = 1f;
        _fadeImage.color = imageColor;
        float elapsedTime = 0f;
        Color startColor = _fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            _fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / _duration);
            yield return null;
        }
        _fadeImage.color = targetColor; // Убеждаемся что альфа = 0
        _fadeImage.gameObject.SetActive(false); // Отключаем изображение по окончанию анимации
    }
}*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutImage : MonoBehaviour
{
    [SerializeField] private Image _fadeImage; // Ссылка на Image

    [SerializeField] private float _duration = 0.5f; // Длительность анимации

    private void Start()
    {
        if (_fadeImage == null)
        {
            Debug.LogError("не назначено затемнение!");
            enabled = false;
            return;
        }

        // Устанавливаем изображение полностью непрозрачным в начале.
        Color imageColor = _fadeImage.color;
        imageColor.a = 1f;
        _fadeImage.color = imageColor;
        StartCoroutine(FadeOut());

    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color startColor = _fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            _fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / _duration);
            yield return null;
        }
        _fadeImage.color = targetColor; // Убеждаемся что альфа = 0
        _fadeImage.gameObject.SetActive(false); // Отключаем изображение по окончанию анимации
    }
}