using UnityEngine;

public class  MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioSource audioSource;
    public float musicLevel;
    public float soundLevel;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        if (audioSource.clip == null || audioSource.clip.name != clip.name)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
