using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class poptxt : MonoBehaviour
{
    private TextMeshPro text;
    public int value;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(60, 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.5f);
        text = GetComponent<TextMeshPro>();
        text.text = "+" + value;
        Destroy(gameObject, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up*5*Time.deltaTime);
    }
}
