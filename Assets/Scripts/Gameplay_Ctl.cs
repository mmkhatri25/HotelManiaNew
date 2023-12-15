using Alg;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay_Ctl : MonoBehaviour
{
	public delegate void AnswerCallback(int time);

	public static Gameplay_Ctl Instance;

	[HideInInspector]
	public UI_Ctl uI_Ctl;

	[HideInInspector]
	public Boosters_Ctl boosters_Ctl;

	private bool is_Tutorial;

	public Transform buildingTransform;

	[HideInInspector]
	public bool gameOver;

	[HideInInspector]
	public bool playing;

	public GameplaySession currentGameplaySession;

	public Elevator_Ctl elevator_Ctl;

	public HotelEvent_Ctl hotelEvent_Ctl;

	public InGameAudioManager inGameAudioManager;

	public GameObject uiCanvas;

	public List<Floor_Ctl> floors = new List<Floor_Ctl>();

	private List<FloorSO> floorsForMatch = new List<FloorSO>();

	private List<CharacterSO> characters = new List<CharacterSO>();

	private List<CharacterSO> specialCharacters = new List<CharacterSO>();

	private List<HotelEventSO> hotelEvents = new List<HotelEventSO>();

	private bool allowSpawn = true;

	public Coroutine charSpawnRoutine;

	private AnimationCurve spawnCurve;

	private float specialCharProb;

	private int maxCharsWithoutSpecial;

	private int currentStepsWithoutSpecial;

	private int pooledNumberPerCharacter = 20;

	private float waitTime;

	private float waitTimeCtl;

	private float secCounter;

	public float pauseTime;

	private float gameStartTime;

	private HotelEventSO lastHotelEvent;

	[HideInInspector]
	public Dictionary<GameObject, int> activeSpecialCharacters = new Dictionary<GameObject, int>();

	public event AnswerCallback OnPlayedTime;

	public event AnswerCallback OnCharactersDelivered;

	public event AnswerCallback OnCharactersDeliveredFloor1;

	public event AnswerCallback OnCharactersDeliveredFloor2;

	public event AnswerCallback OnCharactersDeliveredFloor3;

	public event AnswerCallback OnCharactersDeliveredFloor4;

	public event AnswerCallback OnCharactersDeliveredFloor5;

	public event AnswerCallback OnSpecialCharactersDelivered;

	public event AnswerCallback OnSpecialScientistsDelivered;

	public event AnswerCallback OnSpecialRepairmenDelivered;

	public event AnswerCallback OnSpecialDoubleSpaceDelivered;

	public event AnswerCallback OnSpecialHeavyDelivered;

	public event AnswerCallback OnTotalCoins;

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
		uI_Ctl = GetComponent<UI_Ctl>();
		boosters_Ctl = GetComponent<Boosters_Ctl>();
		AudioManager.Initialize();
		currentGameplaySession = new GameplaySession();
	}

	private void Start()
	{
		if (!PlayerPrefs.HasKey("TutorialDone"))
		{
			is_Tutorial = true;
			//Singleton<AnalyticsManager>.Instance.FirstPlayTimeTutorial();
		}
		else
		{
		//	Singleton<AnalyticsManager>.Instance.StartGame();
		}
		Application.targetFrameRate = 60;
		SetUpMatch();
		GameManager.Instance.mission_Ctl.ResetProgressOnSingleMissions();
		if (is_Tutorial)
		{
			TutorialPart1Popup.GetInstance(null).Init();
		}
		else
		{
			PrematchPopup.GetInstance(uI_Ctl.gameUiTransform).Init(GameManager.Instance.mission_Ctl.currentMissions);
			TipsPopup.GetInstance(uI_Ctl.gameUiTransform);
		}
		gameStartTime = Time.time;
	}

	private void Update()
	{
		if (!playing || gameOver)
		{
			return;
		}
		if (is_Tutorial)
		{
			if (currentGameplaySession.charactersDelivered > 3)
			{
				playing = false;
				UnityEngine.Debug.Log("var ins = TutorialPart1Popup.GetInstance(null);");
				TutorialPart1Popup.GetInstance(null).Init(2);
			}
			else if (currentGameplaySession.charactersSpawn > 3)
			{
				allowSpawn = false;
			}
		}
		else
		{
			secCounter += Time.deltaTime * Time.timeScale;
			if (secCounter > 1f)
			{
				secCounter -= 1f;
				currentGameplaySession.sessionLeght++;
			}
		}
	}

	private void SetUpMatch()
	{
		if (is_Tutorial)
		{
			SetUpGameVars(GameManager.Instance.tutorialGameVars);
		}
		else
		{
			SetUpGameVars(GameManager.Instance.gameVars);
		}
		uI_Ctl.SetScore(0);
	}

	public void SetUpGameVars(GameVars gameVars)
	{
		spawnCurve = gameVars.spawnCurve;
		specialCharProb = gameVars.specialCharProb;
		maxCharsWithoutSpecial = gameVars.maxCharsWithoutSpecial;
		ChooseSpecialCharRoster();
		SetupFloors();
		SetUpHotelEvents();
		CreateCharacterIntancesOnPool();
	}

	public void StartGame()
	{
		//GeneralAudioController.Instance.FadeOutMenuToGameplayFullVolumeStart();
		charSpawnRoutine = StartCoroutine(SpawnCharacters());
		elevator_Ctl.StartHandlingCharacters();
		playing = true;
		if (!is_Tutorial)
		{
			StartCoroutine(SpawnHotelEvent());
		}
	}

	public bool IsGameOver()
	{
		return gameOver;
	}

	public void GameOver(Floor_Ctl fullFloor = null)
	{
		uI_Ctl.HideUI();
		if (gameOver)
		{
			return;
		}
		elevator_Ctl.TimeWithoutMehChech();
		elevator_Ctl.TimeWithSpecialCharChech();
		if (!is_Tutorial)
		{
			GameOverMissionEvents();
			if (!GameManager.Instance.firstMatchPlayed)
			{
				PlayerPrefs.SetInt("firstMatchPlayed", 1);
				GameManager.Instance.firstMatchPlayed = true;
			}
		}
		gameOver = true;
		playing = false;
		elevator_Ctl.SetAllowMovement(value: false);
		elevator_Ctl.currentDirection = Direction.NoDirection;
		GameManager.Instance.GameOver();
		if (fullFloor == null)
		{
			CameraAnim_Ctl.Instance.CameraGameOverAnim();
		}
		else
		{
			StartCoroutine(EndGameAnimation(fullFloor));
		}
		if (is_Tutorial)
		{
			StartCoroutine(GoToMainMenuDelayed(0.8f));
		}
		else if (GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted() == 3)
		{
			PostMatchMissionsDonePopup.GetInstance(uI_Ctl.gameUiTransform).Init();
		}
		else
		{
			PostMatchMissionsPopup.GetInstance(uI_Ctl.gameUiTransform).Init();
		}
	}

	private IEnumerator GoToMainMenuDelayed(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		GameManager.Instance.ChangeScene("MainMenu");
	}

	public void AddCoinsToSession(int coins)
	{
		currentGameplaySession.totalCoins += coins;
		uI_Ctl.SetScore(currentGameplaySession.totalCoins);
	}

	private void ChooseSpecialCharRoster()
	{
		specialCharacters = new List<CharacterSO>(GameManager.Instance.unlockedSpecialCharacters);
	}

	private void SetupFloors()
	{
		if (GameManager.Instance.playersHotelLayout.Count == 0)
		{
			GameManager.Instance.LoadPlayersHotelLayout();
		}
		for (int i = 0; i < GameManager.Instance.playersHotelLayout.Count; i++)
		{
			AddFloorCharacters(GameManager.Instance.playersHotelLayout[i]);
			floors[i].floor = GameManager.Instance.playersHotelLayout[i];
			floors[i].Setup();
		}
	}

	private void AddFloorCharacters(FloorSO floor)
	{
		characters.Add(floor.floorCharacter);
	}

	private void CreateCharacterIntancesOnPool()
	{
		for (int i = 0; i < characters.Count; i++)
		{
			PoolManager.Instance.AddInstancesToPool(characters[i].prefab, pooledNumberPerCharacter);
		}
		for (int j = 0; j < GameManager.Instance.unlockedSpecialCharacters.Count; j++)
		{
			PoolManager.Instance.AddInstancesToPool(GameManager.Instance.unlockedSpecialCharacters[j].prefab, 2);
		}
	}

	public void SetUpHotelEvents()
	{
		hotelEvents.AddRange(GameManager.Instance.avariableWildCardEvents);
	}

	public IEnumerator SpawnCharacters()
	{
		while (!IsGameOver())
		{
			yield return new WaitForSeconds(spawnCurve.Evaluate(Time.time - gameStartTime - pauseTime - waitTime));
			if (!allowSpawn)
			{
				continue;
			}
			CharacterSO characterSO;
			if (currentStepsWithoutSpecial >= maxCharsWithoutSpecial || (float)UnityEngine.Random.Range(0, 101) <= specialCharProb)
			{
				currentStepsWithoutSpecial = 0;
				characterSO = specialCharacters[Random.Range(0, specialCharacters.Count)];
			}
			else
			{
				currentStepsWithoutSpecial++;
				characterSO = characters[Random.Range(0, characters.Count)];
			}
			Character_Ctl component = PoolManager.Instance.InstantiatePooled(characterSO.prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity).GetComponent<Character_Ctl>();
			component.character = characterSO;
			floors[Random.Range(0, floors.Count)].AddCharacter(component);
			Floor_Ctl floor_Ctl = null;
			if (component.character.hasDestination)
			{
				while (floor_Ctl == null)
				{
					floor_Ctl = floors[Random.Range(0, floors.Count)];
					if (floor_Ctl == component.currentFloor)
					{
						floor_Ctl = null;
					}
				}
			}
			component.SetStartingDestinationFloor(floor_Ctl);
			component.SetDestinationFloor(floor_Ctl, changeParent: false);
			Instance.ChechMusicSpeedChange();
			component.currentFloor.EvaluateSiren();
			currentGameplaySession.charactersSpawn++;
		}
	}

	public void SetAllowSpawn(bool value)
	{
		allowSpawn = value;
		if (value)
		{
			waitTime += Time.time - waitTimeCtl;
		}
		else
		{
			waitTimeCtl = Time.time;
		}
	}

	public void SpawnNextHotelEvent()
	{
		StartCoroutine(SpawnHotelEvent());
	}

	private IEnumerator SpawnHotelEvent()
	{
		float timeForEvent = UnityEngine.Random.Range(GameManager.Instance.gameVars.minTimeForEvent, GameManager.Instance.gameVars.maxTimeForEvent);
		while (!IsGameOver() && timeForEvent > 0f)
		{
			timeForEvent -= Time.deltaTime * Time.timeScale;
			yield return new WaitForEndOfFrame();
		}
		if (!IsGameOver())
		{
			lastHotelEvent = ChoseNextEvent();
			hotelEvent_Ctl.LaunchHotelEvent(lastHotelEvent);
			currentGameplaySession.totalHotelEvents++;
		}
	}

	public HotelEventSO ChoseNextEvent()
	{
		if (hotelEvents.Count == 0)
		{
			SetUpHotelEvents();
		}
		int index = UnityEngine.Random.Range(0, hotelEvents.Count);
		HotelEventSO hotelEventSO = hotelEvents[index];
		while (hotelEventSO == lastHotelEvent)
		{
			index = UnityEngine.Random.Range(0, hotelEvents.Count);
			hotelEventSO = hotelEvents[index];
		}
		hotelEvents.RemoveAt(index);
		return hotelEventSO;
	}

	public Floor_Ctl GetMostCrowdedFloor()
	{
		int num = 0;
		List<Floor_Ctl> list = new List<Floor_Ctl>();
		for (int i = 0; i < floors.Count; i++)
		{
			int numberOfCharactersOnFloor = floors[i].GetNumberOfCharactersOnFloor();
			if (numberOfCharactersOnFloor > num)
			{
				list.Clear();
				list.Add(floors[i]);
				num = numberOfCharactersOnFloor;
			}
			else if (numberOfCharactersOnFloor == num)
			{
				list.Add(floors[i]);
			}
		}
		return list[Random.Range(0, list.Count)];
	}

	public void ChechMusicSpeedChange()
	{
		if (GetMostCrowdedFloor().GetNumberOfCharactersOnFloor() >= GameManager.Instance.gameVars.numberOfCharsForFastMusic)
		{
			//GeneralAudioController.Instance.GameplayMusicToFast();
		}
		else
		{
			//GeneralAudioController.Instance.GameplayMusicToSlow();
		}
	}

	private IEnumerator EndGameAnimation(Floor_Ctl floor)
	{
		StartCoroutine(CameraAnim_Ctl.Instance.DoShake(4.5f, 0.07f));
		for (int i = 0; i < floors.Count; i++)
		{
			if (floors[i] == floor && floors[i].siren != null)
			{
				floors[i].siren.GetComponent<Animator>().SetTrigger("GameOver");
				SpriteRenderer component = floors[i].siren.transform.GetChild(1).GetComponent<SpriteRenderer>();
				component.sortingOrder = 15;
				component.DOFade(0.85f, 3f).SetSpeedBased();
			}
			else if (floors[i].siren != null)
			{
				floors[i].siren.GetComponent<Animator>().SetTrigger("Hide");
			}
		}
		yield return new WaitForSeconds(1f);
		uI_Ctl.FadeToGreyAndShowEndGameText();
		yield return new WaitForSeconds(2.5f);
		CameraAnim_Ctl.Instance.CameraGameOverAnim();
	}

	private void GameOverMissionEvents()
	{
	}

	public void TimePlayed(int value)
	{
		if (this.OnPlayedTime != null)
		{
			this.OnPlayedTime(value);
		}
	}

	public void CharactersDelivered(int value)
	{
		if (this.OnCharactersDelivered != null)
		{
			this.OnCharactersDelivered(value);
		}
	}

	public void CharactersDeliveredFloor1(int value)
	{
		if (this.OnCharactersDeliveredFloor1 != null)
		{
			this.OnCharactersDeliveredFloor1(value);
		}
	}

	public void CharactersDeliveredFloor2(int value)
	{
		if (this.OnCharactersDeliveredFloor2 != null)
		{
			this.OnCharactersDeliveredFloor2(value);
		}
	}

	public void CharactersDeliveredFloor3(int value)
	{
		if (this.OnCharactersDeliveredFloor3 != null)
		{
			this.OnCharactersDeliveredFloor3(value);
		}
	}

	public void CharactersDeliveredFloor4(int value)
	{
		if (this.OnCharactersDeliveredFloor4 != null)
		{
			this.OnCharactersDeliveredFloor4(value);
		}
	}

	public void CharactersDeliveredFloor5(int value)
	{
		if (this.OnCharactersDeliveredFloor5 != null)
		{
			this.OnCharactersDeliveredFloor5(value);
		}
	}

	public void SpecialCharactersDelivered(int value)
	{
		if (this.OnSpecialCharactersDelivered != null)
		{
			this.OnSpecialCharactersDelivered(value);
		}
	}

	public void SpecialScientistsDelivered(int value)
	{
		if (this.OnSpecialScientistsDelivered != null)
		{
			this.OnSpecialScientistsDelivered(value);
		}
	}

	public void SpecialRepairmenDelivered(int value)
	{
		if (this.OnSpecialRepairmenDelivered != null)
		{
			this.OnSpecialRepairmenDelivered(value);
		}
	}

	public void SpecialDoubleSpaceDelivered(int value)
	{
		if (this.OnSpecialDoubleSpaceDelivered != null)
		{
			this.OnSpecialDoubleSpaceDelivered(value);
		}
	}

	public void SpecialHeavyDelivered(int value)
	{
		if (this.OnSpecialHeavyDelivered != null)
		{
			this.OnSpecialHeavyDelivered(value);
		}
	}

	public void TotalCoins(int value)
	{
		if (this.OnTotalCoins != null)
		{
			this.OnTotalCoins(value);
		}
	}
}
