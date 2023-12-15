using DG.Tweening;
using UnityEngine;

public class EventButtonPrefab : MonoBehaviour
{
	public Transform ray;

	private void Start()
	{
		ray.DORotate(new Vector3(0f, 0f, 360f), 3.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
	}
}
