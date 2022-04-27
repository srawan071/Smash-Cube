using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleSDKNS
{
    public enum EditorChannel
    {
        Editor,
        Organic
    }
    public class SimpleSDK : MonoBehaviour
    {

        // -- static --
        static public string SDK_VERSION = "native-v0.1.1";
        static public SimpleSDK instance;

        private bool hasInit = false;
        public EditorChannel editorChannel = EditorChannel.Editor;

        public bool editorTestMockAutoLoginReturnSucc = true;
        public bool editorTestMockCheckLoginReturnSucc = true;
        public bool editorTestMockCheckLoginDeviceReturnSucc = true;
        public List<String> editorTestMockShopItemIdsList;
        public bool editorTestMockPaySucc = true;

        public void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
        }
        public void Start()
        {
            Init();
        }

        public void Init()
        {
           this.BindCallbackManager();
           SimpleSDKConfig.GetConfig(this, RunAfterGetConfig);
        }

        private void BindCallbackManager()
        {
            var obj = FindSubObject(gameObject);
            obj.AddComponent<SimpleSDKCallback>();
        }
        private GameObject FindSubObject(GameObject go)
        {
            var trans = go.transform.Find("NativeInternalObject");
            if (trans != null)
            {
                Debug.Log("find NativeInternalObject");
                return trans.gameObject;
            }
            else
            {
                Debug.Log("add NativeInternalObject");
                var obj = new GameObject();
                obj.name = "NativeInternalObject";
                obj.transform.SetParent(go.transform);
                return obj;
            }
        }

        private void RunAfterGetConfig(String inputConfig)
        {
           if (inputConfig != null)
           {
                SimpleSDKBridgeFactory.instance.initWithConfig(inputConfig);
                hasInit = true;
            }
            else
            {
                Debug.Log("SimpleSDK fail to get any config from file or code");
            }
        }

        public void OnApplicationPause(bool isPaused)
        {
            if (hasInit)
            {
                if (isPaused)
                {
                    SimpleSDKBridgeFactory.instance.onPause();
                }
                else
                {
                    SimpleSDKBridgeFactory.instance.onResume();
                }
            }
        }

        public void SetAttributionInfoListener(Action<AttributionInfo> attributionInfoDelegate)
        {
            AttributionHelper.GetInstance().SetAttributionInfoListener(attributionInfoDelegate);
        }

        public AttributionInfo GetAttributionInfo()
        {
            return AttributionHelper.GetInstance().GetAttributionInfo();
        }
        public void Log(string eventName)
        {
            Log(eventName, null);
        }

        public void Log(string eventName, Dictionary<string, string> paramMap)
        {
            SimpleSDKBridgeFactory.instance.Log(eventName, paramMap);
        }
        //helpful log
        public void LogPaySuccess(string store, string transactionID, string productID, DateTime purchaseDate, decimal price, string priceString, string currency)
        {
            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                // GooglePlay or AppleAppStore
                {"store", store},
                {"transactionID", transactionID},
                {"productID", productID},
                {"purchaseDate", purchaseDate.ToString("yyyy-MM-dd HH:mm:ss")},
                {"currency", currency },
                {"priceString", priceString},
                {"price", price.ToString() },
                {"currency",currency},
            };
            Log("pay_success", param);
        }

        public StaticInfo GetStaticInfo()
        {
            return SimpleSDKBridgeFactory.instance.GetStaticInfo();
        }

    }
}
