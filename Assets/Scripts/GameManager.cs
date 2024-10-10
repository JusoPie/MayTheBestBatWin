using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject pauseMenuUI; // Assign the pause panel from the Unity Inspector
    public static GameManager instance;
    public AudioSource backgroundMusic;
    public AudioSource soundEffects;
    public Slider volumeSlider;
    public Slider sfxSlider;
    private bool isPaused = false;

    // Start is called before the first frame update

    void Start()
    {
        // Load saved volume levels
        float savedMusicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float savedSfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);

        // Set initial volume levels
        SetMusicVolume(savedMusicVolume);
        SetSfxVolume(savedSfxVolume);

        // Set slider values to match saved volume levels
        if (volumeSlider != null)
        {
            volumeSlider.value = savedMusicVolume;
            volumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        }
    }



// Update is called once per frame
    void Update()
    {
    // Check if the current scene is NOT the menu scene before allowing the Escape key to pause the game
    if (SceneManager.GetActiveScene().name != "MenuScene")
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    }

    public void StartGame() 

    {
        SceneManager.LoadScene("SideView");
    }

    public void QuitGame() { 
    Application.Quit();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // Hide pause menu
        Time.timeScale = 1f;           // Resume game time
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);   // Show pause menu
        Time.timeScale = 0f;           // Freeze game time
        isPaused = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale in case it's paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void SetMusicVolume(float volume)
    {
    if (backgroundMusic != null)
    {
        backgroundMusic.volume = volume;
    }
    PlayerPrefs.SetFloat("musicVolume", volume);
    PlayerPrefs.Save();
    }

    public void SetSfxVolume(float volume)
    {
    if (soundEffects != null)
    {
        soundEffects.volume = volume;
    }
    PlayerPrefs.SetFloat("sfxVolume", volume);
    PlayerPrefs.Save();
    }

    public void BackToMenu()

    {
        SceneManager.LoadScene("MenuScene");
    }

}
