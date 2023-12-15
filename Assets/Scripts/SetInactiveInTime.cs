using System.Collections;
using UnityEngine;

public class SetInactiveInTime : MonoBehaviour
{
	public float time = 2f;

	public bool setOnEnabled = true;

	private void OnEnable()
	{
		if (setOnEnabled)
		{
			StartCoroutine(Disable());
		}
	}

	private IEnumerator Disable()
	{
		yield return new WaitForSeconds(time);
		PoolManager.Instance.ObjectBackToPool(base.gameObject);
	}

	public void StartCountdown()
	{
		StartCoroutine(Disable());
	}
}
