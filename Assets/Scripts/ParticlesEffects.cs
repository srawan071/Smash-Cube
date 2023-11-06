using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlesEffects : MonoBehaviour
{
    private ParticleSystem[] _particles;
    private ParticleSystem.MainModule[] _main;
    private IObjectPool<ParticlesEffects> _pool;
    

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
        _main = new ParticleSystem.MainModule[_particles.Length];

        for (int i = 0; i < _particles.Length; i++)
        {
            _main[i] = _particles[i].main;
           
        }

    }
    public void SetPool(IObjectPool<ParticlesEffects> pool)
    {
        _pool = pool;
    }
    public IEnumerator SetRelease()
    {

        yield return null;

        _pool.Release(this);

    }

    private void ChangeColor(Color color)
    {
        for (int i = 0; i < _particles.Length; i++)
        {
           
            _main[i].startColor = color;
        }
    }
    public void Initialized(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
    public void Initialized(Vector3 pos,Color color)
    {
        transform.position = pos;
        ChangeColor(color);
        gameObject.SetActive(true);
    }

    private void OnParticleSystemStopped()
    {
        _pool.Release(this);
    }
    // Update is called once per frame
  
}
