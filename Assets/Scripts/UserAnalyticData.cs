using System;
using UnityEngine;

[Serializable]
public struct UserAnalyticData
{
	public string userId;

	public int sessionsPlayed;

	public int userMatchesPlayed;

	public int videoAdsWatchedTotal;

	public string userCountry;

	public int highestStage;

	public int amountOfHighscores;

	public bool firstSession;

	public string firstTimestamp;

	public bool firstLoading;

	public bool firstStartScreen;

	public bool firstPlayButton;

	public bool firstGameOver;

	public bool firstTimeEnterLevel;

	public bool firstTimePlayTutorial;

	public bool firstTimeStopSpawnBooster;

	public bool firstTimeCleanFloorBooster;

	public bool firstTimeMissionMenu;

	public bool firstTimeCatalogueMenu;

	public bool firstTimeCustomizationMenu;

	public bool firstTimeGachaButton;

	public bool firstTimeShareButton;

	public void Init()
	{
		userId = SystemInfo.deviceUniqueIdentifier;
		firstSession = true;
		firstLoading = true;
		firstStartScreen = true;
		firstPlayButton = true;
		firstGameOver = true;
		firstTimePlayTutorial = true;
		firstTimeStopSpawnBooster = true;
		firstTimeCleanFloorBooster = true;
		firstTimeMissionMenu = true;
		firstTimeCatalogueMenu = true;
		firstTimeCustomizationMenu = true;
		firstTimeGachaButton = true;
	}
}
