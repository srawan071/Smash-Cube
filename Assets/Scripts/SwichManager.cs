using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwichManager : MonoBehaviour
{
    public float count;
    public int combo;
    public bool comboZero;
    public TextMeshProUGUI Combotext;
    public GameObject particle;
    public GameObject dollarParticles,ComboCoin,ComboDollar;
    // Start is called before the first frame update
    void Start()
    {
        comboZero = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator OffCount()
    {
        
        while (count <1.5f)
        {
            count += Time.deltaTime;
            if (count > 1.5f)
            {
                combo = 0;
                comboZero = true;

                Combotext.transform.parent.gameObject.SetActive(false);
                Debug.Log("stopped");
            }
            yield return null;
        }
        
    }
    public void Comboo()
    {
        count = 0;
        combo++;
        Debug.Log("Comboo" + combo);
        if (comboZero)
        {

            comboZero = false;
            StartCoroutine(OffCount());
        }

        if (combo > 1)
        {
            InsantiateCombotxt();
            Instantiate(particle, Combotext.transform.position, Quaternion.identity);
        }
    }

    void InsantiateCombotxt()
    {
        Combotext.transform.parent.gameObject.SetActive(false);
        Combotext.text = combo-1 + "X";

        Combotext.transform.parent.gameObject.SetActive(true);
        Instantiate(dollarParticles, Combotext.transform.position, Quaternion.identity);
        MusicVibrate.Instance.ComboRain.Play();

        if (Random.value <= .5f)
            ComboCoin.SetActive(true);
        else
            ComboDollar.SetActive(true);

     

    }
}

