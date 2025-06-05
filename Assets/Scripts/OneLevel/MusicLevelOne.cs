using UnityEngine;
using System.Collections.Generic;

public class MusicLevelOne : MonoBehaviour
{
    public List<AudioClip> musicClips = new List<AudioClip>();  // ������ Audio Clips
    [Range(0f, 1f)]
    public float musicVolume = 0.5f; // ����������� ���������
    public bool loopMusic = true;   // ������������ ������
    private AudioSource audioSource;
    private int _currentClipIndex = 0; // ������ �������� �����


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
            Debug.LogWarning("��� ����������� ������");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchMusicClip();
        }
    }

    // ����� ��� ��������������� ������
    public void PlayMusic()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("����������� ���� �� ����������");
        }
    }

    // ����� ��� ��������� ������
    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    // ����� ��� ��������� ���������
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
    // ����� ��� ��������� ������
    public void ChangeClip(int index)
    {
        if (index >= 0 && index < musicClips.Count)
        {
            audioSource.clip = musicClips[index];
            _currentClipIndex = index;
        }
        else
        {
            Debug.LogWarning("�������� ������ ������������ �����");
        }

    }
    // ����� ��� ������������ ������
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