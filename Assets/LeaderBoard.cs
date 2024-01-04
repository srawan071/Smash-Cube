
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LeaderBoard : MonoBehaviour
{


    public void Start()

    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

    }



    internal void ProcessAuthentication(SignInStatus status)
    {

        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services

        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    public static  void UpdateScore(int value)
    {
        double score = value;
      
        score = Math.Pow(2, score+1);
        //  score = Mathf.Pow(2, value + 1);
        Social.ReportScore((long)score, GPGSIds.leaderboard_smash_cube, (bool success) =>
        {
            // handle success or failure
        });
    }

    public void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }
}

