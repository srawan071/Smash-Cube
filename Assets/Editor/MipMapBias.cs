using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MipMapBias : MonoBehaviour
{
    [MenuItem("DevMode/Enable")]
   static void EnableDevMode()
    {
        UnityEditor.EditorPrefs.SetBool("DeveloperMode", true);
    }
    [MenuItem("DevMode/Disable")]
    private static void DisableDevMode()
    {
        UnityEditor.EditorPrefs.SetBool("DeveloperMode", false);
    }
}
