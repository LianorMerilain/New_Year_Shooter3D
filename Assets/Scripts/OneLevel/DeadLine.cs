using UnityEngine.SceneManagement;
using UnityEngine;
public class DeadLine : MonoBehaviour
{
    public int Dead = -10;
    public GameObject Player;
    DeadWindow deadWindow;
    private void Start()
    {
        deadWindow = FindObjectOfType<DeadWindow>();
    }
    private void Update()
    {
        if(Player.transform.position.y < Dead)
        {
            /* SaveManager.Instance.ResetLevel();*/
            deadWindow.RestartLevel();
        }
    }
}
