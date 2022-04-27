using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public static class ATTHelper
{
    [PostProcessBuild(997)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
#if UNITY_IOS
        if (buildTarget == BuildTarget.iOS)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);

            // 适配 2019.3.9.f1 后的版本
            // 多了 UnityFramework
#if UNITY_2019_3_OR_NEWER
            string unityFrameWork = pbxProject.GetUnityFrameworkTargetGuid();
            pbxProject.AddFrameworkToProject(unityFrameWork, "AppTrackingTransparency.framework", false);
            
#else
            Debug.Log("##########2018###########");
            string target = pbxProject.TargetGuidByName("Unity-iPhone");
            
            // 添加系统框架
            pbxProject.AddFrameworkToProject(target, "AppTrackingTransparency.framework", false);

#endif
            pbxProject.WriteToFile(projectPath);

            var plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            plist.root.SetString("NSUserTrackingUsageDescription", "Non-personal information, such as device unique identifiers, will be used to show you relevant ads. This allows us to improve the game better.");
            plist.WriteToFile(plistPath);
        }
#endif
    }
}

