using UnityEngine;

[CreateAssetMenu(fileName = "GameVars", menuName = "Game/GameVars")]
public class GameVars : ScriptableObject
{
	[Header("Difficulty Curve")]
	public AnimationCurve spawnCurve;

	public float specialCharProb = 5f;

	public int maxCharsWithoutSpecial = 30;

	[Header("Snap Stats")]
	public float perfectSnapMultiplier = 3f;

	public float goodSnapMultiplier = 2f;

	[Header("WildcardEvents")]
	public float minTimeForEvent = 45f;

	public float maxTimeForEvent = 60f;

	[Header("Others")]
	public int positionsPerFloor = 7;

	public int numberOfCharsForFastMusic = 5;

	[Header("Shop")]
	public int floorHardCurrencyPrice = 200;

	public int gatchaPrice = 1000;

	public int hardCurrencyRewardForAllMissionsCompleted = 5;

	public int hoursForNewMission = 3;
}
