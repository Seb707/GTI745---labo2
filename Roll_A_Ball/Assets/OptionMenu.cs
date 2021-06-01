using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
    }
    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
}
