
using UnityEngine;
using UnityEngine.UI;
public class FakeAd : MonoBehaviour
{
    public Text text;
    private float initCountDown = 3;
    private float countDown = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ShowAd()
    {
        Debug.Log("show active true");
        this.gameObject.SetActive(true);
        this.countDown = this.initCountDown;
    }

    private void Update()
    {
        this.countDown -= Time.deltaTime;
        this.UpdateText();
        if(this.countDown <=0)
        {
            AdSuccess();
        }
    }


    private void UpdateText()
    {
        this.text.text = "Ad count down " + Mathf.Ceil(this.countDown);
    }

    private void AdSuccess()
    {
        Debug.Log("play ad finish");
        this.gameObject.SetActive(false);
        //FxSdk.CollectAdFinish(true);
    }

    public void StopAd()
    {
        Debug.Log("stop ad");
        this.gameObject.SetActive(false);
    }
}
