using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;

public class cubeInsantiater : MonoBehaviour
{
    public int score,best,bombamt,coin;
   
    public GameObject cube,rock_obj,popuptxt,Bomb,LoadSaveMenu,scorePart,coinuppart;
    public Color[] Colour;
    public List<Transform> transformpos;
    public Material rockmat, bombmat;
    public TextMeshProUGUI scoretxt, bestscore,bombtxt,coinText;
    bool loadsave;
    Color tempCol;
    float H, S, V;
    public Vector4[] Offset;
    public GameObject[] Shape;
    float XPos;
   
      void Start()
    {
        //  GameManager.singleton.Mode = PlayerPrefs.GetInt("Mode");
        if (GameManager.singleton.Mode == 0)
        {
            cube = Shape[0].gameObject;
        }
        else
        {
            cube = Shape[1].gameObject;
        }
        UpdateCoin(PlayerPrefs.GetInt("Coin"));
        
        bombamt = PlayerPrefs.GetInt("Bomb");
        UPdateBomb(0);
       
       best = PlayerPrefs.GetInt("BestScore");
        bestscore.text = "" + best;
        if (loadsave = GameData.Instance.CheckFile(loadsave))
        {
            ministart();
            LoadSaveMenu.SetActive(true);
            bestscore.GetComponent<CanvasGroup>().alpha = 0;
            scoretxt.text = "" + score;
        }
        
        else
        {

            LoadSaveMenu.SetActive(false);
            InsantiatefirstCubes();
            StartCoroutine("InsantiateSwipeCube");
        }
       
        int pos = -25;
        int i = 1;
        do
        {

            i *= 10;
            pos -= 12;
           
            if (i > best)
            {
                bestscore.transform.GetChild(0).transform.localPosition = new Vector2(pos, 0);

            }
           

        }
        while (i<=best);
       
    }
    void ministart()
    {
       // GameManager.singleton.Mode = PlayerPrefs.GetInt("Mode");
        SaveFile CURRENTSAVE=  GameData.Instance.currentSave;
       
        score = CURRENTSAVE.score;
        GameManager.singleton.isPaused= true;

        Time.timeScale = 0;
        for (int i = 0; i < CURRENTSAVE.savecube.Count; i++)
        {
            InsantiateSavedCube(CURRENTSAVE.savecube[i].value, CURRENTSAVE.savecube[i].Pos, CURRENTSAVE.savecube[i].Rot);
        }
        Time.timeScale = 1;

        //InsantiatePlayerCube
        int value = CURRENTSAVE.PlayerCube.value;
        if (value == 0)
        {
            InsantiateRock();
        }
        else if(value == 1)
        {
            BOMBInsantiation();
        }
        else
        {
            InsantiateNORmalCube(value);
        }
      var Temp = GameObject.Find("/PlayerCubeHolder").transform.GetChild(0);
        Temp.GetComponent<cube>().notjelly = true;
     

    }

    void Update()
    {
        if (score > 0)
        {
            bestscore.GetComponent<CanvasGroup>().alpha = 0;
        }
    }


    public void LoadSavebtn()
    {
        if (bombamt >= 10)
        {
            UPdateBomb(-10);
            LoadSaveMenu.SetActive(false);
            GameManager.singleton.isPaused = false;
            GameData.Instance.Erase();
            
           
        }
    }
   
