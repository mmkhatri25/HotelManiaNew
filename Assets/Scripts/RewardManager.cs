using Alg;

public static class RewardManager
{
	public static void GiveReward(string rewardType, int amount)
	{
		if (!(rewardType == "200Coins"))
		{
			if (!(rewardType == "100HardC"))
			{
				if (rewardType == "ReplaceMission")
				{
			//Singleton<AnalyticsManager>.Instance.ChangedMission(GameManager.Instance.mission_Ctl.currentMissions[amount].missionName, GameManager.Instance.mission_Ctl.currentMissions[amount].difficulty);
					GameManager.Instance.mission_Ctl.ReplaceMission(GameManager.Instance.mission_Ctl.currentMissions[amount]);
					if ((bool)Gameplay_Ctl.Instance)
					{
						Gameplay_Ctl.Instance.currentGameplaySession.replacedMissionCount++;
					}
					if (PrematchPopup.Instance != null)
					{
						PrematchPopup.Instance.SetMissionText(amount);
					}
					else if (MainMenu_UI_Ctl.Instance != null)
					{
						MainMenu_UI_Ctl.Instance.SetMissionText(amount);
					}
				}
			}
			else
			{
				PlayerDataManager.AddHardCurrency(amount);
			}
		}
		else
		{
			PlayerDataManager.AddCoins(amount);
		}
	}
}
