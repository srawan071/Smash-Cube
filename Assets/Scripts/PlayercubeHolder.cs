using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayercubeHolder : MonoBehaviour
{
    public GameObject playerCube;
    public bool moveonX, checkOnce;
   public  Vector2 lastTapPos;
    public float speed;
    public GameObject Target;
    private Ray ray;
    [SerializeField]
  RaycastHit[] hit;
  public  LayerMask layer;
    float tempPos;
    bool shoot;
    cubeInsantiater cubeInstantiater;
    public GameObject TexasHoldim;
    void Start()
    {
        cubeInstantiater = FindObjectOfType<cubeInsantiater>();
        hit = new RaycastHit[1];
        GetChild();

        checkOnce = true;
        moveonX = true;

    }


    public void GetChild()
    {
        if (transform.childCount > 0)
        {

            playerCube = transform.GetChild(0).gameObject;
           


        }
    }

    void Update()
    {
        if (!GameManager.singleton.isPaused)
        {
            if (transform.childCount > 0)
            {
                if (playerCube == null)
                {

                    playerCube = transform.GetChild(0).gameObject;
                    Target.transform.position = playerCube.transform.position;
                 

                }
                /*
                if (Input.GetMouseButton(0))
                {
                    Vector2 curTapPos = Input.mousePosition;

                    if (lastTapPos == Vector2.zero)
                    {
                        lastTapPos = curTapPos;
                        Target.SetActive(true);
                    }
                    float alpha = lastTapPos.y - curTapPos.y;
                    float delta = lastTapPos.x - curTapPos.x;
                    lastTapPos = curTapPos;


                        playerCube.transform.position += (Vector3.right * -delta * Time.deltaTime * .35f);
                  

                }
                if (Input.GetMouseButtonUp(0))
                {
                   
                            FindObjectOfType<MusicVibrate>().swoosh.Play();
                            StartCoroutine(playerCube.GetComponent<cube>().outcheck());
                            playerCube.GetComponent<cube>().rb.velocity = Vector3.forward * 18;
                    FindObjectOfType<cubeInsantiater>().InstantiateSwipeCube();
                            playerCube.transform.SetParent(GameObject.Find("/CubeInsantiater/").transform);
                          
                            playerCube.GetComponent<TrailRenderer>().enabled = true;
                    Target.SetActive(false);

                      
                    
                   

                    lastTapPos = Vector2.zero;
                }
                */
              
                if (Input.GetMouseButton(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.RaycastNonAlloc(ray,hit,100,layer)>0)
                    {
                        if (!shoot)
                        {
                            shoot = true;
                            Target.SetActive(true);
                        }
                        tempPos = Mathf.Clamp(hit[0].point.x, -1.8f, 1.8f);
                        playerCube.transform.position = new Vector3(tempPos, playerCube.transform.position.y,playerCube.transform.position.z);
                        Target.transform.position = new Vector3(playerCube.transform.position.x, 1, 1);
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (shoot)
                    {
                        TexasHoldim.SetActive(false);
                        shoot = false;
                        FindObjectOfType<MusicVibrate>().swoosh.Play();
                        StartCoroutine(playerCube.GetComponent<cube>().outcheck());
                        playerCube.GetComponent<cube>().rb.velocity = Vector3.forward * 18;
                        cubeInstantiater.InstantiateSwipeCube(tempPos);
                        playerCube.transform.SetParent(cubeInstantiater.transform);

                        playerCube.GetComponent<TrailRenderer>().enabled = true;
                        Target.SetActive(false);
                    }

                }

                //   playerCube.transform.position = new Vector3(Mathf.Clamp(playerCube.transform.position.x, -1.8f, 1.8f), playerCube.transform.position.y, Mathf.Clamp(playerCube.transform.position.z, -4.25f, -3));
                // Target.transform.position = new Vector3(playerCube.transform.position.x, 1, 1);
            }

        }

    }
    
}

