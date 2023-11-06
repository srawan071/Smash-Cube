using system=System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using ProMaxUtils;
using Unity.Collections.LowLevel.Unsafe;

public class cubeInsantiater : MonoBehaviour
{
    public int Active, Inactive;
    public int score, best, bombamt, coin;
    public int Index;
    
    public Color[] Colour;
    public List<Transform> transformpos;
  //  [HideInInspector]
    public List<Cube> GroundCubes;
    
   
    Color tempCol;
 
    public Vector4[] Offset;
    public GameObject[] Shape;
   private float XPos;
    [SerializeField]
    private PlayercubeHolder _playerCubeHolder;
    private RigidbodyInterpolation _rigidIntrapolation;
    public Material a, b;
    [SerializeField]
    private ParticleSystem _splash;
    public Coroutine c_swipeCube;
    public OffsetData Offsetdata;
    public ColorData ColorData;
    public ObjectPool<Cube> CubePool;
    public ObjectPool<Cube> BombPool;
    [SerializeField]
    private Cube _cube, _bomb;
    [SerializeField]
    private SwichManager _swishManager;
    [SerializeField]
    private ParticlePool _particlePool;
    [SerializeField]
    private UIManager _uiManager;
   [SerializeField] private int[] _Numbers;

    
    public int StartNumber;

    public static event system.Action<int> OnScoreChange;

    [Header("Datas")]

   [SerializeField] private SkinData _skinData;
    [SerializeField] public ShopData _shopData;
    [SerializeField] private Cube[] _cubetype;
   
    private void Awake()
    {

        CheckSkin();
        CubePool = new ObjectPool<Cube>(CreateCubePool, null, RemoveToCubePool, actionOnDestroy: (obj) => Destroy(obj.gameObject), true, 25, 50);
        BombPool = new ObjectPool<Cube>(CreateBombPool, null, RemoveToBombPool, actionOnDestroy: (obj) => Destroy(obj.gameObject), true, 10, 20);
        CubePoolObj();
        BoomPoolObj();



    }
    public void CheckSkin()
    {
        if (_shopData.SkinSide == 0)
        {
            _cube = _cubetype[0];

            _cube._material.SetTexture("_BaseMap", _skinData.NormalSkin[_shopData.ActiveSkin]);


        }
        else
        {
            _cube = _cubetype[1];
            _cube._material.SetTexture("_BaseMap", _skinData.EpicSkin[_shopData.ActiveSkin]);

        }
    }
    public void CubePoolObj()
    {
       
        for (int j = 0; j < 20; j++)
        {
            Cube c = CubePool.Get();
            StartCoroutine(c.SetCubeRelease());

        }
    }
    public void BoomPoolObj()
    {
       
        for (int b = 0; b < 2; b++)
        {

            Cube c = BombPool.Get();
            StartCoroutine(c.SetBombRelease());

        }
    }
    IEnumerator Start()
    {
      
        GetNumbers();
        XPos = .25f;
       
        yield return null;
        InsantiateNORmalCube(_Numbers[Random.Range(0, _Numbers.Length)]);
    }

    private void OnEnable()
    {
        StartMenu.GameStarted += OnGameStarted;
      
    }
   
    void OnGameStarted()
    {
        GetNumbers();
        if (GameData.Instance.CheckFile())
        {
            ministart();
           
        }

        else
        {
           
            InsantiatefirstCubes();
           
           
        }
    }
    private void FixedUpdate()
    {
        Active = CubePool.CountActive;
        Inactive = CubePool.CountInactive;
    }

    private Cube CreateCubePool()
    {
        Cube cube = Instantiate(_cube, transform.position, Quaternion.identity, transform.parent);
        cube.SetCubePool(CubePool);
       
        cube.name = "cube";
        return cube;
    }
    private Cube CreateBombPool()
    {
        Cube bomb = Instantiate(_bomb,transform.position, Quaternion.identity, transform.parent);
       // Cube bomb = Instantiate(_bomb);
       
        bomb.SetBombPool(BombPool);
        bomb.name = "bomb";
        return bomb;
    }
    private void RemoveToCubePool(Cube cube)
    {

        cube.check = false;
        cube.transform.SetParent(null);
      
        cube.rb.isKinematic = false;
         cube.rb.interpolation = RigidbodyInterpolation.None;
        //   cube.Trail.enabled = false;
        cube.gameObject.SetActive(false);


    }
   
    private void RemoveToBombPool(Cube cube)
    {

        cube.check = false;
        cube.transform.SetParent(null);
        //  cube.Trail.enabled = false;
        cube.gameObject.SetActive(false);


    }

