using UnityEngine;
public class Musiconthemenu : MonoBehaviour
{
    public AudioClip musicClip;      // ���������� ���� Audio Clip �� Inspector
    [Range(0f, 1f)]
    public float musicVolume = 0.5f; // ����������� ���������
    public bool loopMusic = true;   // ������������ ������
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

    // ����� ��� ��������������� ������
    public void PlayMusic()
    {
        if (musicClip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("������ �� ���� ���������!");
        }

    }

    // ����� ��� ��������� ������
    public void StopMusic()
    {
        audioSource.Stop();
    }
    // ����� ��� ��������� ���������
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
    // ����� ��� ��������� ������
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