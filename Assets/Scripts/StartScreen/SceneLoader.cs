using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    /*  public int SceneIndex;
      public Button ButtonStart;
      public Button ButtonExit;
      private bool _isFirstLoad = true;

      void Start()
      {
          ButtonStart.onClick.AddListener(LoadNextScene);
          ButtonExit.onClick.AddListener(QuitGame);

      }

      void LoadNextScene()
      {
          SceneManager.LoadScene(SceneIndex);

      }
      private void OnEnable()
      {
          SceneManager.sceneLoaded += OnSceneLoaded;
      }
      private void OnDisable()
      {
          SceneManager.sceneLoaded -= OnSceneLoaded;
      }
      private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
      {
          if (scene.buildIndex == 1 && _isFirstLoad)
          {
              _isFirstLoad = false;
              SaveManager.Instance.CreateInitialSave();  // Убрали индекс сцены
              Debug.Log("Initial save created!");
          }
          else if (scene.buildIndex == 1)
          {
              SaveManager.Instance.LoadGameOnStart();   // Убрали индекс сцены
              Debug.Log("Initial load");
          }

      }
      private void QuitGame()
      {
          Application.Quit();
      }*/
    public string sceneName; // Имя сцены, на которую нужно перейти
    public Button button;    // Кнопка, запускающая переход
    [SerializeField] private Image _fadeImage;
    bool already = false;
    void Start()
    {
        Color imageColor = _fadeImage.color;
        imageColor.a = 0f;
        _fadeImage.color = imageColor;
        _fadeImage.gameObject.SetActive(false);
        if (button == null)
        {
            Debug.LogError("Button not assigned!");
            return;
        }
        button.onClick.AddListener(Active);
    }
    void Active()
    {
        StartCoroutine(FadeInImageAndWin());
    }
    private IEnumerator FadeInImageAndWin()
    {
        _fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        float duration = 1f;
        Color startColor = _fadeImage.color;
        Color targetColor = new(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }
        _fadeImage.color = targetColor;
        already = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && already == true)
        {
            LoadNextScene();
        }
    }
    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}