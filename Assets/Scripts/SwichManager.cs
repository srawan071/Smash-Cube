using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;
using ProMaxUtils;
using System.Security.Cryptography;
using System.Text;

public class SwichManager : MonoBehaviour
{
    public float count;
    public int combo;
    public bool comboZero;
    public TMP_Text ComboValue;
    public GameObject particle;

    private IObjectPool<poptxt> _textPool;
    [SerializeField]
    private poptxt _popUpText;
    [SerializeField]
    private AnimationCurve _CombooPop;
    [SerializeField]
    private RectTransform CombooText;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    private StringBuilder sb= new StringBuilder(3);

    private Coroutine c;
    //    public GameObject dollarParticles,ComboCoin,ComboDollar;
    // Start is called before the first frame update

    void Start()
    {
        comboZero = true;
       
       _textPool = new ObjectPool<poptxt>(Create, null, Release, actionOnDestroy: (obj) => Destroy(obj), true, 5, 10);
     
        for (int j = 0; j < 5; j++)
        {
            poptxt text= _textPool.Get();
            StartCoroutine(text.SetRelease());

        }
       
    }

  

    private poptxt Create()
    {
        poptxt text = Instantiate(_popUpText, transform.position, Quaternion.identity, null);
        text.SetPool(_textPool);
        return text;
    }
   
    void Release(poptxt text)
    {
        text.gameObject.SetActive(false);
    }
    // Update is called once per frame

    IEnumerator OffCount()
    {
        
        while (count <1.5f)
        {
            count += Time.deltaTime;
            if (count > 1.5f)
            {
                combo = 0;
                comboZero = true;
              //  StartCoroutine(AnimateCombooOut());
                //Combotext.transform.parent.gameObject.SetActive(false);

            }
            yield return null;
        }
        
    }
    public void Comboo(int popUpvalue,Vector3 value)
    {
      var text=  _textPool.Get();
        text.Initialized(popUpvalue,value);
        count = 0;
        combo++;
      //  Debug.Log("Comboo" + combo);
        if (comboZero)
        {

            comboZero = false;
            StartCoroutine(OffCount());
        }

        if (combo > 1)
        {
            InsantiateCombotxt();
         //   Instantiate(particle, Combotext.transform.position, Quaternion.identity);
        }
    }

    void InsantiateCombotxt()
    {
        
        sb.Clear();
        sb.Append(combo - 1);
        sb.Append("X");
        ComboValue.SetText(sb.ToString());
        // ComboValue.text = combo-1 + "X";
       

        CombooText.gameObject.SetActive(true);
        if (c != null)
            StopCoroutine(c);
        c=StartCoroutine(AnimateCombooIN());
       
       // Instantiate(dollarParticles, Combotext.transform.position, Quaternion.identity);
     //   MusicVibrate.Instance.ComboRain.Play();

       
     

    }

   public IEnumerator AnimateCombooIN()
    {
        //  CombooText.transform.localPosition = new Vector3(0, 434.5f, -11f);

      
        CombooText.anchoredPosition3D = new Vector3(0, -160f, -10f);
       
      //  _canvasGroup.alpha = 0;
        float t = 0;
        Vector3 startScale = new Vector3(1.75f,1.5f,1);
        Vector3 finalScale = Vector3.one ;
        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime *2);
            CombooText.transform.localScale = Vector3.LerpUnclamped(startScale, finalScale, _CombooPop.Evaluate(t));
           // _canvasGroup.alpha = t*2;

            yield return null;


        }
        yield return new WaitForSeconds(1.5f);

        t = 0;

        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * 2);
            CombooText.transform.position += Vector3.up * Time.deltaTime * 1;
            // _canvasGroup.alpha =1- t*2 ;

            yield return null;
        }
        CombooText.gameObject.SetActive(false);

    }
    IEnumerator AnimateCombooOut()
    {
       // _canvasGroup.alpha = 1;
        float t = 0;
       
        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime*2);
            CombooText.anchoredPosition += Vector2.up *Time.deltaTime*1;
           // _canvasGroup.alpha =1- t*2 ;

            yield return null;
        }
        CombooText.gameObject.SetActive(false);
    }
}

