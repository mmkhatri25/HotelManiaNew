using UnityEngine;
using UnityEngine.UI;

public class DevPanel_Ctl : MonoBehaviour
{
	public Button forceReviewButton;

	public Button addHCButton;

	private void Start()
	{
		forceReviewButton.onClick.AddListener(ShowReviewPopup);
		addHCButton.onClick.AddListener(AddHC);
	}

	private void ShowReviewPopup()
	{
		Review_Ctl.Instance.ShowReviewPopup();
	}

	private void AddHC()
	{
		PlayerDataManager.AddHardCurrency(100);
	}
}
