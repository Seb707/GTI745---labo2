using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUi;
    public GameObject optionsMenuUi;
    public GameObject controlsMenuUi;
    public GameObject creditsMenuUi;
    public AudioMixer audioMixer;
    // Update is called once per frame
    void Start()
    {
        GameIsPaused = false;
    }

        void Update()
    {
        if (!GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                    Pause();
            }
        }
    }

    public void testPause() {
        if (!GameIsPaused)
        { 
            Pause();
        }
    }

    public void removePauseUI() {
        print("ez");
        pauseMenuUi.SetActive(false);
        GameIsPaused = false;
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OptionsMenu()
    {
        optionsMenuUi.SetActive(true);
        pauseMenuUi.SetActive(false);
    }

    public void ControlsMenu()
    {
        controlsMenuUi.SetActive(true);
        pauseMenuUi.SetActive(false);
    }

    public void CreditsMenu()
    {
        creditsMenuUi.SetActive(true);
        pauseMenuUi.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
}
