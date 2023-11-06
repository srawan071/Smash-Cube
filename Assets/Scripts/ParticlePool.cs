using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool : MonoBehaviour
{

    private IObjectPool<ParticlesEffects> _splashPool;
    private IObjectPool<ParticlesEffects> _explosionPool;
    private IObjectPool<ParticlesEffects> _collisionPool;
    private IObjectPool<Trail> _trailPool;

    [SerializeField]
    private ParticlesEffects _splash;
    [SerializeField]
    private ParticlesEffects _explosion;
    [SerializeField]
    private ParticlesEffects _collision;
    [SerializeField]
    private Trail _trail;
    public Vector3 PoolCapacity;
    public int TrailAmt;
    public int Splash, Explosion, Collision;
    void Start()
    {
      
        Splash = 0;
        Explosion = 1;
        Collision = 2;
        _splashPool = new ObjectPool<ParticlesEffects>(CreateSplash, null, SetRelease, actionOnDestroy: (obj) => Destroy(obj), true, 5, 10);
        _explosionPool = new ObjectPool<ParticlesEffects>(CreateExplosion, null, SetRelease, actionOnDestroy: (obj) => Destroy(obj), true, 2, 5);
        _collisionPool = new ObjectPool<ParticlesEffects>(CreateCollision, null, SetRelease, actionOnDestroy: (obj) => Destroy(obj), true, 5, 10);
        _trailPool = new ObjectPool<Trail>(CreateTrail, null, (obj)=>obj.gameObject.SetActive(false), actionOnDestroy: (obj) => Destroy(obj), true, 3,5);

        for (int i = 0; i < 5; i++)
        {
            var effect =_splashPool.Get();
            StartCoroutine(effect.SetRelease());

             effect = _collisionPool.Get();
            StartCoroutine(effect.SetRelease());

        }
        for (int i = 0; i < 2; i++)
        {
            var effect = _explosionPool.Get();
            StartCoroutine(effect.SetRelease());

            var effect1 = _trailPool.Get();
            StartCoroutine(effect1.SetRelease());

        }


    }
    private void Update()
    {
        PoolCapacity.x = _splashPool.CountInactive;
        PoolCapacity.y = _explosionPool.CountInactive;
        PoolCapacity.z = _collisionPool.CountInactive;
        TrailAmt = _trailPool.CountInactive;
    }

    private ParticlesEffects CreateSplash()
    {
        var effect = Instantiate(_splash, transform.position, Quaternion.identity);
        effect.SetPool(_splashPool);
      
        return effect;

    }
    private ParticlesEffects CreateExplosion()
    {
        var effect = Instantiate(_explosion, transform.position, Quaternion.identity);
        effect.SetPool(_explosionPool);

        return effect;

    }
    private ParticlesEffects CreateCollision()
    {
        var effect = Instantiate(_collision, transform.position, Quaternion.identity);
        effect.SetPool(_collisionPool);
       
        return effect;

    }
    private Trail CreateTrail()
    {
       // Debug.Log("TrailCreated");
        var effect = Instantiate(_trail, transform.position, Quaternion.Euler(0,0,0));
        effect.SetPool(_trailPool);

        return effect;
    }
   

  private  void SetRelease(ParticlesEffects pool)
    {
        pool.gameObject.SetActive(false);
    }
    // Update is called once per frame
  
   public ParticlesEffects Get(int type)
    {
        ParticlesEffects particles=null ;
        switch (type)
        {
            case 0:
                particles = _splashPool.Get();
                break;
            case 1:
                particles = _explosionPool.Get();
                break;
            case 2:
                particles = _collisionPool.Get();
                break;
            
      

        }
        return particles;
    }
    public Trail Get()
    {
        return _trailPool.Get();
    }
}
