using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.U2D;
using JetBrains.Annotations;
using System.Linq;

public class Shop : MonoBehaviour, IRewardable
{
    [SerializeField]
    public ShopData _shopData;
    [SerializeField]
    private Transform _normalContent;
    [SerializeField]
    private Transform _epicContent;
    [SerializeField]
    private Sprite btnShopBack,btnShopFront;
    [SerializeField]
    private Image _epicBtn, _cubeBtn;
    [SerializeField]
    private SpriteAtlas spriteAtlas;

    [SerializeField]
    private Image[] _normalItems, _EpicItems;
   
    public Sprite QuestionMark;
    public SkinData skndata;
    [SerializeField]
    Sprite white,orange,green;
    [SerializeField] ShowModel _model;

    int _StartNumber;

    // Start is called before the first frame update
    void Start()
    {

       
        btnShopBack = spriteAtlas.GetSprite("btn-shop-back");
        btnShopFront = spriteAtlas.GetSprite("btn-shop-front");
        white= spriteAtlas.GetSprite("blue-white-circle");
        orange = spriteAtlas.GetSprite("blue-orange-circle");
        green = spriteAtlas.GetSprite("blue-green-circle");
        QuestionMark = spriteAtlas.GetSprite("question");

        _StartNumber =  PlayerPrefs.GetInt("StartNumber", 5);
       


        _shopData.LoadData();
        _model.UpdateVisuals(Random.Range(_StartNumber - 5, _StartNumber + 1), 0);
        SetNormalContent();
        SetEpicContent();
      
    }

  
    private void SetNormalContent()
    {
        int index = _shopData.SavedSkindata.LockedNormalIndexs.Count;
       
       
        int[] indx = _shopData.SavedSkindata.LockedNormalIndexs.ToArray();
       
        for(int i = 0; i < 9; i++)
        {
            _normalContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = skndata.ShopNormalSkin[i];
            _normalContent.GetChild(i).GetComponent<Button>().interactable = true;
        }
      for(int i = 0; i < index; i++)
        {
          //  Debug.Log("NotReturned");
            _normalContent.GetChild(indx[i]).GetChild(0).GetComponent<Image>().sprite = QuestionMark;
            _normalContent.GetChild(indx[i]).GetComponent<Button>().interactable = false;
            
            
        }
        if (_shopData.SkinSide == 0)
        {
            _normalItems[_shopData.ActiveSkin].sprite = green;
        }
    }
    private void SetEpicContent()
    {
        int index = _shopData.SavedSkindata.LockedEpicIndexs.Count;
        int[] indx = new int[index];
        indx = _shopData.SavedSkindata.LockedEpicIndexs.ToArray();

        for (int i = 0; i < 9; i++)
        {
            _epicContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = skndata.ShopEpicSkin[i];
            _epicContent.GetChild(i).GetComponent<Button>().interactable = true;
        }
        for (int i = 0; i < index; i++)
        {
            _epicContent.GetChild(indx[i]).GetChild(0).GetComponent<Image>().sprite = QuestionMark;
            _epicContent.GetChild(indx[i]).GetComponent<Button>().interactable = false;

        }
        if (_shopData.SkinSide == 1)
        {
            _EpicItems[_shopData.ActiveSkin].sprite = green;
        }
    }
    public void UnlockRandomBtn()
    {
        Admanager.Instance.ShowRewardedAd(this);
        Sounds.PlayTapSound();
    }
    public void NormalBtn()
    {
      _cubeBtn.sprite = btnShopFront;
      _epicBtn.sprite = btnShopBack;
        _epicContent.gameObject.SetActive(false);
        _normalContent.gameObject.SetActive(true);
        _shopData.ActiveSide = 0;
        Sounds.PlayTapSound();
    }
    public void EpicBtn()
    {
        _epicBtn.sprite = btnShopFront;
        _cubeBtn.sprite = btnShopBack;
        _epicContent.gameObject.SetActive(true);
        _normalContent.gameObject.SetActive(false);
        _shopData.ActiveSide = 1;
        Sounds.PlayTapSound();
    }
    public void CloseBtn()
    {
        // Destroy(this);
        //SceneManager.UnloadSceneAsync(1);
       SceneManager.LoadScene(0);
        Sounds.PlayTapSound();
    }
    public void NotmalItemClick(Transform transform)
    {
        //parent transform of all btn
        _shopData.ActiveSkin = transform.GetSiblingIndex();
        _shopData.SkinSide = 0;
        UpdateItemsIcon(_normalItems[_shopData.ActiveSkin]);
        _model.UpdateVisuals(Random.Range(_StartNumber - 5, _StartNumber + 1), 0);
        Sounds.PlayTapSound();
        // Debug.Log(button.transform.GetSiblingIndex());
    }
    public void EpicItemClicked(Transform transform)
    {
        _shopData.ActiveSkin = transform.GetSiblingIndex();
        _shopData.SkinSide = 1;
        UpdateItemsIcon(_EpicItems[_shopData.ActiveSkin]);
        _model.UpdateVisuals(Random.Range(_StartNumber - 5, _StartNumber + 1), 0);
      

    }

 
  
