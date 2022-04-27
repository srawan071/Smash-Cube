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
using System.Text;
using AppLovinMax.Scripts.IntegrationManager.Editor;


public class SimpleSDKSDKAdPluginiOSPostBuild
{
    private const string OutputFileName = "AppLovinQualityServiceSetup.rb";

    [PostProcessBuild(1001)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string sdkKey = SimpleSDKAdPluginHelper.GetSdkKey("ios");
            if (sdkKey == null)
            {
                Debug.Log("not find applovin sdk key " + sdkKey);
                return;
            }

            Debug.Log("find applovin sdk key " + sdkKey);

            var outputFilePath = Path.Combine(buildPath, OutputFileName);

            // Check if Quality Service is already installed.
            if (File.Exists(outputFilePath) && Directory.Exists(Path.Combine(buildPath, "AppLovinQualityService")))
            {
                // TODO: Check if there is a way to validate if the SDK key matches the script. Else the pub can't use append when/if they change the SDK Key.
                return;
            }

            // Download the ruby script needed to install Quality Service
    #if UNITY_2017_2_OR_NEWER
            var downloadHandler = new DownloadHandlerFile(outputFilePath);
    #else
                var downloadHandler = new AppLovinDownloadHandler(path);
    #endif
            var postJson = string.Format("{{\"sdk_key\" : \"{0}\"}}", sdkKey);
            var bodyRaw = Encoding.UTF8.GetBytes(postJson);
            var uploadHandler = new UploadHandlerRaw(bodyRaw);
            uploadHandler.contentType = "application/json";

            var unityWebRequest = new UnityWebRequest("https://api2.safedk.com/v1/build/ios_setup2")
            {
                method = UnityWebRequest.kHttpVerbPOST,
                downloadHandler = downloadHandler,
                uploadHandler = uploadHandler
            };

    #if UNITY_2017_2_OR_NEWER
            var operation = unityWebRequest.SendWebRequest();
    #else
                var operation = webRequest.Send();
    #endif

            // Wait for the download to complete or the request to timeout.
            while (!operation.isDone) { }

    #if UNITY_2020_1_OR_NEWER
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
    #elif UNITY_2017_2_OR_NEWER
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
    #else
                if (webRequest.isError)
    #endif
            {
                Debug.Log("AppLovin Quality Service installation failed. Failed to download script with error: " + unityWebRequest.error);
                return;
            }

            // Check if Ruby is installed
            var rubyVersion = AppLovinCommandLine.Run("ruby", "--version", buildPath);
            if (rubyVersion.ExitCode != 0)
            {
                Debug.Log("AppLovin Quality Service installation requires Ruby. Please install Ruby, export it to your system PATH and re-export the project.");
                return;
            }

            // Ruby is installed, run `ruby AppLovinQualityServiceSetup.rb`
            var result = AppLovinCommandLine.Run("ruby", OutputFileName, buildPath);

            // Check if we have an error.
            if (result.ExitCode != 0) Debug.Log("Failed to set up AppLovin Quality Service");

            Debug.Log(result.Message);
    
        }
    }

}

#endif