using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    [SerializeField] Sound[] sounds;

    public static AudioManager instance;
	// Use this for initialization
	void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
		foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
	}

    private void Start()
    {
        Play("MainTheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + s.name + " is Invalid!");
            return;
        }
        s.audioSource.Play();
        // Put this anywhere FindObjOfType<AudioManager>().Play("SOUND NAME");
    }
}
