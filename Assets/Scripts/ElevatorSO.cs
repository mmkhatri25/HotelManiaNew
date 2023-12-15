using UnityEngine;

[CreateAssetMenu(fileName = "Elevator", menuName = "Game/Elevator")]
[SerializeField]
public class ElevatorSO : ScriptableObject
{
	[HideInInspector]
	public float currentGapOfPositions = 0.28f;

	public float speed;

	public float accelerationTime = 0.1f;

	public float autoAdjustSpeed;

	public int positions = 3;

	public float doorSpeed;

	public float weight = 1f;

	public float perfectSnap = 0.05f;

	public float goodSnap = 0.25f;

	public float weightMinClamp = 0.2f;

	public float weightMaxClamp = 3.5f;
}
