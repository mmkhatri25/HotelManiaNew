using UnityEngine;
using UnityEngine.EventSystems;

public class CustomizableFloorButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public int floorNumber;

	public FloorSO floorSO;

	public GameObject backButton;

	[SerializeField]
	private GameObject floorArt;

	[SerializeField]
	private AudioSource audioSource;

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Customize_Ctl.Instance.FloorToCustomizePressed(this);
		audioSource.Play();
		backButton.SetActive(true);
	}

	public void Setup(FloorSO floorSO)
	{
		this.floorSO = floorSO;
		SetArt(floorSO);
	}

	private void SetArt(FloorSO selectedFloor)
	{
		if (floorArt != null)
		{
			UnityEngine.Object.Destroy(floorArt);
		}
		floorArt = UnityEngine.Object.Instantiate(selectedFloor.floorArt, base.transform);
		floorArt.transform.localPosition = new Vector3(0.031f, 0f, 0f);
	}
}
