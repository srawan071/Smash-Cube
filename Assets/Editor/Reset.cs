using UnityEditor;
using UnityEngine;


public  class Reset:MonoBehaviour
{

    [MenuItem("Clear Data/clear")]
    public  static void ClearData()
    {
       
      //  Reset reset = new Reset();

      
        PlayerPrefs.DeleteAll();
        FindAnyObjectByType<GameData>().Erase();
        FindFirstObjectByType<cubeInsantiater>()._shopData.Reset();
      
    }

   
}
