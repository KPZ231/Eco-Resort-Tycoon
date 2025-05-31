using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds; // Array of sounds to manage

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void PlaySound(string soundName)
    {
        foreach(Sound sound in sounds)
        {
            if (sound.soundName == soundName)
            {
                if (sound.source != null)
                {
                    sound.source.clip = sound.clip;
                    sound.source.volume = sound.volume;
                    sound.source.pitch = sound.pitch;
                    sound.source.Play();
                }
                else
                {
                    Debug.LogWarning($"Sound source for {soundName} is not assigned.");
                }
                return; // Exit after playing the first matching sound
            }
        }
    }
}
