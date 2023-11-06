 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager singleton;
    public GameObject gameovermenu;
    public bool isPaused, gameOver;

  
    cubeInsantiater Cubeinstanstiater;
    
   

    public int BombAmount;
    public int Score;
    public int BestScore;
    public int BestCube;
    public int RewardThresHold;
    public int LocalBestCube;
    // Start is called before the first frame update
    void Awake()
  {
       

        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
        Cubeinstanstiater = FindObjectOfType<cubeInsantiater>();
    }
  
      void Start()
    {
      
       BestCube = PlayerPrefs.GetInt("BESTCUBE");
       
       
        BombAmount = PlayerPrefs.GetInt("BOMB",9);
       
        RewardThresHold = PlayerPrefs.GetInt("StartNumber",5)+1;
       
        Time.timeScale = 1;
      
       
    }
   
   
 
  

 
        
    
   
    public void GameOver()
    {
       
        isPaused = true;
        Time.timeScale = 0;
        gameOver = true;
        gameovermenu.SetActive(true);
     

    }
   
 
  
}
