using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProMaxUtils
{
    public class StartMenu : MonoBehaviour, IRewardable
    {
        public static event Action GameStarted;
        private Material _ModelMat,_ModelNumberMat;
       [SerializeField] private ShopData _shopData;
     [SerializeField ]  private SkinData _skinData;
        [SerializeField] private OffsetData _offsetData;
    [SerializeField  ]  private Mesh cube, sphere;
     [SerializeField]   private MeshRenderer _modelRenderer;

        [SerializeField] private MeshRenderer _modelStartNumber;
        [SerializeField]    private MeshFilter _modelFilter;
        [SerializeField] private MeshFilter _modelStartNumberFilter;

        [SerializeField]private cubeInsantiater _cubeInstianter;

        private int _baseMap;
        [SerializeField]
        private GameObject UpgradeBtn;

        private float _offset=0.003f;

      [SerializeField]  private ShowModel _bestCube;
      [SerializeField]  private ShowModel _levelUpgrade;

        [SerializeField] public GameObject _effect;
       // popup pop;

        IEnumerator Start()
        {
            yield return null;
         
           
            UpdateVisuals();
           
        }

        private void UpdateVisuals()
        {/*
            if (_shopData.SkinSide == 0)
            {
                _modelFilter.mesh = cube;
                _modelStartNumberFilter.mesh = cube;

                _ModelMat.SetTexture(_baseMap, _skinData.NormalSkin[_shopData.ActiveSkin]);
                _ModelNumberMat.SetTexture(_baseMap, _skinData.NormalSkin[_shopData.ActiveSkin]);

            }
            else
            {
                _modelFilter.mesh = sphere;
                _modelStartNumberFilter.mesh = sphere;
                _ModelMat.SetTexture(_baseMap, _skinData.EpicSkin[_shopData.ActiveSkin]);
                _ModelNumberMat.SetTexture(_baseMap, _skinData.EpicSkin[_shopData.ActiveSkin]);
            }

            _ModelMat.SetTextureOffset(_baseMap, _offsetData.Offset[GameManager.singleton.BestCube]+Vector2.one*_offset);
            _ModelNumberMat.SetTextureOffset(_baseMap, _offsetData.Offset[_cubeInstianter.StartNumber + 1] + Vector2.one * _offset);*/
            _bestCube.UpdateVisuals(GameManager.singleton.BestCube, 0);
            _levelUpgrade.UpdateVisuals(_cubeInstianter.StartNumber, 1);
            if (_cubeInstianter.StartNumber >= 29)
            {
                UpgradeBtn.SetActive(false);
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


        public void SettingBTN()
        {
          // pop.POP();
        }
        public void TapToPlay()
        {
            GameStarted?.Invoke();
            gameObject.SetActive(false);
            Sounds.PlayTapSound();

        }
        public void UpgradeStartNumberBtn()
        {
           
            Admanager.Instance.ShowRewardedAd(this);
        }
        public void OnUpgradeStartNumber()
        {
            _cubeInstianter.StartNumber++;
            _cubeInstianter.StartNumber = Mathf.Clamp(_cubeInstianter.StartNumber, 5, 29);
            Debug.Log(_cubeInstianter.StartNumber);
              PlayerPrefs.SetInt("StartNumber", _cubeInstianter.StartNumber);
            _cubeInstianter.DestroyPlayerCube();
            _cubeInstianter.InsantiateNORmalCube(_cubeInstianter.StartNumber);
            GameManager.singleton.RewardThresHold = _cubeInstianter.StartNumber+1;
            UpdateVisuals();
            _effect.SetActive(true);
            Sounds.PlaySoundSource(3);
        }
        public void ShopBtn()
        {
            Sounds.PlayTapSound();
            SceneManager.LoadScene(1);
        }

        public void GetReward()
        {
            OnUpgradeStartNumber();
        }
    }
    
}