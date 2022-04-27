using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class preloader : MonoBehaviour
{
    void Start()
    {
        Invoke("loadScene", 2f);
    }

    void loadScene()
    {
        SceneManager.LoadScene(1);
    }
   
}
