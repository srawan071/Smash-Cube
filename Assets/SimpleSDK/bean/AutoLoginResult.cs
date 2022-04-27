using System;
namespace SimpleSDKNS
{
    [Serializable]
    public class AutoLoginResult
    {
        public string gameId;
        public long gameAccountId;
        public string loginType;
        public bool isNew;

        public AutoLoginResult(string gameId, long gameAccountId, string loginType, bool isNew)
        {
            this.gameId = gameId;
            this.gameAccountId = gameAccountId;
            this.loginType = loginType;
            this.isNew = isNew;
        }
    }
}