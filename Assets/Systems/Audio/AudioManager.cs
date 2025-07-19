using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    
    public AudioSource musicSource, sfxSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    
    public void PlayMusic(string name, bool loop = true)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("AudioManager: Sonido de música no encontrado: " + name);
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.loop = loop; // Aplicamos si debe ser loopeable
            musicSource.Play();
        }
    }

    
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("AudioManager: SFX no encontrado: " + name);
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
}