using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaverLoad : MonoBehaviour
{
    public Button LoadButton;
    public Button SaveButton;
    private SaveManager2 _SaveManager2;
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        SaveManager2 _SaveManager2 = GetComponent<SaveManager2>();
    }
    private void Awake()
    {
        LoadButton.onClick.AddListener(LoadGame);
        SaveButton.onClick.AddListener(SaveGame);
    }
    private void SaveGame()
    {
        _SaveManager2.SaveGame();
    }
    private void LoadGame()
    {
        _SaveManager2.LoadGame();
    }
}