    public IEnumerator UnlockRandom()
    {
        if (_shopData.ActiveSide == 0)
        {
           
            int random = 0;
            int index=0;
            
            List<int> tempList= _shopData.SavedSkindata.LockedNormalIndexs.ToList();
            if (_shopData.SavedSkindata.LockedNormalIndexs.Count > 0)
            {
                int j = 0;
                if (_shopData.SavedSkindata.LockedNormalIndexs.Count == 1)
                {
                 //   Debug.Log("less");
                    j = 5;
                    index = _shopData.SavedSkindata.LockedNormalIndexs[0];
                }  
                while(j<5)
                {  
                    if (tempList.Count <= 0)
                    {
                       // Debug.Log("Added");
                        tempList = _shopData.SavedSkindata.LockedNormalIndexs.ToList();
                    }

                    random = Random.Range(0, tempList.Count);
                    index = tempList[random];
                   // Debug.Log(tempList[random]);
                    tempList.RemoveAt(random);
                    _normalItems[index].sprite = orange;
                    yield return new WaitForSeconds(.25f);
                    _normalItems[index].sprite = white;
                    j++;

                }
                for(int i = 0; i < 3; i++)
                {
                    _normalItems[index].sprite = orange;
                    yield return new WaitForSeconds(.2f);
                    _normalItems[index].sprite = white;
                    yield return new WaitForSeconds(.1f);
                }
               
                // Debug.Log(index);

                _normalContent.GetChild(index).GetChild(0).GetComponent<Image>().sprite = skndata.ShopNormalSkin[index];
                _normalContent.GetChild(index).GetComponent<Button>().interactable = true;
              //s  _normalItems[index].sprite = green;
                UpdateItemsIcon(_normalItems[index]);
                _shopData.SavedSkindata.LockedNormalIndexs.Remove(index);
                _shopData.SkinSide = 0;
                _shopData.ActiveSkin = index;
            }
        }
        else
        {
            int random = 0;
            int index = 0;

            List<int> tempList = _shopData.SavedSkindata.LockedEpicIndexs.ToList();
            if (_shopData.SavedSkindata.LockedEpicIndexs.Count > 0)
            {
                int j = 0;
                if (_shopData.SavedSkindata.LockedEpicIndexs.Count == 1)
                {
                   // Debug.Log("less");
                    j = 5;
                    index = _shopData.SavedSkindata.LockedEpicIndexs[0];
                }
                while (j < 5)
                {
                    if (tempList.Count <= 0)
                    {
                      //  Debug.Log("Added");
                        tempList = _shopData.SavedSkindata.LockedEpicIndexs.ToList();
                    }

                    random = Random.Range(0, tempList.Count);
                    index = tempList[random];
                 //   Debug.Log(tempList[random]);
                    tempList.RemoveAt(random);
                    _EpicItems[index].sprite = orange;
                    yield return new WaitForSeconds(.25f);
                    _EpicItems[index].sprite = white;
                    j++;

                }
                for (int i = 0; i < 3; i++)
                {
                    _EpicItems[index].sprite = orange;
                    yield return new WaitForSeconds(.2f);
                    _EpicItems[index].sprite = white;
                    yield return new WaitForSeconds(.1f);
                }

                // Debug.Log(index);

                _epicContent.GetChild(index).GetChild(0).GetComponent<Image>().sprite = skndata.ShopEpicSkin[index];
                _epicContent.GetChild(index).GetComponent<Button>().interactable = true;
                //s  _normalItems[index].sprite = green;
                UpdateItemsIcon(_EpicItems[index]);
                _shopData.SavedSkindata.LockedEpicIndexs.Remove(index);
                _shopData.SkinSide = 1;
                _shopData.ActiveSkin = index;
            }
        }

        _model.UpdateVisuals(Random.Range(_StartNumber - 5, _StartNumber + 1), 0);
    }
   private void UpdateItemsIcon(Image image)
    {
        for(int i = 0; i < 9; i++)
        {
            _normalItems[i].sprite = white;
            _EpicItems[i].sprite = white;
        }
        image.sprite = green;
    }
   
    private void OnDisable()
    {
        _shopData.SaveData();
    }

    public void GetReward()
    {
        StartCoroutine(UnlockRandom());
    }
}

