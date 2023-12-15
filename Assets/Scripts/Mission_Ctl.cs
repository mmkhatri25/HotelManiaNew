using System;
using System.Collections.Generic;
using UnityEngine;

public class Mission_Ctl : MonoBehaviour
{
	public List<Mission> currentMissions = new List<Mission>();

	private void Awake()
	{
		currentMissions = PlayerDataManager.GetCurrentMissions();
	}

	public void CreateMissionList()
	{
		currentMissions.Add(MissionDispacher.GetMission("Easy"));
		currentMissions.Add(MissionDispacher.GetMission("Medium", currentMissions[0].missionGroup));
		currentMissions.Add(MissionDispacher.GetMission("Hard", currentMissions[0].missionGroup, currentMissions[1].missionGroup));
		PlayerDataManager.SetLastMissionTime(GetRoundedTo12Time());
		PlayerDataManager.SetCurrentMissions(currentMissions);
		PlayerDataManager.SetBooster1(MissionDispacher.Getbooster1Amount());
		PlayerDataManager.SetBooster2(MissionDispacher.Getbooster2Amount());
		PlayerDataManager.SaveData();
	}

	public void ReplaceAllMissions()
	{
		Mission mission = MissionDispacher.GetMission("Easy", currentMissions[0].missionGroup, currentMissions[1].missionGroup, currentMissions[2].missionGroup);
		Mission mission2 = MissionDispacher.GetMission("Medium", currentMissions[0].missionGroup, currentMissions[1].missionGroup, currentMissions[2].missionGroup, mission.missionGroup);
		Mission mission3 = MissionDispacher.GetMission("Hard", currentMissions[0].missionGroup, currentMissions[1].missionGroup, currentMissions[2].missionGroup, mission.missionGroup, mission2.missionGroup);
		currentMissions.Clear();
		currentMissions.Add(mission);
		currentMissions.Add(mission2);
		currentMissions.Add(mission3);
		PlayerDataManager.SetLastMissionTime(GetRoundedTo12Time());
		PlayerDataManager.SetHardCurrencyGivenForMissionPack(value: false);
		PlayerDataManager.SetCurrentMissions(currentMissions);
		PlayerDataManager.SetBooster1(3);
		PlayerDataManager.SetBooster2(3);
		PlayerDataManager.SaveData();
	}

