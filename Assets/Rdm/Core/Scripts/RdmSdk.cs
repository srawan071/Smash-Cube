using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSDKNS;

namespace RdmNS
{
    public enum RdmStatus
    {
        NOT_INIT,INITING, RDM_OPEN, RDM_CLOSE
    }
    public struct InitInfo
    {
        public string gameId;
        public string system;
        public string idfa;
        public string appVersion;
        public string deviceId;
        public string channel;
        public string packageName;
        public InitInfo(StaticInfo staticInfo, string channel)
        {
            this.gameId = staticInfo.gameName;
            this.system = RdmBase.GetSystem();
            this.idfa = staticInfo.idfa;
            this.appVersion = staticInfo.appVersion;
            this.deviceId = staticInfo.deviceid;
            this.channel = channel;
            this.packageName = staticInfo.pn;
        }
    }
    public class RdmSdk : MonoBehaviour
    {
        //global config

        static public string RULE_URL = "http://rd-server.bepicgames.com/mulitConfigV2_1";
        static public string CASH_URL = "http://rd-server.bepicgames.com/submitV2_1";
        static public string TIME_UTL = "http://rd-server.bepicgames.com/timestamp";
        static public string RDM_SDK_VERSION = "rdmv2-v0.2.3";
        static public string AES_SECRET = "audhyj23hxnADDEd";
        static private RdmSdk instance;
        static public RdmStatus rdmStatus = RdmStatus.NOT_INIT;

        //true to show 
        //center prefab56
        public InitInfo initInfo;
        public RdmConfig rdmConfig;
        private bool hasInit;

        public RdmCallbackManager rdmCallbackManager;
        public RdmAchievementManager achievementManager;
        public RdmPopManager rdmPopManager;
        private RdmApi.OpenRdmSuccess openRdmSuccess;
        private RdmApi.OpenRdmFail openRdmFail;

        //全局唯一性  
        private void Awake()
        {
            if (instance != null)
            {
                RdmBase.Log("rdm:object exists!");
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                instance = this;

                rdmCallbackManager = new RdmCallbackManager(this);
                achievementManager = new RdmAchievementManager(this);
            }
        }

        void InnerInit(InitInfo initInfo)
        {
            if (!hasInit)
            {
                this.initInfo = initInfo;

                RdmBase.Log("rdm:init the sdk object!");
                hasInit = true;
                rdmStatus = RdmStatus.INITING;
                RdmConfigManager rdmConfigManager = new RdmConfigManager(this, LoadConfigFinish, initInfo);
                rdmConfigManager.Init();
                
            }
        }
        
        public void LoadConfigFinish(RdmConfig rdmConfig)
        {
            if(rdmConfig != null)
            {
                this.rdmConfig = rdmConfig;
                //上报返现日志
                UploadLog("rdm_open", new Dictionary<string, string>()
                {
                    {"rdm_sdk_version", RDM_SDK_VERSION},
                    {"is_open", this.rdmConfig.isSuccess.ToString()},
                    {"open_times", achievementManager.GetOpenTimes().ToString()}
                });
                if (this.rdmConfig.isSuccess)
                {
                    OpenRdm();
                    rdmStatus = RdmStatus.RDM_OPEN;
                    if (openRdmSuccess != null)
                    {
                        openRdmSuccess(this.rdmConfig);
                    }
                    //上报当前状态
                    var achieves = achievementManager.GetAllAchievement();
                    var achieveStr = "";
                    if (achieves != null) {
                        achieveStr = Json.Serialize(achievementManager.GetAllAchievement());
                    }
                    UploadLog("rdm_status", new Dictionary<string, string>()
                    {
                        {"archieves", achieveStr}
                    });
                }
                else
                {
                    rdmStatus = RdmStatus.RDM_CLOSE;
                    if(openRdmFail!= null)
                    {
                        openRdmFail("did not pass the rule");
                    }
                }
            }
            else
            {
                rdmStatus = RdmStatus.RDM_CLOSE;
                if (openRdmFail != null)
                {
                    openRdmFail("did not pass the rule");
                }
            }
        }
         
        private void OpenRdm()
        {
            Debug.Log(rdmConfig);
            Debug.Log(rdmCallbackManager);
            //rdmUIInterface.OpenRdm(rdmConfig, rdmCallbackManager, achievementManager);
            rdmPopManager = new RdmPopManager(rdmConfig);
        }

        public string GetReturnType()
        {
            return rdmConfig.returnType;
        }

        static public RdmSdk GetInstance()
        {
            return instance;
        }

        //在游戏加载后调用Init进行初始化
        static public void Init(InitInfo initInfo, RdmApi.OpenRdmSuccess success, RdmApi.OpenRdmFail fail)
        {
            instance.openRdmSuccess = success;
            instance.openRdmFail = fail;
            instance.InnerInit(initInfo);
        }     

        static private void UploadLog(string eventName, Dictionary<string, string> param)
        {
            SimpleSDK.instance.Log(eventName, param);
        }

    }
}
