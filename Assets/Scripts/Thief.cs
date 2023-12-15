using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour
{
	private List<Floor_Ctl> floorsToVisit = new List<Floor_Ctl>();

	[SerializeField]
	private GameObject thiefGO;

	[SerializeField]
	private GameObject thiefArt;

	[SerializeField]
	private GameObject policeManGO;

	[SerializeField]
	private GameObject policeArt;

	[SerializeField]
	private Animator policeAC;

	[SerializeField]
	private Animator thiefAC;

	private Coroutine steal;

	private Vector3 thiefDestination;

	private int thiefPositionsIndex;

	[SerializeField]
	private Sprite robbedSprite;

	public float policeSpeed = 1.3f;

	public float thiefSpeed = 0.95f;

	public float dragSpeed = 0.7f;

	private float thiefStartingWait;

	public float thiefTimeBetweenFloors = 1.3f;

	private HotelEvent_Ctl hotelEvent_Ctl;

	private Vector3 artLookRight = new Vector3(-0.525f, 0.525f, 0.525f);

	private Vector3 artLookLeft = new Vector3(0.525f, 0.525f, 0.525f);

	private Vector3 artPoliceLookRight = new Vector3(-0.6f, 0.6f, 0.525f);

	private Vector3 artPoliceLookLeft = new Vector3(0.6f, 0.6f, 0.525f);

	private Floor_Ctl currentFloor;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioSource sireneSource;

	[SerializeField]
	private AudioClip introClip;

	[SerializeField]
	private AudioClip stealClip;

	[SerializeField]
	private AudioClip hitClip;

	[SerializeField]
	private AudioClip button;

	private void OnEnable()
	{
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheByTime += FinishEventTime;
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheBySucess += FinishEventSucess;
		floorsToVisit = new List<Floor_Ctl>();
		floorsToVisit.AddRange(Gameplay_Ctl.Instance.floors);
		//thiefAC.SetBool("Walking", value: true);
		//thiefAC.SetBool("Stealing", value: false);
		StartCoroutine(DoFloor(ChoseFloor()));
		Gameplay_Ctl.Instance.inGameAudioManager.hotelEvenButton.clip = button;
		sireneSource.clip = introClip;
		sireneSource.Play();
	}

	private Floor_Ctl ChoseFloor()
	{
		if (floorsToVisit.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, floorsToVisit.Count);
			Floor_Ctl result = floorsToVisit[index];
			floorsToVisit.RemoveAt(index);
			return result;
		}
		return null;
	}

	private IEnumerator DoFloor(Floor_Ctl currentFloor)
	{
		this.currentFloor = currentFloor;
		if (currentFloor != null)
		{
			thiefGO.transform.position = currentFloor.entryPoint.transform.position;
			thiefArt.transform.localScale = artLookRight;
			thiefGO.SetActive(value: true);
			thiefDestination = currentFloor.positions[0].position + Vector3.right * currentFloor.floor.gap / 3f;
			thiefPositionsIndex = 0;
			bool floorDone = false;
			while (!floorDone)
			{
				thiefGO.transform.position = Vector3.MoveTowards(thiefGO.transform.position, thiefDestination, thiefSpeed * Time.deltaTime);
				if (IsThiefAtDestiantion())
				{
					thiefPositionsIndex++;
					if (thiefPositionsIndex == 1)
					{
						yield return new WaitForSeconds(thiefStartingWait);
						thiefArt.transform.localScale = artLookLeft;
						steal = StartCoroutine(DoSteal(currentFloor, thiefPositionsIndex));
						yield return steal;
						thiefAC.SetBool("Walking", value: true);
						thiefAC.SetBool("Stealing", value: false);
						thiefDestination = currentFloor.positions[thiefPositionsIndex].position + Vector3.right * currentFloor.floor.gap / 3f;
					}
					else if (thiefPositionsIndex == currentFloor.positions.Count - 1)
					{
						thiefDestination = currentFloor.entryPoint.transform.position;
					}
					else if (currentFloor.entryPoint.transform.position == thiefDestination)
					{
						thiefGO.SetActive(value: false);
						yield return new WaitForSeconds(thiefTimeBetweenFloors);
						thiefPositionsIndex = 0;
						floorDone = true;
					}
					else
					{
						thiefDestination = currentFloor.positions[thiefPositionsIndex].position;
						steal = StartCoroutine(DoSteal(currentFloor, thiefPositionsIndex));
						yield return steal;
						thiefAC.SetBool("Walking", value: true);
						thiefAC.SetBool("Stealing", value: false);
					}
				}
				yield return null;
			}
			StartCoroutine(DoFloor(ChoseFloor()));
		}
		else
		{
			if (hotelEvent_Ctl == null)
			{
				hotelEvent_Ctl = Gameplay_Ctl.Instance.gameObject.GetComponent<HotelEvent_Ctl>();
			}
			hotelEvent_Ctl.currentEventMaxTime = 0f;
		}
	}

	private bool IsThiefAtDestiantion()
	{
		return Mathf.Abs(thiefDestination.x - thiefGO.transform.position.x) < 0.01f;
	}

	private IEnumerator DoSteal(Floor_Ctl currentFloor, int positionsIndex)
	{
		if (currentFloor.positions[positionsIndex - 1].character != null && currentFloor.positions[positionsIndex - 1].character.IsAtDestination() && !currentFloor.positions[positionsIndex - 1].character.robbed)
		{
			thiefAC.SetBool("Walking", value: false);
			thiefAC.SetBool("Stealing", value: true);
			yield return new WaitForSeconds(0.5f);
			audioSource.clip = stealClip;
			audioSource.Play();
			if (currentFloor.positions[positionsIndex - 1].character != null)
			{
				currentFloor.positions[positionsIndex - 1].character.SetRobbed(robbedSprite);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void FinishEventSucess()
	{
		StartCoroutine(PoliceTakesThief());
	}

	private IEnumerator PoliceTakesThief()
	{
		sireneSource.Play();
		policeManGO.transform.position = currentFloor.entryPoint.transform.position;
		policeArt.transform.localScale = artPoliceLookRight;
		policeManGO.SetActive(value: true);
		while (Vector2.Distance(policeManGO.transform.position, thiefGO.transform.position) > 0.44f)
		{
			policeManGO.transform.position = Vector3.MoveTowards(policeManGO.transform.position, thiefGO.transform.position, policeSpeed * Time.deltaTime);
			yield return null;
		}
		thiefGO.transform.position = policeManGO.transform.position + Vector3.right * 0.44f;
		StopAllCoroutines();
		StartCoroutine(PoliceHitAndCarry());
	}

	private IEnumerator PoliceHitAndCarry()
	{
		thiefDestination = thiefGO.transform.position;
		policeAC.SetBool("Hitting", value: true);
		thiefAC.SetBool("Hitting", value: true);
		audioSource.clip = hitClip;
		audioSource.Play();
		while (policeAC.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.PoliceManHitting") || policeAC.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.PoliceManWalking"))
		{
			yield return null;
			thiefGO.transform.position = Vector3.MoveTowards(thiefGO.transform.position, policeManGO.transform.position + Vector3.right * 0.33f, dragSpeed * 0.5f * Time.deltaTime);
		}
		policeAC.SetBool("Hitting", value: false);
		thiefAC.SetBool("Hitting", value: false);
		policeArt.transform.localScale = artPoliceLookLeft;
		Vector3 exitPos = currentFloor.entryPoint.transform.position;
		while (Vector2.Distance(policeManGO.transform.position, exitPos) > 0f)
		{
			policeManGO.transform.position = Vector3.MoveTowards(policeManGO.transform.position, exitPos, dragSpeed * Time.deltaTime);
			thiefGO.transform.position = Vector3.MoveTowards(thiefGO.transform.position, policeManGO.transform.position + Vector3.right * 0.33f, dragSpeed * 1.3f * Time.deltaTime);
			yield return null;
		}
		CommonEventEnding();
	}

	private void FinishEventTime()
	{
		StopAllCoroutines();
		StartCoroutine(CloseHotelEventTime());
	}

	private IEnumerator ThiefExits()
	{
		thiefDestination = currentFloor.entryPoint.transform.position;
		thiefArt.transform.localScale = artLookLeft;
		while (!IsThiefAtDestiantion())
		{
			thiefGO.transform.position = Vector3.MoveTowards(thiefGO.transform.position, thiefDestination, thiefSpeed * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator CloseHotelEventTime()
	{
		thiefAC.SetBool("Walking", value: true);
		thiefAC.SetBool("Stealing", value: false);
		yield return StartCoroutine(ThiefExits());
		CommonEventEnding();
	}

	private void CommonEventEnding()
	{
		Gameplay_Ctl.Instance.elevator_Ctl.SetEnabledCollisionTrigger(value: false);
		currentFloor = null;
		Gameplay_Ctl.Instance.hotelEvent_Ctl.currentEvent = null;
		thiefGO.SetActive(value: false);
		policeManGO.SetActive(value: false);
		if (!Gameplay_Ctl.Instance.IsGameOver())
		{
			Gameplay_Ctl.Instance.SpawnNextHotelEvent();
		}
		PoolManager.Instance.ObjectBackToPool(base.gameObject);
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheByTime -= FinishEventTime;
		Gameplay_Ctl.Instance.hotelEvent_Ctl.HotelEventFinisheBySucess -= FinishEventSucess;
		thiefGO.SetActive(value: false);
		policeManGO.SetActive(value: false);
	}
}
