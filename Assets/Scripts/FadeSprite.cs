using DG.Tweening;
using UnityEngine;

public class FadeSprite : MonoBehaviour
{
	public SpriteRenderer renderer;

	public void FadeOutSpeedBased()
	{
		renderer.DOFade(0f, 1f).SetSpeedBased().SetEase(Ease.Linear);
	}

	public void FadeInSpeedBased()
	{
		renderer.DOFade(1f, 1f).SetSpeedBased().SetEase(Ease.Linear);
	}

	public void FadeInTimeBased()
	{
		renderer.DOFade(0.75f, 0.2f).SetSpeedBased().SetEase(Ease.Linear);
		renderer.sortingOrder = 15;
	}
}
