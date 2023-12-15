using System;
using UnityEngine;

[Serializable]
public class ReviewConditions
{
	public int sessionsWithoutCrash;

	public DateTime firstLaunchTime;

	public CrashReport lastCrashReportDetected;
}
