using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
//using Unity.Notifications.Android;
#elif UNITY_IPHONE
using Unity.Notifications.iOS;
#endif

namespace ProMaxUtils
{

    public class ProMaxsUtils : MonoBehaviour
    {
        public static ProMaxsUtils Instance = null;

        public bool Sound;
        public bool Music;
        public bool Vibrate;
        [SerializeField]
        private Sounds _sounds;
        [SerializeField]
       
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
        private void Start()
        {
            CheckMusic();
            CheckVibrate();
            CheckSound();
           
           // Notify();
        }
        /*
        public void Notify()
        {
            Debug.Log("Notify");

#if UNITY_ANDROID
            var c = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Default Channel",
                Description = "Generic notifications",
                Importance = Importance.High,
            };
            AndroidNotificationCenter.RegisterNotificationChannel(c);

            AndroidNotificationCenter.CancelAllScheduledNotifications();

            var notification = new AndroidNotification();
            notification.Title = "This is So Satisfying";
            notification.Text = "I swap color in just 2 Moves";
            notification.SmallIcon = "small_wala";
            notification.LargeIcon = "big_wala";
            notification.FireTime = System.DateTime.Now.AddHours(20);
            AndroidNotificationCenter.SendNotification(notification, "channel_id");

#elif UNITY_IPHONE
            
            iOSNotificationCenter.RemoveAllScheduledNotifications();
           var timeTrigger = new iOSNotificationTimeIntervalTrigger()
    {
               TimeInterval = new TimeSpan(8, 0, 0),

               Repeats = false
    };
            var notification = new iOSNotification()
            {
                // You can optionally specify a custom identifier which can later be 
                // used to cancel the notification, if you don't set one, a unique 
                // string will be generated automatically.
                Identifier = "_notification_01",
                Title = "Smash some cubes",
                Body = "Hey have some fun",
                Subtitle = "Hey gamers cubes are waiting for you",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,

        };
    iOSNotificationCenter.ScheduleNotification(notification);

#endif


        }
    */

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
            CheckSound();
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
            CheckMusic();
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
            CheckVibrate();
            
        }

        private void CheckSound()
        {
            if (PlayerPrefs.GetInt("Muted", 0) == 0)
            {
                Sound= true;
                _sounds.InstiantiateSound();
            }
            else
            {
                Sound= false;
                _sounds.DestroySound();
            }

        }
        private void CheckMusic()
        {
            if (PlayerPrefs.GetInt("Mute", 0) == 0)
            {
                
                Music= true;
                _sounds.InstiantiateMusic();
                

            }
            else
            {

                Music = false;
                _sounds.DestroyMusic();
                
            }
        }

        bool wait=true;
        void Wait()
        {
            wait = true;
        }
        public void vibrate()
        {

            if (Vibrate)
            {

                if (wait)
                {
                    wait = false;
                    Invoke("Wait", .1f);
                    Vibration.Vibrate(55);
                }

            }
        }
       public void CheckVibrate()
        {
            if (PlayerPrefs.GetInt("Silent", 0) == 0)
            {
                Vibrate = true;
            }
            else
            {
                Vibrate = false;
            }

        }


    }
}