   public void ministart()
    {
       
        SaveFile CURRENTSAVE = GameData.Instance.currentSave;
       

        score = CURRENTSAVE.score;
        GameManager.singleton.isPaused = true;

        Time.timeScale = 0;
        for (int i = 0; i < CURRENTSAVE.savecube.Count; i++)
        {
            InsantiateSavedCube(CURRENTSAVE.savecube[i].value, CURRENTSAVE.savecube[i].Pos, CURRENTSAVE.savecube[i].Rot);
        }
        Time.timeScale = 1;

        //InsantiatePlayerCube
/*        int value = CURRENTSAVE.PlayerCube.value;
        if (value == 1)
        {
            BOMBInsantiation();
        }
        else
        {
            InsantiateNORmalCube(value);
        }*/



    }

 
    void InsantiateSavedCube(int value, Vector3 Pos, Vector3 Rot)
    {

        Cube savedcube = null;
        if (value == -1)
        {

            savedcube = BombPool.Get();
            savedcube.transform.SetPositionAndRotation(Pos, Quaternion.Euler(Rot));
            savedcube.transform.SetParent(transform);

            savedcube.InitializeBomb(null);
           
        }
        else if (value < StartNumber - 5)
        {
            return;
        }
        else
        {

            savedcube = CubePool.Get();
            savedcube.transform.SetPositionAndRotation(Pos, Quaternion.Euler(Rot));
            savedcube.transform.SetParent(transform);
            GroundCubes.Add(savedcube);
          //  int index = GetIndex(value);
            savedcube.InitializeFirstCubes(value, Offsetdata.Offset[value]);
        }


    }
    void InsantiatefirstCubes()
    {
        int random = Random.Range(2, 7);

        int value = 0;
        for (int i = 0; i < random; i++)
        {
            //GIve Value
           

            value = _Numbers[Random.Range(0, _Numbers.Length-1)];

            //Insantiate one by one
            int y = Random.Range(0, transformpos.Count);

            //   var firstcubes=  Instantiate(cube, transformpos[y].position, Quaternion.identity);
            var firstcubes = CubePool.Get();
            firstcubes.transform.SetPositionAndRotation(transformpos[y].position, Quaternion.identity);
            firstcubes.transform.SetParent(transform);
            GroundCubes.Add(firstcubes);
            transformpos.RemoveAt(y);
          //  int index = GetIndex(value);

            firstcubes.InitializeFirstCubes(value, Offsetdata.Offset[value]);
        }
    }


    public void InsantiateshootCube(Vector3 pos, int value)
    {
        Sounds.PlaySoundSource(0);
        ProMaxsUtils.Instance.vibrate();
        if (value <= 35)
        {
           // int index = GetIndex(value);
            var shoot = CubePool.Get();
           
            shoot.transform.position = pos;
           
            shoot.InitializeShootCube(value, Offsetdata.Offset[value]);
            _swishManager.Comboo(value, pos);
            
          OnScoreChange?.Invoke(value);
         
            ParticlesEffects particle = _particlePool.Get(_particlePool.Splash);
            particle.Initialized(pos, ColorData.colors[value]);
          

        }
        else
        {
            OnScoreChange?.Invoke(value);
        }

    }

   public int GetIndex(int value)
    {
        int j = 0;
        for (int i = 2; i <= value; i *= 2)
        {
            if (i == value)
            {
                value = j;
                break;

            }
            j++;
        }
        return value;
    }
    [ContextMenu("GetNumber")]
  private void GetNumbers()
    {

        StartNumber = PlayerPrefs.GetInt("StartNumber", 5);
        _Numbers = new int[6];
        int multiple = 5;
        for(int i = 0; i < _Numbers.Length; i++)
        {
            _Numbers[i] = StartNumber-multiple;
            multiple--;
          
        }
       
    }

  
    public IEnumerator InsantiateSwipeCube()
    {


        yield return new WaitForSeconds(.25f);
        if (Random.value <= .01f)
        {

            BOMBInsantiation();

        }
        else
        {
            int value = _Numbers[Random.Range(0, _Numbers.Length)];
            InsantiateNORmalCube(value);
        }

    }
   public void InsantiateNORmalCube(int value)
    {
        var swipecube = CubePool.Get();
        swipecube.transform.SetPositionAndRotation(new Vector3(XPos, 1.5f, -3f), Quaternion.identity);
        swipecube.transform.SetParent(_playerCubeHolder.transform);
        _playerCubeHolder._playerCube = swipecube;
      //  int index = GetIndex(value);
      
        Trail trail = _particlePool.Get();

        trail.Initialized(ColorData.colors[value],swipecube.transform);


        _playerCubeHolder._playerCube.InitializeSwipeCube(value, Offsetdata.Offset[value], trail);

    }

    public IEnumerator InsantiateBomb()
    {
        // FindObjectOfType<MusicVibrate>().tap.Play();

        if (_playerCubeHolder.transform.childCount > 0 && _playerCubeHolder._playerCube.Value!=-1)
        {
            // Destroy(_playerCubeHolder._playerCube.gameObject);
            DestroyPlayerCube();
            yield return new WaitForSeconds(.1f);
            BOMBInsantiation();

        }

    }

    public void StopAllCoroutine()
    {
        StopAllCoroutines();
    }
    public void DestroyPlayerCube()
    {
        _playerCubeHolder._playerCube.trail.Release();
        if (_playerCubeHolder._playerCube.bomb)
            BombPool.Release(_playerCubeHolder._playerCube);
        else
        CubePool.Release(_playerCubeHolder._playerCube);
        if (c_swipeCube != null)
            StopCoroutine(c_swipeCube);

    }
    public void BOMBInsantiation()
    {
        var bomb = BombPool.Get();
        //  var bomb = bompool._BombPool.Get();

        bomb.transform.SetPositionAndRotation(new Vector3(XPos, 1.5f, -3f), Quaternion.identity);
        bomb.transform.SetParent(_playerCubeHolder.transform);
        _playerCubeHolder._playerCube = bomb;


        Trail trail = _particlePool.Get();

        trail.Initialized(Color.black,bomb.transform);
        _playerCubeHolder._playerCube.InitializeBomb(trail);
    }
 

    public void InstantiateSwipeCube(float pos)
    {
        XPos = pos;
        c_swipeCube = StartCoroutine(InsantiateSwipeCube());

    }


    private void OnDisable()
    {
        StartMenu.GameStarted -= OnGameStarted;
    }
}
