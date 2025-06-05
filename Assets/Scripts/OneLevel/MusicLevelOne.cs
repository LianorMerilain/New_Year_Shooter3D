using UnityEngine;
using System.Collections.Generic;

public class MusicLevelOne : MonoBehaviour
{
    public List<AudioClip> musicClips = new List<AudioClip>();  // Список Audio Clips
    [Range(0f, 1f)]
    public float musicVolume = 0.5f; // Регулировка громкости
    public bool loopMusic = true;   // Зацикливание музыки
    private AudioSource audioSource;
    private int _currentClipIndex = 0; // Индекс текущего клипа


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = loopMusic;
        if (musicClips.Count > 0)
        {
            ChangeClip(0);
            audioSource.volume = musicVolume;
            PlayMusic();
        }
        else
        {
            Debug.LogWarning("Нет музыкальных клипов");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchMusicClip();
        }
    }

    // Метод для воспроизведения музыки
    public void PlayMusic()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Музыкальный клип не установлен");
        }
    }

    // Метод для остановки музыки
    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    // Метод для изменения громкости
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
    // Метод для изменения музыки
    public void ChangeClip(int index)
    {
        if (index >= 0 && index < musicClips.Count)
        {
            audioSource.clip = musicClips[index];
            _currentClipIndex = index;
        }
        else
        {
            Debug.LogWarning("Неверный индекс музыкального клипа");
        }

    }
    // Метод для переключения музыки
    public void SwitchMusicClip()
    {
        _currentClipIndex++;
        if (_currentClipIndex >= musicClips.Count)
        {
            _currentClipIndex = 0;
        }
        ChangeClip(_currentClipIndex);
        PlayMusic();
    }
    void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
            audioSource.loop = loopMusic;
        }
    }
}