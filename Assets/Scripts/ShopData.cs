using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/Shopdatamanager")]
public class ShopData : ScriptableObject
{

   
    [SerializeField]
    public SavedSkinData SavedSkindata;
  
    public int ActiveSide;
    public int SkinSide;
    public int ActiveSkin;
    private string _datapath;

    public void SaveData()
    {
        _datapath = Application.persistentDataPath +  "/SkinData.json";
        var serializer = new XmlSerializer(typeof(SavedSkinData));
        var stream = new FileStream(_datapath, FileMode.Create);
        serializer.Serialize(stream, SavedSkindata);
        stream.Close();
        SetCurrentSkinData();

    }
    public void LoadData()
    {
        _datapath = Application.persistentDataPath + "/SkinData.json";
        if (System.IO.File.Exists(_datapath))
        {
            var serializer = new XmlSerializer(typeof(SavedSkinData));
            var stream = new FileStream(_datapath, FileMode.Open);
            SavedSkindata = serializer.Deserialize(stream) as SavedSkinData;
            stream.Close();
           
          //  Debug.Log("Loaded");
        }
        GetCurrentSkinData();
    }
    public void GetCurrentSkinData()
    {
      ActiveSide = 0;
       ActiveSkin = PlayerPrefs.GetInt("ActiveSkin");
        SkinSide= PlayerPrefs.GetInt("SkinSide");
    }
    public void SetCurrentSkinData()
    {
        //PlayerPrefs.SetInt("ActiveSide", ActiveSide);
        PlayerPrefs.SetInt("ActiveSkin", ActiveSkin);
        PlayerPrefs.SetInt("SkinSide", SkinSide);
    }
    public void Erase()
    {
        _datapath = Application.persistentDataPath + "/SkinData.json";
        if (System.IO.File.Exists(_datapath))
        {
            File.Delete(_datapath);

        }

    }
    public void Reset()
    {
       // Debug.Log("Reset");
        Erase();
        SavedSkindata.LockedNormalIndexs.Clear();
        SavedSkindata.LockedEpicIndexs.Clear();

       for (int i = 0; i < 9; i++)
        {
            SavedSkindata.LockedNormalIndexs.Add(i);
            SavedSkindata.LockedEpicIndexs.Add(i);
        }
        SavedSkindata.LockedNormalIndexs.RemoveAt(0);
        PlayerPrefs.SetInt("ActiveSide", 0);
        PlayerPrefs.SetInt("ActiveSkin", 0);
        PlayerPrefs.SetInt("SkinSide", 0);
        GetCurrentSkinData();
    }

}

    [System.Serializable]
    public class SavedSkinData
    {
        public List<int> LockedNormalIndexs;
        public List<int> LockedEpicIndexs;
    }
