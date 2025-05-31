using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
    public string soundName; // Name of the sound   
    public AudioClip clip; // The audio clip to play
    public AudioMixerGroup mixerGroup; // The audio mixer group to use
    public AudioSource source; // Optional audio source to play the sound on
    [Range(0f, 1f)] public float volume = 1f; // Volume of the sound
    [Range(0.3f, 3f)] public float pitch = 1f; // Pitch of the sound
}
