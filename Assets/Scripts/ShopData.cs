using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/Shopdatamanager")]
public class ShopData : ScriptableObject
{

   
    [SerializeField]
    public SavedSkinData SavedSkindata;
  
    public int ActiveSide;
    public int SkinSide;
    public int ActiveSkin;

    public void SaveData()
    {
        string dataPath = Application.persistentDataPath;
        var serializer = new XmlSerializer(typeof(SavedSkinData));
        var stream = new FileStream(dataPath + "/" + "SkinData" + ".save", FileMode.Create);
        serializer.Serialize(stream, SavedSkindata);
        stream.Close();
        SetCurrentSkinData();
    }
    public void LoadData()
    {
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/" + "SkinData" + ".save"))
        {
            var serializer = new XmlSerializer(typeof(SavedSkinData));
            var stream = new FileStream(dataPath + "/" + "SkinData" + ".save", FileMode.Open);
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
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/" + "SkinData" + ".save"))
        {
            File.Delete(dataPath + "/" + "SkinData" + ".save");

        }

    }
    public void Reset()
    {
        Debug.Log("Reset");
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
