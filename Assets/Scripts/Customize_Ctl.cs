using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customize_Ctl : MonoBehaviour
{
	public static Customize_Ctl Instance;

	[SerializeField]
	private Transform uiSafeCanvas;

	[HideInInspector]
	public Custumize_UI_Ctl uI_Ctl;

	public Image fadeImage;

	public Button gachaButton;

	private List<FloorSO> nonEquipedFloors = new List<FloorSO>();

	[SerializeField]
	private List<CustomizableFloorButton> floorButtons = new List<CustomizableFloorButton>();

	[HideInInspector]
	public CustomizableFloorButton currentSelectedButton;

	public int currentFloorNumber;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void Start()
	{
		PrepareEquipedRooms();
		PrepareNonEquipedFloors();
		gachaButton.onClick.AddListener(GachaButtonClicked);
	}

	private void PrepareEquipedRooms()
	{
		for (int i = 0; i < GameManager.Instance.playersHotelLayout.Count; i++)
		{
			floorButtons[i].Setup(GameManager.Instance.playersHotelLayout[i]);
		}
	}

	private void PrepareNonEquipedFloors()
	{
		for (int i = 0; i < GameManager.Instance.unlockedFloors.Count; i++)
		{
			if (!GameManager.Instance.playersHotelLayout.Contains(GameManager.Instance.unlockedFloors[i]))
			{
				nonEquipedFloors.Add(GameManager.Instance.unlockedFloors[i]);
			}
		}
	}

	public List<FloorSO> GetNonEquipedFloors()
	{
		return nonEquipedFloors;
	}

	public void FloorToCustomizePressed(CustomizableFloorButton selectedButton)
	{
		currentSelectedButton = selectedButton;
		currentFloorNumber = selectedButton.floorNumber;
		CustomizePopup2 instance = CustomizePopup2.GetInstance(uiSafeCanvas);
		instance.Init(currentSelectedButton.floorSO);
		instance.gameObject.SetActive(value: true);
	}

	public void FloorToCustomizePressed(int index)
	{
		currentFloorNumber = index;
		currentSelectedButton = floorButtons[index];
		CustomizePopup2.Instance.CreateCustomizeFloorList(currentSelectedButton.floorSO);
	}

	public void ChangeFloor(FloorSO selectedFloor)
	{
		nonEquipedFloors.Remove(selectedFloor);
		nonEquipedFloors.Add(currentSelectedButton.floorSO);
		if ((bool)CustomizePopup2.Instance)
		{
			CustomizePopup2.Instance.DeselectButtonIfChangedFloor(currentSelectedButton.floorSO);
		}
		GameManager.Instance.ReplaceInPlayersHotelLayout(currentSelectedButton.floorSO, selectedFloor);
		PlayerDataManager.RemoveIfOnNeverEquipped(selectedFloor.name);
		currentSelectedButton.Setup(selectedFloor);
	}

	public void BackToMainMenu()
	{
		GetComponent<CameraAnim_Ctl>().CloseCustomizationAnim();
		PlayerDataManager.SavePlayersHotelLayout(GameManager.Instance.playersHotelLayout);
		StartCoroutine(BackToMainMenuIE());
	}

	private IEnumerator BackToMainMenuIE()
	{
		yield return new WaitForSeconds(1.1f);
		GameManager.Instance.ChangeScene("MainMenu");
	}

	public int GetFloorEquipedPosition(FloorSO floor)
	{
		for (int i = 0; i < GameManager.Instance.playersHotelLayout.Count; i++)
		{
			if (floor == GameManager.Instance.playersHotelLayout[i])
			{
				return i;
			}
		}
		return -1;
	}

	private void GachaButtonClicked()
	{
		fadeImage.gameObject.SetActive(value: true);
		fadeImage.DOFade(1f, 0.7f).OnComplete(delegate
		{
			GameManager.Instance.ChangeScene("GachaScene");
		});
	}
}
