using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System;

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
    public static void UpdateScore(int value)
    {
        // Check if the user is authenticated before sending data
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning("Cannot update score: Player is not logged in.");
            return;
        }

        double score = Math.Pow(2, value + 1);

        // REPLACEMENT: Use PlayGamesPlatform.Instance instead of the legacy Social wrapper
        PlayGamesPlatform.Instance.ReportScore((long)score, GPGSIds.leaderboard_smash_cube, (bool success) =>
        {
            if (success)
            {
                Debug.Log($"Successfully reported score: {(long)score}");
            }
            else
            {
                Debug.LogError("Failed to report score to the leaderboard.");
            }
        });
    }

    public void ShowLeaderBoard()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            // REPLACEMENT: Opens the leaderboard UI directly for this specific leaderboard
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_smash_cube);
        }
        else
        {
            Debug.LogWarning("Cannot show leaderboard: Player is not logged in.");
            // Optional: Trigger ManuallyAuthenticate() here to prompt the user
        }
    }
}