	private DateTime GetRoundedTo12Time()
	{
		DateTime now = DateTime.Now;

        if (now.Hour < 1)
        {
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        }
        if (now.Hour >= 1 && now.Hour < 2)
        {
            return new DateTime(now.Year, now.Month, now.Day, 1, 0, 0);
        }
        if (now.Hour >= 2 && now.Hour < 3)
        {
            return new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);
        }
        if (now.Hour >= 3 && now.Hour < 4)
        {
            return new DateTime(now.Year, now.Month, now.Day, 3, 0, 0);
        }
        if (now.Hour >= 4 && now.Hour < 5)
        {
            return new DateTime(now.Year, now.Month, now.Day, 4, 0, 0);
        }
        if (now.Hour >= 5 && now.Hour < 6)
        {
            return new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
        }
        if (now.Hour >= 6 && now.Hour < 7)
        {
            return new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
        }
        if (now.Hour >= 7 && now.Hour < 8)
        {
            return new DateTime(now.Year, now.Month, now.Day, 7, 0, 0);
        }
        if (now.Hour >= 7 && now.Hour < 8)
        {
            return new DateTime(now.Year, now.Month, now.Day, 7, 0, 0);
        }
        if (now.Hour >= 7 && now.Hour < 8)
        {
            return new DateTime(now.Year, now.Month, now.Day, 7, 0, 0);
        }
        if (now.Hour >= 7 && now.Hour < 8)
        {
            return new DateTime(now.Year, now.Month, now.Day, 7, 0, 0);
        }
        if (now.Hour >= 8 && now.Hour < 9)
        {
            return new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
        }
        if (now.Hour >= 9 && now.Hour < 10)
        {
            return new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
        }
        if (now.Hour >= 10 && now.Hour < 11)
        {
            return new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
        }
        if (now.Hour >= 11 && now.Hour < 12)
        {
            return new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);
        }
        if (now.Hour >= 12 && now.Hour < 13)
        {
            return new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
        }
        if (now.Hour >= 13 && now.Hour < 14)
        {
            return new DateTime(now.Year, now.Month, now.Day, 13, 0, 0);
        }
        if (now.Hour >= 14 && now.Hour < 15)
        {
            return new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
        }
        if (now.Hour >= 15 && now.Hour < 16)
        {
            return new DateTime(now.Year, now.Month, now.Day, 15, 0, 0);
        }
        if (now.Hour >= 16 && now.Hour < 17)
        {
            return new DateTime(now.Year, now.Month, now.Day, 16, 0, 0);
        }
        if (now.Hour >= 17 && now.Hour < 18)
        {
            return new DateTime(now.Year, now.Month, now.Day, 17, 0, 0);
        }
        if (now.Hour >= 18 && now.Hour < 19)
        {
            return new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
        }
        if (now.Hour >= 19 && now.Hour < 20)
        {
            return new DateTime(now.Year, now.Month, now.Day, 19, 0, 0);
        }
        if (now.Hour >= 20 && now.Hour < 21)
        {
            return new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
        }
        if (now.Hour >= 21 && now.Hour < 22)
        {
            return new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);
        }
        if (now.Hour >= 22 && now.Hour < 23)
        {
            return new DateTime(now.Year, now.Month, now.Day, 22, 0, 0);
        }
        return new DateTime(now.Year, now.Month, now.Day, 23, 0, 0);
        //if (now.Hour < 3)
        //{
        //	return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        //}
        //if (now.Hour >= 3 && now.Hour < 6)
        //{
        //	return new DateTime(now.Year, now.Month, now.Day, 3, 0, 0);
        //}
        //if (now.Hour >= 6 && now.Hour < 9)
        //{
        //	return new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
        //}
        //if (now.Hour >= 9 && now.Hour < 12)
        //{
        //	return new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
        //}
        //if (now.Hour >= 12 && now.Hour < 15)
        //{
        //	return new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
        //}
        //if (now.Hour >= 15 && now.Hour < 18)
        //{
        //	return new DateTime(now.Year, now.Month, now.Day, 15, 0, 0);
        //}
        //if (now.Hour >= 18 && now.Hour < 21)
        //{
        //	return new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
        //}
        //return new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);
    }

	public int GetNumberOfMissionsCompleted()
	{
		int num = 0;
		for (int i = 0; i < currentMissions.Count; i++)
		{
			if (currentMissions[i].IsCompleted())
			{
				num++;
			}
		}
		return num;
	}

	private bool isCurrentMissionListPreviousHours()
	{
		DateTime lastMissionTime = PlayerDataManager.GetLastMissionTime();
		if (lastMissionTime == DateTime.MinValue)
		{
			return true;
		}
		if (lastMissionTime.AddHours(GameManager.Instance.gameVars.hoursForNewMission) > DateTime.Now)
		{
			return false;
		}
		return true;
	}

	public void ReplaceMission(Mission missionToReplace)
	{
		int num = currentMissions.IndexOf(missionToReplace);
		if (num != -1)
		{
			currentMissions[num] = MissionDispacher.GetMission(missionToReplace.difficulty, currentMissions[0].missionName, currentMissions[1].missionName, currentMissions[2].missionName);
		}
		PlayerDataManager.SetCurrentMissions(currentMissions);
		PlayerDataManager.SetLastMissionTime(GetRoundedTo12Time());
		PlayerDataManager.SaveData();
	}

	public void ResetProgressOnSingleMissions()
	{
		for (int i = 0; i < currentMissions.Count; i++)
		{
			currentMissions[i].ResetIfSingleAndNotCompleted();
		}
	}

	public int GetCoinsFromCurrentMissionsAndMarkAsDone(int index)
	{
		int num = 0;
		if (currentMissions[index].IsCompleted() && !currentMissions[index].rewardsGiven)
		{
			num += currentMissions[index].coinRew;
			currentMissions[index].rewardsGiven = true;
		}
		return num;
	}

	public void ForceMissionCompletion(int index)
	{
		currentMissions[index].ForceMissionCompletion();
	}

	public void EvaluateMissionsOnGameOver(GameplaySession session)
	{
		for (int i = 0; i < currentMissions.Count; i++)
		{
			if (currentMissions[i].missionName.CompareTo("Nonstop") == 0)
			{
				currentMissions[i].AddProgress(session.sessionLeght);
			}
			else if (currentMissions[i].missionName.CompareTo("Workload") == 0)
			{
				currentMissions[i].AddProgress(session.charactersDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Busy") == 0)
			{
				switch (currentMissions[i].objFloor)
				{
				case 1:
					currentMissions[i].AddProgress(session.charactersDeliveredFloor1);
					break;
				case 2:
					currentMissions[i].AddProgress(session.charactersDeliveredFloor2);
					break;
				case 3:
					currentMissions[i].AddProgress(session.charactersDeliveredFloor3);
					break;
				case 4:
					currentMissions[i].AddProgress(session.charactersDeliveredFloor4);
					break;
				case 5:
					currentMissions[i].AddProgress(session.charactersDeliveredFloor5);
					break;
				}
			}
			else if (currentMissions[i].missionName.CompareTo("Amity") == 0)
			{
				currentMissions[i].AddProgress(session.sameFamilyDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Kindred") == 0)
			{
				currentMissions[i].AddProgress(session.sameFloorDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Exclusive") == 0 || currentMissions[i].missionName.CompareTo("Favoritism") == 0)
			{
				currentMissions[i].AddProgress(session.specialCharactersDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Power Nap") == 0)
			{
				switch (currentMissions[i].objFloor)
				{
				case 1:
					currentMissions[i].AddProgress((int)session.elevatorStopedAtFloor0);
					break;
				case 2:
					currentMissions[i].AddProgress((int)session.elevatorStopedAtFloor1);
					break;
				case 3:
					currentMissions[i].AddProgress((int)session.elevatorStopedAtFloor2);
					break;
				case 4:
					currentMissions[i].AddProgress((int)session.elevatorStopedAtFloor3);
					break;
				case 5:
					currentMissions[i].AddProgress((int)session.elevatorStopedAtFloor4);
					break;
				}
			}
			else if (currentMissions[i].missionName.CompareTo("Supplements") == 0)
			{
				currentMissions[i].AddProgress(session.powerupsUsed);
			}
			else if (currentMissions[i].missionName.CompareTo("Straight edge") == 0)
			{
				if (session.powerupsUsed == 0 && !session.forcedGameOver)
				{
					currentMissions[i].AddProgress(1);
				}
			}
			else if (currentMissions[i].missionName.CompareTo("Hardwork") == 0 || currentMissions[i].missionName.CompareTo("Prodigy") == 0)
			{
				currentMissions[i].AddProgress(session.totalCoins);
			}
			else if (currentMissions[i].missionName.CompareTo("Eureka!") == 0)
			{
				currentMissions[i].AddProgress(session.specialScientistsDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Molten Core!") == 0)
			{
				currentMissions[i].AddProgress(session.specialRepairmenDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Love Birds") == 0)
			{
				currentMissions[i].AddProgress(session.specialDoubleSpaceDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Atlas") == 0)
			{
				currentMissions[i].AddProgress(session.specialHeavyDelivered);
			}
			else if (currentMissions[i].missionName.CompareTo("Steady") == 0 || currentMissions[i].missionName.CompareTo("Pinpoint") == 0)
			{
				currentMissions[i].AddProgress(session.perfectStopCount);
			}
			else if (currentMissions[i].missionName.CompareTo("Intervention") == 0)
			{
				currentMissions[i].AddProgress(session.eventsStoppedBeforeTimer);
			}
			else if (currentMissions[i].missionName.CompareTo("Spotless") == 0)
			{
				currentMissions[i].AddProgress(session.bestTimeWithoutMeh);
			}
			else if (currentMissions[i].missionName.CompareTo("Marathon") == 0 || currentMissions[i].missionName.CompareTo("RPM") == 0)
			{
				currentMissions[i].AddProgress((int)session.elevatorMovedMeters);
			}
			else if (currentMissions[i].missionName.CompareTo("Throuple") == 0)
			{
				currentMissions[i].AddProgress(session.newlywedsAndRepairmanSameTimeAtElevator);
			}
			else if (currentMissions[i].missionName.CompareTo("Frank & Igor") == 0)
			{
				currentMissions[i].AddProgress(session.scientistAndHaulerSameTimeAtElevator);
			}
			else if (currentMissions[i].missionName.CompareTo("Full House") == 0)
			{
				currentMissions[i].AddProgress(session.twoSpecialCharactersElevartorAtSameTimeCounter);
			}
			else if (currentMissions[i].missionName.CompareTo("Hijack") == 0)
			{
				currentMissions[i].AddProgress(session.maxTimeWithSpecialCharInElevator);
			}
			else if (currentMissions[i].missionName.CompareTo("Private Party") == 0)
			{
				currentMissions[i].AddProgress(session.elevatorFilledWithSpecialChars);
			}
		}
		PlayerDataManager.SaveData();
	}
}
