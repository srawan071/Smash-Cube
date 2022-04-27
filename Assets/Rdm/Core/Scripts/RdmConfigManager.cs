using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

namespace RdmNS
{
    public class RdmConfigManager
    {
        public delegate void LoadConfigFinish(RdmConfig rdmConfig);

        private string fileName = "rdm_config.dat";

        private MonoBehaviour mono;
        private InitInfo initInfo;

        private LoadConfigFinish loadConfigFinish;

        private int tryTimes = 3;
        private readonly int waitSeconds = 10;

        public RdmConfigManager(MonoBehaviour mono ,LoadConfigFinish loadConfigFinish, InitInfo initInfo)
        {
            this.mono = mono;
            this.loadConfigFinish = loadConfigFinish;
            this.initInfo = initInfo;
        }

        public void Init()
        {
            LoadConfig();
        }
        public void LoadConfig()
        {
            Dictionary<string, object> body = new Dictionary<string, object>();
            body.Add("gameId", initInfo.gameId);
            body.Add("system", initInfo.system);
            body.Add("packageName", initInfo.packageName);
            body.Add("deviceId", initInfo.deviceId);
            body.Add("idfa", initInfo.idfa);
            body.Add("channel", initInfo.channel);
            body.Add("appVersion", initInfo.appVersion);

            var jsonBody = Json.Serialize(body);
            RdmBase.Log("read to send " + RdmSdk.RULE_URL + " " + jsonBody);
            RdmBase.HttpPostJson(this.mono, RdmSdk.RULE_URL, jsonBody, RdmSdk.AES_SECRET, LoadHttpSuccess, LoadHttpFail);
        }

        private void LoadHttpSuccess(string result)
        {
            var httpRdmConfig = RdmConfig.Parse(result);
            //save file and set
            if(httpRdmConfig!= null)
            {
                RdmBase.Log("save config to file "+fileName);
                RdmBase.SaveFile(fileName, result);
                this.loadConfigFinish(httpRdmConfig);
            }
            else
            {
                LoadFromFile();
            }
        }

        private void LoadHttpFail(long code, string errorMsg)
        {
            RdmBase.Log("load http config error" + code + " " + errorMsg);
            LoadFromFile();

        }
        private void LoadFromFile() {
            RdmBase.Log("load config from file "+fileName);
            var result = RdmBase.LoadFile(fileName);
            if( result != null)
            {
                var fileRdmConfig = RdmConfig.Parse(result);
                if(fileRdmConfig != null)
                {
                    RdmBase.Log("load from file success");
                    this.loadConfigFinish(fileRdmConfig);
                }
                else
                {
                    RdmBase.Log("parse file error");
                    this.LoadFailAndTryAgain();
                }
            }
            else
            {
                RdmBase.Log("load from file error");
                this.LoadFailAndTryAgain();
            }
        }

        private void LoadFailAndTryAgain()
        {
            tryTimes--;
            RdmBase.Log("wait " + waitSeconds + " seconds and try again. try times "+ tryTimes);
            if ( tryTimes > 0)
            {
                this.mono.StartCoroutine(WaitAndRun());
            }
            else
            {
                RdmBase.Log("load config fail with try all times");
                this.loadConfigFinish(null);
            }
        }

        private IEnumerator WaitAndRun()
        {
            yield return new WaitForSeconds(waitSeconds);
            this.LoadConfig() ;
        }
    }
}