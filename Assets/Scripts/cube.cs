using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;
using ProMaxUtils;
using static UnityEngine.ParticleSystem;

public class Cube : MonoBehaviour
{

    public int Value;
    public bool bomb;
    public bool check;

    public Rigidbody rb;
    [SerializeField]
    private int _colValue;
    private cubeInsantiater _cubeInstantiater;
    private int _id;
    private int _basemap;
   public Material _material;
    private Vector3 midpos;
    [SerializeField]
    private Collider _colider;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private AnimationCurve _curveSwipe,_curveShoot;
    [SerializeField]
    private bool _col;
    private ObjectPool<Cube> _cubePool;
    private ObjectPool<Cube> _bombPool;
    private bool CollideOnce;
    private ParticlePool _particlePool;
    private CameraController _cameraController;
    private ParticlesEffects _particles;
    public Trail trail;
    private int blinkValue;
    private Cube _anotherCube;
    private const string box = "box";
    // public Coroutine OutCheck;

    private void Awake()
    {
       // Camera.main.gameObject.SetActive(false);
        _id = GetInstanceID();

        _cubeInstantiater = FindObjectOfType<cubeInsantiater>();
        _cameraController = FindObjectOfType<CameraController>();
        _particlePool = FindObjectOfType<ParticlePool>();

        _material = _meshRenderer.material;

        _basemap = Shader.PropertyToID("_BaseMap");
        blinkValue = Shader.PropertyToID("_lerp");


        _col = true;
       
      
    }

    //1stTime
    public IEnumerator SetCubeRelease()
    {
        _colider.enabled = false;
        yield return null;
        _colider.enabled = true;
        _cubePool.Release(this);
    }
    public IEnumerator SetBombRelease()
    {
        _colider.enabled = false;
        yield return null;
        _colider.enabled = true;
        _bombPool.Release(this);

    }

    public void SetCubePool(ObjectPool<Cube> pool)
    {
        _cubePool = pool;
    }
    public void SetBombPool(ObjectPool<Cube> pool)
    {

        _bombPool = pool;

    }
    public void InitializeShootCube(int value, Vector2 offset)
    {
       // rb.isKinematic = false;
       // rb.interpolation = RigidbodyInterpolation.None;
        offset += Vector2.one * 0.003f;
        _colValue = 1;
        CollideOnce = false;
        
        Value = value;
        _material.SetTextureOffset(_basemap, offset);
        _material.SetFloat(blinkValue, 0);
        gameObject.SetActive(true);
        StartCoroutine(JellyCube(1f,_curveShoot));
        StartCoroutine(EnableCheck());
       
        shoot();


    }
    public void InitializeSwipeCube(int value, Vector2 offset, Trail Traill)
    {
       
        offset += Vector2.one * 0.003f;
        trail = Traill;
        //trail.transform.SetParent(transform);
     //   trail.Initialized(transform.position);
        _colValue = 1;
        CollideOnce = true;
        
        Value = value;
        _material.SetTextureOffset(_basemap, offset);
        _material.SetFloat(blinkValue, 0);
         rb.isKinematic = true;
         rb.interpolation = RigidbodyInterpolation.Interpolate;
       
        gameObject.SetActive(true);



        StartCoroutine(JellyCube(1.75f,_curveSwipe));
        if(_cubeInstantiater._shopData.SkinSide==1)
            rb.angularDrag = 10; 
    }
    public void InitializeFirstCubes(int value, Vector2 offset)
    {

        offset += Vector2.one * 0.003f;
        _colValue = 1;
        CollideOnce = true;
       
        Value = value;
        check = true;
        _material.SetTextureOffset(_basemap, offset);
        _material.SetFloat(blinkValue, 0);
        rb.velocity = Vector3.zero;
      

        gameObject.SetActive(true);
    }
    public void InitializeBomb(Trail Traill)
    {

        trail = Traill;
      //  trail.transform.SetParent(transform);
      
        _colValue = 1;
        CollideOnce = false;
      
        Value = -1;
      
        bomb = true;
        gameObject.SetActive(true);
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
 public void Initialize2X(Trail Traill)
    {
        trail = Traill;
        //  trail.transform.SetParent(transform);

        _colValue = 1;
        CollideOnce = false;

        Value = -2;

        
        gameObject.SetActive(true);
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
       
        if (check)
        {
            if (transform.position.z < -1.45f)
            {
                check = false;

                GameManager.singleton.GameOver();
               
                

            }
        }
        if (transform.position.z < -.75f && rb.velocity.z < 0)
        {
            rb.velocity = new Vector3(0, 0, 1);
        }
    }

    public void outcheck()
    {
        CollideOnce = false;
        if(this.isActiveAndEnabled)
        StartCoroutine(EnableCheck());
    }
    IEnumerator EnableCheck()
    {

        yield return new WaitForSeconds(1f);
        check = true;
    }

    IEnumerator JellyCube(float speed,AnimationCurve _curve)
    {
        transform.localScale = new Vector3(0, 0, 0);
       
        float t = 0;
       
        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime *speed);

            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, _curve.Evaluate(t));
          
