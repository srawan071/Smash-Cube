 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager singleton;
    public GameObject gameovermenu,rewardmenu,continuebtn,UIcube,settingMenu,HighestSmashmenu,Bestcube,BestParticle,fingerpoint;
    public bool isPaused, gameOver,continuegarni,Showbstcube;
    public int rewardthreshold = 128,bestCube;
    public TextMeshProUGUI youCreated, youGot,youscore,yourbest;
    public TextMeshPro []text,bestCubetxt;
    private int bombAmt,score;
    private float i;
    public Transform pos;
    private static int count, katicount;
    cubeInsantiater Cubeinstanstiater;
    Color tempCol;
    float H, S, V;
    public int Mode;

    // Start is called before the first frame update
    void Awake()
  {
        Cubeinstanstiater = FindObjectOfType<cubeInsantiater>();

        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

    }
      void Start()
    {
        Application.targetFrameRate = 30;
       bestCube = PlayerPrefs.GetInt("BESTCUBE");
        continuegarni=true;
        Mode = PlayerPrefs.GetInt("Mode");
        if (GameData.Instance.CheckFile(true))
        {
            continuegarni = GameData.Instance.currentSave.continuee;
        }
        Time.timeScale = 1;
        if (bestCube > 1&& Showbstcube==GameData.Instance.CheckFile(Showbstcube))
        {
            Showbstcube = true;
            Bestcube.transform.parent.gameObject.SetActive(true);
        }
        if (GameData.Instance.CheckFile(true)==false)
        {
            Showbstcube = true;
            fingerpoint.SetActive(true);
        }
        BestCube();
    }
    
    void Update()
    {
        if (Showbstcube)
        {
            if (Input.GetMouseButtonDown(0))
                {
                Showbstcube = false;
                Bestcube.transform.parent.gameObject.SetActive(false);
                fingerpoint.SetActive(false);
            }
        }
          if(gameOver&&i<score)
        {
             
            i+=Time.unscaledDeltaTime*score*1.75f;
                youscore.text = "" + (int)i;
            if (score < i)
                youscore.text = "" + score;


         }
       
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameovermenu.activeSelf)
                {
                    Restart();
                }
                else if (rewardmenu.activeInHierarchy)
                {
                    offRewardMenu();
                }
                else if (settingMenu.activeSelf)
                {
                    offSetingbtn();
                    
                }
                else if (HighestSmashmenu.activeSelf)
            {
                offHighsmashmenu();
            }
                else
                {
                    Application.Quit();
                }
           
        }
    }
    public void Restart()
    {
        Changemode();
        GameData.Instance.Erase();
        ShowFullScreenAd();
        MusicVibrate.Instance.tap.Play();
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Continue()
    {
         gameOver=false;
        Time.timeScale = 1;
        gameovermenu.SetActive(false);
        isPaused = false;

       Cubeinstanstiater.StopCoroutine("InsantiateSwipeCube");
        cube[] Cubes = FindObjectsOfType<cube>();
        foreach(cube cb in Cubes)
        {
            if (cb.transform.position.z < -.5f)
            {
                Destroy(cb.gameObject);
            }
        }
        Cubeinstanstiater.StartCoroutine("InsantiateSwipeCube");
       

    }
    void BestCube()
    {
       // tempCol = Cubeinstanstiater.kalar(bestCube, Color.white);
        Bestcube.GetComponent<MeshRenderer>().material.SetColor("_Edge", Cubeinstanstiater.kalar(bestCube, Color.white));
        Bestcube.GetComponent<MeshRenderer>().material.SetVector("_offset", Cubeinstanstiater.CalculateOffset(bestCube, Vector2.zero));
        /*
        Color.RGBToHSV(tempCol, out H, out S, out V);
        S = .5f;
        V = 1;
        if (bestCube == 128)
        {

            S = 0;
            V = .5f;
        }
        Bestcube.GetComponent<MeshRenderer>().material.SetColor("_Middle", Color.HSVToRGB(H, S, V));
        */


        for (int i = 0; i < text.Length; i++)
        {
           
             if (bestCube < 100)
            {
                bestCubetxt[i].fontSize = 9;
            }
            else 
            {

                bestCubetxt[i].fontSize = 7;
            }
           
            if (bestCube < 10000)
            {
                bestCubetxt[i].text = "" + bestCube;
            }
            else
            {
                bestCubetxt[i].text = "" + bestCube/1000 +"k";
            }
        }
    }

    public void Givereward(int value ,Color color)
    {
        
        bombAmt = Random.Range(1,4);
    

        for(int i = rewardthreshold; i <= value; i *= 2)
        {
            for (int j = 0; j < text.Length; j++)
            {
                if (value < 10000)
                {
                    text[j].text = "" + value;
                }
                else
                {
                    text[j].text = "" + value/1000+"k";
                }
            }
            if (i == value)
            {
                
                Time.timeScale = 0;
                //  UIcube.GetComponent<MeshRenderer>().material.color = color;
               // tempCol = color;
                UIcube.GetComponent<MeshRenderer>().material.SetColor("_Edge", color);
                UIcube.GetComponent<MeshRenderer>().material.SetVector("_offset", Cubeinstanstiater.CalculateOffset(value, Vector2.zero)); 
                /*
                Color.RGBToHSV(tempCol, out H, out S, out V);
                S = .5f;
                V = 1;
                if (value == 128)
                {

                    S = 0;
                    V = .5f;
                }
               UIcube.GetComponent<MeshRenderer>().material.SetColor("_Middle", Color.HSVToRGB(H, S, V));

                */
                rewardmenu.SetActive(true);
                MusicVibrate.Instance.RainCoin.Play();
                 Instantiate(BestParticle,pos.position,Quaternion.identity);
                isPaused = true;
                youCreated.text = "You created: " + value;
                youGot.text = "You got: " + value;
                if (rewardthreshold < 500)
                {
                    rewardthreshold *= 2;
                }
                break;
            }
        }
    }
     public void ContinueButton()
    {
    //    Admanager.Instance.kwala = "CONTINUE";
      //  Admanager.Instance.UserChoseToWatchAd();
    }
    public void BombaddBtn()
    {
       
    //   Admanager.Instance.kwala = "2BOMB";
      //  Admanager.Instance.UserChoseToWatchAd();
    }
    public void AddBomb()
    {
        FindObjectOfType<cubeInsantiater>().UPdateBomb(2);
       
    }
    public void Doublebombbtn()
    {
     //  Admanager.Instance.kwala = "2XBOMB";
       // Admanager.Instance.UserChoseToWatchAd();
    }
    public void DoubleBomb()
    {
        MusicVibrate.Instance.tap.Play();
        rewardmenu.SetActive(false);
        FindObjectOfType<cubeInsantiater>().UPdateBomb(bombAmt*2);
        Time.timeScale = 1;
        StartCoroutine(InPutOn());
    }

    public void GameOver()
    {
        if (continuegarni)
        {
            continuebtn.SetActive(true);
            continuegarni = false;
        }
        else
        {
            continuebtn.SetActive(false);
        }
        FindObjectOfType<SoundVibrate>().Vibrata();
        isPaused = true;
        Time.timeScale = 0;
        gameOver = true;
        gameovermenu.SetActive(true);
        yourbest.text = "" + PlayerPrefs.GetInt("BestScore");
         score= FindObjectOfType<cubeInsantiater>().score;
           youscore.text = "" + score;
         i=0;
      


       
    }
     public void offRewardMenu()
    {
        ShowFullScreenAd();
        MusicVibrate.Instance.tap.Play();
        rewardmenu.SetActive(false);
        FindObjectOfType<cubeInsantiater>().UPdateBomb(bombAmt);
        Time.timeScale = 1;
        StartCoroutine(InPutOn());
    }
    public void offHighsmashmenu()
    {
        ShowFullScreenAd();
        MusicVibrate.Instance.tap.Play();
        HighestSmashmenu.SetActive(false);
        FindObjectOfType<cubeInsantiater>().UPdateBomb(10);
        Time.timeScale = 1;
        StartCoroutine(InPutOn());

    }
    public void settingbtn()
    {

        MusicVibrate.Instance.tap.Play();
        settingMenu.SetActive(true);
        isPaused = true;
    }
    public void offSetingbtn()
    {
        FindObjectOfType<PlayercubeHolder>().lastTapPos = Vector3.zero;
        ShowFullScreenAd();
        MusicVibrate.Instance.tap.Play();
        settingMenu.SetActive(false);
        StartCoroutine(InPutOn());
    }

    public void LeaderBoardBtn()
    {

       
    }
         public void ShowFullScreenAd()
    {
        if (count == katicount)
        {
            count = 0;

            if (katicount != 0) { 
        //    Admanager.Instance.ShowFullScreenAds();
                }
            katicount = Random.Range(2, 4);
        }
        else
        {
            count++;
        }
    }
   public IEnumerator InPutOn()
    {
        yield return new WaitForSeconds(0.5f);
        isPaused = false;
        
    }
    public void Changemode()
    {
        if (Mode == 0)
            Mode = 1;
        else
            Mode = 0;

        PlayerPrefs.SetInt("Mode", Mode);

       

    }
}
