using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVibrate : MonoBehaviour
{
   private MusicVibrate musicvibrate;

    [SerializeField]
    public GameObject volumeicon;

    [SerializeField]
    public GameObject vibrateicon;

    [SerializeField]
    public GameObject volumecircle;

    [SerializeField]
    public GameObject vibratecircle;

    public GameObject musicicon;

    public GameObject musiccircle;


    private int firstPlayint;
    public Slider backgroundslider;
    public float backgroundfloat;


    void Start()
    {

        musicvibrate = GameObject.FindObjectOfType<MusicVibrate>();

        firstPlayint = PlayerPrefs.GetInt("FirstPlay");
        if (firstPlayint == 0)
        {
            backgroundfloat = .25f;
            backgroundslider.value = backgroundfloat;
            PlayerPrefs.SetFloat("Awaz", backgroundfloat);
            PlayerPrefs.SetInt("FirstPlay", -1);
        }
        else
        {
            backgroundfloat = PlayerPrefs.GetFloat("Awaz");
            backgroundslider.value = backgroundfloat;
           
        }
        updateawaz();
        updatemusicicon();
        updatevibtateicon();
        updatesoundicon();
    }
    private void SaveSoundvalue()
    {
        PlayerPrefs.SetFloat("Awaz", backgroundslider.value);
    }
   
    public void updateawaz()
    {
        musicvibrate.Musick.volume = backgroundslider.value;

        SaveSoundvalue();

    }

    public void pauseMusic()
    {
        musicvibrate.tap.Play();
        musicvibrate.ToggleMusic();
        updatemusicicon();
    }
    public void pausevibrate()
    {

        musicvibrate.tap.Play();
        musicvibrate.ToggleVibtation();
        updatevibtateicon();
    }
    public void pauseSound()
    {

        musicvibrate.tap.Play();
        musicvibrate.ToggleSound();
        updatesoundicon();
    }

    void updatemusicicon()
    {
        if (PlayerPrefs.GetInt("Mute", 0) == 0)
        {
            //  AudioListener.volume = 1;
            musicicon.GetComponent<Image>().color = Color.green;
            musiccircle.transform.localPosition = new Vector3(16f, 0, 0);

            musicvibrate.Musick.mute = false;


        }
        else
        {
            musicvibrate.Musick.mute = true;
            // AudioListener.volume = 0;
            musicicon.GetComponent<Image>().color = new Color(129, 128, 127, .8f);
            // volumecircle.transform.position = new Vector3(-14f, transform.position.y, transform.position.z);
            musiccircle.transform.localPosition = new Vector3(-16f, 0, 0);

        }
    }

    void updatesoundicon()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            //  AudioListener.volume = 1;
            volumeicon.GetComponent<Image>().color = Color.green;
            volumecircle.transform.localPosition = new Vector3(16f, 0, 0);
            musicvibrate.bomb.mute = false;
            musicvibrate.score.mute = false;
            musicvibrate.tap.mute = false;
          
            musicvibrate.tak.mute = false;
            musicvibrate.swoosh.mute = false;

               
        }
        else
        {
            // AudioListener.volume = 0;
            volumeicon.GetComponent<Image>().color = new Color(129, 128, 127, .8f);
            // volumecircle.transform.position = new Vector3(-14f, transform.position.y, transform.position.z);
            volumecircle.transform.localPosition = new Vector3(-16f, 0, 0);
            musicvibrate.bomb.mute = true;
            musicvibrate.score.mute = true;
            musicvibrate.tap.mute = true;
          
            musicvibrate.tak.mute = true;
            musicvibrate.swoosh.mute = true;

        }
    }

    void updatevibtateicon()
    {
        if (PlayerPrefs.GetInt("Silent", 0) == 0)
        {

            vibrateicon.GetComponent<Image>().color = Color.green;
            vibratecircle.transform.localPosition = new Vector3(16f, 0, 0);
        }
        else
        {
            vibrateicon.GetComponent<Image>().color = new Color(127, 128, 129, .8f);
            vibratecircle.transform.localPosition = new Vector3(-16f, 0, 0);
        }
    }

    public void Vibrata()
    {
        if (PlayerPrefs.GetInt("Silent", 0) == 0)
        {
            Handheld.Vibrate();
        }
    }
}


