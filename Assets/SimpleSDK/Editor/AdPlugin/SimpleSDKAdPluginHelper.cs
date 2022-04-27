using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSDKNS;

public class SimpleSDKAdPluginHelper : MonoBehaviour
{
    public static string GetSdkKey(string system)
    {
        string configContent = System.IO.File.ReadAllText("Assets/StreamingAssets/SimpleSDKConfig.json");
        string j = configContent.Replace('\n', ' ');
        Debug.Log("simple sdk start with " + j);

        Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(j);

        string key = "androidTopOnKey";
        if(system == "ios")
        {
            key = "iosTopOnKey";
        }
        if (dict.ContainsKey(key)) return (string)dict[key];
        else return null;
    }
}
