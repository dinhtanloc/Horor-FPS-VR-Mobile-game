using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] playlist;  // Danh sách nhạc nền
    private AudioSource audioSource;
    private int currentTrack = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayNext();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNext();
        }
    }

    void PlayNext()
    {
        if (playlist.Length == 0) return;

        audioSource.clip = playlist[currentTrack];
        audioSource.Play();

        currentTrack = (currentTrack + 1) % playlist.Length; // lặp lại từ đầu sau khi hết
    }
}
