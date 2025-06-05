using UnityEngine;
public class ActivatorWINNER : MonoBehaviour
{
    public WinWindows _winWindows;
    public HealthEnemy _healthEnemy;
    private bool _oneACTIVE = true;// Çהמנמגüו גנאדא
    private void Start()
    {
        _winWindows = FindObjectOfType<WinWindows>();
    }
    private void Update()
    {
        if (_healthEnemy.CurrentHealthEnemy <= 0 && _oneACTIVE == true)
        { 
            _winWindows.HandleBossDead();
            _oneACTIVE = false;
        }
    }
}
