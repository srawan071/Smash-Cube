using ProMaxUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public  class UIManager : MonoBehaviour
{
   
    public OffsetData offsetData;
    [SerializeField]
    internal ProgressBarCube[] _progressBarCube;
    public int _BaseMap;
    public RewardMenu RewardMenu;
   [SerializeField] private GameObject BombUI;
    [SerializeField]
    private GameObject HighestSmashMenu;
    private void Awake()
    {
        _BaseMap = Shader.PropertyToID("_BaseMap");
    }
    private void OnEnable()
    {
        StartMenu.GameStarted += OnGameStarted;
    }
    void  Start()
    {
       
        if (GameData.Instance.CheckFile())
        
            GameManager.singleton.LocalBestCube = GameData.Instance.currentSave.LocalHighestCube;
      
      
       
    }
    private void OnGameStarted()
    {
        _progressBarCube[0].transform.parent.gameObject.SetActive(true);
        BombUI.SetActive(true);
        UpdateProgressBarCube(GameManager.singleton.LocalBestCube>0? GameManager.singleton.LocalBestCube :GameManager.singleton.RewardThresHold-5);
    }
    
    public void UpdateProgressBarCube(int index)
    {
        for (int i = 0; i < _progressBarCube.Length; i++)
        {
            _progressBarCube[i].UpdatePos(index);
        }
    }
    public void UpdateProgressCubeSkin(int index)
    {

        for (int i = 0; i < _progressBarCube.Length; i++)
        {
            _progressBarCube[i].UpdateSkin(index);
        }
    }

  public void EnableRewardMenu(int value)
    {
        GameManager.singleton.isPaused = true;
        Time.timeScale = 0;
        
        RewardMenu.gameObject.SetActive(true);
        RewardMenu.Initilized(value);

    }
    public void EnableGameOverMenu()
    {
        
    }
    public void EnableSettingMenu()
    {

    }
    public void EnableHighSmashMenu()
    {
        HighestSmashMenu.SetActive(true);
    }
    private void OnDisable()
    {
        StartMenu.GameStarted -= OnGameStarted;
    }
}
