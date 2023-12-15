using UnityEngine;

public class ChoseRandomValueForFloorAnim : StateMachineBehaviour
{
	public int clips;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetInteger("RandomClip", Random.Range(0, clips));
	}
}
