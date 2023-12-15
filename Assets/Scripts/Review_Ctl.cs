using Alg;
using System;
using UnityEngine;

public class Review_Ctl : MonoBehaviour
{
	public static Review_Ctl Instance;

	public GameObject rateGamePopup;

	public ReviewConditions reviewConditionsStatus;

	public int numberOfCollecteRoomsGoal = 2;

	public int sessionsWithoutCrashGoal = 5;

	public int numberOfHoursPlayedGoal = 12;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
		if (PlayerPrefs.HasKey("reviewConditions"))
		{
			reviewConditionsStatus = JsonUtility.FromJson<ReviewConditions>(PlayerPrefs.GetString("reviewConditions"));
		}
		else
		{
			reviewConditionsStatus = new ReviewConditions();
			PlayerPrefs.SetString("reviewConditions", JsonUtility.ToJson(reviewConditionsStatus));
		}
		if ((CrashReport.lastReport != null && reviewConditionsStatus.lastCrashReportDetected == null) || (CrashReport.lastReport != null && reviewConditionsStatus.lastCrashReportDetected != null && CrashReport.lastReport.time != reviewConditionsStatus.lastCrashReportDetected.time))
		{
			reviewConditionsStatus.lastCrashReportDetected = CrashReport.lastReport;
			reviewConditionsStatus.sessionsWithoutCrash = 0;
		}
	}

	public bool MoreThan12HoursSinceFirstLaunch()
	{
		return (DateTime.Now - reviewConditionsStatus.firstLaunchTime).TotalHours > (double)numberOfHoursPlayedGoal;
	}

	private bool CanShowPopUp()
	{
		UnityEngine.Debug.Log("Facebook linked: " + Singleton<DBConnection_Ctl>.Instance.linkedFB.ToString());
		UnityEngine.Debug.Log("MoreThan12HoursSinceFirstLaunch: " + MoreThan12HoursSinceFirstLaunch().ToString());
		UnityEngine.Debug.Log("numberOfCollecteRoomsGoal: " + (PlayerDataManager.GetUnlockedFloors().Count - 5 > numberOfCollecteRoomsGoal).ToString());
		UnityEngine.Debug.Log("sessionsWithoutCrashGoal: " + (reviewConditionsStatus.sessionsWithoutCrash >= sessionsWithoutCrashGoal).ToString());
		if (Singleton<DBConnection_Ctl>.Instance.linkedFB && MoreThan12HoursSinceFirstLaunch() && PlayerDataManager.GetUnlockedFloors().Count - 5 > numberOfCollecteRoomsGoal)
		{
			return reviewConditionsStatus.sessionsWithoutCrash >= sessionsWithoutCrashGoal;
		}
		return false;
	}

	public void CheckAndShowReviewFLow()
	{
		if (CanShowPopUp())
		{
			ShowReviewPopup();
		}
	}

	private void OnApplicationQuit()
	{
		PlayerPrefs.SetString("reviewConditions", JsonUtility.ToJson(reviewConditionsStatus));
	}

	public void ShowReviewPopup()
	{
		UnityEngine.Object.Instantiate(rateGamePopup);
	}
}
