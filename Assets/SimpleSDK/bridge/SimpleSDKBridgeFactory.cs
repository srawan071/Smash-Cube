using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
	public class SimpleSDKBridgeFactory {
		static public SimpleSDKBridge instance = newBridge();
		static private SimpleSDKBridge newBridge()
        {

#if UNITY_EDITOR
            return new SimpleSDKBridgeEditor();
#elif UNITY_ANDROID
            return new SimpleSDKBridgeAndroid();
#elif UNITY_IOS
			return new SimpleSDKBridgeIOS();
#else
			return new SimpleSDKBridgeEditor();
#endif

		}
	}
}