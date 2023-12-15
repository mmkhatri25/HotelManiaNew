using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerData
{
	public int highestScore;

	public int highestScoreCount;

	public int coins;

	public int total_coins;

	public int hardCurrency;

	public int total_hardCurrency;

	public int exp;

	public bool hasDoubler;

	public int unlockedFloorsCount;

	[SerializeField]
	public List<string> floorsToUnlock;

	[SerializeField]
	public List<string> unlockedFloors;

	[SerializeField]
	public List<string> neverEquippedFloors;

	[SerializeField]
	public List<string> playersHotelLayout;

	[SerializeField]
	public List<Mission> currentMissions;

	[SerializeField]
	public string lastDateChagedMission;

	[SerializeField]
	public bool hardCurrencyGivenForMissionPack;

	public int booster1Amount;

	public int booster2Amount;

	public void Init()
	{
		playersHotelLayout = new List<string>
		{
			"Casino",
			"Dojo",
			"Graveyard",
			"SpaceStation",
			"Theater"
		};
		currentMissions = new List<Mission>();
		neverEquippedFloors = new List<string>();
		booster1Amount = 0;
		booster2Amount = 0;
	}
}
