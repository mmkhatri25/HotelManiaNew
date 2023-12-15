using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Character_Ctl : MonoBehaviour
{
	public CharacterSO character;

	public string charClass;

	public float speed;

	public int positions;

	private int baseScore;

	private int matchedScore = 1;

	private int familyScore = 1;

	public float weightMod;

	public float elevatorDirMod = 1f;

	public Floor_Ctl originFloor;

	public Floor_Ctl currentFloor;

	public Floor_Ctl destinationFloor;

	public Floor_Ctl startingDestinationFloor;

	public Vector3 destinationPos;

	public bool groupBonus;

	public bool familyBonus;

	public bool robbed;

	public bool gotIntoElevator;

	[Header("VISUAL REFS")]
	public Animator animator;

	public GameObject art;

	[SerializeField]
	private SpriteRenderer artBody;

	[SerializeField]
	private SpriteRenderer artLegLeft;

	[SerializeField]
	private SpriteRenderer artLegRight;

	[SerializeField]
	private SpriteRenderer statusSpriteR;

	private int artLegsStartingLayer;

	private int artBodyStartingLayer;

	private bool destinationVisualSet;

	private bool waveOnQueueMade;

	[SerializeField]
	private SpriteRenderer bubble;

	private Vector3 bubbleStartScale;

	[SerializeField]
	private TextMesh floorText;

	private Vector3 floorTextStartScale;

	private int bubbleStartingSortingOrder;

	private int floorTextStartingLayer;

	[SerializeField]
	private MeshLayerOrder floorTextMeshLayerOrder;

	private Vector3 lookLeft = new Vector3(0.525f, 0.525f, 0.525f);

	private Vector3 lookLeftText;

	private Vector3 lookRight = new Vector3(-0.525f, 0.525f, 0.525f);

	private Vector3 lookRightText;

	[SerializeField]
	private ParticleSystem dustParticleSystem;

	[SerializeField]
	private GameObject coinParticle;

	[SerializeField]
	private GameObject confettiParticle;

	private BoxCollider2D _boxCollider2D;

	private Rigidbody2D _rigidbody2D;

	[HideInInspector]
	public SpecialCharacterGeneralAnimation specialCharacterGeneralAnimation;

	[Header("Visual Refs ONLY SPECIAL")]
	[SerializeField]
	private SpecialCharacterArtOrder specialCharacterArtOrder;

	private GameObject specialCharGeneralAnimRef;

	private void Awake()
	{
		floorTextMeshLayerOrder = floorText.GetComponent<MeshLayerOrder>();
		bubbleStartingSortingOrder = bubble.sortingOrder;
		floorTextStartingLayer = bubbleStartingSortingOrder + 1;
		floorTextStartScale = floorText.transform.localScale;
		bubbleStartScale = bubble.transform.localScale;
		_boxCollider2D = art.GetComponent<BoxCollider2D>();
		_rigidbody2D = art.GetComponent<Rigidbody2D>();
		artLegsStartingLayer = artLegLeft.sortingOrder;
		artBodyStartingLayer = artBody.sortingOrder;
	}

	private void Update()
	{
		if (!(destinationPos != Vector3.zero) || Gameplay_Ctl.Instance.IsGameOver() || Time.timeScale != 1f)
		{
			return;
		}
		if (!destinationVisualSet && originFloor.entryPoint.position.x < base.transform.position.x + 0.1f)
		{
			destinationVisualSet = true;
			SetDestinationFloorVisual(destinationFloor);
		}
		if (!ArrivedToDestination())
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, new Vector3(destinationPos.x, base.transform.position.y, base.transform.position.z), speed * Time.deltaTime);
		}
		else if (destinationFloor == currentFloor && destinationPos != currentFloor.entryPoint.position)
		{
			StartCoroutine(CharGotOut());
		}
		else if (destinationPos.x != currentFloor.entryPoint.position.x)
		{
			SetDestinationPosition(Vector3.zero);
			if (base.transform.position.x > currentFloor.positions[0].position.x)
			{
				animator.SetBool("InElevator", value: true);
				animator.SetBool("Running", value: false);
				LookLeft();
				return;
			}
			animator.SetBool("Running", value: false);
			animator.SetBool("InElevator", value: false);
			LookRight();
			if (!waveOnQueueMade)
			{
				currentFloor.MakeWave(this);
				waveOnQueueMade = true;
			}
		}
		else if (destinationPos.x == currentFloor.entryPoint.position.x)
		{
			BackToPool();
		}
	}

	public void Setup(Floor_Ctl origin)
	{
		base.name = character.charClass;
		charClass = character.charClass;
		speed = character.baseStatsSO.speed;
		positions = character.positions;
		matchedScore = character.baseStatsSO.matchedScore;
		familyScore = character.baseStatsSO.familyScore;
		baseScore = character.baseStatsSO.baseScore;
		weightMod = character.weightMod;
		elevatorDirMod = character.elevatorDirectionMod;
		SetOrigin(origin);
		groupBonus = false;
		familyBonus = false;
		robbed = false;
		gotIntoElevator = false;
		waveOnQueueMade = false;
		statusSpriteR.gameObject.SetActive(value: false);
		statusSpriteR.sprite = null;
		destinationVisualSet = false;
		animator = art.GetComponent<Animator>();
		floorText.transform.localScale = Vector3.zero;
		bubble.transform.localScale = Vector3.zero;
		SetEnabledCollisionTrigger(value: false);
		bubble.enabled = true;
		LookRight();
		if (character.charsArtList.Count > 0 && !character.isSpecialChar)
		{
			int index = UnityEngine.Random.Range(0, character.charsArtList.Count);
			artBody.sprite = character.charsArtList[index].body;
			artLegLeft.sprite = character.charsArtList[index].legLeft;
			artLegRight.sprite = character.charsArtList[index].legRigth;
		}
	}

	public void SetOrigin(Floor_Ctl origin)
	{
		currentFloor = origin;
		originFloor = origin;
		base.transform.position = origin.entryPoint.position;
	}

	public void SetDestinationFloor(Floor_Ctl destination, bool changeParent = true)
	{
		destinationFloor = destination;
		if (changeParent && destination != null)
		{
			base.transform.parent = destinationFloor.transform;
		}
	}

	public void SetStartingDestinationFloor(Floor_Ctl destination)
	{
		startingDestinationFloor = destination;
	}

	public void SetDestinationFloorVisual(Floor_Ctl destination, bool setJustText = false)
	{
		if (destination == null)
		{
			floorText.text = "?";
		}
		else
		{
			floorText.text = destination.floorNumber.ToString();
		}
		if (!setJustText)
		{
			floorText.transform.DOScale(floorTextStartScale, 0.3f).SetEase(Ease.OutBack);
			bubble.transform.DOScale(bubbleStartScale, 0.3f).SetEase(Ease.OutBack);
		}
	}

	private void HideFloorDestinationVisual()
	{
		bubble.gameObject.SetActive(value: false);
	}

	public void SetDestinationPosition(Vector3 destPos)
	{
		destinationPos = base.transform.TransformVector(destPos);
		if (destPos == Vector3.zero)
		{
			MoveArtBack();
			animator.SetBool("Running", value: false);
			SetEmitDustEnabled(value: false);
		}
		else
		{
			MoveArtFront();
			animator.SetBool("Running", value: true);
			SetEmitDustEnabled(value: true);
		}
	}

	public bool ArrivedToDestination()
	{
		return new Vector3(destinationPos.x, base.transform.position.y, base.transform.position.z) == base.transform.position;
	}

	public bool IsAtDestination()
	{
		return destinationPos == Vector3.zero;
	}

	private bool IsAtElevator()
	{
		return base.transform.position.x > Gameplay_Ctl.Instance.elevator_Ctl.door.transform.position.x;
	}

	private IEnumerator CharGotOut()
	{
		destinationPos = currentFloor.entryPoint.position;
		bubble.enabled = false;
		floorText.transform.localScale = Vector3.zero;
		int coins = 0;
		if (!robbed)
		{
			if (groupBonus || familyBonus)
			{
				int num = 0;
				if (groupBonus)
				{
					num += matchedScore;
				}
				if (familyBonus)
				{
					num += familyScore;
				}
				coins = Mathf.FloorToInt((float)(num * currentFloor.points) * Gameplay_Ctl.Instance.elevator_Ctl.lastSnapMultiplayer);
				GameObject gameObject = PoolManager.Instance.InstantiatePooled(confettiParticle, art.transform.position + Vector3.up * 0.3f, Quaternion.identity, null);
				gameObject.transform.parent = base.transform;
				gameObject.SetActive(value: true);
			}
			else
			{
				coins = Mathf.FloorToInt((float)(baseScore * currentFloor.points) * Gameplay_Ctl.Instance.elevator_Ctl.lastSnapMultiplayer);
			}
			yield return new WaitForSeconds(1f);
		}
		if (!Gameplay_Ctl.Instance.IsGameOver())
		{
			if (!robbed)
			{
				PoolManager.Instance.InstantiatePooled(coinParticle, art.transform.position + Vector3.up * 0.3f, Quaternion.identity, null).SetActive(value: true);
			}
			Gameplay_Ctl.Instance.AddCoinsToSession(coins);
			Gameplay_Ctl.Instance.currentGameplaySession.charactersDelivered++;
			switch (currentFloor.floorNumber)
			{
			case 1:
				Gameplay_Ctl.Instance.currentGameplaySession.charactersDeliveredFloor1++;
				break;
			case 2:
				Gameplay_Ctl.Instance.currentGameplaySession.charactersDeliveredFloor2++;
				break;
			case 3:
				Gameplay_Ctl.Instance.currentGameplaySession.charactersDeliveredFloor3++;
				break;
			case 4:
				Gameplay_Ctl.Instance.currentGameplaySession.charactersDeliveredFloor4++;
				break;
			case 5:
				Gameplay_Ctl.Instance.currentGameplaySession.charactersDeliveredFloor5++;
				break;
			}
			if (groupBonus)
			{
				Gameplay_Ctl.Instance.currentGameplaySession.sameFloorDelivered++;
			}
		}
	}

	public void BackToPool()
	{
		floorText.text = "";
		base.gameObject.SetActive(value: false);
		base.transform.parent = PoolManager.Instance.poolContainer;
	}

	public void SetGroupBonus()
	{
		groupBonus = true;
		animator.SetTrigger("Happy");
	}

	public void SetFamilyBonus()
	{
		familyBonus = true;
		animator.SetTrigger("Happy");
	}

	public void SetRobbed(Sprite robbedSprite)
	{
		robbed = true;
		statusSpriteR.sprite = robbedSprite;
		statusSpriteR.gameObject.SetActive(value: true);
	}

	public void LookRight()
	{
		art.transform.localScale = lookRight;
		floorText.transform.localScale = floorTextStartScale;
	}

	public void LookLeft()
	{
		art.transform.localScale = lookLeft;
		floorText.transform.localScale = floorTextStartScale;
	}

	private void SetEmitDustEnabled(bool value)
	{
		dustParticleSystem.enableEmission = value;
	}

	private void MoveArtFront()
	{
		bubble.sortingOrder = bubbleStartingSortingOrder + 5;
		floorTextMeshLayerOrder.SetSortingOrder(floorTextStartingLayer + 5);
		if (character.isSpecialChar)
		{
			specialCharacterArtOrder.MoveArtFront();
			return;
		}
		artLegLeft.sortingOrder = artLegsStartingLayer + 5;
		artLegRight.sortingOrder = artLegsStartingLayer + 5;
		artBody.sortingOrder = artBodyStartingLayer + 5;
	}

	private void MoveArtBack()
	{
		bubble.sortingOrder = bubbleStartingSortingOrder;
		floorTextMeshLayerOrder.SetSortingOrder(floorTextStartingLayer);
		if (character.isSpecialChar)
		{
			specialCharacterArtOrder.MoveArtBack();
			return;
		}
		artLegLeft.sortingOrder = artLegsStartingLayer;
		artLegRight.sortingOrder = artLegsStartingLayer;
		artBody.sortingOrder = artBodyStartingLayer;
	}

	public void GotIntoElevator()
	{
		if (!gotIntoElevator)
		{
			gotIntoElevator = true;
			if (character.isSpecialChar)
			{
				SpecialCharacterAnim_Ctl.Instance.AddToShow(character.specialCharIntroAnimPrefab);
			}
		}
	}

	public void SetEnabledCollisionTrigger(bool value)
	{
		_boxCollider2D.enabled = value;
		_rigidbody2D.simulated = value;
	}

	public void Dissappear()
	{
	}
}
