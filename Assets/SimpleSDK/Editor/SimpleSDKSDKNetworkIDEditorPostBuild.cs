using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#if UNITY_IOS
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityEditor.iOS.Xcode;


public class SimpleSDKSDKNetworkIDEditorPostBuild
{
    [Serializable]
    public class SkAdNetworkData
    {
        [SerializeField] public string[] SkAdNetworkIds;
    }

    private static readonly List<string> Networks = new List<string>
        {
            "AdColony",
            //"Amazon",
            "ByteDance",
            //"Chartboost",
            "Facebook",
            "Fyber",
            "Google",
            //"InMobi",
            "IronSource",
            //"Maio",
            "Mintegral",
            //"MyTarget",
            //"MoPub",
            //"Nend",
            //"Ogury",
            //"Smaato",
            "Tapjoy",
            //"TencentGDT",
            "UnityAds",
            //"VerizonAds",
            "Vungle",
            //"Yandex"
        };

    private static SkAdNetworkData GetSkAdNetworkData()
    {
        var uriBuilder = new UriBuilder("https://dash.applovin.com/docs/v1/unity_integration_manager/sk_ad_networks_info");

        // Get the list of installed ad networks to be passed up
        
        var adNetworks = string.Join(",", Networks);
        if (!string.IsNullOrEmpty(adNetworks))
        {
            uriBuilder.Query += string.Format("adnetworks={0}", adNetworks);
        }
        

        var unityWebRequest = UnityWebRequest.Get(uriBuilder.ToString());

#if UNITY_2017_2_OR_NEWER
        var operation = unityWebRequest.SendWebRequest();
#else
            var operation = unityWebRequest.Send();
#endif
        // Wait for the download to complete or the request to timeout.
        while (!operation.isDone) { }

#if UNITY_2017_2_OR_NEWER
        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
#else
            if (unityWebRequest.isError)
#endif
        {
            Debug.Log("Failed to retrieve SKAdNetwork IDs with error: " + unityWebRequest.error);
            return new SkAdNetworkData();
        }

        try
        {
            return JsonUtility.FromJson<SkAdNetworkData>(unityWebRequest.downloadHandler.text);
        }
        catch (Exception exception)
        {
            Debug.Log("Failed to parse data '" + unityWebRequest.downloadHandler.text + "' with exception: " + exception);
            return new SkAdNetworkData();
        }
    }


    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);

            var plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            if(plist.root.values.ContainsKey("SKAdNetworkItems"))
            {
                Debug.Log("ready to delete the old SKAdNetworkItems");
                plist.root.values.Remove("SKAdNetworkItems");
            }
            PlistElementArray skAdNetworkItems = plist.root.CreateArray("SKAdNetworkItems");
            var skAdNetworkIds = GetSkAdNetworkData();
            foreach (string skAdNetworkId in skAdNetworkIds.SkAdNetworkIds)
            {
                PlistElementDict dict = skAdNetworkItems.AddDict();
                dict.SetString("SKAdNetworkIdentifier", skAdNetworkId);
            }
            plist.WriteToFile(plistPath);
        }
    }

}

#endif