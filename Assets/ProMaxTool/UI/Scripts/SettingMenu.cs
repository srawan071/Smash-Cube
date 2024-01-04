using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ProMaxUtils
{
    public class SettingMenu : MonoBehaviour
    {
        [SerializeField]
        Image soundbg, soundDot, vibrateBg, vibrateDot,musicbg,musicDot;
        [SerializeField]
        Color green, grey;
        [SerializeField]
        private TextMeshProUGUI Fps;
        [SerializeField]
        private TextMeshProUGUI Resolution;
        public Vector3 pos;


        public static int QualityLevel;
        [SerializeField]
        private TextMeshProUGUI _qualityText;
        void Start()
        {
            
        
            updatevibtateicon();
            updatesoundicon();
            UpdateMusicIcon();
            Fps.text = Application.targetFrameRate.ToString();
            Resolution.text = Screen.width.ToString() + "X" + Screen.height.ToString();


            UpdateQualityText();


        }
        private void OnEnable()
        {
            if (ProMaxsUtils.Instance.Sound)
            {
                Sounds.ButtonTapSource.Play();
            }
            Quality.OnFpsChange +=FpsChanged;
            Quality.OnResolutionChange += ResolutionChanged;
           //Debug.Log("enable");
        }

     
        public void pausevibrate()
        {
            if (ProMaxsUtils.Instance.Vibrate)
            {
                Vibration.Vibrate(55);
            }
            if (ProMaxsUtils.Instance.Sound)
            {
                Sounds.ButtonTapSource.Play();
            }
            //   musicvibrate.tap.Play();
            ProMaxsUtils.Instance.ToggleVibtation();
            updatevibtateicon();
        }
        public void pauseSound()
        {
            if (ProMaxsUtils.Instance.Sound)
            {
                Sounds.ButtonTapSource.Play();
            }

            //    musicvibrate.tap.Play();
            ProMaxsUtils.Instance.ToggleSound();
            updatesoundicon();
        }

     public   void PauseMusic()
        {
            if (ProMaxsUtils.Instance.Sound)
            {
                Sounds.ButtonTapSource.Play();
            }

            //    musicvibrate.tap.Play();
            ProMaxsUtils.Instance.ToggleMusic();
            UpdateMusicIcon();

           
        }


        void updatesoundicon()
        {
           
            if (PlayerPrefs.GetInt("Muted", 0) == 0)
            {
                //  AudioListener.volume = 1;
                soundbg.GetComponent<Image>().color = green;
                soundDot.transform.localPosition = new Vector3(28f, 0, 0);



            }
            else
            {
                // AudioListener.volume = 0;
                soundbg.GetComponent<Image>().color = grey;
                // volumecircle.transform.position = new Vector3(-14f, transform.position.y, transform.position.z);
                soundDot.transform.localPosition = new Vector3(-28f, 0, 0);


            }
        }
        void UpdateMusicIcon()
        {

            if (PlayerPrefs.GetInt("Mute", 0) == 0)
            {
                //  AudioListener.volume = 1;
                musicbg.GetComponent<Image>().color = green;
               musicDot.transform.localPosition = new Vector3(28f, 0, 0);



            }
            else
            {
                // AudioListener.volume = 0;
                musicbg.GetComponent<Image>().color = grey;
                // volumecircle.transform.position = new Vector3(-14f, transform.position.y, transform.position.z);
                musicDot.transform.localPosition = new Vector3(-28f, 0, 0);


            }
        }

        void updatevibtateicon()
        {
            if (PlayerPrefs.GetInt("Silent", 0) == 0)
            {

                vibrateBg.GetComponent<Image>().color = green;
                vibrateDot.transform.localPosition = new Vector3(28f, 0, 0);
            }
            else
            {
                vibrateBg.GetComponent<Image>().color = grey;
                vibrateDot.transform.localPosition = new Vector3(-28f, 0, 0);
            }
        }

        public void Vibrata()
        {
            if (PlayerPrefs.GetInt("Silent", 0) == 0)
            {
#if UNITY_ANDROID
                Handheld.Vibrate();
#endif
            }
        }

        public void PrivacyPolicy()
        {
            Application.OpenURL("https://sites.google.com/view/privacypolicy-smashcube/home");
        }

        public void onClickClose()
        {
            if (ProMaxsUtils.Instance.Sound)
            {
                Sounds.ButtonTapSource.Play();
            }
            StartCoroutine(DisableObject());
        }
        IEnumerator DisableObject()
        {
            yield return new WaitForSecondsRealtime(1);
            gameObject.SetActive(false);
        }
       
        private void FpsChanged()
        {
            Fps.text = Application.targetFrameRate.ToString();
        }
        private void ResolutionChanged()
        {
          
          //  yield return null;
            Resolution.text = Screen.width.ToString  ()+ "X" + Screen.height.ToString();
            Invoke("updateRes", .1f);
        }

        private void OnDisable()
        {
            Quality.OnFpsChange -= FpsChanged;
            Quality.OnResolutionChange -= ResolutionChanged;
           // Debug.Log("Disable");
        }
       void updateRes()
        {
          
            Resolution.text = Screen.width.ToString() + "X" + Screen.height.ToString();
        }

        public void ResBtn()
        {
            FindObjectOfType<Quality>().ResolutionButton();
        }
        public void FpsBtn()
        {
            FindObjectOfType<Quality>().FPSButton();
        }


        public void ToggleQualityLevel()
        {
          //  Debug.Log("Clicked");
            Quality.ToggleQuality();

            UpdateQualityText();
        }

        private void UpdateQualityText()
        {
            switch (Quality.QualityLevel)
            {
                case 0:
                    _qualityText.SetText("<Low>");
                    break;
                case 1:
                    _qualityText.SetText("<Medium>");
                    break;
                case 2:
                    _qualityText.SetText("<High>");
                    break;
            }
        }
    }
}