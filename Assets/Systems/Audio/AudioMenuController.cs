using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMenuController : MonoBehaviour
{
    
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _masterSlider;


    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else 
        {
            SetMusicVolume();
            SetSFXVolume();
            SetMasterVolume();
        }

    }

    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        _masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
    }

    public void ToggleMusic() 
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX() 
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void SetMusicVolume() 
    {
    float volume = Mathf.Clamp(_musicSlider.value, 0.0001f, 1f);
    myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    PlayerPrefs.SetFloat("musicVolume", _musicSlider.value);
    }

    public void SetSFXVolume() 
    {
    float volume = Mathf.Clamp(_sfxSlider.value, 0.0001f, 1f);
    myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    PlayerPrefs.SetFloat("sfxVolume", _sfxSlider.value);
    }

    public void SetMasterVolume() 
    {
    float volume = Mathf.Clamp(_masterSlider.value, 0.0001f, 1f);
    myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    PlayerPrefs.SetFloat("masterVolume", _masterSlider.value);
    }


}