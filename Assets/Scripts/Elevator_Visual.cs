using UnityEngine;

public class Elevator_Visual : MonoBehaviour
{
	[SerializeField]
	private Animator AnimationController;

	[SerializeField]
	private Transform LeftCog;

	[SerializeField]
	private Transform RightCog;

	[SerializeField]
	private float CogSpeed;

	[SerializeField]
	private float w = 666f;

	private float oldPosY;

	private void Update()
	{
		AnimationController.SetFloat("Blend", (base.transform.position.y - oldPosY) * Time.deltaTime * w + 0.5f);
		LeftCog.Rotate(0f, 0f, (0f - CogSpeed) * Time.deltaTime);
		RightCog.Rotate(0f, 0f, CogSpeed * Time.deltaTime);
		oldPosY = base.transform.position.y;
	}
}
