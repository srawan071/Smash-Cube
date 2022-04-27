using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RdmNS
{
    public class RdmStore
    {
        static public string keyPrefix = "RDM_";

        static public string ALL_KEYS = "RDM_all_keys";

        static private void InnerSetInt(string name, int value)
        {
            PlayerPrefs.SetInt(name, value);
            RecordKey(name, 1);
        }
        static private void InnerSetString(string name, string value)
        {
            PlayerPrefs.SetString(name, value);
            RecordKey(name, 2);
        }
        static private void InnerSetFloat(string name, float value)
        {
            PlayerPrefs.SetFloat(name, value);
            RecordKey(name, 3);
        }
        static private void InnerSetDouble(string name, double value)
        {
            var s= RdmBase.DoubleToStirng(value);
            PlayerPrefs.SetString(name, s);
            RecordKey(name, 4);
        }
        static private double InnerGetDouble(string name, double value)
        {
            var s = PlayerPrefs.GetString(name);
            if (s != null && s.Length > 0)
            {
                return RdmBase.ParseDouble(s);
            }
            else return value;
        }

        static public void RecordKey(string key, int type)
        {

            RdmBase.Log("add key " + key);
            var saveKey = key + "#" + type + ",";
            var now = PlayerPrefs.GetString(ALL_KEYS, "");
            if (now.IndexOf(saveKey) == -1)
            {
                now += saveKey;
                RdmBase.Log("save " + ALL_KEYS + " " + now);
                PlayerPrefs.SetString(ALL_KEYS, now);
            }

        }

        
        static public string GetString(string name, string defaultValue)
        {
            return PlayerPrefs.GetString(keyPrefix + name, defaultValue);
        }
        static public void SetString(string name, string value)
        {
            InnerSetString(keyPrefix + name, value);
        }
        static public int GetInt(string name, int defaultValue)
        {
            return PlayerPrefs.GetInt(keyPrefix + name, defaultValue);
        }
        static public void SetInt(string name, int value)
        {
            InnerSetInt(keyPrefix + name, value);
        }
        static public void AddInt(string name, int value)
        {
            var now = PlayerPrefs.GetInt(keyPrefix + name, 0);
            InnerSetInt(keyPrefix + name, now + value);
        }
        //static public float GetFloat(string name, float defaultValue)
        //{
        //    return PlayerPrefs.GetFloat(keyPrefix + name, defaultValue);
        //}
        //static public void SetFloat(string name, float value)
        //{
        //    InnerSetFloat(keyPrefix + name, value);
        //}
        //static public void AddFloat(string name, float value)
        //{
        //    var now = PlayerPrefs.GetFloat(keyPrefix + name, 0);
        //    InnerSetFloat(keyPrefix + name, now + value);
        //}
        static public double GetDouble(string name, double defaultValue)
        {
            return InnerGetDouble(keyPrefix + name, defaultValue);
        }
        static public void SetDouble(string name, double value)
        {
            InnerSetDouble(keyPrefix + name, value);
        }
        static public void AddDouble(string name, double value)
        {
            var nowValue = InnerGetDouble(keyPrefix + name, 0);
            var newDouble = nowValue + value;
            InnerSetDouble(keyPrefix + name, newDouble);
        }

        static public void Save()
        {
            PlayerPrefs.Save();
        }


        static public Dictionary<string, string> GetAll()
        {
            var m = new Dictionary<string, string>();
            var temp = PlayerPrefs.GetString(ALL_KEYS, "");
            var keys = temp.Split(',');
            foreach (string key in keys)
            {
                Debug.Log("add key " + key);
                if (key.Length > 0)
                {
                    var kv = key.Split('#');
                    var k = kv[0];
                    switch (int.Parse(kv[1]))
                    {
                        case 1: m.Add(k, PlayerPrefs.GetInt(k, 0).ToString()); break;
                        case 2: m.Add(k, PlayerPrefs.GetString(k, "").ToString()); break;
                        case 3:
                            {
                                var f = PlayerPrefs.GetFloat(k, 0f);
                                var fString = RdmBase.FloatToStirng(f);
                                m.Add(k, fString);
                                break;
                            }
                        case 4:
                            {
                                var f = PlayerPrefs.GetString(k, "0");
                                m.Add(k, f);
                                break;
                            }
                    }
                }
            }
            return m;
        }
    }
}