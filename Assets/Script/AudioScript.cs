using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    private AudioSource bgmAudioSource;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        bgmAudioSource = GetComponent<AudioSource>();

        // Ensure the audio source is set to loop
        bgmAudioSource.loop = true;

        // Start playing the background music
        bgmAudioSource.Play();
    }
}
