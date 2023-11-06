using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparenrShadow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterial.SetShaderPassEnabled("ShadowCaster", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
