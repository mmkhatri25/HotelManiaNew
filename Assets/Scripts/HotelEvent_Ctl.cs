using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotelEvent_Ctl : MonoBehaviour
{
	[SerializeField]
	private HotelEventSO outageSO;

	[SerializeField]
	private HotelEventSO thiefSO;

	[SerializeField]
	private HotelEventButton hotelEventButton;

	private Transform hotelEventButtonContainerTransform;

	private float buttonWaitIntervalMin = 0.5f;

	private float buttonWaitIntervalMax = 1f;

	[HideInInspector]
	public float currentEventMaxTime;

	private WaitForSeconds buttonWaitTime;

	[SerializeField]
	private List<Transform> buttonPositions = new List<Transform>();

	private int _lastIndex = -1;

	private int _currentRequieredTaps;

	private HotelEventSO currentHotelEvent;

	public Coroutine currentEvent;

	public event Action HotelEventFinisheByTime;

	public event Action HotelEventFinisheBySucess;

	private void Start()
	{
		hotelEventButtonContainerTransform = hotelEventButton.transform.parent.transform;
		buttonWaitTime = new WaitForSeconds(UnityEngine.Random.Range(buttonWaitIntervalMin, buttonWaitIntervalMax));
		GameManager.Instance.OnGameOver += OnGameOver;
		hotelEventButton.WildCardButtonPressed += ButtonPressed;
	}

	public void LaunchHotelEvent(HotelEventSO hotelEventSO)
	{
		Gameplay_Ctl.Instance.elevator_Ctl.SetEnabledCollisionTrigger(value: true);
		currentHotelEvent = hotelEventSO;
		hotelEventButton.SetBubbleImage(hotelEventSO.tapImage);
		currentEvent = StartCoroutine(LaunchEvent());
	}

	private void ButtonPressed()
	{
		Gameplay_Ctl.Instance.inGameAudioManager.HotelEventButton();
		_currentRequieredTaps--;
		if (_currentRequieredTaps > 0)
		{
			StartCoroutine(ButtonPressedIE());
		}
	}

	private IEnumerator ButtonPressedIE()
	{
		hotelEventButtonContainerTransform.gameObject.SetActive(value: false);
		buttonWaitTime = new WaitForSeconds(UnityEngine.Random.Range(buttonWaitIntervalMin, buttonWaitIntervalMax));
		yield return buttonWaitTime;
		if (_currentRequieredTaps > 0)
		{
			PlaceButtonOnRandomPos();
		}
	}

	private void PlaceButtonOnRandomPos()
	{
		hotelEventButtonContainerTransform.localScale = Vector3.zero;
		hotelEventButtonContainerTransform.gameObject.SetActive(value: true);
		int num;
		for (num = UnityEngine.Random.Range(0, buttonPositions.Count); num == _lastIndex; num = UnityEngine.Random.Range(0, buttonPositions.Count))
		{
		}
		_lastIndex = num;
		hotelEventButtonContainerTransform.position = buttonPositions[_lastIndex].position;
		hotelEventButtonContainerTransform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutElastic);
	}

	private IEnumerator LaunchEvent()
	{
		currentEventMaxTime = UnityEngine.Random.Range(currentHotelEvent.minTime, currentHotelEvent.maxTime);
		_currentRequieredTaps = UnityEngine.Random.Range(currentHotelEvent.minTaps, currentHotelEvent.maxTaps + 1);
		PoolManager.Instance.InstantiatePooled(currentHotelEvent.prefab, Vector3.zero, Quaternion.identity, Gameplay_Ctl.Instance.buildingTransform);
		PlaceButtonOnRandomPos();
		hotelEventButton.transform.parent.gameObject.SetActive(value: true);
		while (currentEventMaxTime > 0f && _currentRequieredTaps > 0)
		{
			currentEventMaxTime -= Time.deltaTime * Time.timeScale;
			yield return new WaitForEndOfFrame();
		}
		hotelEventButton.transform.parent.gameObject.SetActive(value: false);
		if (_currentRequieredTaps <= 0)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.eventsStoppedBeforeTimer++;
			//HotelEventFinisheBySucess();
		}
		else if (this.HotelEventFinisheByTime != null)
		{
			HotelEventFinisheByTime();
		}
	}

	public void OnGameOver()
	{
		if (this.HotelEventFinisheByTime != null)
		{
			this.HotelEventFinisheByTime();
		}
		if (currentEvent != null)
		{
			StopCoroutine(currentEvent);
		}
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnGameOver -= OnGameOver;
		hotelEventButton.WildCardButtonPressed -= ButtonPressed;
	}
}
