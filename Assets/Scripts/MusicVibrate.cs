using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVibrate : MonoBehaviour
{
    public static MusicVibrate Instance = null;



    public AudioSource Musick;
    public AudioSource bomb, score, tap,tak,swoosh,ComboRain,RainCoin;



    private void Awake()
    {
        
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }


    }
    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            PlayerPrefs.SetInt("Muted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 0);
        }
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("Mute", 0) == 0)
        {
            PlayerPrefs.SetInt("Mute", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Mute", 0);
        }
    }

    public void ToggleVibtation()
    {
        if (PlayerPrefs.GetInt("Silent", 0) == 0)
        {
            PlayerPrefs.SetInt("Silent", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Silent", 0);
        }
    }
}