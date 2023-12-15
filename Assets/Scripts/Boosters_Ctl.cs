using Alg;
using System.Collections;
using UnityEngine;

public class Boosters_Ctl : MonoBehaviour
{
	private UI_Ctl _ui_Ctl;

	public BoosterSO stopSpawnBooster;

	public BoosterSO cleanFloorBooster;

	public void Start()
	{
		_ui_Ctl = GetComponent<UI_Ctl>();
	}

	public void UseStopSpawnBuilding()
	{
		//Singleton<AnalyticsManager>.Instance.FirstTimeStopSpawnBooster();
		Gameplay_Ctl.Instance.currentGameplaySession.totalPowerupStopSpawn++;
		int num = PlayerDataManager.GetPlayerData().booster1Amount - 1;
		_ui_Ctl.StartStopSpawnButtonVisual(stopSpawnBooster.cooldownTime, num);
		StartCoroutine(DoEffectStopSpawn());
		Gameplay_Ctl.Instance.currentGameplaySession.powerupsUsed++;
		PlayerDataManager.SetBooster1(num);
	}

	public void UseCleanFloor()
	{
		//Singleton<AnalyticsManager>.Instance.FirstTimeCleanFloorBooster();
		Gameplay_Ctl.Instance.currentGameplaySession.totalPowerupCleanFloor++;
		int num = PlayerDataManager.GetPlayerData().booster2Amount - 1;
		_ui_Ctl.StartCleanFloorButtonVisual(cleanFloorBooster.cooldownTime, num);
		DoEffectCleanFloor();
		Gameplay_Ctl.Instance.currentGameplaySession.powerupsUsed++;
		PlayerDataManager.SetBooster2(num);
	}

	private IEnumerator DoEffectStopSpawn()
	{
		Gameplay_Ctl.Instance.SetAllowSpawn(value: false);
		Gameplay_Ctl.Instance.StopCoroutine(Gameplay_Ctl.Instance.charSpawnRoutine);
		PoolManager.Instance.InstantiatePooled(stopSpawnBooster.animationPrefab, Gameplay_Ctl.Instance.buildingTransform.transform.position, Quaternion.identity);
		float waitTime = stopSpawnBooster.effectTime;
		while (waitTime > 0f)
		{
			waitTime -= Time.deltaTime * Time.timeScale;
			yield return null;
		}
		Gameplay_Ctl.Instance.SetAllowSpawn(value: true);
		Gameplay_Ctl.Instance.charSpawnRoutine = StartCoroutine(Gameplay_Ctl.Instance.SpawnCharacters());
	}

	private void DoEffectCleanFloor()
	{
		Floor_Ctl mostCrowdedFloor = Gameplay_Ctl.Instance.GetMostCrowdedFloor();
		mostCrowdedFloor.ClearFloorPositions();
		PoolManager.Instance.InstantiatePooled(cleanFloorBooster.animationPrefab, mostCrowdedFloor.transform.position, Quaternion.identity, mostCrowdedFloor.transform);
		mostCrowdedFloor.EvaluateSiren();
	}
}
