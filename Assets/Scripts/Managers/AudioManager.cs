using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] audioSources;

    public bool isSfxActivated;
    public bool isMusicActivated;

    public Image sfxCheckMark;
    public Image musicCheckMark;

    // awake
    private void Awake()
    {
        instance = this;
        LoadSettings();
    }

    // toggle sfx
    public void ToggleSfx()
    {
        Debug.Log("ToggleSfx");
        isSfxActivated = !isSfxActivated;
        sfxCheckMark.enabled = isSfxActivated;

        // save player prefs
        PlayerPrefsManager.isSfxActivated = isSfxActivated;
        SaveSettings();
    }

    // toggle music
    public void ToggleMusic()
    {
        Debug.Log("ToggleMusic");
        isMusicActivated = !isMusicActivated;
        musicCheckMark.enabled = isMusicActivated;

        // if is false, stop music
        if (!isMusicActivated)
        {
            foreach (AudioSource audioSource in audioSources)
                audioSource.Stop();
        }
        // if is true, play music
        else
        {
            foreach (AudioSource audioSource in audioSources)
                audioSource.Play();
        }

        // save player prefs
        PlayerPrefsManager.isMusicActivated = isMusicActivated;
        SaveSettings();
    }

    // save settings to player prefs
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("isSfxActivated", isSfxActivated ? 1 : 0);
        PlayerPrefs.SetInt("isMusicActivated", isMusicActivated ? 1 : 0);
        PlayerPrefs.Save();
    }

    // load settings from player prefs
    private void LoadSettings()
    {
        // Default to true if keys do not exist
        isSfxActivated = PlayerPrefs.GetInt("isSfxActivated", 1) == 1;
        isMusicActivated = PlayerPrefs.GetInt("isMusicActivated", 1) == 1;

        sfxCheckMark.enabled = isSfxActivated;
        musicCheckMark.enabled = isMusicActivated;

        // Apply the music setting
        if (isMusicActivated)
        {
            foreach (AudioSource audioSource in audioSources)
                audioSource.Play();
        }
        else
        {
            foreach (AudioSource audioSource in audioSources)
                audioSource.Stop();
        }
    }
}
