using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RdmNS
{
	public interface RdmUIInterface
	{
		void OpenRdm(RdmConfig config, RdmCallbackManager rdmCallbackManager, RdmAchievementManager rdmAchievementManager);
		void ShowCollectPanel(string name, CanShowResult result);
		void CollectAdFinish(bool isSuccess);
	}
}