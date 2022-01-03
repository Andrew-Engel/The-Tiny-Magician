using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
  
    //FindObjectOfType<AudioManager>().Play("name of sound desired"); = line to play a sound in other scripts
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
            s.source.Play();
        if (s == null)
        {
            Debug.Log("Sound:" + name + "not found!");
            return;
            
        }
        
    }
    public void StopPlaying (string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s== null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
           
        }
        if (s != null)
            s.source.Stop();
    }
    public void Silence(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s != null)
            s.source.volume=0f;
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");

        }
    }
    public void Unsilence(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        
        if (s != null)
        s.source.volume = 0.6f;
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");

        }
    }
    public bool IsSoundPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s != null)
        {
            return s.source.isPlaying;
        }
       else
        {
            Debug.LogWarning("Sound:" + name + "not found!");
            return false;
        }
        
           
    }
    public void Pause(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s != null)
            s.source.Pause();
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");

        }
    }
    public void UnPause (string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s != null)
            s.source.UnPause();
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");

        }
    }

}
