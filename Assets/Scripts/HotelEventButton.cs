using System;
using UnityEngine;

public class HotelEventButton : MonoBehaviour
{
	[SerializeField]
	private GameObject bubbleImage;

	public event Action WildCardButtonPressed;

	public void SetBubbleImage(GameObject bubbleArt)
	{
		if (bubbleImage != null)
		{
			UnityEngine.Object.Destroy(bubbleImage);
		}
		bubbleImage = UnityEngine.Object.Instantiate(bubbleArt, base.transform);
	}

	private void Pressed()
	{
		this.WildCardButtonPressed();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		this.WildCardButtonPressed();
	}
}
