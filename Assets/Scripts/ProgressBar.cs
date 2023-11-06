using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public GameObject[] cube;
    private Material []_material;
    public Image Fill;
    private ProgressBarCube[] cubes;
    
  

    // Update is called once per frame
   
     public IEnumerator UpdateVisual()
    {
        Transform[] cubeTransform = new Transform[3];
        
        yield return null;
    }
    private IEnumerator MoveLeft()
    {
        yield return null;
    }
    private IEnumerator ChangePlace()
    {
        yield return null;
    }
}
