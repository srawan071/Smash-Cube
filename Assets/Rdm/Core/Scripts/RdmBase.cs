using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Security.Cryptography;
using System.Globalization;
using SimpleSDKNS;
public class RdmBase
{
    public delegate void SuccessDelegate(string result);
    public delegate void FailDelegate(long code, string errorMsg);

    static public string GetSystem()
    {
#if UNITY_IPHONE
        return "ios";
#else
        return "android";
#endif
    }
    static public void HttpGET(MonoBehaviour mono, string url,SuccessDelegate success, FailDelegate fail)
    {
        mono.StartCoroutine(InternalHttpGet(url, success, fail));
    }
    static IEnumerator InternalHttpGet(string url,SuccessDelegate success, FailDelegate fail)
    {
        UnityWebRequest unityWeb = UnityWebRequest.Get(url);
        unityWeb.timeout = 5;
        yield return unityWeb.SendWebRequest();
        if (unityWeb.isHttpError || unityWeb.isNetworkError)
        {
            Debug.Log("rdm: http request failed");
            Debug.Log("rdm:" + unityWeb.error);
            fail(unityWeb.responseCode, unityWeb.error);
        }
        else
        {
            string result = unityWeb.downloadHandler.text;
            success(result);
        }

    }


    static public void HttpPostJson(MonoBehaviour mono, string url, string jsonBody, string aesSecret, SuccessDelegate success, FailDelegate fail)
    {
        mono.StartCoroutine(InternalHttpPost(url, jsonBody, aesSecret, success, fail));
    }
    static IEnumerator InternalHttpPost(string url, string jsonBody, string aesSecret, SuccessDelegate success, FailDelegate fail)
    {
        string encodeBody = AesEncryptorBase64(jsonBody, aesSecret);
        byte[] body = Encoding.UTF8.GetBytes(encodeBody);
        UnityWebRequest unityWeb = new UnityWebRequest(url, "POST");
        unityWeb.uploadHandler = new UploadHandlerRaw(body);
        
        unityWeb.SetRequestHeader("Content-Type", "text/plain");
        unityWeb.downloadHandler = new DownloadHandlerBuffer();
        unityWeb.timeout = 15;
        yield return unityWeb.SendWebRequest();
        if (unityWeb.isHttpError || unityWeb.isNetworkError)
        {
            RdmBase.Log("rdm: http request failed");
            RdmBase.Log("rdm:"+unityWeb.error);
            fail(unityWeb.responseCode, unityWeb.error);
        }
        else
        {
            string result = unityWeb.downloadHandler.text;
            string decodeResponse = AesDecryptorBase64(result, aesSecret);
            success(decodeResponse);
        }

    }

    static public void SaveFile(string filename, string data)
    {
        StreamWriter sw = null;
        try
        {
            string destination = Application.persistentDataPath + "/" + filename;

            FileStream fs = new FileStream(destination, FileMode.OpenOrCreate);
            sw = new StreamWriter(fs);
            sw.Write(data);
            
        }
        catch(Exception e)
        {
            RdmBase.LogException("save file to " + filename + "catch exception", e);
            //ignore
        }
        finally
        {
            if(sw != null)
            {
                try
                {
                    sw.Close();
                }
                catch {
                    //ignore
                }
            }
        }
    }

    static public string LoadFile(string filename)
    {
        StreamReader sr = null;
        try
        {
            string destination = Application.persistentDataPath + "/" + filename;
            
            if (!File.Exists(destination)) return null;

            sr = new StreamReader(destination);
            string config = sr.ReadToEnd();
   
            return config; 
        }
        catch (Exception e)
        {
            RdmBase.LogException("read file with " + filename + "catch exception", e);
            return null;
            //ignore
        }
        finally
        {
            if (sr != null)
            {
                try
                {
                    sr.Close();
                }
                catch
                {
                    //ignore
                }
            }
        }
        
    }

    static public string MD5(string ori)
    {
        // byte array representation of that string
        byte[] encodedPassword = new UTF8Encoding().GetBytes(ori);

        // need MD5 to calculate the hash
        byte[] data = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    static public string CentToString(int cent)
    {
        string s = (Math.Abs(cent) % 100).ToString();
        if (s.Length == 1) s = '0' + s;
        var i = cent / 100;
        return i.ToString() + "." + s;

    }

    static public void SetRect(GameObject obj, Vector3 pos, Vector2 widthHeight)
    {
        var rt = obj.GetComponent<RectTransform>();
        rt.localPosition = pos;
        rt.sizeDelta = widthHeight;
    }

    static public string GetTimeZone()
    {
        return DateTime.Now.ToString("%z");
    }

    //lessThan moreThan less more equal
    static public bool JudgeOp(int ori, string op, int value)
    {
        switch (op)
        {
            case "lessThan": return ori <= value;
            case "moreThan": return ori >= value;
            case "less": return ori < value;
            case "more": return ori > value;
            case "equal": return ori == value;
        }
        return false;
    }

    static public void Log(string s)
    {
        if (SimpleSDKBase.debug)
        {
            Debug.Log("rdm:" + s);
        }
    }
    static public void LogException(string s, Exception e)
    {
        if (SimpleSDKBase.debug)
        {
            Debug.Log("rdm:" + s);
            Debug.LogException(e);
        }
        
    }

    static public bool CheckEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

    static public int ToTimestamp(DateTime value)
    {
        TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        return (int)span.TotalSeconds;
    }

    static public DateTime FromTimestamp(int timestamp)
    {
        //create a new DateTime value based on the Unix Epoch
        DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        //add the timestamp to the value
        DateTime newDateTime = converted.AddSeconds(timestamp);

        //return the value in string format
        return newDateTime;
    }


    private static RijndaelManaged GetRijndaelManaged(string key)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(key);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        return rDel;
    }

    public static string AesEncryptorBase64(string EncryptStr, string Key)
    {
        try
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(EncryptStr);

            ICryptoTransform cTransform = GetRijndaelManaged(Key).CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        catch (Exception ex)
        {
            Debug.Log("aes encryptor error");
            Debug.LogException(ex);
            return null;
        }
    }

    public static string AesDecryptorBase64(string DecryptStr, string Key)
    {
        try
        {
            byte[] toEncryptArray = Convert.FromBase64String(DecryptStr);
            ICryptoTransform cTransform = GetRijndaelManaged(Key).CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);//  UTF8Encoding.UTF8.GetString(resultArray);
        }
        catch (Exception ex)
        {
            Debug.Log("aes decryptor error");
            Debug.LogException(ex);
            return null;
        }
    }
    public static string FloatToStirng(float f)
    {
        return f.ToString("R", CultureInfo.InvariantCulture);
    }
    public static string DoubleToStirng(double d)
    {
        return d.ToString("G", CultureInfo.InvariantCulture);
    }
    public static double ParseDouble(string str)
    {
        return double.Parse(str, CultureInfo.InvariantCulture);
    }
    public static int CompareDouble(double a, double b)
    {
        double diff = a - b;
        if (diff < 1e-7) return 0;
        else if (diff > 0) return 1;
        else return -1;
    }
    
}
