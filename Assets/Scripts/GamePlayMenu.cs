using ProMaxUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GamePlayMenu : MonoBehaviour,IRewardable
{
    public TMP_Text ScoreText;
    public TextMeshProUGUI BombText;
    [SerializeField]
    private Pop_Up popUp;
    [SerializeField]
    private cubeInsantiater _cubeInsantiater;
    [SerializeField]
    private UIManager _uiManager;
    private int localHighCube=2;
    // Start is called before the first frame update
  
    private void OnEnable()
    {
        cubeInsantiater.OnScoreChange +=  UpdateScoreText;
       

    }
    private IEnumerator Start()
    {
        yield return null;
        UPdateBomb(0);

    }
    // Update is called once per frame

    public void UpdateScoreText(int score)
    {

        if (score > 35)
        {
            _uiManager.EnableHighSmashMenu();
            return;
        }
        GameManager.singleton.Score +=score;
      
        ScoreText.SetText(GameManager.singleton.Score.ToString());

        popUp.OnButtonClick();
        if (score > GameManager.singleton.LocalBestCube)
        {
            GameManager.singleton.LocalBestCube=score ;

            _uiManager.UpdateProgressBarCube(score);
        }
        if (score> GameManager.singleton.RewardThresHold)
        {
          
            GameManager.singleton.RewardThresHold++;
            _uiManager.EnableRewardMenu(score);
        }
        else if (score > _cubeInsantiater.StartNumber + 3)
        {
            _uiManager.EnableRewardMenu(score);
        }
        if (score > GameManager.singleton.BestCube)
        {
            GameManager.singleton.BestCube = score;
            PlayerPrefs.SetInt("BESTCUBE",score);
        }

    }

   
    public void Bombbtn()
    {
       
        if (GameManager.singleton.BombAmount > 0)
        {
            StartCoroutine(_cubeInsantiater.InsantiateBomb());
            UPdateBomb(-1);
        }

        Sounds.PlayTapSound();

    }
    public void AddBombBtn()
    {
        Admanager.Instance.ShowRewardedAd(this);
        Sounds.PlayTapSound();
    }
    public void UPdateBomb(int amt)
    {
        int bombamt = GameManager.singleton.BombAmount;
        bombamt += amt;

        BombText.SetText(bombamt.ToString());

        if (bombamt >= 1000)
        {
            BombText.text = "" + bombamt / 1000 + "k+";
        }
        PlayerPrefs.SetInt("BOMB", bombamt);
        GameManager.singleton.BombAmount = bombamt;
       

        if (bombamt < 10)
        {
            BombText.fontSize = 23;
        }
        else if (bombamt < 100)
        {
            BombText.fontSize = 18;
        }
        else if (bombamt < 1000)
        {
            BombText.fontSize = 13;
        }
        else
        {
            BombText.fontSize = 15;
        }
    }

    public void UpdateSkin(int index)
    {
        _uiManager.UpdateProgressCubeSkin(index);
    }
    private void OnDisable()
    {
        cubeInsantiater.OnScoreChange -= UpdateScoreText;

       
    }
    public void Restart()
    {
        Sounds.PlayTapSound();
        GameData.Instance.Erase();
        SceneManager.LoadScene(0);
        Admanager.Instance.ShowFullScreenAd();
    }

    public void GetReward()
    {
        UPdateBomb(1);
    }
}