   void InsantiateSavedCube(int value, Vector3 Pos ,Vector3 Rot)
    {
        GameObject go;
        if (value == 1)
        {
            go = Bomb;
        }
        else
        {
            go = cube;
        }
       
        var savedcube = Instantiate(go, Pos,Quaternion.Euler(Rot));
        savedcube.transform.SetParent(GameObject.Find("/CubeInsantiater/").transform);
        savedcube.GetComponent<cube>().swipe = true;
        savedcube.GetComponent<cube>().notjelly = true;
        if (value == 0)
        {
            savedcube.name = "rock";
            savedcube.GetComponent<Rigidbody>().mass = 2;
            savedcube.GetComponent<MeshRenderer>().material = rockmat;
        }
        else if (value == 1)
        {
            savedcube.name = "bomb";
            savedcube.GetComponent<cube>().bomb = true;
            savedcube.GetComponent<MeshRenderer>().material = bombmat;
        }
        else
        {
            savedcube.name = "" + value;
          //  savedcube.GetComponent<MeshRenderer>().material.color = kalar(value, Color.white);

            tempCol = kalar(value, Color.white);
            savedcube.GetComponent<MeshRenderer>().material.SetColor("_Edge", tempCol);
            savedcube.GetComponent<MeshRenderer>().material.SetVector("_offset", CalculateOffset(value, Vector2.zero));

            Color.RGBToHSV(tempCol, out H, out S, out V);
            S = .5f;
            V = 1;
            if (value == 128)
            {

                S = 0;
                V = .5f;
            }
            savedcube.GetComponent<MeshRenderer>().material.SetColor("_Middle", Color.HSVToRGB(H, S, V));
        }
        savedcube.GetComponent<cube>().value = value;
        savedcube.GetComponent<TrailRenderer>().enabled = false;
        savedcube.GetComponent<cube>().check = true;
        savedcube.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }
    void InsantiatefirstCubes()
    {
        int random = Random.Range(2, 7);
        for (int i = 0; i < random; i++)
        {
            //GIve Value

            int[] num = new int[] { 2, 4, 8, 16, 32 };
            int value = num[Random.Range(0, num.Length)];

            //Insantiate one by one
            int y = Random.Range(0, transformpos.Count);
            var firstcubes=  Instantiate(cube, transformpos[y].position, Quaternion.identity);
            transformpos.RemoveAt(y);
            firstcubes.transform.SetParent(GameObject.Find("/CubeInsantiater/").transform);
            firstcubes.GetComponent<cube>().swipe = true;
        
            firstcubes.name = "" + value;
            firstcubes.GetComponent<cube>().value = value;
            firstcubes.GetComponent<TrailRenderer>().enabled = false;
            tempCol = kalar(value, Color.white);
            firstcubes.GetComponent<MeshRenderer>().material.SetColor("_Edge", kalar(value, tempCol));
          
            firstcubes.GetComponent<MeshRenderer>().material.SetVector("_offset", CalculateOffset(value, Vector2.zero));
            
           Color.RGBToHSV(tempCol, out H, out S, out V);
            S = .5f;
           firstcubes.GetComponent<MeshRenderer>().material.SetColor("_Middle", Color.HSVToRGB(H, S, V));
            
            firstcubes.GetComponent<cube>().check = true;
        }
    }

   
    public void InsantiateshootCube(Vector3 pos ,int value)
    {
            if (value < 1000000)
            {
          
            
                FindObjectOfType<MusicVibrate>().score.Play();
                var shoot = Instantiate(cube, pos, Quaternion.identity);
                shoot.GetComponent<cube>().swipe = false;
                shoot.name = "" + value;
                shoot.GetComponent<cube>().value = value;
           tempCol = kalar(value, Color.white);
           shoot.GetComponent<MeshRenderer>().material.SetColor("_Edge", tempCol);
            shoot.GetComponent<MeshRenderer>().material.SetVector("_offset", CalculateOffset(value, Vector2.zero));
           
            Color.RGBToHSV(tempCol, out H, out S, out V);
            S = .5f;
            V = 1;
            if (value == 128)
            {

                S = 0;
                V = .5f;
            }
            shoot.GetComponent<MeshRenderer>().material.SetColor("_Middle", Color.HSVToRGB(H, S, V));
           
            var Popuptext = Instantiate(popuptxt, pos,Quaternion.identity);
                Popuptext.GetComponent<poptxt>().value = POPTXTVALUE(value);
                score += POPTXTVALUE(value);
                if (score > best)
                {
                    best = score;
                    PlayerPrefs.SetInt("BestScore", best);
                }
            scoretxt.text = score.ToString();
                GameManager.singleton.Givereward(value, kalar(value, Color.white));
                if (value > GameManager.singleton.bestCube)
                {
                    GameManager.singleton.bestCube = value;
                    PlayerPrefs.SetInt("BESTCUBE", value);
                }
            UpdateCoin(POPTXTVALUE(value));
             Instantiate(scorePart, pos, Quaternion.identity);
            Instantiate(coinuppart, pos, Quaternion.identity);
         //   var par = effect.GetComponent<ParticleSystem>().main;
        //    var chi = effect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        //    par.startColor = kalar(value, Color.white);
        //    chi.startColor = kalar(value, Color.white);

        }
            else
            {
                GameManager.singleton.HighestSmashmenu.SetActive(true);
                GameManager.singleton.isPaused = true;
                Time.timeScale = 0;
            }
       
    }
    int POPTXTVALUE(int val)
    {
        int j = 0;
        for(int i = 2; i <= val; i *= 2)
        {
            if (i == val)
            {
                val = j;
                break;

            }
            j++;
        }
        return val;
    }
    public Color kalar(int value, Color color)
    {

        int j = 0;
        for(int i= 2; i <= value; i *= 2)
        {
            if (i == value)
            {
                color = Colour[j];
                break;
            }
            
            j++;
        }
        return color;
       
    }
   
  public Vector2 CalculateOffset(int value,Vector2 offset)
    {
        int j = 0;
        for (int i = 2; i <= value; i *= 2)
        {
            if (i == value)
            {
                offset = Offset[j];
                break;
            }

            j++;
        }
        return offset;
    }
   
