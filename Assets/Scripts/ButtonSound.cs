using UnityEngine;
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound; // ���� �������
    private AudioSource _audioSource;
    private void PlayClickSound()
    {
        if (_audioSource != null && _clickSound != null)
            _audioSource.PlayOneShot(_clickSound); // ����������� ����
        else
        {
            Debug.LogError("�� ������ AudioSource ��� �� �������� ���� ������");
        }
    }
}