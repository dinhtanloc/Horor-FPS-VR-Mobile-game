using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [Header("Footsteps")]
    public AudioClip[] footstepClips; // Mảng các âm thanh bước chân
    public float footstepInterval = 0.5f; // Khoảng cách giữa các bước chân
    private float footstepTimer;

    [Header("Growl (Tiếng la)")]
    public AudioClip growlClip;
    public float growlInterval = 5f; // Khoảng cách giữa các lần la hét
    private float growlTimer;

    private AudioSource audioSource;
    private CharacterController controller;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        PlayFootsteps();
        PlayGrowl();
    }

    void PlayFootsteps()
    {
        if (controller != null && controller.velocity.magnitude > 0.5f)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                if (footstepClips.Length > 0)
                {
                    AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
                    audioSource.PlayOneShot(clip);
                    footstepTimer = 0f;
                }
            }
        }
    }

    void PlayGrowl()
    {
        growlTimer += Time.deltaTime;
        if (growlTimer >= growlInterval)
        {
            if (growlClip != null)
            {
                audioSource.PlayOneShot(growlClip);
                growlTimer = 0f;
            }
        }
    }
}