using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FloorCustomizeSelectionButton : MonoBehaviour
{
	[SerializeField]
	private Image floorArt;

	[SerializeField]
	private Image selectedFrame;

	[SerializeField]
	private GameObject selectedTick;

	[SerializeField]
	private Text inUseText;

	public FloorSO floorSO;

	[SerializeField]
	private Image inUseFadeImage;

	[SerializeField]
	private Text floorNameText;

	[SerializeField]
	private Image newExclamationImage;

	private Sequence exclamationSeq;

	private bool inUse;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(FloorSelected);
		exclamationSeq = DOTween.Sequence().Append(newExclamationImage.DOFade(0f, 0.5f)).Append(newExclamationImage.DOFade(1f, 0.5f))
			.Append(newExclamationImage.DOFade(0f, 0.5f))
			.Append(newExclamationImage.DOFade(1f, 0.5f))
			.SetLoops(-1);
	}

	public void SetUp(FloorSO floorSO)
	{
		this.floorSO = floorSO;
		SetArt();
		int floorEquipedPosition = Customize_Ctl.Instance.GetFloorEquipedPosition(floorSO);
		if (floorEquipedPosition == -1)
		{
			Deselect();
			return;
		}
		if (floorEquipedPosition == Customize_Ctl.Instance.currentFloorNumber)
		{
			SetSelected(status: true);
			SetActiveInUseText(status: false);
			inUseFadeImage.gameObject.SetActive(value: false);
		}
		else
		{
			SetSelected(status: false);
			SetActiveInUseText(status: true);
			SetInUseFloorNumber(floorEquipedPosition);
			inUseFadeImage.gameObject.SetActive(value: true);
		}
		inUse = true;
	}

	public void Deselect()
	{
		SetActiveInUseText(status: false);
		SetSelected(status: false);
		inUseFadeImage.gameObject.SetActive(value: false);
		inUse = false;
	}

	private void SetArt()
	{
		floorArt.gameObject.SetActive(value: true);
		floorArt.sprite = floorSO.floorSprite;
		if (floorNameText != null)
		{
			floorNameText.gameObject.SetActive(value: true);
			floorNameText.text = floorSO.displayName;
		}
		if (floorSO.neverEquipped)
		{
			if (exclamationSeq != null)
			{
				exclamationSeq.Complete();
			}
			newExclamationImage.gameObject.SetActive(value: true);
			if (exclamationSeq != null)
			{
				exclamationSeq.Complete();
				exclamationSeq.Play();
			}
		}
		else
		{
			newExclamationImage.gameObject.SetActive(value: false);
		}
	}

	private void FloorSelected()
	{
		if (inUse)
		{
			return;
		}
		Customize_Ctl.Instance.ChangeFloor(floorSO);
		SetSelected(status: true);
		inUse = true;
		CustomizePopup2.Instance.ClosePopupPressed();
		if (floorSO.neverEquipped)
		{
			newExclamationImage.gameObject.SetActive(value: false);
			if (exclamationSeq != null)
			{
				exclamationSeq.Complete();
			}
			floorSO.neverEquipped = false;
		}
	}

	private void SetSelected(bool status)
	{
		selectedTick.SetActive(status);
		selectedFrame.gameObject.SetActive(status);
	}

	public void Disable()
	{
		floorArt.gameObject.SetActive(value: false);
		selectedFrame.gameObject.SetActive(value: false);
		selectedTick.SetActive(value: false);
		SetActiveInUseText(status: false);
		inUseFadeImage.gameObject.SetActive(value: false);
		floorNameText.gameObject.SetActive(value: false);
		newExclamationImage.gameObject.SetActive(value: false);
	}

	public void SetActiveInUseText(bool status)
	{
		inUseText.gameObject.SetActive(status);
	}

	public void SetInUseFloorNumber(int i)
	{
		inUseText.text = "In Use (floor " + (i + 1).ToString() + ")";
	}
}