    public IEnumerator InsantiateSwipeCube()
    {
       

        yield return new WaitForSeconds(.5f);
        if (Random.value <= .01f)
        {
            
                BOMBInsantiation();
           
        }
        else
        {
            int[] num = new int[] { 2, 4, 8, 16, 32, 64 };
            int value = num[Random.Range(0, num.Length)];
            InsantiateNORmalCube(value);
        }

    }
    void InsantiateNORmalCube(int value)
    {
       
        var swipecube = Instantiate(cube, new Vector3(XPos, 1.5f, -3f), Quaternion.Euler(0,90,0));
        swipecube.GetComponent<cube>().swipe = true;
        swipecube.transform.SetParent(GameObject.Find("/PlayerCubeHolder").transform);
        swipecube.name = "" + value;
        swipecube.GetComponent<cube>().value = value;
        tempCol = kalar(value, Color.white);
        swipecube.GetComponent<MeshRenderer>().material.SetColor( "_Edge", tempCol);
        swipecube.GetComponent<MeshRenderer>().material.SetVector("_offset", CalculateOffset(value, Vector2.zero));
       
        
        Color.RGBToHSV(tempCol,out H ,out S,out V);
        S = .5f;
        swipecube.GetComponent<MeshRenderer>().material.SetColor("_Middle", Color.HSVToRGB(H,S,V));
        
        FindObjectOfType<PlayercubeHolder>().GetChild();
        ColorTargetballs(kalar(value, Color.white));

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(kalar(value, Color.white), 0.0f), new GradientColorKey(kalar(value,Color.white),.5f),new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f),new GradientAlphaKey(0, 1f) }
        );
        swipecube.GetComponent<TrailRenderer>().colorGradient = gradient;
    }
     void InsantiateRock()
    {
        var rock =Instantiate(rock_obj, new Vector3(0.0001f, 1.5f, -3f), Quaternion.identity);
        rock.GetComponent<cube>().swipe = true;
        rock.transform.SetParent(GameObject.Find("/PlayerCubeHolder").transform);
        rock.GetComponent<cube>().value=0;
        rock.name = "rock";
        rock.GetComponent<MeshRenderer>().material = rockmat;
        rock.GetComponent<Rigidbody>().mass = 2;
        FindObjectOfType<PlayercubeHolder>().GetChild();
        ColorTargetballs(rockmat.color);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.grey, 0.0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(0, 1f) }
        );
        rock.GetComponent<TrailRenderer>().colorGradient = gradient;

    }
    public IEnumerator InsantiateBomb()
    { 
        FindObjectOfType<MusicVibrate>().tap.Play();
        var PCH = FindObjectOfType<PlayercubeHolder>();
       if(PCH.transform.childCount>0&& PCH.playerCube.name!="rock" && PCH.playerCube.name!= "bomb"&& bombamt > 0) 
        {
            Destroy(PCH.playerCube);
           
            yield return new WaitForSeconds(.1f);
            StopCoroutine("InsantiateSwipeCube");
           BOMBInsantiation();
            UPdateBomb(-1);
        }

    }
    void BOMBInsantiation()
    {

        var bomb = Instantiate(Bomb, new Vector3(XPos, 1.5f, -3f), Quaternion.identity);
        bomb.GetComponent<cube>().swipe = true;
        bomb.GetComponent<cube>().bomb = true;
        bomb.GetComponent<cube>().notjelly = true;
        bomb.transform.SetParent(GameObject.Find("/PlayerCubeHolder").transform);
        bomb.GetComponent<cube>().value = 1;
        bomb.name = "bomb";
     //   bomb.GetComponent<MeshRenderer>().material = bombmat;
        FindObjectOfType<PlayercubeHolder>().GetChild();
        ColorTargetballs(bombmat.color);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(bombmat.color, 0.0f), new GradientColorKey(Color.white, .7f) },
            new GradientAlphaKey[] { new GradientAlphaKey(.001f, 0.0f), new GradientAlphaKey(0, 1f) }
        );
        bomb.GetComponent<TrailRenderer>().colorGradient = gradient;
    }
    public void UPdateBomb(int amt)
    {
        bombamt += amt;
        bombtxt.text = "" + bombamt;
        if (bombamt >= 1000)
        {
            bombtxt.text = "" + bombamt/1000+"k+";
        }
        PlayerPrefs.SetInt("Bomb", bombamt);

        if (bombamt < 10)
        {
            bombtxt.fontSize = 23;
        }
        else if (bombamt < 100)
        {
            bombtxt.fontSize = 18;
        }
        else if(bombamt<1000)
        {
            bombtxt.fontSize = 13;
        }
        else
        {
            bombtxt.fontSize = 15;
        }
    }
    void UpdateCoin(int amt)
    {
        coin += amt;
        coinText.text = coin.ToString();
        PlayerPrefs.SetInt("Coin", coin);
    }
    public void Bombbtn()
    {
        GameManager.singleton.isPaused = true;
        StartCoroutine(InsantiateBomb());
        StartCoroutine(GameManager.singleton.InPutOn());
      
    }
    void ColorTargetballs(Color ballcolor)
    {
        targetballs[] trgball = FindObjectsOfType<targetballs>();
        foreach(targetballs targetball in trgball)
        {
            targetball.GetComponent<MeshRenderer>().material.color = ballcolor;
        }
    }
    public void InstantiateSwipeCube(float pos)
    {
        XPos = pos;
        StartCoroutine("InsantiateSwipeCube");
        
    }
    
}
