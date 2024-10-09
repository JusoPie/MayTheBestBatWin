using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject pauseMenuUI; // Assign the pause panel from the Unity Inspector
    private bool isPaused = false;
    public static GameManager instance;
    public AudioSource backgroundMusic;
    public Slider volumeSlider;

    // Start is called before the first frame update

    void Awake()
    {

    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        SetVolume(savedVolume);

        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (Slider slider in sliders)
        {
            slider.value = savedVolume; // Set each slider to the saved volume
            slider.onValueChanged.AddListener(SetVolume); // Ensure each slider updates volume
        }

        if (backgroundMusic != null && volumeSlider != null)
        {
            volumeSlider.value = backgroundMusic.volume; // Match slider to current volume
            volumeSlider.onValueChanged.AddListener(SetVolume); // Add listener to handle slider changes
        }

    }

    // Update is called once per frame
    void Update()
    {
        {
            // Check for pause input (e.g., Escape key or a button click)
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

    public void StartGame() {
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

    public void SetVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume; // Set the volume on the AudioSource
        }
        PlayerPrefs.SetFloat("musicVolume", volume); // Save the volume setting
        PlayerPrefs.Save(); // Ensure the data is saved
    }
}
