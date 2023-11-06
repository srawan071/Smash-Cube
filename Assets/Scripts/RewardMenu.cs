
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardMenu :MonoBehaviour,IRewardable
{
    [SerializeField]
    private TextMeshProUGUI _valueText;
    [SerializeField]
    private SkinData _skinData;
    [SerializeField]
    private ShopData _shopData;
   
  
    [SerializeField]
    private cubeInsantiater _cubeInstianter;
   
    [SerializeField]
    private Image _rewardImage;
    [SerializeField]
    private Sprite _bombSprite;
    private int _index;
   
  //  private string Type;
    [SerializeField]
    private PlayercubeHolder _pch;
    [SerializeField]
    private ShowModel _model;
    [SerializeField]
    private GamePlayMenu _gamePlayMenu;

    private int _value;
    [SerializeField] private GameObject _twinkleStars;
    private enum Type
    {
        normal,
        epic,
        bomb
    }

    private Type _type;
    public void Nothanks()
    {
        
        GameManager.singleton.isPaused = false;
        Time.timeScale = 1;
        _twinkleStars.SetActive(false);
        gameObject.SetActive(false);
        Admanager.Instance.ShowFullScreenAd();

    }
    public void ClaimBtn()
    {

        Admanager.Instance.ShowRewardedAd(this);
      
       
    }
    public void Initilized(int value)
    {
        _model.UpdateVisuals(value,0);
        _valueText.SetText(Mathf.Pow(2,(long)value+1).ToString());
        RandomChooseReward();
        _twinkleStars.SetActive(true);
        Sounds.PlaySoundSource(4);
    }

   private void RandomChooseReward()
    {

       
        if (_shopData.SavedSkindata.LockedNormalIndexs.Count > 0 && _shopData.SavedSkindata.LockedEpicIndexs.Count > 0)
        {
            if (Random.value < .3f)
            {
                _type = Type.normal;
                _index = _shopData.SavedSkindata.LockedNormalIndexs[Random.Range(0, _shopData.SavedSkindata.LockedNormalIndexs.Count)];
                _rewardImage.sprite = _skinData.ShopNormalSkin[_index];
                
            }else if (Random.value < .7f)
            {
                _type = Type.epic;
                _index = _shopData.SavedSkindata.LockedEpicIndexs[ Random.Range(0, _shopData.SavedSkindata.LockedEpicIndexs.Count)];
                _rewardImage.sprite = _skinData.ShopEpicSkin[_index];
            }
            else
            {
                _type = Type.bomb;
                _rewardImage.sprite = _bombSprite;
            }
        }
        else if(_shopData.SavedSkindata.LockedNormalIndexs.Count > 0)
        {
            if (Random.value < .7f)
            {

                _type = Type.normal;
                _index =   _shopData.SavedSkindata.LockedNormalIndexs[Random.Range(0, _shopData.SavedSkindata.LockedNormalIndexs.Count)];
                _rewardImage.sprite = _skinData.ShopNormalSkin[_index];
            }
            else
            {
                _type = Type.bomb;
                _rewardImage.sprite = _bombSprite;
            }
        }
        else if (_shopData.SavedSkindata.LockedEpicIndexs.Count > 0)
        {
            if (Random.value < .7f)
            {
                _type = Type.epic;
                _index = _shopData.SavedSkindata.LockedEpicIndexs[Random.Range(0, _shopData.SavedSkindata.LockedEpicIndexs.Count)];
                _rewardImage.sprite = _skinData.ShopEpicSkin[_index];
            }
            else
            {
                _type = Type.bomb;
                _rewardImage.sprite = _bombSprite;
            }
        }
        else
        {
            _type = Type.bomb;
            _rewardImage.sprite = _bombSprite;
        }
    }

    public void GetReward()
    {
        switch (_type)
        {
            case Type.bomb:
                //bomb++
                _gamePlayMenu.UPdateBomb(1);
              
                GameManager.singleton.isPaused = false;
                _twinkleStars.SetActive(false);
                Time.timeScale = 1;
                gameObject.SetActive(false);
                break;
            case Type.normal:
                _shopData.ActiveSkin = _index;
                _shopData.SkinSide = 0;
                _shopData.SavedSkindata.LockedNormalIndexs.Remove(_index);
                
                _shopData.SaveData();
                StartCoroutine(SkinToCube());
                break;
            case Type.epic:
                _shopData.ActiveSkin = _index;
                _shopData.SkinSide = 1;
                _shopData.SavedSkindata.LockedEpicIndexs.Remove(_index);
                _shopData.SaveData();
                StartCoroutine(SkinToCube());
                break;


        }
        
       
        
    }
   
    public IEnumerator SkinToCube()
    {
        GameData.Instance.SavingData();
        //  _cubeInstianter.CubePool.Clear();

        Cube[] cubelist = _cubeInstianter.GetComponentsInChildren<Cube>();
        foreach (Cube cube in cubelist)
        {
            if (cube.isActiveAndEnabled)
            {
                if (cube.bomb)
                    _cubeInstianter.BombPool.Release(cube);
                else
                _cubeInstianter.CubePool.Release(cube);
            }
        }
        int value = 0;
        _cubeInstianter.StopAllCoroutine();
        if (_pch._playerCube != null)
        {
            _cubeInstianter.DestroyPlayerCube();
            value = -10;
        }
        else
        {
            value = _cubeInstianter.StartNumber - 5;
        }

        _cubeInstianter.CubePool.Dispose();
        _cubeInstianter.BombPool.Dispose();
        _cubeInstianter.GroundCubes.Clear();

     
        _cubeInstianter.CheckSkin();
        _cubeInstianter.CubePoolObj();
        _cubeInstianter.BoomPoolObj();
        Time.timeScale = 1;
      

        yield return null;

        _cubeInstianter.ministart();


        if (GameData.Instance.currentSave.PlayerCube.value == -1)
        {
            _cubeInstianter.BOMBInsantiation();
            
        }

        else if (value < 0)
        {
           
            _cubeInstianter.InsantiateNORmalCube(GameData.Instance.currentSave.PlayerCube.value);
        }
        else
        {
           
            _cubeInstianter.InsantiateNORmalCube(value);
        }
      
        GameManager.singleton.isPaused = false;
        Time.timeScale = 1;
        _gamePlayMenu.UpdateSkin(GameManager.singleton.LocalBestCube);
        _twinkleStars.SetActive(false);
        gameObject.SetActive(false);
    }
   
}
