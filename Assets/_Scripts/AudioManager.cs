using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip music;
    public Sound_controller[] sounds;
    public Sound_controller[] musics;
    private AudioSource musicSource;
    public bool randomizePitch = true;
    private static AudioManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else{
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        foreach (Sound_controller sound in sounds){
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;

        }
        musicSource = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        musicSource.clip = musics[0].clip;
        musicSource.volume = musics[0].volume;
        musicSource.pitch = musics[0].pitch;
        musicSource.loop = musics[0].loop;
    }

    public static void StartMusic()
    {
        if (_instance.music) {
            _instance.musicSource.loop = true;
            _instance.musicSource.clip = _instance.music;
            _instance.musicSource.Play();
        }
    }

    public static void PlaySFX(string name, float pitch = 1f, float volume = 1f, float time = 0f)
   {
        Sound_controller s = Array.Find(_instance.sounds, sound => sound.name == name);
        if (s == null){
            return;
        }
        s.source.time = time;
        s.source.volume = volume;
        s.source.pitch = pitch;
        s.source.Play();
   }
   public static void SetAmbience(AudioClip audioClip)
   {
       _instance.musicSource.Stop();
       _instance.musicSource.clip = audioClip;
       _instance.musicSource.Play();
   }

}
