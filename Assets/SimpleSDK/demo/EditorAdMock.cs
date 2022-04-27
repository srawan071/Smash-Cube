using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorAdMock : MonoBehaviour {

    //private float left = 0;
    private Action<bool> finish = null;
    //private bool playing = false;
    public GameObject bg;
    public GameObject rewardSuccess;
    public GameObject rewardFail;
    public GameObject interClose;

    //public Text leftText;

    void Awake()
    {
        instance = this;
        bg.SetActive(false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (playing)
        //{
        //    left -= Time.deltaTime;
        //    if (left < 0)
        //    {
        //        End(true);
        //    }
        //    leftText.text = left.ToString("f0") + "s";
        //}
    }

    void ShowReward(Action<bool> finish)
    {
        //playing = true;
        //this.left = left;
        rewardFail.SetActive(true);
        rewardSuccess.SetActive(true);
        interClose.SetActive(false);
        bg.SetActive(true);
        this.finish = finish;
    }
    void ShowInter(Action<bool> finish)
    {
        rewardFail.SetActive(false);
        rewardSuccess.SetActive(false);
        interClose.SetActive(true);
        //playing = true;
        //this.left = left;
        bg.SetActive(true);
        this.finish = finish;
    }

    public void RewardAdFail()
    {
        End(false);
    }

    public void RewardAdSuccess()
    {
        End(true);
    }

    public void InterClose()
    {
        End(true);
    }

    private void End(bool isReward)
    {
        bg.SetActive(false);
        finish(isReward);
    }

    static EditorAdMock instance;
    static public bool HasInit()
    {
        return instance != null;
    }
    static public void ShowRewardAd(Action<bool> finish)
    {
        if(instance!= null)
        {
            instance.ShowReward(finish);
        }
    }
    static public void ShowInterAd(Action<bool> finish)
    {
        if (instance != null)
        {
            instance.ShowInter(finish);
        }
    }
}
