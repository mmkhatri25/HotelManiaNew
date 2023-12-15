using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public bool buttonPressed;

	public void OnPointerDown(PointerEventData eventData)
	{
		buttonPressed = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		buttonPressed = false;
	}
}
