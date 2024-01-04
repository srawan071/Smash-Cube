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
    public TextMeshProUGUI _2xText;
    [SerializeField]
    private Pop_Up popUp;
    [SerializeField]
    private cubeInsantiater _cubeInsantiater;
    [SerializeField]
    private UIManager _uiManager;
    private int localHighCube=2;

    enum Reward
    {
        bomb,
        _2X
    }
    private Reward _RewardType= Reward.bomb;
   
    
   
    // Start is called before the first frame update
  
    private void OnEnable()
    {
        cubeInsantiater.OnScoreChange +=  UpdateScoreText;
       

    }
    private IEnumerator Start()
    {
        yield return null;
        UPdateBomb(0);
        UPdate2X(0);

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
            LeaderBoard.UpdateScore(score);
        }
        if (score> GameManager.singleton.RewardThresHold)
        {
          
            GameManager.singleton.RewardThresHold++;
            _uiManager.EnableRewardMenu(score);
           
        }
        else if (score > _cubeInsantiater.StartNumber + 2)
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
            _cubeInsantiater.SpawnBomb();
            UPdateBomb(-1);
        }

        Sounds.PlayTapSound();

    }
    public void _2xBtn()
    {
        if (GameManager.singleton._2XAmt > 0)
        {
            _cubeInsantiater.SpawnStart2X();
            UPdate2X(-1);
        }

        Sounds.PlayTapSound();
    }
    public void Add2XBtn()
    {
        _RewardType = Reward._2X;
#if UNITY_EDITOR
        GetReward();

#endif

        Admanager.Instance.ShowRewardedAd(this);
        Sounds.PlayTapSound();

    }
    public void AddBombBtn()
    {
        _RewardType = Reward.bomb;
#if UNITY_EDITOR
        GetReward();

#endif

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
    public void UPdate2X(int amt)
    {
        int _2xamt = GameManager.singleton._2XAmt;
        _2xamt += amt;

        _2xText.SetText(_2xamt.ToString());

        if (_2xamt >= 1000)
        {
            _2xText.text = "" + _2xamt / 1000 + "k+";
        }
        PlayerPrefs.SetInt("2X", _2xamt);
        GameManager.singleton._2XAmt = _2xamt;


        if (_2xamt < 10)
        {
            _2xText.fontSize = 23;
        }
        else if (_2xamt < 100)
        {
            _2xText.fontSize = 18;
        }
        else if (_2xamt < 1000)
        {
            _2xText    .fontSize = 13;
        }
        else
        {
            _2xText.fontSize = 15;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Admanager.Instance.ShowFullScreenAd();
    }

    public void GetReward()
    {
        switch (_RewardType)
        {
            case Reward.bomb:
                UPdateBomb(1);
                break;
            case Reward._2X:
                UPdate2X(1);
                break;
        }

        _RewardType = Reward.bomb;
    }
}
