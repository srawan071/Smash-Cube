using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
    [Serializable]
    public class State
    {
        public int code;
        public string msg;
        public State(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }
    }
}