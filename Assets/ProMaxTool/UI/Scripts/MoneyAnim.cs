using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProMaxUtils;
public class MoneyAnim : MonoBehaviour
{
    public Transform[] moneyicon;
    public float speed;
    public Transform targetPos;
    public Vector3 startPos, endPos;
    public float a;
    void Start()
    {
        startPos = transform.position;
        endPos = targetPos.position;
        // StartCoroutine(MoneyAnimate());
        StartCoroutine(Animate());
    }


    public IEnumerator MoneyAnimate()
    {
        
        while (a < 1)
        {
            a += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, a * speed);
            for (int i = 0; i < moneyicon.Length; i++)
            {
                moneyicon[i].localPosition = Vector3.Lerp(moneyicon[i].localPosition, Vector3.zero, a * .25f);
            }
            yield return null;
        }
    }
    public IEnumerator Animate()
    {
       
        
            if (ProMaxsUtils.Instance.Sound)
            {
                Sounds.source[0].Play();
            }
        
        float x = 0;
        x = Vector3.Distance(startPos,endPos)/speed;
        while (a < 1)
        {
            a += Time.deltaTime;
          //  x = Mathf.MoveTowards(0, 1, a*speed);
           // transform.position = Vector3.Lerp(startPos, endPos, a * speed);
            for (int i = 0; i < moneyicon.Length; i++)
            {
                //  moneyicon[i].localPosition = Vector3.Lerp(moneyicon[i].localPosition, Vector3.zero, a * .25f);
                moneyicon[i].transform.position = Vector3.MoveTowards(moneyicon[i].transform.position, endPos, Time.deltaTime*x);
            }
            yield return null;
        }
        for (int i = 0; i < moneyicon.Length; i++)
        {
            //  moneyicon[i].localPosition = Vector3.Lerp(moneyicon[i].localPosition, Vector3.zero, a * .25f);
          //  moneyicon[i].transform.position = endPos;
        }
        transform.gameObject.SetActive(false);
    }
    public void play()
    {
        gameObject.SetActive(true);

        startPos = transform.position;
        endPos = targetPos.position;

        StartCoroutine(MoneyAnimate());
    }
}
