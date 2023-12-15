using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Ctl : MonoBehaviour
{
	[SerializeField]
	public GameObject door;

	private int currentNumberOfPositions;

	private float currentGapOfPositions;

	private float currentSpeed;

	private float currentAccelerationTime;

	private float accelerationStart;

	private float currentAutoAdjustSpeed;

	private float currentDoorSpeed;

	private float currentWeight;

	private float currentPerfectSnap;

	private float currentGoodSnap;

	private float currentWeightMinClamp;

	private float currentWeightMaxClamp;

	[Header("Movement Vars")]
	private bool allowMovement = true;

	[HideInInspector]
	public Direction currentDirection;

	[HideInInspector]
	public Direction previousDirection;

	public Floor_Ctl destination;

	private Tween automovementTween;

	public Floor_Ctl currentFloor;

	private float halfDistanceBetweenFloors;

	public float lastSnapMultiplayer = 1f;

	private bool snapSet;

	[Header("Character Related")]
	private List<PositionAndChar> positions = new List<PositionAndChar>();

	private Coroutine charsGetOutInEnumerator;

	private int elevatorEmptySpaces;

	private Character_Ctl[] moving_characters;

	private float addedWeight;

	[Header("Door Related")]
	[SerializeField]
	private DoorState currentDoorState;

	[SerializeField]
	private SpriteRenderer doorSpriteR;

	private SpriteRenderer floorDoorSpriteR;

	public SpriteRenderer snapSprite;

	public Vector3 snapSpriteLocalPos;

	[SerializeField]
	private List<Sprite> perfectSprites;

	[SerializeField]
	private List<Sprite> goodSprites;

	[SerializeField]
	private List<Sprite> mehSprites;

	private Tween doorOpenTween;

	private Tween doorCloseTween;

	private Tween floorDoorOpenTween;

	private Tween floorDoorCloseTween;

	[Header("Effect Icons")]
	[SerializeField]
	private List<SpriteRenderer> statusIcons = new List<SpriteRenderer>();

	[HideInInspector]
	public float elevatorDirModifier = 1f;

	[SerializeField]
	private Rigidbody2D _rigidbody2D;

	[SerializeField]
	private BoxCollider2D _boxCollider2D;

	private bool stoppedAtFloor;

	private float currentTimeWithoutMeh;

	private bool twoSpecialsAtElevator;

	private float currentTimeWith2SpecialsAtElevator;

	private bool specialAtElevator;

	private float currentTimeWithSpecialAtElevator;

	public event Action OnDirectionChange;

	public event Action OnDoorOpen;

	public event Action OnElevatorStop;

	public event Action OnComboDone;

	private void Start()
	{
		SetEnabledCollisionTrigger(value: false);
		SetUpElevatorStats(GameManager.Instance.elevatorStats);
		moving_characters = new Character_Ctl[currentNumberOfPositions];
		currentFloor = Gameplay_Ctl.Instance.floors[0];
		snapSpriteLocalPos = snapSprite.transform.localPosition;
		CreatePositions();
		elevatorEmptySpaces = currentNumberOfPositions;
		halfDistanceBetweenFloors = (Gameplay_Ctl.Instance.floors[1].transform.position.y - Gameplay_Ctl.Instance.floors[0].transform.position.y) / 2f;
		stoppedAtFloor = true;
		currentDoorState = DoorState.Open;
		currentFloor = Gameplay_Ctl.Instance.floors[0];
		floorDoorSpriteR = currentFloor.floorDoorSprite;
		base.transform.parent.position = new Vector3(base.transform.parent.position.x, currentFloor.transform.position.y, 0f);
	}

	private void Update()
	{
		if (allowMovement)
		{
			ManageInput();
		}
		if (Gameplay_Ctl.Instance.gameOver || !Gameplay_Ctl.Instance.playing)
		{
			return;
		}
		if (stoppedAtFloor)
		{
			AddTimeToStoppedFloor(Time.deltaTime * Time.timeScale);
		}
		if (twoSpecialsAtElevator)
		{
			currentTimeWith2SpecialsAtElevator += Time.deltaTime * Time.timeScale;
		}
		else
		{
			if (Gameplay_Ctl.Instance.currentGameplaySession.twoSpecialCharactersElevartorAtSameTimeCounter < (int)currentTimeWith2SpecialsAtElevator)
			{
				Gameplay_Ctl.Instance.currentGameplaySession.twoSpecialCharactersElevartorAtSameTimeCounter = (int)currentTimeWith2SpecialsAtElevator;
			}
			currentTimeWith2SpecialsAtElevator = 0f;
		}
		if (specialAtElevator)
		{
			currentTimeWithSpecialAtElevator += Time.deltaTime * Time.timeScale;
		}
		else
		{
			if (Gameplay_Ctl.Instance.currentGameplaySession.maxTimeWithSpecialCharInElevator < (int)currentTimeWithSpecialAtElevator)
			{
				Gameplay_Ctl.Instance.currentGameplaySession.maxTimeWithSpecialCharInElevator = (int)currentTimeWithSpecialAtElevator;
			}
			currentTimeWithSpecialAtElevator = 0f;
		}
		currentTimeWithoutMeh += Time.deltaTime * Time.timeScale;
		if (currentDoorState == DoorState.Closed && (currentDirection == Direction.Up || currentDirection == Direction.Down))
		{
			Gameplay_Ctl.Instance.currentGameplaySession.elevatorMovedMeters += 2f * (currentSpeed * GetAccelerationValue() * (1f / Mathf.Clamp(currentWeight + addedWeight, currentWeightMinClamp, currentWeightMaxClamp)) * Time.deltaTime * Time.timeScale);
		}
	}

	public void SetUpElevatorStats(ElevatorSO elevatorStats)
	{
		currentGapOfPositions = elevatorStats.currentGapOfPositions;
		currentNumberOfPositions = elevatorStats.positions;
		currentSpeed = elevatorStats.speed;
		currentAutoAdjustSpeed = elevatorStats.autoAdjustSpeed;
		currentDoorSpeed = elevatorStats.doorSpeed;
		currentWeight = elevatorStats.weight;
		currentPerfectSnap = elevatorStats.perfectSnap;
		currentGoodSnap = elevatorStats.goodSnap;
		currentAccelerationTime = elevatorStats.accelerationTime;
		currentWeightMinClamp = elevatorStats.weightMinClamp;
		currentWeightMaxClamp = elevatorStats.weightMaxClamp;
	}

	private void CreatePositions()
	{
		if (currentNumberOfPositions % 2 == 0)
		{
			for (int i = 0; i < currentNumberOfPositions; i++)
			{
				PositionAndChar item = new PositionAndChar(new Vector3(currentGapOfPositions / 2f + (float)(i - currentNumberOfPositions / 2) * currentGapOfPositions, -0.68f, 0f), null);
				positions.Add(item);
			}
		}
		else
		{
			for (int j = 0; j < currentNumberOfPositions; j++)
			{
				PositionAndChar item2 = new PositionAndChar(new Vector3((float)(Mathf.CeilToInt(-currentNumberOfPositions / 2) + j) * currentGapOfPositions, -0.68f, 0f), null);
				positions.Add(item2);
			}
		}
	}

	private void ManageInput()
	{
		if (currentDirection == Direction.Up || currentDirection == Direction.Down)
		{
			ManageDoor(open: false);
		}
		if (currentDoorState == DoorState.Closed)
		{
			if (previousDirection != currentDirection)
			{
				this.OnDirectionChange();
				if (currentDirection != 0)
				{
					previousDirection = currentDirection;
				}
			}
			if (currentDirection == Direction.Up)
			{
				ResetAutoMovement();
				MoveUp();
			}
			else if (currentDirection == Direction.Down)
			{
				ResetAutoMovement();
				MoveDown();
			}
			else if (currentDirection == Direction.NoDirection)
			{
				if (destination == null)
				{
					GetClosestDestination();
				}
				if (destination != currentFloor)
				{
					if (!CheckIfArrivedToDestination() && destination != null && automovementTween == null)
					{
						MoveToDestination(destination, currentAutoAdjustSpeed);
					}
					else if (CheckIfArrivedToDestination())
					{
						ArrivedToFloor();
					}
				}
			}
		}
		Vector3 position = base.transform.parent.position;
		position.y = Mathf.Clamp(base.transform.parent.position.y, Gameplay_Ctl.Instance.floors[0].transform.position.y - halfDistanceBetweenFloors, Gameplay_Ctl.Instance.floors[Gameplay_Ctl.Instance.floors.Count - 1].transform.position.y + halfDistanceBetweenFloors);
		base.transform.parent.position = position;
	}

	private void MoveUp()
	{
		base.transform.parent.Translate(elevatorDirModifier * Vector3.up * (currentSpeed * GetAccelerationValue() * (1f / (currentWeight + addedWeight)) * Time.deltaTime));
	}

	private void MoveDown()
	{
		base.transform.parent.Translate(elevatorDirModifier * Vector3.down * (currentSpeed * GetAccelerationValue() * (1f / (currentWeight + addedWeight)) * Time.deltaTime));
	}

	public void SetAllowMovement(bool value)
	{
		if (!value)
		{
			this.OnElevatorStop();
		}
		allowMovement = value;
	}

	public void MoveToDestination(Floor_Ctl floor, float speed)
	{
		previousDirection = Direction.NoDirection;
		CheckSnapBonus(floor);
		if (automovementTween == null)
		{
			automovementTween = base.transform.parent.DOMoveY(floor.transform.position.y, speed * (1f / (currentWeight + addedWeight)) * Time.deltaTime).SetSpeedBased().SetEase(Ease.Linear);
		}
	}

	public void StopElevator()
	{
		SetAllowMovement(value: false);
		if (automovementTween != null)
		{
			automovementTween.Kill();
		}
	}

	private float GetAccelerationValue()
	{
		accelerationStart += Time.deltaTime;
		return Mathf.Clamp01(accelerationStart / currentAccelerationTime);
	}

	private bool CheckIfArrivedToDestination()
	{
		if (destination != null)
		{
			return Mathf.Abs(destination.transform.position.y - base.transform.parent.position.y) < 0.001f;
		}
		return false;
	}

	private void GetClosestDestination()
	{
		float num = 2.14748365E+09f;
		for (int i = 0; i < Gameplay_Ctl.Instance.floors.Count; i++)
		{
			float num2 = Mathf.Abs(Gameplay_Ctl.Instance.floors[i].transform.position.y - base.transform.parent.position.y);
			if (num2 < num)
			{
				num = num2;
				destination = Gameplay_Ctl.Instance.floors[i];
			}
		}
	}

	private void ResetAutoMovement()
	{
		if (automovementTween != null)
		{
			automovementTween.Kill();
		}
		automovementTween = null;
		destination = null;
	}

	private void ArrivedToFloor()
	{
		this.OnElevatorStop();
		accelerationStart = 0f;
		snapSet = false;
		currentFloor = destination;
		previousDirection = Direction.NoDirection;
		ResetAutoMovement();
		base.transform.parent.position = new Vector3(base.transform.parent.position.x, currentFloor.transform.position.y, 0f);
		ManageDoor(open: true);
	}

	private void CheckSnapBonus(Floor_Ctl floor)
	{
		if (!snapSet && GetNumberOfFreePositions() < currentNumberOfPositions && CharactersWithDestinationOnCurrentFloor(floor))
		{
			snapSet = true;
			if (Mathf.Abs(destination.transform.position.y - base.transform.parent.position.y) < halfDistanceBetweenFloors * currentPerfectSnap)
			{
				PerfectSpap();
				SetSnapSprite(perfectSprites);
				return;
			}
			if (Mathf.Abs(destination.transform.position.y - base.transform.position.y) < halfDistanceBetweenFloors * currentGoodSnap)
			{
				GoodSpap();
				SetSnapSprite(goodSprites);
				return;
			}
			BadSpap();
			SetSnapSprite(mehSprites);
			TimeWithoutMehChech();
			currentTimeWithoutMeh = 0f;
		}
	}

	private void PerfectSpap()
	{
		lastSnapMultiplayer = GameManager.Instance.gameVars.perfectSnapMultiplier;
		Gameplay_Ctl.Instance.currentGameplaySession.perfectStopCount++;
	}

	private void GoodSpap()
	{
		lastSnapMultiplayer = GameManager.Instance.gameVars.goodSnapMultiplier;
		Gameplay_Ctl.Instance.currentGameplaySession.goodStopCount++;
	}

	private void BadSpap()
	{
		lastSnapMultiplayer = 1f;
	}

	public void SetSnapSprite(List<Sprite> spriteList)
	{
		snapSprite.DOKill();
		snapSprite.transform.parent = base.transform;
		snapSprite.transform.localPosition = snapSpriteLocalPos;
		snapSprite.sprite = spriteList[UnityEngine.Random.Range(0, spriteList.Count)];
		snapSprite.DOFade(1f, 0f);
		snapSprite.transform.parent = null;
		snapSprite.transform.DOLocalMoveY(snapSprite.transform.position.y + 0.35f, 1f);
		snapSprite.DOFade(0f, 0.5f).SetDelay(0.5f);
	}

	private void ManageDoor(bool open)
	{
		if (open && currentDoorState != DoorState.Opening && currentDoorState != 0)
		{
			this.OnDoorOpen();
			floorDoorSpriteR = currentFloor.floorDoorSprite;
			currentDoorState = DoorState.Opening;
			if (doorCloseTween != null)
			{
				doorCloseTween.Kill();
			}
			if (floorDoorCloseTween != null)
			{
				floorDoorCloseTween.Kill();
			}
			doorOpenTween = doorSpriteR.DOFade(0f, currentDoorSpeed).SetSpeedBased().SetEase(Ease.Linear)
				.OnComplete(DoorOpen);
			floorDoorOpenTween = floorDoorSpriteR.DOFade(0f, currentDoorSpeed).SetSpeedBased().SetEase(Ease.Linear);
		}
		else if (!open && currentDoorState != DoorState.Closing && currentDoorState != DoorState.Closed)
		{
			currentDoorState = DoorState.Closing;
			DoorClosing();
			if (doorOpenTween != null)
			{
				doorOpenTween.Kill();
			}
			if (floorDoorOpenTween != null)
			{
				floorDoorOpenTween.Kill();
			}
			doorCloseTween = doorSpriteR.DOFade(1f, currentDoorSpeed).SetSpeedBased().SetEase(Ease.Linear)
				.OnComplete(SetDoorClosed);
			floorDoorCloseTween = floorDoorSpriteR.DOFade(1f, currentDoorSpeed).SetSpeedBased().SetEase(Ease.Linear);
		}
	}

	private void DoorOpen()
	{
		currentDoorState = DoorState.Open;
		doorOpenTween = null;
		doorCloseTween = null;
		floorDoorOpenTween = null;
		doorCloseTween = null;
		charsGetOutInEnumerator = StartCoroutine(HandleCharOutIn());
	}

	private void SetDoorClosed()
	{
		currentDoorState = DoorState.Closed;
		doorOpenTween = null;
		doorCloseTween = null;
		floorDoorOpenTween = null;
		doorCloseTween = null;
		currentFloor.EvaluateSiren();
		currentFloor = null;
		CheckCharactersDestinations();
		CheckElevatorMods();
	}

	private void CheckCharactersDestinations()
	{
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i].character != null)
			{
				positions[i].character.SetDestinationFloor(positions[i].character.startingDestinationFloor, changeParent: false);
			}
		}
	}

	private void CheckElevatorMods()
	{
		addedWeight = 0f;
		elevatorDirModifier = 1f;
		for (int i = 0; i < positions.Count; i++)
		{
			CheckElevatorWeight(positions[i].character);
			CheckElevatorDirectionMod(positions[i].character);
		}
	}

	private void CheckElevatorWeight(Character_Ctl character)
	{
		if (character != null)
		{
			addedWeight += character.weightMod;
		}
	}

	private void CheckElevatorDirectionMod(Character_Ctl character)
	{
		if (character != null)
		{
			elevatorDirModifier *= character.elevatorDirMod;
		}
	}

	public void StartHandlingCharacters()
	{
		charsGetOutInEnumerator = StartCoroutine(HandleCharOutIn());
	}

	private void AssignPositionsToCharacter(Character_Ctl currentChar)
	{
		int emptyPosIndex = GetEmptyPosIndex();
		if (currentChar.positions == 1)
		{
			currentChar.SetDestinationPosition(base.transform.TransformPoint(positions[emptyPosIndex].position));
		}
		else
		{
			currentChar.SetDestinationPosition(base.transform.TransformPoint(positions[emptyPosIndex].position + Vector3.right * currentGapOfPositions * 0.5f));
		}
		for (int i = 0; i < currentChar.positions; i++)
		{
			positions[emptyPosIndex + i].character = currentChar;
		}
		ModifyEmptySpaces(-currentChar.positions);
	}

	private IEnumerator HandleCharOutIn()
	{
		if (positions != null)
		{
			for (int i = 0; i < positions.Count; i++)
			{
				if (!(positions[i].character != null))
				{
					continue;
				}
				Character_Ctl currentChar2 = positions[i].character;
				if (currentChar2.destinationFloor == null && currentChar2.originFloor != currentFloor)
				{
					currentChar2.destinationFloor = currentFloor;
				}
				if (!(currentChar2.destinationFloor == currentFloor))
				{
					continue;
				}
				bool wentOut2 = false;
				currentChar2.LookLeft();
				moving_characters[0] = currentChar2;
				RemoveFromPositions(currentChar2);
				if (GetNumberOfFreePositions() < currentNumberOfPositions && RepositionCharacters())
				{
					i = -1;
				}
				if (currentChar2.character.elevatorStatusSprite != null)
				{
					DisableStatusIcon(currentChar2.character.elevatorStatusSprite);
				}
				currentChar2.SetDestinationPosition(currentFloor.exitPoint.position);
				while (currentDoorState == DoorState.Open && !wentOut2)
				{
					if (currentChar2.ArrivedToDestination())
					{
						wentOut2 = true;
						CharacterGotOut(currentChar2);
					}
					yield return new WaitForEndOfFrame();
				}
			}
		}
		while (elevatorEmptySpaces > 0 && currentDoorState == DoorState.Open)
		{
			if (currentFloor.positions[0].character != null && GetNumberOfFreePositions() >= currentFloor.positions[0].character.positions)
			{
				Character_Ctl currentChar2 = currentFloor.positions[0].character;
				for (int j = 0; j < currentChar2.positions; j++)
				{
					currentFloor.positions[j].character = null;
				}
				AssignPositionsToCharacter(currentChar2);
				currentFloor.MoveRow(currentChar2.positions);
				for (int k = 0; k < moving_characters.Length; k++)
				{
					if (moving_characters[k] == null)
					{
						moving_characters[k] = currentChar2;
						break;
					}
				}
				bool wentOut2 = false;
				while (currentDoorState == DoorState.Open && !wentOut2)
				{
					if (currentChar2.ArrivedToDestination())
					{
						wentOut2 = true;
						currentChar2.transform.parent = base.transform;
						RemoveFromMovingCharacters(currentChar2);
						currentChar2.LookLeft();
						CharacterGotIn(currentChar2);
					}
					yield return null;
				}
			}
			yield return null;
		}
	}

	private void ManageMovingCharacters()
	{
		for (int i = 0; i < moving_characters.Length; i++)
		{
			if (moving_characters[i] != null)
			{
				if (moving_characters[i].transform.position.x > door.transform.position.x)
				{
					moving_characters[i].transform.parent = base.transform;
					moving_characters[i].LookRight();
					if (!HasCharacterOnPosition(moving_characters[i]))
					{
						AssignPositionsToCharacter(moving_characters[i]);
					}
					else
					{
						moving_characters[i].SetDestinationPosition(base.transform.TransformPoint(GetCharacterPosition(moving_characters[i])));
					}
					moving_characters[i].transform.parent = base.transform;
					CharacterGotIn(moving_characters[i]);
				}
				else
				{
					if (currentFloor != moving_characters[i].destinationFloor)
					{
						currentFloor.InsertCharAtFirstPos(moving_characters[i]);
					}
					if (moving_characters[i].transform.position.x > moving_characters[i].currentFloor.exitPoint.position.x)
					{
						moving_characters[i].LookLeft();
					}
					CharacterGotOut(moving_characters[i]);
				}
			}
			moving_characters[i] = null;
		}
	}

	private void CheckSpecialCharacterGeneralAnimInstantiation(Character_Ctl character)
	{
		if (character.character.specialCharGeneralAnim != null)
		{
			if (character.character.specialCharGeneralAnim.GetInstance() == null)
			{
				character.specialCharacterGeneralAnimation = PoolManager.Instance.InstantiatePooled(character.character.specialCharGeneralAnim.gameObject, Vector3.zero, Quaternion.identity).GetComponent<SpecialCharacterGeneralAnimation>();
			}
			else if (character.character.specialCharGeneralAnim.GetInstance().gameObject.activeInHierarchy)
			{
				character.specialCharacterGeneralAnimation = character.character.specialCharGeneralAnim.GetInstance();
			}
			else
			{
				character.specialCharacterGeneralAnimation = PoolManager.Instance.InstantiatePooled(character.character.specialCharGeneralAnim.gameObject, Vector3.zero, Quaternion.identity).GetComponent<SpecialCharacterGeneralAnimation>();
			}
			character.character.specialCharGeneralAnim.GetInstance().ModifyActiveSpecialCharNumber(1);
		}
	}

	private void CheckSpecialCharacterGeneralAnimInstantiationDisable(Character_Ctl character)
	{
		if (character.character.specialCharGeneralAnim != null)
		{
			character.character.specialCharGeneralAnim.GetInstance().ModifyActiveSpecialCharNumber(-1);
			if (character.character.specialCharGeneralAnim.GetInstance().GetActiveSpecialCharNumber() == 0)
			{
				character.specialCharacterGeneralAnimation.GetInstance().Disable();
			}
		}
	}

	public int GetEmptyPosIndex()
	{
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i].character == null)
			{
				return i;
			}
		}
		for (int j = 0; j < positions.Count; j++)
		{
			UnityEngine.Debug.Log(j + " " + positions[j].character.charClass);
		}
		return -1;
	}

	private bool HasCharacterOnPosition(Character_Ctl character)
	{
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i] != null && positions[i].character != null && positions[i].character == character)
			{
				return true;
			}
		}
		return false;
	}

	private Vector3 GetCharacterPosition(Character_Ctl character)
	{
		int num = -1;
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i] != null && positions[i].character != null && positions[i].character == character)
			{
				num = i;
			}
		}
		if (num != -1)
		{
			return positions[num].position;
		}
		UnityEngine.Debug.LogWarning("Error, characted has not position");
		return Vector3.zero;
	}

	private void RemoveFromPositions(Character_Ctl character)
	{
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i] != null && positions[i].character != null && positions[i].character == character)
			{
				positions[i].character = null;
			}
		}
		ModifyEmptySpaces(character.positions);
	}

	private int GetNumberOfFreePositions()
	{
		int num = 0;
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i].character == null)
			{
				num++;
			}
		}
		return num;
	}

	private void ModifyEmptySpaces(int value)
	{
		elevatorEmptySpaces += value;
	}

	private void RemoveFromMovingCharacters(Character_Ctl character)
	{
		int num = 0;
		while (true)
		{
			if (num < moving_characters.Length)
			{
				if (moving_characters[num] != null && moving_characters[num] == character)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		moving_characters[num] = null;
	}

	private void DoorClosing()
	{
		if (charsGetOutInEnumerator != null)
		{
			StopCoroutine(charsGetOutInEnumerator);
		}
		RepositionCharacters();
		ManageMovingCharacters();
	}

	private void CharacterGotOut(Character_Ctl character)
	{
		character.transform.parent = currentFloor.transform;
		character.currentFloor = currentFloor;
		RemoveFromMovingCharacters(character);
		RemoveFromPositions(character);
		if (character.character.isSpecialChar)
		{
			SpecialCharDelivered(character);
			character.animator.SetBool("InElevator", value: false);
			if (character.gotIntoElevator)
			{
				CheckSpecialCharacterGeneralAnimInstantiationDisable(character);
			}
		}
		CheckNumberOfSpecialCharEqualOrBiggerThan();
	}

	private void CharacterGotIn(Character_Ctl character)
	{
		if (character.character.elevatorStatusSprite != null)
		{
			AddStatusIcon(character.transform.position + Vector3.up * 0.2f, character.character.elevatorStatusSprite);
		}
		CheckSameGroupCharacters();
		CheckSameDestinationCharacters();
		if (character.character.isSpecialChar && !character.gotIntoElevator)
		{
			CheckSpecialCharacterGeneralAnimInstantiation(character);
		}
		character.GotIntoElevator();
		CheckSpecialCharCouplesAtElevator(character);
		CheckNumberOfSpecialCharEqualOrBiggerThan();
		CheckElevatorFilledWithSpecialChars();
	}

	private void CheckSameDestinationCharacters()
	{
		if (GetNumberOfFreePositions() != 0)
		{
			return;
		}
		bool flag = false;
		Floor_Ctl destinationFloor = positions[0].character.destinationFloor;
		for (int i = 1; i < positions.Count; i++)
		{
			if (positions[i].character.destinationFloor != null && destinationFloor != positions[i].character.destinationFloor)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < positions.Count; j++)
			{
				positions[j].character.SetGroupBonus();
			}
			Gameplay_Ctl.Instance.currentGameplaySession.sameFloorDelivered++;
			this.OnComboDone();
		}
	}

	private void CheckElevatorFilledWithSpecialChars()
	{
		if (GetNumberOfFreePositions() != 0)
		{
			return;
		}
		for (int i = 0; i < positions.Count; i++)
		{
			if (!positions[i].character.character.isSpecialChar)
			{
				return;
			}
		}
		Gameplay_Ctl.Instance.currentGameplaySession.elevatorFilledWithSpecialChars++;
	}

	private void CheckSameGroupCharacters()
	{
		if (GetNumberOfFreePositions() != 0)
		{
			return;
		}
		bool flag = false;
		string charClass = positions[0].character.charClass;
		for (int i = 1; i < positions.Count; i++)
		{
			if (positions[i].character != null && !charClass.Equals(positions[i].character.character.charClass))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			return;
		}
		for (int j = 0; j < positions.Count; j++)
		{
			if (positions[j].character != null)
			{
				positions[j].character.SetFamilyBonus();
			}
		}
		Gameplay_Ctl.Instance.currentGameplaySession.sameFamilyDelivered++;
		this.OnComboDone();
	}

	private void CheckNumberOfSpecialCharEqualOrBiggerThan(int number = 2)
	{
		int num = 0;
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i].character != null && positions[i].character.character.isSpecialChar)
			{
				num++;
			}
		}
		twoSpecialsAtElevator = (num >= number);
		specialAtElevator = (num >= 1);
	}

	private void CheckSpecialCharCouplesAtElevator(Character_Ctl lastEnteredChar)
	{
		for (int i = 0; i < positions.Count; i++)
		{
			if (!(positions[i].character != null) || !(positions[i].character != lastEnteredChar) || !positions[i].character.character.isSpecialChar)
			{
				continue;
			}
			if (positions[i].character.charClass.CompareTo("Hauler") == 0)
			{
				if (lastEnteredChar.character.charClass.CompareTo("Scientist") == 0)
				{
					Gameplay_Ctl.Instance.currentGameplaySession.scientistAndHaulerSameTimeAtElevator++;
				}
			}
			else if (positions[i].character.charClass.CompareTo("Newlyweds") == 0)
			{
				if (lastEnteredChar.character.charClass.CompareTo("RepairMan") == 0)
				{
					Gameplay_Ctl.Instance.currentGameplaySession.newlywedsAndRepairmanSameTimeAtElevator++;
				}
			}
			else if (positions[i].character.charClass.CompareTo("RepairMan") == 0)
			{
				if (lastEnteredChar.character.charClass.CompareTo("Newlyweds") == 0)
				{
					Gameplay_Ctl.Instance.currentGameplaySession.newlywedsAndRepairmanSameTimeAtElevator++;
				}
			}
			else if (positions[i].character.charClass.CompareTo("Scientist") == 0 && lastEnteredChar.character.charClass.CompareTo("Hauler") == 0)
			{
				Gameplay_Ctl.Instance.currentGameplaySession.scientistAndHaulerSameTimeAtElevator++;
			}
		}
	}

	private void SpecialCharDelivered(Character_Ctl character)
	{
		Gameplay_Ctl.Instance.currentGameplaySession.specialCharactersDelivered++;
		if (character.character.charClass.CompareTo("Newlyweds") == 0)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.specialDoubleSpaceDelivered++;
		}
		else if (character.character.charClass.CompareTo("RepairMan") == 0)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.specialRepairmenDelivered++;
		}
		else if (character.character.charClass.CompareTo("Scientist") == 0)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.specialScientistsDelivered++;
		}
		else if (character.character.charClass.CompareTo("Hauler") == 0)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.specialHeavyDelivered++;
		}
	}

	private bool RepositionCharacters()
	{
		bool result = false;
		for (int i = 0; i < positions.Count - 1; i++)
		{
			if (positions[i].character == null && positions[i + 1].character != null)
			{
				if (positions[i + 1].character.positions == 1)
				{
					positions[i].character = positions[i + 1].character;
					positions[i + 1].character = null;
					positions[i].character.SetDestinationPosition(base.transform.TransformPoint(GetCharacterPosition(positions[i].character)));
				}
				else if (positions[i + 1].character.positions == 2)
				{
					positions[i].character = positions[i + 2].character;
					positions[i + 2].character = null;
					positions[i].character.SetDestinationPosition(base.transform.TransformPoint(positions[i].position + Vector3.right * currentGapOfPositions * 0.5f));
					i++;
				}
				positions[i].character.transform.parent = base.transform;
				positions[i].character.LookLeft();
				i = -1;
				result = true;
			}
		}
		return result;
	}

	private bool CharactersWithDestinationOnCurrentFloor(Floor_Ctl floor)
	{
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i].character != null && positions[i].character.destinationFloor == floor)
			{
				return true;
			}
		}
		return false;
	}

	public void AddStatusIcon(Vector3 startingPos, Sprite sprite)
	{
		int num = 0;
		while (true)
		{
			if (num < statusIcons.Count)
			{
				if (!statusIcons[num].gameObject.activeInHierarchy)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		Vector3 localPosition = statusIcons[num].transform.localPosition;
		statusIcons[num].sprite = sprite;
		statusIcons[num].transform.position = startingPos;
		statusIcons[num].gameObject.SetActive(value: true);
		statusIcons[num].transform.DOLocalMove(localPosition, 0.3f);
	}

	public void DisableStatusIcon(Sprite sprite)
	{
		int num = 0;
		while (true)
		{
			if (num < statusIcons.Count)
			{
				if (statusIcons[num].sprite == sprite)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		statusIcons[num].sprite = null;
		statusIcons[num].gameObject.SetActive(value: false);
	}

	public void SetEnabledCollisionTrigger(bool value)
	{
		_boxCollider2D.enabled = value;
		_rigidbody2D.simulated = value;
	}

	private void AddTimeToStoppedFloor(float timeToAdd)
	{
		if (currentFloor != null)
		{
			switch (currentFloor.floorNumber)
			{
			case 1:
				Gameplay_Ctl.Instance.currentGameplaySession.elevatorStopedAtFloor0 += timeToAdd;
				break;
			case 2:
				Gameplay_Ctl.Instance.currentGameplaySession.elevatorStopedAtFloor1 += timeToAdd;
				break;
			case 3:
				Gameplay_Ctl.Instance.currentGameplaySession.elevatorStopedAtFloor2 += timeToAdd;
				break;
			case 4:
				Gameplay_Ctl.Instance.currentGameplaySession.elevatorStopedAtFloor3 += timeToAdd;
				break;
			case 5:
				Gameplay_Ctl.Instance.currentGameplaySession.elevatorStopedAtFloor4 += timeToAdd;
				break;
			}
		}
	}

	public void TimeWithoutMehChech()
	{
		if (currentTimeWithoutMeh > (float)Gameplay_Ctl.Instance.currentGameplaySession.bestTimeWithoutMeh)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.bestTimeWithoutMeh = (int)currentTimeWithoutMeh;
		}
	}

	public void TimeWithSpecialCharChech()
	{
		if (currentTimeWithSpecialAtElevator > (float)Gameplay_Ctl.Instance.currentGameplaySession.maxTimeWithSpecialCharInElevator)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.maxTimeWithSpecialCharInElevator = (int)currentTimeWithSpecialAtElevator;
		}
	}
}
