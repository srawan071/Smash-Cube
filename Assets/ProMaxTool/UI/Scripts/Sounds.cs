using ProMaxUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sounds : MonoBehaviour
{
    public AudioClip[] clip;
    
    public static AudioSource[] source;
    public AudioClip bgMusic, Buttontap;
    public static AudioSource BgMusicSource, ButtonTapSource;
    [SerializeField]
    private float[] _volume;
    

    public void InstiantiateMusic()
    {
        if (BgMusicSource == null && bgMusic != null)
            BgMusicSource = gameObject.AddComponent<AudioSource>();
        BgMusicSource.playOnAwake = true;
        BgMusicSource.clip = bgMusic;
        BgMusicSource.loop = true;
        BgMusicSource.volume = 0.1f;
        BgMusicSource.Play();
    }
    public void DestroyMusic()
    {
        if (BgMusicSource != null)
            Destroy(BgMusicSource);
    }
    public void InstiantiateSound()
    {
        if (ButtonTapSource == null && Buttontap != null)
        {
            ButtonTapSource = gameObject.AddComponent<AudioSource>();
           
            ButtonTapSource.clip = Buttontap;
            if (Time.timeSinceLevelLoad > 2)
            {
                ButtonTapSource.Play();
            };

            source = new AudioSource[clip.Length];

            for (int i = 0; i < clip.Length; i++)
            {
                source[i] = gameObject.AddComponent<AudioSource>();
                source[i].playOnAwake = false;
                source[i].clip = clip[i];
                source[i].volume = _volume[i];
            }
        }
       
    }
    public void DestroySound()
    {
        if (ButtonTapSource != null)
        {
            Destroy(ButtonTapSource);
            for (int i = 0; i < clip.Length; i++)
            {
                Destroy(source[i]);
            }
        }

    }
  static  bool play = true;

    public static void PlaySoundSource(int index)
    {
         if (ProMaxsUtils.Instance.Sound)
            {
            if (play)
            {
                play = false;
                
                Sounds.source[index].Play();
            }
            }
        
        
    }
    float t=0;
    private void Update()
    {
        PlayTrue();
    }
   
    void PlayTrue()
    {
        if (play == false)
        {
            t += Time.deltaTime;
            if (t >.05f)
            {
                play = true;
                t = 0;
            }
        }
       
    }
    public static void PlayTapSound()
    {
        if (ProMaxsUtils.Instance.Sound)
        {
            Sounds.ButtonTapSource.Play();
        }
    }
}

