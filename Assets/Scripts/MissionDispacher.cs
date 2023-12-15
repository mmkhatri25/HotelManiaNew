using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MissionDispacher
{
	public static Mission GetMission(string difficulty, string excludeType1 = "", string excludeType2 = "", string excludeType3 = "", string excludeType4 = "", string excludeType5 = "")
	{
		IEnumerable<MissionRaw> source = from rawMission in MissionManager.missionRawList
			where rawMission.difficulty.CompareTo(difficulty) == 0 && rawMission.missionName.CompareTo(excludeType1) != 0 && rawMission.missionName.CompareTo(excludeType2) != 0 && rawMission.missionName.CompareTo(excludeType3) != 0 && rawMission.missionName.CompareTo(excludeType4) != 0 && rawMission.missionName.CompareTo(excludeType5) != 0 && rawMission.enabled
			select rawMission;
		int index = Random.Range(0, source.Count());
		Debug.Log("source.ElementAt(index) - "+ source.ElementAt(index).missionName);
		return new Mission(source.ElementAt(index));
	}

	public static int Getbooster1Amount()
	{
		return 3;
	}

	public static int Getbooster2Amount()
	{
		return 3;
	}
}
