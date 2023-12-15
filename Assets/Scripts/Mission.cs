using System;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class Mission
{
	public string missionName;

	public int tier;

	public string missionGroup;

	public string type;

	public string difficulty;

	public string description;

	public int goal;

	public int objFloor;

	public int expRew;

	public int coinRew;

	public int progress;

	public DateTime missionDate;

	public bool rewardsGiven;

	public Mission(MissionRaw _missionRaw)
	{
		missionName = _missionRaw.missionName;
		tier = _missionRaw.tier;
		missionGroup = _missionRaw.missionGroup;
		type = _missionRaw.type;
		difficulty = _missionRaw.difficulty;
		description = _missionRaw.description;
		goal = _missionRaw.variableA;
		objFloor = -1;
		expRew = _missionRaw.expRew;
		coinRew = _missionRaw.coinRew;
		progress = 0;
		if (_missionRaw.variableB.CompareTo("X") == 0)
		{
			objFloor = UnityEngine.Random.Range(0, 5) + 1;
			description = Regex.Replace(description, "X", objFloor.ToString());
		}
		missionDate = DateTime.Now;
	}

	public void AddProgress(int value)
	{
		if (!IsCompleted())
		{
			progress += value;
		}
	}

	public int GetProgress()
	{
		return progress;
	}

	public bool IsCompleted()
	{
		return progress >= goal;
	}

	public void ResetIfSingleAndNotCompleted()
	{
		if (type.CompareTo("Single") == 0 && !IsCompleted())
		{
			progress = 0;
		}
	}

	public float GetCompletionPropertion()
	{
		return Mathf.Clamp01((float)progress / (float)goal);
	}

	public void ForceMissionCompletion()
	{
		progress = goal;
	}
}
