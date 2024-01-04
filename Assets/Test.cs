using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("TestAwake");
    }
    private void OnEnable()
    {
        Debug.Log("TestOnEnable");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TestStart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }
}
