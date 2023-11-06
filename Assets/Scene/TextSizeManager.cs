using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeManager : MonoBehaviour
{
    // Start is called before the first frame update

    // sphere with radius 15,10,8,5.5;
    // cube withOut Raduis 17,12,7,6

    public TextMeshPro ui;
    public enum type
    {
        WithOutRadius,
        WithRaduis
        
    };
    public type Type;
    void Awake()
    {
        Debug.Log(ui.gameObject.name.Length);
         //Type = new type();
        if (Type == type.WithOutRadius)
        {
            Debug.Log(Type);
            TextMeshPro[] text = gameObject.transform.GetComponentsInChildren<TextMeshPro>();
            foreach (TextMeshPro txt in text)
            {
               
                if (txt.gameObject.name.Length == 1)
                {
                    txt.fontSize = 17;
                }
                else if (txt.gameObject.name.Length == 2)
                {
                    txt.fontSize = 12;
                }
                else if (txt.gameObject.name.Length == 3)
                {
                    txt.fontSize = 9;
                }
                else if (txt.gameObject.name.Length == 4)
                {
                    txt.fontSize = 6f;
                }
            }
        }
        else if(Type== type.WithRaduis)
        {

            Debug.Log(Type);
            TextMeshPro[] text = gameObject.transform.GetComponentsInChildren<TextMeshPro>();

            foreach(TextMeshPro txt in text)
            {
                
                if (txt.gameObject.name.Length == 1)
                {
                    txt.fontSize = 15;
                }
                else if(txt.gameObject.name.Length == 2)
                {
                    txt.fontSize = 10;
                }
                else if(txt.gameObject.name.Length == 3)
                {
                    txt.fontSize = 8;
                }
                else if(txt.gameObject.name.Length == 4)
                {
                    txt.fontSize = 5.5f;
                }
            }
        }
    }


}
