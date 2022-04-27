using System;

namespace SimpleSDKNS
{
    [Serializable]
    public class CheckLoginResult
    {
        public string gameId;
        public long gameAccountId;
        public CheckLoginResult(string gameId, long gameAccountId)
        {
            this.gameId = gameId;
            this.gameAccountId = gameAccountId;
        }
    }
}