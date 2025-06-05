using System.Collections.Generic;
using UnityEngine;
public class DestructionSound : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToTrack = new List<GameObject>();
    [SerializeField] private AudioClip _destructionSound;
    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (_destructionSound == null)
        {
            Debug.LogWarning("не назначен звук разрушения!");
        }
    }
    private void Update()
    {
        if (_objectsToTrack == null) return;

        for (int i = _objectsToTrack.Count - 1; i >= 0; i--)
        {
            if (_objectsToTrack[i] == null)
            {
                if (_audioSource != null && _destructionSound != null)
                    _audioSource.PlayOneShot(_destructionSound);
                else
                {
                    Debug.LogError("не найден AudioSource или не назначен звук разрушения");
                }

                _objectsToTrack.RemoveAt(i); // Удаляем из списка
            }
        }
    }
}