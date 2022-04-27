using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleSDKNS
{
    public enum LOGIN_TYPE
    {
        DEVICE,
        FACEBOOK,
        GOOGLE_PLAY,
        GAME_CENTER
    }

    public class LOGIN_TYPE_HELPER
    {
        static public LOGIN_TYPE GetLoginTypeWithSyste()
        {
#if UNITY_IOS
            return LOGIN_TYPE.GAME_CENTER;
#else
            return LOGIN_TYPE.GOOGLE_PLAY;
#endif
        }
    }

}