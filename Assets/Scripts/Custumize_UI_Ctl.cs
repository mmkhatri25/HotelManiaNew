using DG.Tweening;
using UnityEngine;

public class Custumize_UI_Ctl : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer hotelFacade;

	public void FadeHotelFacade(float time, Ease selectedEasing, float value)
	{
		hotelFacade.DOFade(value, time).SetEase(selectedEasing);
	}
}
