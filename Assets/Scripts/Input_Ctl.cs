using UnityEngine;

public class Input_Ctl : MonoBehaviour
{
	[SerializeField]
	private DirectionButton upDirectionButton;

	[SerializeField]
	private DirectionButton downDirectionButton;

	[SerializeField]
	private Elevator_Ctl elevator_Ctl;

	private void Update()
	{
		if (!Gameplay_Ctl.Instance.IsGameOver())
		{
			if (UnityEngine.Input.GetAxisRaw("Vertical") > 0f || upDirectionButton.buttonPressed)
			{
				elevator_Ctl.currentDirection = Direction.Up;
			}
			else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0f || downDirectionButton.buttonPressed)
			{
				elevator_Ctl.currentDirection = Direction.Down;
			}
			if (!upDirectionButton.buttonPressed && !downDirectionButton.buttonPressed && UnityEngine.Input.GetAxisRaw("Vertical") == 0f)
			{
				elevator_Ctl.currentDirection = Direction.NoDirection;
			}
		}
	}
}
