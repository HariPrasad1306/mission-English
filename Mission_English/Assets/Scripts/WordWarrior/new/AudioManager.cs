using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource; // Reference to AudioSource component
    public AudioClip selectSound;  // Clip for selection sound
    public AudioClip correctSound; // Clip for correct sound
    public AudioClip incorrectSound; // Clip for incorrect sound

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keeps the AudioManager across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance of AudioManager
        }

        // Ensure that the AudioSource is attached
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component is missing on AudioManager!");
            }
        }
    }

    public void PlaySelectSound()
    {
        if (audioSource == null || selectSound == null)
        {
            Debug.LogWarning("AudioSource or SelectSound is missing.");
            return;
        }

        if (audioSource.isPlaying) return; // Prevent overlapping sounds

        audioSource.PlayOneShot(selectSound);
    }

    public void PlayCorrectSound()
    {
        if (correctSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            Debug.LogWarning("Correct sound or AudioSource is missing.");
        }
    }

    // Similar methods for PlayIncorrectSound, etc.
}
