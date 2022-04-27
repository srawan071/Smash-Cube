﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System;
#if UNITY_ANDROID

#elif UNITY_IPHONE
using Unity.Notifications.iOS;
#endif

public class GameData : MonoBehaviour
{
    public SaveFile currentSave;
    public static GameData Instance;
    public PlayercubeHolder PCH;
    public cubeInsantiater CI;
    
   
    public void Awake()
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


        PCH = FindObjectOfType<PlayercubeHolder>();
        CI = FindObjectOfType<cubeInsantiater>();

        Load();
       
        
    }
    private void Start()
    {
        
        Notify();
    }
   
    void Save()
    {
        string dataPath =  Application.persistentDataPath;
        var serializer = new XmlSerializer(typeof(SaveFile));
        var stream = new FileStream(dataPath + "/" + "naya" + ".save", FileMode.Create);
        serializer.Serialize(stream, currentSave);
        stream.Close();
       
    }
    void Load()
    {
        string dataPath = Application.persistentDataPath;
        if(System.IO.File.Exists(dataPath + "/" + "naya" + ".save"))
        {
            var serializer = new XmlSerializer(typeof(SaveFile));
            var stream = new FileStream(dataPath + "/" + "naya" + ".save", FileMode.Open);
            currentSave = serializer.Deserialize(stream) as SaveFile;
            stream.Close();
           
        }
        
    }
    public bool CheckFile(bool exist)
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/" + "naya" + ".save"))
        {
            exist = true;
        }
        else
        {
            exist = false;
        }
            return exist;
    }
   public void Erase()
    {
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/" + "naya" + ".save"))
        {
            File.Delete(dataPath + "/" + "naya" + ".save");
            
       }
        
    }
    void SavingData()
    {

        PCH = FindObjectOfType<PlayercubeHolder>();
        CI = FindObjectOfType<cubeInsantiater>();
        currentSave.score = CI.score;
        currentSave.continuee = GameManager.singleton.continuegarni;
        if(PCH.transform.childCount > 0)
        {
            currentSave.PlayerCube.value = PCH.playerCube.GetComponent<cube>().value;
           
        }
        else
        {
            int[] num = new int[] { 2, 4, 8, 16, 32, 64 };
            currentSave.PlayerCube.value = num[UnityEngine.Random.Range(0, num.Length)];
            
        }
        if (CI.transform.childCount > 0)
        {
            currentSave.savecube.Clear();
            for(int i = 0; i < CI.transform.childCount; i++)
            {
                currentSave.savecube.Add(new SaveCube());
                currentSave.savecube[i].value = CI.transform.GetChild(i).GetComponent<cube>().value;
                currentSave.savecube[i].Pos = CI.transform.GetChild(i).transform.position;
                currentSave.savecube[i].Rot = CI.transform.GetChild(i).transform.eulerAngles;
            }
        }
       else{
            currentSave.savecube.Clear();
       }

        Save();


    }
    private void OnApplicationQuit()
    {
         if(GameManager.singleton.gameOver)
           {
             Erase();
           }
            else{
            SavingData();
        }
     

    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
 
          if(GameManager.singleton.gameOver)
           {
             Erase();
           }
            else{
            SavingData();
               }
        }
    }

void Notify()
{
        /*

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
        notification.Title = "Smash some cubes";
        notification.Text = "Hey cubes are waiting for you";
        notification.SmallIcon = "small_wala";
        notification.LargeIcon = "big_wala";

        notification.FireTime = System.DateTime.Now.AddHours(8);


        AndroidNotificationCenter.SendNotification(notification, "channel_id");
#elif UNITY_IPHONE
        /*
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
        */

}
    

}

    [System.Serializable]
    public class SaveCube
    {
        public int value;
        public Vector3 Pos, Rot;
    }

    [System.Serializable]
    public class SaveFile
    {
        public List<SaveCube> savecube;
   
        public SaveCube PlayerCube;
        public bool continuee;
        public int score,bgvalue;
    
         
    }

