
using ProMaxUtils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManu : MonoBehaviour, IRewardable
{
   [SerializeField] private cubeInsantiater _cubeInstianter;
   [SerializeField] private PlayercubeHolder _playerCubeHolder;

    [SerializeField] private Image circle;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private Pop_Up _PopUP;
    [SerializeField]
    private GameObject NoThanksBtn;




    private void OnEnable()
    {
       
            StartCoroutine(CountDown());
            StartCoroutine(CircleAnim());
        
        Sounds.PlaySoundSource(6);
        NoThanksBtn.SetActive(false);
        StartCoroutine(EnableNoThanks());
    }
    public void ContinueButton()
    {
#if UNITY_EDITOR
        GetReward();

#endif
       
        Sounds.PlayTapSound();
      Admanager.Instance.ShowRewardedAd(this);
    }
    public void Continue()
    {

        int index = 0;
        while (index < _cubeInstianter.GroundCubes.Count) { 
            if (_cubeInstianter.GroundCubes[index].transform.position.z < 0)
            {
                _cubeInstianter.GroundCubes[index].DestroyCube();

            }
            else
            {
                index++;
            }
        }
        

       
        if (_playerCubeHolder._playerCube == null)
        {
            _cubeInstianter.StopAllCoroutine();
            _cubeInstianter.InsantiateNORmalCube(_cubeInstianter.StartNumber - 5);
           
        }
       
        GameManager.singleton.isPaused = false;
        Time.timeScale = 1;
        gameObject.SetActive(false);

    }
    public void Restart()
    {
       /* GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level",GameManager.singleton.LocalBestCube);*/
       Admanager.Instance.ShowFullScreenAd();
        Sounds.PlayTapSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    private IEnumerator CountDown()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1);
        float t = 10;
       
        while (t >= 0)
        {

            _PopUP.OnButtonClick();
            _timer.SetText(t.ToString());

            t--;
           
            yield return wait;
        }

       
    }
    private IEnumerator CircleAnim()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.unscaledDeltaTime*.1f;
            circle.fillAmount = t;
            yield return null;
        }
    }

    public void GetReward()
    {
        GameManager.singleton.SaveData = true;
        Continue();
    }
    IEnumerator EnableNoThanks()
    {
        //  yield return new WaitForSecondsRealtime(.5f);

        yield return new WaitForSecondsRealtime(2f);
        NoThanksBtn.SetActive(true);
    }
}
