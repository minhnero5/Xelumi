using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Play(string name)
    {
        AudioClip clip = Array.Find(audioClips, sound => sound.name == name);

        if (clip == null)
        {
            Debug.LogWarning("Clip not found: " + name);
            return;
        }

        AudioSource tempAudioSource = gameObject.AddComponent<AudioSource>();

        if (name == "Coin")
            tempAudioSource.volume = .2f;

        tempAudioSource.clip = clip;
        tempAudioSource.Play();
        Destroy(tempAudioSource, clip.length);
    }
}