using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 startpos, desiredpos;
       bool lerp;
    public MeshRenderer ms;
    public Color[] color;
    public int [,] Try;
    private Vector3 _CamInitialPos;
   
    void Start()
    {
        _CamInitialPos = transform.position;
        UpdateBg();
        startpos = transform.position;
        desiredpos = _CamInitialPos;
        lerp = true;
        transform.position = startpos;
        Invoke("offlerp", 5f);
    }
    void UpdateBg()
    {
       
        int value = Random.Range(0, color.Length);
        if (GameData.Instance.CheckFile())
        {
          
            value = GameData.Instance.currentSave.bgvalue;
        }
        else
        {
            GameData.Instance.currentSave.bgvalue = value;
        }
       
        ms.material.SetColor("_ColorB", color[value]);
       // ms.sharedMaterial.color = color[value];
       

       
    }
    public void ShakeCamera(float duration ,float magnitude)
    {
        StartCoroutine(camShake(duration, magnitude));
    }
    public IEnumerator camShake(float duration ,float magnitude)
    {
        Vector3 Startpos = desiredpos;

        float time = 0;
        while (time < duration)
        {
            float x = Startpos.x + Random.Range(-1, 1) * magnitude;
            float y = Startpos.y + Random.Range(-1, 1) * magnitude;
            transform.position = new Vector3(x, y, transform.position.z);
            time += Time.unscaledDeltaTime;
           

            yield return null;
        }

        transform.position = desiredpos;
        
    }

    void Update()
    {
        
        if (lerp)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredpos, 10*Time.deltaTime);
        }
    }
     void offlerp()
    {
        lerp = false;
    }
}
