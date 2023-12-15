using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class InGameAudioManager : BaseAudio
{
	[SerializeField]
	private AudioClip elevatorUpStart;

	[SerializeField]
	private AudioClip elevatorUpLoop;

	[SerializeField]
	private AudioClip elevatorDownStart;

	[SerializeField]
	private AudioClip elevatorDownLoop;

	[SerializeField]
	private List<AudioClip> elevatorStop;

	[SerializeField]
	private List<AudioClip> doorOpen;

	[SerializeField]
	private AudioClip comboClip;

	[SerializeField]
	private AudioClip hotelbackgroundClip;

	private int lineClearAmount;

	private bool isPlayingLineClearCoroutine;

	private AudioSource audioSource;

	private AudioSource audioSourceLoop;

	private AudioSource comboSource;

	private AudioSource hotelBgSource;

	private AudioSource loseFxSource;

	[HideInInspector]
	public AudioSource hotelEvenButton;

	private AudioSource buttonFlip;

	[SerializeField]
	private AudioClip buttonFlip1;

	[SerializeField]
	private AudioClip buttonFlip2;

	[SerializeField]
	private AudioClip loseFxClip;

	private void Start()
	{
		audioSource = CreateAudioSource(null, AudioChannel.SndFx);
		audioSourceLoop = CreateAudioSource(null, AudioChannel.SndFx, playOnAwake: false, loop: true);
		comboSource = CreateAudioSource(null, AudioChannel.SndFx);
		hotelBgSource = CreateAudioSource(null, AudioChannel.SndFx, playOnAwake: false, loop: true);
		buttonFlip = CreateAudioSource(null, AudioChannel.SndFx);
		loseFxSource = CreateAudioSource(null, AudioChannel.SndFx);
		hotelEvenButton = CreateAudioSource(null, AudioChannel.SndFx);
		hotelBgSource.clip = hotelbackgroundClip;
		comboSource.clip = comboClip;
		loseFxSource.clip = loseFxClip;
		GameManager.Instance.OnGameOver += OnGameOver;
		Gameplay_Ctl.Instance.elevator_Ctl.OnDoorOpen += DoorOpen;
		Gameplay_Ctl.Instance.elevator_Ctl.OnDirectionChange += DirectionChange;
		Gameplay_Ctl.Instance.elevator_Ctl.OnElevatorStop += ElevatorStop;
		Gameplay_Ctl.Instance.elevator_Ctl.OnComboDone += ComboDone;
	}

	public void PlayHotelBgAudio()
	{
		hotelBgSource.Play();
	}

	private void DoorOpen()
	{
		audioSource.Stop();
		audioSourceLoop.Stop();
		audioSource.clip = doorOpen[Random.Range(0, doorOpen.Count)];
		audioSource.Play();
	}

	private void DirectionChange()
	{
		if (Gameplay_Ctl.Instance.elevator_Ctl.currentDirection == Direction.Up)
		{
			ElevatorUp();
		}
		else if (Gameplay_Ctl.Instance.elevator_Ctl.currentDirection == Direction.Down)
		{
			ElevatorDown();
		}
	}

	private void ElevatorUp()
	{
		audioSource.Stop();
		audioSourceLoop.Stop();
		audioSource.clip = elevatorUpStart;
		audioSourceLoop.clip = elevatorUpLoop;
		audioSource.Play();
		audioSourceLoop.PlayDelayed(elevatorUpStart.length);
	}

	private void ElevatorDown()
	{
		audioSource.clip = elevatorDownStart;
		audioSourceLoop.clip = elevatorDownLoop;
		audioSource.Play();
		audioSourceLoop.PlayDelayed(elevatorDownStart.length);
	}

	private void ElevatorStop()
	{
		audioSource.Stop();
		audioSourceLoop.Stop();
		audioSource.clip = elevatorStop[Random.Range(0, elevatorStop.Count)];
		audioSource.Play();
	}

	private void ComboDone()
	{
		if (!comboSource.isPlaying)
		{
			comboSource.Play();
		}
	}

	public void FlipButton1()
	{
		buttonFlip.clip = buttonFlip1;
		buttonFlip.Play();
	}

	public void FlipButton2()
	{
		buttonFlip.clip = buttonFlip2;
		buttonFlip.Play();
	}

	public void HotelEventButton()
	{
		hotelEvenButton.Play();
	}

	private void OnGameOver()
	{
		loseFxSource.Play();
		if (hotelBgSource.isPlaying)
		{
			hotelBgSource.DOFade(0f, 1.5f);
		}
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnGameOver -= OnGameOver;
		Gameplay_Ctl.Instance.elevator_Ctl.OnDoorOpen -= DoorOpen;
		Gameplay_Ctl.Instance.elevator_Ctl.OnDirectionChange -= DirectionChange;
		Gameplay_Ctl.Instance.elevator_Ctl.OnElevatorStop -= ElevatorStop;
		Gameplay_Ctl.Instance.elevator_Ctl.OnComboDone -= ComboDone;
	}
}
