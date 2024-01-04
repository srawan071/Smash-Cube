
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Spawncubes : MonoBehaviour
{
    // Start is called before the first frame update
    public cubeInsantiater CI;
    public GameObject cube;
    public GameObject particles;

   public int collide = 0;



    private void Start()
    {
        SpawnInLine();
    }
    private void Update()
    {
        if (Keyboard.current.yKey.isPressed)
        {
            
        }
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {


            GameManager.singleton.BestCube = 200;
            GameManager.singleton.RewardThresHold = 200;
            GameManager.singleton.LocalBestCube = 200;
            CI.StartNumber = 200;
            for (int i = 0; i < 20; i++)
            {
                CI.InsantiateshootCube(Random.insideUnitSphere * 1 + new Vector3(0,2,3),Random.Range(0,8));
            }
        }
    }

    public void SpawnInLine()
    {
        Vector3 pos=Vector3.zero;
        float Xpos = -1.75f;
        float YPos=0;
        int count = 0;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                CI.InsantiateshootCube(new Vector3(Xpos,1.5f, YPos), count);
                count++;
                Xpos += 1.75f;
            }
            Xpos = -1.75f;
            YPos += 2;
        }
    }
}
