using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // File -> build setting -> Mettre la scene de MainMenu en premier ensuite le jeu 
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
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
