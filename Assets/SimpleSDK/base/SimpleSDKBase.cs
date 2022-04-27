using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;

namespace SimpleSDKNS
{
    public class SimpleSDKBase
    {
        static public bool debug = false;
        static public string prefix = "ss:";
        static public void Log(string s)
        {
            if (debug)
            {
                Debug.Log(prefix + s);
            }
        }

        static public void Log(string s, Exception e)
        {
            if (debug)
            {
                Debug.Log(prefix + s);
                Debug.LogException(e);
            }
        }
        public static IEnumerator ICoroutine(double time = 0, Action callBack = null, bool ignoreTimeScale = false)
        {
            if (time < 0.01)
            {
                yield return null;
            }
            else if (time > 0)
            {
                if (ignoreTimeScale)
                {
                    float start = Time.realtimeSinceStartup;
                    while (Time.realtimeSinceStartup < start + time)
                    {
                        yield return null;
                    }
                }
                else
                    yield return new WaitForSeconds((float)time);
            }

            if (callBack != null)
            {
                callBack();
            }
        }
        static public int NowTimestamp()
        {
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int)span.TotalSeconds;
        }
        static public int ToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int)span.TotalSeconds;
        }
    }
}