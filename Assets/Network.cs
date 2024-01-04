using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour
{
    [SerializeField]
    private GameObject NoNetworkMenu;
    private string url = "https://google.com";
    private bool Connected;
   

    public  void CheckInternet()
    {
        StopAllCoroutines();
        StartCoroutine(CheckNetwork());
    }
   IEnumerator CheckNetwork()
    {


        do
        {
            yield return new WaitForSecondsRealtime(3);

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                  //  Debug.Log("Connected");
                    NoNetworkMenu.SetActive(false);
                    Time.timeScale = 1;
                    Connected = true;
                }
                else
                {
                  //  Debug.Log("error");
                    NoNetworkMenu.SetActive(true);
                    Time.timeScale = 0;
                    Connected = false;
                }

            }



        }
        while (!Connected);



    }
}
