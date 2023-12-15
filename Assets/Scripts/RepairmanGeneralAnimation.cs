using System.Collections;
using UnityEngine;

public class RepairmanGeneralAnimation : SpecialCharacterGeneralAnimation
{
	private static RepairmanGeneralAnimation Instance;

	[SerializeField]
	private Transform elevatorBoostContainer;

	[SerializeField]
	private Animator elevatorBoostAC;

	public int activeSpecialCharNumber;

	private Direction lastDirection;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (activeSpecialCharNumber != 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		activeSpecialCharNumber = 0;
		SetParent();
	}

	public override SpecialCharacterGeneralAnimation GetInstance()
	{
		return Instance;
	}

	private void OnEnable()
	{
		SetParent();
	}

	private void SetParent()
	{
		elevatorBoostContainer.parent = Gameplay_Ctl.Instance.elevator_Ctl.transform;
		elevatorBoostContainer.localPosition = Vector3.zero;
		elevatorBoostContainer.gameObject.SetActive(value: true);
	}

	public override int GetActiveSpecialCharNumber()
	{
		return activeSpecialCharNumber;
	}

	public override void ModifyActiveSpecialCharNumber(int i)
	{
		activeSpecialCharNumber += i;
	}

	private void Update()
	{
		if (Gameplay_Ctl.Instance.elevator_Ctl.currentDirection == Direction.Up && Direction.Up != lastDirection)
		{
			if (Gameplay_Ctl.Instance.elevator_Ctl.elevatorDirModifier == 1f)
			{
				elevatorBoostAC.SetBool("Idle", value: false);
				elevatorBoostAC.SetTrigger("Up");
				lastDirection = Direction.Up;
			}
			else
			{
				elevatorBoostAC.SetBool("Idle", value: false);
				elevatorBoostAC.SetTrigger("Down");
				lastDirection = Direction.Down;
			}
		}
		else if (Gameplay_Ctl.Instance.elevator_Ctl.currentDirection == Direction.Down && Direction.Down != lastDirection)
		{
			if (Gameplay_Ctl.Instance.elevator_Ctl.elevatorDirModifier == 1f)
			{
				elevatorBoostAC.SetBool("Idle", value: false);
				elevatorBoostAC.SetTrigger("Down");
				lastDirection = Direction.Down;
			}
			else
			{
				elevatorBoostAC.SetBool("Idle", value: false);
				elevatorBoostAC.SetTrigger("Up");
				lastDirection = Direction.Up;
			}
		}
		else if (Gameplay_Ctl.Instance.elevator_Ctl.currentDirection == Direction.NoDirection && lastDirection != 0)
		{
			elevatorBoostAC.SetBool("Idle", value: true);
			lastDirection = Direction.NoDirection;
		}
	}

	public override void Disable()
	{
		StartCoroutine(DisableAfterAnimsFinishes());
	}

	private IEnumerator DisableAfterAnimsFinishes()
	{
		elevatorBoostAC.SetBool("Idle", value: false);
		elevatorBoostAC.SetTrigger("End");
		yield return new WaitForSeconds(0.2f);
		elevatorBoostContainer.parent = base.transform;
		PoolManager.Instance.ObjectBackToPool(base.gameObject);
	}
}
