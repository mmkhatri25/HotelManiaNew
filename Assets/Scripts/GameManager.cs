using Alg;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameVars gameVars;

	public GameVars tutorialGameVars;

	public ElevatorSO elevatorStats;

	public Mission_Ctl mission_Ctl;

	public List<CharacterSO> unlockedSpecialCharacters = new List<CharacterSO>();

	public Dictionary<string, FloorSO> allFloors = new Dictionary<string, FloorSO>();

	public List<FloorSO> allFloorsSO = new List<FloorSO>();

	public List<FloorSO> unlockedFloors = new List<FloorSO>();

	public List<FloorSO> lockedFloors = new List<FloorSO>();

	public List<FloorSO> playersHotelLayout;

	public List<HotelEventSO> avariableWildCardEvents = new List<HotelEventSO>();

	public CoinAnimation_Ctl coinAnimation_Ctl;

	[HideInInspector]
	public bool introAnimPlayed;

	public bool freeGachaUsed;

	public bool firstMatchPlayed;

	public event Action OnGameOver;

	private void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
		//UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		MissionManager.Init();
		LoadAssets();
		if (PlayerPrefs.HasKey("freeGachaUsed"))
		{
			freeGachaUsed = true;
		}
		if (PlayerPrefs.HasKey("firstMatchPlayed"))
		{
			firstMatchPlayed = true;
		}
	}

	public void ChangeScene(string scene)
	{
		SceneManager.LoadSceneAsync(scene);
	}

	private void Start()
	{
		
	}

	private void LoadAssets()
	{
		LoadFloorsList();
		LoadPlayersHotelLayout();
	}

	private void LoadFloorsList()
	{
		List<FloorSO> list = new List<FloorSO>(Resources.LoadAll<FloorSO>("FloorsSO/StartingSet"));
		List<FloorSO> list2 = new List<FloorSO>(Resources.LoadAll<FloorSO>("FloorsSO/UnlockableSet"));
		allFloorsSO = new List<FloorSO>(list);
		allFloorsSO.AddRange(list2);
		for (int i = 0; i < allFloorsSO.Count; i++)
		{
			allFloors.Add(allFloorsSO[i].floorName, allFloorsSO[i]);
		}
		if (PlayerDataManager.GetUnlockedFloors() == null || PlayerDataManager.GetUnlockedFloors().Count == 0)
		{
			PlayerDataManager.CreateUnlockedFloors();
			for (int j = 0; j < list.Count; j++)
			{
				unlockedFloors.Add(list[j]);
				PlayerDataManager.GetUnlockedFloors().Add(list[j].floorName);
			}
		}
		else
		{
			for (int k = 0; k < PlayerDataManager.GetUnlockedFloors().Count; k++)
			{
				for (int l = 0; l < allFloorsSO.Count; l++)
				{
					if (allFloorsSO[l].floorName == PlayerDataManager.GetUnlockedFloors()[k])
					{
						unlockedFloors.Add(allFloorsSO[l]);
						break;
					}
				}
			}
		}
		PlayerDataManager.CreateFloorsToUnlock();
		for (int m = 0; m < list2.Count; m++)
		{
			lockedFloors.Add(list2[m]);
			PlayerDataManager.GetFloorsToUnlock().Add(list2[m].floorName);
		}
		lockedFloors = (from a in lockedFloors
			orderby (!a.isSecret) ? 0 : 1
			select a).ToList();
		for (int num = PlayerDataManager.GetFloorsToUnlock().Count - 1; num >= 0; num--)
		{
			for (int n = 0; n < unlockedFloors.Count; n++)
			{
				if (unlockedFloors[n].floorName == PlayerDataManager.GetFloorsToUnlock()[num])
				{
					PlayerDataManager.GetFloorsToUnlock().RemoveAt(num);
					//lockedFloors.RemoveAt(num);
					break;
				}
			}
		}
		PlayerDataManager.SaveData();
	}

	public FloorSO UnlockFloor(string floorName)
	{
		for (int i = 0; i < lockedFloors.Count; i++)
		{
			if (lockedFloors[i].floorName == floorName)
			{
				FloorSO floorSO = lockedFloors[i];
				//lockedFloors.Remove(floorSO);
				unlockedFloors.Add(floorSO);
				return floorSO;
			}
		}
		return null;
	}

	public void LoadPlayersHotelLayout()
	{
		playersHotelLayout = new List<FloorSO>();
		for (int i = 0; i < PlayerDataManager.GetPlayersHotelLayout().Count; i++)
		{
			for (int j = 0; j < unlockedFloors.Count; j++)
			{
				if (unlockedFloors[j].floorName == PlayerDataManager.GetPlayersHotelLayout()[i])
				{
					playersHotelLayout.Add(unlockedFloors[j]);
					break;
				}
			}
		}
	}

	public void ReplaceInPlayersHotelLayout(FloorSO floorOut, FloorSO floorIn)
	{
		int num = playersHotelLayout.IndexOf(floorOut);
		if (num != -1)
		{
			playersHotelLayout[num] = floorIn;
		}
	}

	public void GameOver()
	{
		this.OnGameOver();
	}
}
