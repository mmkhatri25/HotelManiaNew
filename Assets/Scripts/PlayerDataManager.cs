using Alg;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerDataManager
{
	private static string playerDataKey;

	private static PlayerData data;

	public static int Coins => data.coins;

	public static int HardCurrency => data.hardCurrency;

	public static int TotalExp => data.exp;

	public static int HighScore
	{
		get
		{
			return data.highestScore;
		}
		set
		{
			if (data.highestScore < value)
			{
				data.highestScoreCount++;
				data.highestScore = value;
				SaveData();
			}
		}
	}

	public static event Action<int> OnCoinChanged;

	public static event Action OnHardCurrencyChanged;

	public static event Action OnBoosterAmmountModified;

	static PlayerDataManager()
	{
		playerDataKey = "PlayerData";
		if (PlayerPrefs.HasKey(playerDataKey))
		{
			data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(playerDataKey));
			return;
		}
		data.Init();
		SaveData();
	}

	public static void ClearData()
	{
		if (PlayerPrefs.HasKey(playerDataKey))
		{
			PlayerPrefs.DeleteKey(playerDataKey);
		}
		data = default(PlayerData);
		data.Init();
		SaveData();
	}

	public static PlayerData GetPlayerData()
	{
		return data;
	}

	public static void SaveData()
	{
		string value = JsonUtility.ToJson(data, prettyPrint: true);
		PlayerPrefs.SetString(playerDataKey, value);
	}

	public static void AddCoins(int amount, bool showOnUI = true)
	{
        Debug.Log("My LOG - before total_coins  - " + data.total_coins);
        Debug.Log("My LOG - win coins  - " + amount);

        data.coins += amount;
		if (amount > 0)
		{
			data.total_coins += amount;
		}
		if (showOnUI && PlayerDataManager.OnCoinChanged != null)
		{
			PlayerDataManager.OnCoinChanged(amount);
		}
        Debug.Log("My LOG - after data.total_coins  - " + data.total_coins);
        SaveData();
	}

	public static void DeductCoins(int amount)
	{
		AddCoins(-amount);
	}

	public static void AddHardCurrency(int amount)
	{
        Debug.Log("My LOG - before data.hardCurrency - " + data.hardCurrency);
        Debug.Log("My LOG - win hardcurrency  - " + amount);
        data.hardCurrency += amount;
		if (amount > 0)
		{
			data.total_hardCurrency += amount;
		}
		if (PlayerDataManager.OnHardCurrencyChanged != null)
		{
			PlayerDataManager.OnHardCurrencyChanged();
		}
		Debug.Log("My LOG - data.hardCurrency - " + data.hardCurrency);
		SaveData();
	}

	public static void DeductHardCurrency(int amount)
	{
		AddHardCurrency(-amount);
	}

	public static void SetHighscore(int score)
	{
		if (score <= data.highestScore)
		{
			UnityEngine.Debug.Log("Cannot save a highscore that is smaller than the current highscore of " + data.highestScore);
			return;
		}
		data.highestScore = score;
		if (Gameplay_Ctl.Instance != null)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.isHighScore = true;
		}
		if (Singleton<DBConnection_Ctl>.Instance.linkedFB)
		{
			//ayfabManager.Instance.UpdatePlayerStatistics(null);
		}
		SaveData();
	}

	public static void SetHasDoubler(bool value)
	{
		data.hasDoubler = value;
		SaveData();
	}

	public static void SetBooster1(int amount)
	{
		if(amount<=0)
		{
			amount = 0;
		}
		data.booster1Amount = amount;
		SaveData();
	}

	public static void SetBooster2(int amount)
	{
		if(amount<=0)
		{
			amount = 0;
		}
		data.booster2Amount = amount;
		SaveData();
	}

	public static void CreateUnlockedFloors()
	{
		data.unlockedFloors = new List<string>();
		SaveData();
	}

	public static void CreateFloorsToUnlock()
	{
		data.floorsToUnlock = new List<string>();
		SaveData();
	}

	public static List<string> GetUnlockedFloors()
	{
		return data.unlockedFloors;
	}

	public static List<string> GetFloorsToUnlock()
	{
		return data.floorsToUnlock;
	}

	public static List<string> GetPlayersHotelLayout()
	{
		if (data.playersHotelLayout.Count == 0)
		{
			data.playersHotelLayout = new List<string>
			{
				"Casino",
				"Dojo",
				"Graveyard",
				"SpaceStation",
				"Theater"
			};
		}
		return data.playersHotelLayout;
	}

	public static void SavePlayersHotelLayout(List<FloorSO> currentLayout)
	{
		data.playersHotelLayout.Clear();
		for (int i = 0; i < currentLayout.Count; i++)
		{
			data.playersHotelLayout.Add(currentLayout[i].floorName);
		}
		SaveData();
	}

	public static bool CanPlayerUnlockRoom()
	{
		if (data.floorsToUnlock.Count > 0)
		{
			return GameManager.Instance.gameVars.gatchaPrice <= Coins;
		}
		return false;
	}

	public static FloorSO UnlockRandomFloor()
	{
		List<FloorSO> list = (from a in GameManager.Instance.lockedFloors
			where !a.isSecret
			select a).ToList();
		if (list.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			FloorSO floorSO = list[index];
			data.unlockedFloors.Add(floorSO.floorName);
			data.floorsToUnlock.Remove(floorSO.floorName);
			FloorSO floorSO2 = GameManager.Instance.UnlockFloor(floorSO.floorName);
			data.neverEquippedFloors.Add(floorSO2.name);
			data.unlockedFloorsCount++;
			SaveData();
			return floorSO2;
		}
		UnityEngine.Debug.Log("No rooms to unlock");
		return null;
	}

	public static void UnlockFloor(FloorSO floorSO)
	{
		for (int i = 0; i < data.floorsToUnlock.Count; i++)
		{
			if (data.floorsToUnlock[i].CompareTo(floorSO.floorName) == 0)
			{
				data.floorsToUnlock.Remove(data.floorsToUnlock[i]);
				data.unlockedFloors.Add(floorSO.name);
				GameManager.Instance.UnlockFloor(floorSO.name);
				data.neverEquippedFloors.Add(floorSO.name);
				break;
			}
		}
		data.unlockedFloorsCount++;
		SaveData();
	}

	public static bool HasNeverEquippedFloors()
	{
		return data.neverEquippedFloors.Count > 0;
	}

	public static void RemoveIfOnNeverEquipped(string floorName)
	{
		if (data.neverEquippedFloors.Count > 0)
		{
			data.neverEquippedFloors.Remove(floorName);
			SaveData();
		}
	}

	public static bool IsUnlocked(FloorSO floorSO)
	{
		for (int i = 0; i < data.floorsToUnlock.Count; i++)
		{
			if (data.floorsToUnlock[i].CompareTo(floorSO.floorName) == 0)
			{
				return false;
			}
		}
		return true;
	}

	public static List<Mission> GetCurrentMissions()
	{
		return data.currentMissions;
	}

	public static void SetCurrentMissions(List<Mission> currentMissions)
	{
		data.currentMissions = currentMissions;
		SaveData();
	}

	public static void SetHardCurrencyGivenForMissionPack(bool value)
	{
		data.hardCurrencyGivenForMissionPack = value;
		SaveData();
	}

	public static void SetLastMissionTime(DateTime newtime)
	{
		data.lastDateChagedMission = newtime.ToString();
		SaveData();
	}

	public static DateTime GetLastMissionTime()
	{
		if (data.lastDateChagedMission.CompareTo(string.Empty) == 0 || data.lastDateChagedMission == null)
		{
			return DateTime.MinValue;
		}
		return DateTime.Parse(data.lastDateChagedMission);
	}

	public static string SerializeUserStats()
	{
		return JsonUtility.ToJson(data);
	}

	public static void SetUserStatsFromJson(string serializedUserStats)
	{
		data = JsonUtility.FromJson<PlayerData>(serializedUserStats);
	}
}
