using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
namespace SimpleSDKNS
{
    [Serializable]
    public class StaticInfo
    {
        public string gameName="";
        public string pn = "";
        public string appVersion = "";
        public string deviceid = "";
        public string platform = "";
        public string idfa = "";
        public string uid = "";
        public string sessionId = "";

        public string idfv = "";
        public string android_id = "";

        public string band = "";
        public string model = "";
        public string deviceName = "";
        public string systemVersion = "";
        public string network = "";

        private StaticInfo() { }

        public string toJson()
        {
            //var dict = new Dictionary<string, object>();
            //dict["gameName"] = gameName;
            //dict["pn"] = pn;
            //dict["appVersion"] = appVersion;
            //dict["deviceid"] = deviceid;
            //dict["platform"] = platform;
            //dict["idfa"] = idfa;
            //dict["uid"] = uid;
            //dict["sessionId"] = sessionId;
            //dict["idfv"] = idfv;
            //dict["android_id"] = android_id;
            //dict["band"] = band;
            //dict["model"] = model;
            //dict["deviceName"] = deviceName;
            //dict["systemVersion"] = systemVersion;
            //dict["network"] = network;
            //return Json.Serialize(dict);
            return JsonUtility.ToJson(this);
        }

        static public StaticInfo fromJson(string str)
        {
            //Debug.Log("ready to from json with " + str);
            //Dictionary<string,object> dict = (Dictionary<string, object>) Json.Deserialize(str);
            //StaticInfo info = new StaticInfo();
            //info.gameName = (string)dict["gameName"];
            //info.pn = (string)dict["pn"];
            //info.appVersion = (string)dict["appVersion"];
            //info.deviceid = (string)dict["deviceid"];
            //info.platform = (string)dict["platform"];
            //info.idfa = (string)dict["idfa"];
            //info.uid = (string)dict["uid"];
            //info.sessionId = (string)dict["sessionId"];
            //info.idfv = (string)dict["idfv"];
            //info.android_id = (string)dict["android_id"];
            //info.band = (string)dict["band"];
            //info.model = (string)dict["model"];
            //info.deviceName = (string)dict["deviceName"];
            //info.systemVersion = (string)dict["systemVersion"];
            //info.network = (string)dict["network"];
            //return info;
            return JsonUtility.FromJson<StaticInfo>(str);
        }

    }
}