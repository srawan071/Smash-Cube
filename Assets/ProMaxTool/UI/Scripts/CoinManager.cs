using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class CoinManager : MonoBehaviour
{
    public int coinAmt;
    public TextMeshProUGUI cointxt;
    public float speed;
    private void Start()
    {
       
        coinAmt = PlayerPrefs.GetInt("COIN");
        cointxt.text = coinAmt.ToString();
        StartCoroutine(UpdateCoin(68));
    }
    public IEnumerator UpdateCoin(int num)
    {
        yield return new WaitForSeconds(.5f);
        int temp = coinAmt;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            coinAmt = (int)Mathf.Lerp(temp, num + temp, t*speed);
            cointxt.text = coinAmt.ToString();
            yield return null;
        }
        coinAmt = temp+num;
        cointxt.text = coinAmt.ToString();
        PlayerPrefs.SetInt("COIN",coinAmt);
        

    }
   
   
}