            yield return null;
        }
        transform.localScale = Vector3.one;
      
    }


    

    private void shoot()
    {
        Transform target = this.transform;

        for(int i=0; i < _cubeInstantiater.GroundCubes.Count;i++)
        {
            if (_cubeInstantiater.GroundCubes[i].Value == Value)
            {
                target = _cubeInstantiater.GroundCubes[i].transform;
              
                break;
            }
        }
      

        Vector3 angle = target.position - transform.position;
     
        if (target==this.transform)
        {
            angle.x = Random.Range(-2f, 2f);
            angle.z = Random.Range(-2f, 4f);
        }
       // angle.Normalize();
        angle /= 2;
        rb.velocity = Random.value>.1f? new Vector3(angle.x, 7, angle.z): new Vector3(angle.x, Random.Range(8, 15), angle.z);
        if(_cubeInstantiater._shopData.SkinSide==0)
        rb.angularVelocity = new Vector3(2, 0, 3);
        else
        {
            rb.angularDrag = 0;
            rb.angularVelocity = new Vector3(3, 0, 4);
            StartCoroutine(ApplyAngularDrag());
          
        }

        transform.SetParent(_cubeInstantiater.transform);
        _cubeInstantiater.GroundCubes.Add(this);
       
    }
    private IEnumerator ApplyAngularDrag()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime*.01f;
            rb.angularDrag += t * 10;
            yield return null;
        }
        rb.angularDrag = 10;
    }
    private IEnumerator Blink()
    {
      
        float t = 0;
        int repeat=2;
        float speed = 8f;
        while (t<repeat)
        {
            t = Mathf.MoveTowards(t, repeat, Time.unscaledDeltaTime * speed);
            _material.SetFloat(blinkValue, (((Mathf.Sin((t - .5f) * 3) + 1) * .5f)));
         

            yield return null;
        }
      
        _material.SetFloat(blinkValue, 0);
    }
  
    private void EnableCollision()
    {
        _col = true;
    }

   
    private void OnCollisionEnter(Collision collision)
    {
       

        if (_colValue<1&&!CollideOnce)
        {
           
            if (this.isActiveAndEnabled)
           StartCoroutine(Blink());
            CollideOnce = true;
             _particles = _particlePool.Get(_particlePool.Collision);
            _particles.Initialized(transform.position);
          
            if (trail!=null&& trail.isActiveAndEnabled)
            {
                ProMaxsUtils.Instance.vibrate();
                //  Debug.Log(collision.gameObject);
                trail.Release();
                trail = null;
                
            }
           
        }
        _colValue--;

       
        if (collision.gameObject.CompareTag(box))
        {
            _anotherCube = collision.gameObject.GetComponent<Cube>();


            if (_anotherCube.Value == Value&&Value>-1)
            {
            

                if (_id > collision.gameObject.GetInstanceID())
                {
                    //Only oncss
                    midpos = (transform.position + collision.transform.position) * .5f;
                    _cubeInstantiater.InsantiateshootCube(midpos, Value +1);
                  
                }
                if (this.isActiveAndEnabled)
                {
                    _cubeInstantiater.GroundCubes.Remove(this);
                    _cubePool.Release(this);
                }

               
            }



            else if (bomb)
            {

                if (_anotherCube.Value == -1)
                {
                    if (_id > collision.gameObject.GetInstanceID())
                    {
                        //OnlyOness
                        _cameraController.ShakeCamera(.25f, .5f);

                        _particles = _particlePool.Get(_particlePool.Explosion);
                        _particles.Initialized(transform.position);
                        Sounds.PlaySoundSource(1);
                        ProMaxsUtils.Instance.vibrate();
                    }
                    //  Debug.Log("BothAreBombsWow");
                    if (this.isActiveAndEnabled)
                    
                        _bombPool.Release(this);
                   

                    return;
                }

                _particles = _particlePool.Get(_particlePool.Explosion);
                _particles.Initialized(transform.position);
               
                _cameraController.ShakeCamera(.25f, .5f);
               // Debug.Log("BombCollideToObj");
                if (this.isActiveAndEnabled)
                    _bombPool.Release(this);

                Sounds.PlaySoundSource(1);
                ProMaxsUtils.Instance.vibrate();

            }
            else if (_anotherCube.Value==-1)
            {
                //2X collide to bomb so return from here
                if (Value == -2)
                    return;


                //Object Collide To Bomb

                if (this.isActiveAndEnabled)
                {
                    _cubeInstantiater.GroundCubes.Remove(this);
                    _cubePool.Release(this);

                }
             //   Sounds.PlaySoundSource(1);

                // Debug.Log("ObjectCollideToBomb");
            }

        }
        else if (collision.gameObject.CompareTag("2X"))
        {
            if (Value == -2)
            {
                Destroy(gameObject);
                return;
            }
            //bomb collide to 2X
            if (bomb)
            {
                _particles = _particlePool.Get(_particlePool.Explosion);
                _particles.Initialized(transform.position);

                _cameraController.ShakeCamera(.25f, .5f);
               
                if (this.isActiveAndEnabled)
                    _bombPool.Release(this);

                Sounds.PlaySoundSource(1);
                ProMaxsUtils.Instance.vibrate();
            }
            else
            {
                // Cube colide to 2X
                midpos = (transform.position + collision.transform.position) * .5f;
                _cubeInstantiater.InsantiateshootCube(midpos, Value + 1);

                if (this.isActiveAndEnabled)
                {
                    _cubeInstantiater.GroundCubes.Remove(this);
                    _cubePool.Release(this);

                }
            }
            Destroy(collision.gameObject);
        }


       
    }

    public void DestroyCube()
    {
        if (this.isActiveAndEnabled)
        {
            _cubeInstantiater.GroundCubes.Remove(this);
            _cubePool.Release(this);

        }
    }

   

}