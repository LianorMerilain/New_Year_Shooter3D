using UnityEngine;
using System.Collections;

public class PlaySoundOnDestroy : MonoBehaviour
{
    [SerializeField] private AudioClip _destructionSound;
    [SerializeField] private float _delay = 0.5f; // «адержка перед проигрыванием звука в секундах
    private AudioSource _audioSource;
    private bool _hasPlayed = false;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (_destructionSound == null)
        {
            Debug.LogWarning("не назначен звук разрушени€!");
        }
    }
    private void Start()
    {
        StartCoroutine(WaitForDestroyAndPlaySound());
    }
    private IEnumerator WaitForDestroyAndPlaySound()
    {
        while (gameObject != null && !_hasPlayed)
        {
            yield return null;
        }
        yield return new WaitForSeconds(_delay); // ∆дем указанное врем€

        if (_audioSource != null && _destructionSound != null)
        {
            _audioSource.PlayOneShot(_destructionSound);
            _hasPlayed = true; // „тобы звук не проигралс€ несколько раз, когда объект был уничтожен несколько раз
        }
        else
        {
            Debug.LogError("не найден AudioSource или не назначен звук разрушени€!");
        }
    }
}