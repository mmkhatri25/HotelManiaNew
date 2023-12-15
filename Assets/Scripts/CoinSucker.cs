using System.Collections.Generic;
using UnityEngine;

public class CoinSucker : MonoBehaviour
{
	[SerializeField]
	private GameObject coinSucker;

	private List<Floor_Ctl> objectiveFloors;

	private List<GameObject> coinSuckerCharInstances;

	public int numberOfAffectedFloors;

	private void OnEnable()
	{
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheByTime += FinishEvent;
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheBySucess += FinishEventSucess;
		objectiveFloors = new List<Floor_Ctl>();
		coinSuckerCharInstances = new List<GameObject>();
		ApplyCoinSuckerEffect();
	}

	private void ApplyCoinSuckerEffect()
	{
		List<int> list = new List<int>
		{
			0,
			1,
			2,
			3,
			4
		};
		for (int i = 0; i < numberOfAffectedFloors; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			list.RemoveAt(index);
			Gameplay_Ctl.Instance.floors[index].points = 0;
			objectiveFloors.Add(Gameplay_Ctl.Instance.floors[index]);
			GameObject gameObject = PoolManager.Instance.InstantiatePooled(coinSucker, Gameplay_Ctl.Instance.floors[index].entryPoint.transform.position, Quaternion.identity);
			gameObject.SetActive(value: true);
			coinSuckerCharInstances.Add(gameObject);
		}
	}

	private void FinishEvent()
	{
		CloseHotelEvent();
	}

	private void FinishEventSucess()
	{
		CloseHotelEvent();
	}

	private void CloseHotelEvent()
	{
		for (int i = 0; i < objectiveFloors.Count; i++)
		{
			objectiveFloors[i].points = objectiveFloors[i].floor.floorPoints;
			PoolManager.Instance.ObjectBackToPool(coinSuckerCharInstances[i]);
		}
		PoolManager.Instance.ObjectBackToPool(base.gameObject);
		Gameplay_Ctl.Instance.elevator_Ctl.SetEnabledCollisionTrigger(value: false);
		base.gameObject.SetActive(value: false);
		Gameplay_Ctl.Instance.hotelEvent_Ctl.currentEvent = null;
		if (!Gameplay_Ctl.Instance.IsGameOver())
		{
			Gameplay_Ctl.Instance.SpawnNextHotelEvent();
		}
	}

	private void OnDisable()
	{
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheByTime -= FinishEvent;
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheBySucess -= FinishEventSucess;
	}
}
