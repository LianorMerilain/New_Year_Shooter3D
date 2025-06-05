using UnityEngine;
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound; // Звук нажатия
    private AudioSource _audioSource;
    private void PlayClickSound()
    {
        if (_audioSource != null && _clickSound != null)
            _audioSource.PlayOneShot(_clickSound); // Проигрываем звук
        else
        {
            Debug.LogError("не найден AudioSource или не назначен звук кнопки");
        }
    }
}