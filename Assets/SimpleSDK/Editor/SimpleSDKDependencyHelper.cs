using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SimpleSDKDependencyHelper
{
    static public string xmlFile = "./Assets/SimpleSDK/Editor/Dependencies.xml";
    static public bool androidUsePayment()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFile);
        {
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/dependencies/androidPackages/androidPackage");
            foreach (XmlNode one in nodes)
            {
                string x = one.Attributes["spec"].Value;
                if (x.Contains("SimpleNativeUserPayment"))
                {
                    Debug.Log("find SimpleNativeUserPayment in dependency ");
                    return true;
                }
            }
        }
        return false;
    }

    static public bool iosUsePayment()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFile);
        {
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/dependencies/iosPods/iosPod");
            foreach (XmlNode one in nodes)
            {
                string x = one.Attributes["name"].Value;
                if (x.Contains("SimpleNativeV2UserPaymentIOSSDK"))
                {
                    Debug.Log("find SimpleNativeV2UserPaymentIOSSDK in dependency ");
                    return true;
                }
            }
        }
        return false;
    }
}
