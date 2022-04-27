using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cube : MonoBehaviour
{
    public TMP_Text[] text;
    public int value;
    public bool sound, swipe,bomb,notjelly;
    public Vector3 midpos;
    public Rigidbody rb;
    public bool check;
    public GameObject Bombpart,StrikePart;
    public GameObject CoinPart, DollarPart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Start()
    {
        sound = true;
        if(!bomb&& !notjelly)
        StartCoroutine(jellycube());
        for (int i = 0; i < text.Length; i++)
        {
            if (value == 0)
            {
                text[i].fontSize = 0;
            }
            else if (value == 1)
            {
                text[i].fontSize = 0;
            }
            else if (value < 100)
            {
                text[i].fontSize = 10;
            }
            else if(value<1000)
            {

                text[i].fontSize = 8;
            }
            else
            {

                text[i].fontSize = 7;
            }
            if (value < 10000)
            {
                text[i].text = "" + value;
            }
            else
            {
                text[i].text = "" + value/1000+"k";
            }
        }

        if (swipe)
        {
          
        }
        else
        {
            shoot();
            check = true;
        }

        GetComponent<TrailRenderer>().enabled = false;
    }

    private void Update()
    {
        if (check)
        {
            if (transform.position.z < -1.35f)
            {
                check = false;
                GameManager.singleton.GameOver();
                
            }
        }
        if (transform.position.z < -.75f&& rb.velocity.z<0)
        {
            rb.velocity = new Vector3(0,0,0);
        }
    }

    public IEnumerator outcheck()
    {
        yield return new WaitForSeconds(1f);
        check = true;
    }
    IEnumerator jellycube()
    {
        Vector3 temp = transform.localScale = new Vector3(0, 0, 0);

        int x = 0;
        while (x < 3)
        {
            switch (x)
            {
                case 0:
                    if (GameManager.singleton.Mode == 0)
                    {

                        for (int i = 0; i < 5; i++)
                        {
                            temp.x += 0.154f;
                            temp.y += 0.154f;
                            temp.z += 0.154f;
                            transform.localScale = temp;
                            yield return new WaitForSeconds(0.0000000000000001f);



                        }
                    }
                    else
                    {

                        for (int i = 0; i < 5; i++)
                        {
                            temp.x += 0.11f;
                            temp.y += 0.11f;
                            temp.z += 0.11f;
                            transform.localScale = temp;
                            yield return new WaitForSeconds(0.0000000000000001f);

                        }
                    }

                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {

                        temp.x -= 0.05f;
                        temp.y -= 0.05f;
                        temp.z -= 0.05f;
                        transform.localScale = temp;
                        yield return new WaitForSeconds(0.000000000000000000001f);

                    }
                    break;
                case 2:
                    for (int i = 0; i <3 ; i++)
                    {

                        temp.x += 0.0333f;
                        temp.y += 0.0333f;
                        temp.z += 0.0333f;
                        transform.localScale = temp;
                        yield return new WaitForSeconds(0.000000000000000000001f);

                    }
                    break;

            }
            x++;

        }
        if (GameManager.singleton.Mode == 0)
            transform.localScale = new Vector3(.7f, .7f, .7f);
        else
        {
            transform.localScale = new Vector3(.5f, .5f, .5f);
            if (swipe)
            {
               
            }
        }
    }

    void shoot()
    {
        GameObject target = GameObject.Find("/CubeInsantiater/" + value);

        if (target == null)
        {
            target = this.gameObject;
           
           
        }

        Vector3 angle = target.transform.position - transform.position;
        if(angle.x==0&& angle.z == 0)
        {
            angle.x = Random.Range(-2f, 2f);
            angle.z = Random.Range(2f, 4f);
        }
        rb.velocity = new Vector3(angle.x / 2,7 , angle.z / 2);
        transform.SetParent(GameObject.Find("/CubeInsantiater").transform);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "" + value)
        {

           
            if (Mathf.Abs(rb.velocity.z) > Mathf.Abs(collision.gameObject.GetComponent<cube>().rb.velocity.z))
            {
                FindObjectOfType<SwichManager>().Comboo();
                
                midpos = (transform.position + collision.transform.position) / 2;
                FindObjectOfType<cubeInsantiater>().InsantiateshootCube(midpos, value * 2);
              
                Destroy(collision.gameObject);

                Destroy(gameObject);
               
            }

        }

        else if (collision.gameObject.tag == "box")
        {

            if (bomb)
            {
                Instantiate(Bombpart, transform.position, Quaternion.identity);
                FindObjectOfType<MusicVibrate>().bomb.Play();
                FindObjectOfType<CameraController>().ShakeCamera(.25f, .5f);
                Instantiate(CoinPart,transform.position,Quaternion.identity);
                Instantiate(DollarPart,transform.position,Quaternion.identity);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }

            if (GetComponent<TrailRenderer>().enabled)
            {
                GetComponent<TrailRenderer>().enabled = false;
            }
            if (sound && !notjelly)
            {
                if (PlayerPrefs.GetInt("Silent", 0) == 0)
                {

                    Vibration.Vibrate(55);
                }
                sound = false;
                FindObjectOfType<MusicVibrate>().tak.Play();
                Instantiate(StrikePart, transform.position, Quaternion.identity);
            }
        }

    }



}

     
   
    

