using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace SimpleSDKNS
{
    public class SimpleSDKConfig
    {
        static readonly string fileName = "SimpleSDKConfig.json";

        static private Action<string> configCallback;

        static public void GetConfig(MonoBehaviour mono, Action<string> configCallback)
        {
            SimpleSDKConfig.configCallback = configCallback;
            mono.StartCoroutine(ReadStreamAssetsFile(fileName, RunAfterReadFile));
        }

        static public void RunAfterReadFile(string fileContent)
        {
            if(configCallback != null)
            {
                Debug.Log("config load finish and callback");
                configCallback.Invoke(fileContent);
            }
        }
        static public IEnumerator ReadStreamAssetsFile(string fileName, Action<string> fileContent)
        {
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

            string result;

            if (filePath.Contains("://") || filePath.Contains(":///"))
            {
                WWW www = new WWW(filePath);
                yield return www;
                result = www.text;
            }
            else
            {
                result = System.IO.File.ReadAllText(filePath);
            }

            fileContent(result);
        }
    }
}