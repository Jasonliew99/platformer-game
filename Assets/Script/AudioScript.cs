using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioScript : MonoBehaviour
{
    // Static instance to ensure only one instance of BackgroundMusic exists
    private static AudioScript instance = null;

    // This ensures that the instance is not destroyed between scene loads
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add your AudioClip for BGM in the Unity Editor
    public AudioClip mainMenuBGM;
    public AudioClip tutorialBGM;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // If there's no clip set or the audio source is not playing, play the appropriate BGM
        if (audioSource.clip == null || !audioSource.isPlaying)
        {
            // Check current scene and play appropriate music
            if (SceneManager.GetActiveScene().buildIndex == 0) // Assuming the main menu scene is the first in the build settings
            {
                PlayMainMenuBGM();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1) // Assuming the tutorial scene is the second in the build settings
            {
                PlayTutorialBGM();
            }
        }

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the scene's build index is neither 0 nor 1, stop the BGM
        if (scene.buildIndex != 0 && scene.buildIndex != 1)
        {
            StopBGM();
        }
        else if (!audioSource.isPlaying) // If returning to the first or second scene, and BGM is not playing, start playing it again
        {
            if (scene.buildIndex == 0) // Assuming the main menu scene is the first in the build settings
            {
                PlayMainMenuBGM();
            }
            else if (scene.buildIndex == 1) // Assuming the tutorial scene is the second in the build settings
            {
                PlayTutorialBGM();
            }
        }
    }

    // Play the main menu background music
    private void PlayMainMenuBGM()
    {
        audioSource.clip = mainMenuBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Play the tutorial background music
    private void PlayTutorialBGM()
    {
        audioSource.clip = tutorialBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Stop the background music
    private void StopBGM()
    {
        audioSource.Stop();
    }
}
