using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizePopup2 : MonoBehaviour
{
	public static CustomizePopup2 Instance;

	public Button backButton;

	public GameObject floorCustomizeSelectionButton;

	public GameObject dot;

	[SerializeField]
	public Transform dotContainer;

	private List<Image> dotsRenderers = new List<Image>();

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private static CustomizePopup2 prefab;

	[SerializeField]
	private Transform buttonContainer;

	[SerializeField]
	private List<FloorCustomizeSelectionButton> floorCustomizeSelectionButtons = new List<FloorCustomizeSelectionButton>();

	public int currentListIndex;

	public GameObject leftButton;

	public GameObject rightButton;

	[SerializeField]
	private AudioSource menuOutAudioSource;

	public static CustomizePopup2 GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<CustomizePopup2>("Popups/Popup.Customize2");
			Resources.UnloadUnusedAssets();
		}
		return Instance = UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init(FloorSO startingRoom)
	{
		backButton.onClick.AddListener(ClosePopupPressed);
		CreateCustomizeFloorList(startingRoom);
		CreateDots();
		EvaluateArrows();
	}

	public void CreateCustomizeFloorList(FloorSO floorSO = null)
	{
		if (floorSO != null)
		{
			int num = 0;
			for (int i = 0; i < GameManager.Instance.unlockedFloors.Count; i++)
			{
				if (floorSO == GameManager.Instance.unlockedFloors[i])
				{
					num = i;
					break;
				}
			}
			currentListIndex = num / 4;
		}
		for (int j = floorCustomizeSelectionButtons.Count * currentListIndex; j < (currentListIndex + 1) * floorCustomizeSelectionButtons.Count; j++)
		{
			if (j < GameManager.Instance.unlockedFloors.Count)
			{
				floorCustomizeSelectionButtons[j % floorCustomizeSelectionButtons.Count].SetUp(GameManager.Instance.unlockedFloors[j]);
			}
			else
			{
				floorCustomizeSelectionButtons[j % floorCustomizeSelectionButtons.Count].Disable();
			}
		}
	}

	private void CreateDots()
	{
		for (int i = 0; i < GameManager.Instance.unlockedFloors.Count / floorCustomizeSelectionButtons.Count + 1; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(dot, dotContainer);
			dotsRenderers.Add(gameObject.GetComponent<Image>());
		}
		SetDotStatus();
	}

	private void SetDotStatus()
	{
		for (int i = 0; i < dotsRenderers.Count; i++)
		{
			if (i == currentListIndex)
			{
				dotsRenderers[i].color = new Color(0.73f, 0.38f, 0.13f, 1f);
				dotsRenderers[i].transform.localScale = Vector3.one;
			}
			else
			{
				dotsRenderers[i].color = new Color(0.73f, 0.38f, 0.13f, 0.6f);
				dotsRenderers[i].transform.localScale = Vector3.one * 0.5f;
			}
		}
	}

	public void DeselectButtonIfChangedFloor(FloorSO floorSOToDisable)
	{
		for (int i = 0; i < floorCustomizeSelectionButtons.Count; i++)
		{
			if (floorCustomizeSelectionButtons[i].floorSO == floorSOToDisable)
			{
				floorCustomizeSelectionButtons[i].Deselect();
			}
		}
	}

	public void ClosePopupPressed()
	{
		_animator.SetTrigger("Out");
		menuOutAudioSource.Play();
	}

	public void ClosePopup()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void MoveLeft()
	{
		currentListIndex--;
		EvaluateArrows();
		CreateCustomizeFloorList();
		SetDotStatus();
	}

	public void MoveRight()
	{
		currentListIndex++;
		EvaluateArrows();
		CreateCustomizeFloorList();
		SetDotStatus();
	}

	private void EvaluateArrows()
	{
		if (currentListIndex == 0)
		{
			leftButton.SetActive(value: false);
			rightButton.SetActive(value: true);
		}
		else if (1f >= (float)GameManager.Instance.unlockedFloors.Count / ((float)floorCustomizeSelectionButtons.Count * ((float)currentListIndex + 1f)))
		{
			rightButton.SetActive(value: false);
			leftButton.SetActive(value: true);
		}
		else
		{
			leftButton.SetActive(value: true);
			rightButton.SetActive(value: true);
		}
	}
}
