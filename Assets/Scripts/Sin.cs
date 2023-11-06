using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sin : MonoBehaviour
{
    // Start is called before the first frame update
   public float input;
   public float result;
   public float speed;
   public float frequency;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        result = (((Mathf.Sin(input*speed)+1)*.5f) * frequency);
      //  result = Time.time;
    }
}
