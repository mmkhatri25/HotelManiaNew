using Alg;
using UnityEngine;
using UnityEngine.UI;

public class AdForMissionPopup : MonoBehaviour
{
	private static AdForMissionPopup prefab;

	private int selectedMissionIndex;

	public Button yesButton;

	public Button closeButton;

	[SerializeField]
	private Animator _animator;

	public static AdForMissionPopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<AdForMissionPopup>("Popups/Popup.AdForMission");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init(int index)
	{
		selectedMissionIndex = index;
		yesButton.onClick.AddListener(ClickedYes);
		closeButton.onClick.AddListener(ClickedClose);
	}

	public void ClickedYes()
	{
		UnityEngine.Debug.Log("selectedMissionIndex: " + selectedMissionIndex);
		Singleton<VideoAdsManager>.Instance.currentReward = new RewardData("ReplaceMission", selectedMissionIndex);
		Singleton<VideoAdsManager>.Instance.PlayRewardVideo(Singleton<VideoAdsManager>.Instance.currentReward, Singleton<VideoAdsManager>.Instance.HandleRewardVideoCompleteDelegate);
		ClickedClose();
	}

	public void ClickedClose()
	{
		_animator.SetTrigger("Out");
	}

	public void ClosePopup()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
