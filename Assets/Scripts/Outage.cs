using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outage : MonoBehaviour
{
	[SerializeField]
	private List<Animator> darkRoomAnimators;

	public int numberOfAffectedFloors = 3;

	private void OnEnable()
	{
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheByTime += FinishEvent;
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheBySucess += FinishEventSucess;
		ActivateDarkFloors();
	}

	private void ActivateDarkFloors()
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
			darkRoomAnimators[index].gameObject.SetActive(value: true);
		}
	}

	private void FinishEvent()
	{
		StartCoroutine(CloseHotelEvent());
	}

	private void FinishEventSucess()
	{
		StartCoroutine(CloseHotelEvent());
	}

	private IEnumerator CloseHotelEvent()
	{
		for (int i = 0; i < darkRoomAnimators.Count; i++)
		{
			if (darkRoomAnimators[i].gameObject.activeInHierarchy)
			{
				darkRoomAnimators[i].SetTrigger("LightsOn");
			}
		}
		yield return new WaitForSeconds(0.7f);
		for (int j = 0; j < darkRoomAnimators.Count; j++)
		{
			darkRoomAnimators[j].gameObject.SetActive(value: false);
		}
		PoolManager.Instance.ObjectBackToPool(base.gameObject);
		Gameplay_Ctl.Instance.elevator_Ctl.SetEnabledCollisionTrigger(value: false);
		Gameplay_Ctl.Instance.hotelEvent_Ctl.currentEvent = null;
		if (!Gameplay_Ctl.Instance.IsGameOver())
		{
			Gameplay_Ctl.Instance.SpawnNextHotelEvent();
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < darkRoomAnimators.Count; i++)
		{
			darkRoomAnimators[i].gameObject.SetActive(value: false);
		}
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheByTime -= FinishEvent;
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheBySucess -= FinishEventSucess;
	}
}
