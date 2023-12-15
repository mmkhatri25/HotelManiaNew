using UnityEngine;

public class EmitWhenEnabled : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem particleSystem;

	[SerializeField]
	private int numberOfParticles = 15;

	private void OnEnable()
	{
		particleSystem.Emit(numberOfParticles);
	}
}
