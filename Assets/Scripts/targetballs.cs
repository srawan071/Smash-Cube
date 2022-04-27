using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetballs : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PlayercubeHolder>().playerCube != null)
        {
            if (transform.position.z < FindObjectOfType<PlayercubeHolder>().playerCube.transform.position.z)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {

                GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
       
    }
}
