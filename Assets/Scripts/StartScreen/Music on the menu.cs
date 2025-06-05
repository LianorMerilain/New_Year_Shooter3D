using UnityEngine;
public class Musiconthemenu : MonoBehaviour
{
    public AudioClip musicClip;      // Перетащите сюда Audio Clip из Inspector
    [Range(0f, 1f)]
    public float musicVolume = 0.5f; // Регулировка громкости
    public bool loopMusic = true;   // Зацикливание музыки
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = musicClip;
        audioSource.volume = musicVolume;
        audioSource.loop = loopMusic;
        PlayMusic();

    }

    // Метод для воспроизведения музыки
    public void PlayMusic()
    {
        if (musicClip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Музыка не была назначена!");
        }

    }

    // Метод для остановки музыки
    public void StopMusic()
    {
        audioSource.Stop();
    }
    // Метод для изменения громкости
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
    // Метод для изменения музыки
    public void ChangeClip(AudioClip clip)
    {
        audioSource.clip = clip;
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