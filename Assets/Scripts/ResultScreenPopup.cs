using Alg;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreenPopup : MonoBehaviour
{
	public Button collectButton;

	public Button videoButton;

	public Text collectText;

	public Text cateringText;

	public Text missionsText;

	public Text videoCoinsText;

	private GameObject videoContainer;

	[SerializeField]
	private Animator animator;

	public int coins;

	[SerializeField]
	private AudioSource menuOutAudioSource;

	private static ResultScreenPopup prefab;

	public ResultScreenPopup Instance;

	private RewardVideoCompleteDelegate rewardVideoCompleteDelegate;

	private bool delegateAdded;

	[Header("Debug")]
	[SerializeField]
	private GameObject debugPanel;

	[SerializeField]
	private Text charsDeliveredPerMinuteText;

	[SerializeField]
	private Text charsDeliveredText;

	[SerializeField]
	private Text totalGameTimeText;

	[SerializeField]
	private Text normalCharsDeliveredText;

	[SerializeField]
	private Text VIPDeliveredText;

	[SerializeField]
	private Text cateringDebugText;

	[SerializeField]
	private Text missionsDebugText;

	public static ResultScreenPopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<ResultScreenPopup>("Popups/Popup.ResultScreen");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init(int catering, int missions)
	{
		coins = catering + missions;
		videoContainer = videoButton.transform.parent.gameObject;
		if (collectText) collectText.text = coins.ToString();
		if (cateringText) cateringText.text = "+" + catering;
		if (missionsText) missionsText.text = "+" + missions;
		if (PlayerDataManager.HighScore < coins)
		{
			SetNewHighscore();
		}
		if (PlayerDataManager.GetPlayerData().hasDoubler)
		{
	//coins += coins;
			DoDoublerAnim();
		}
		collectButton.onClick.AddListener(CollectButtonClicked);
/*if (Singleton<VideoAdsManager>.Instance.IsRewardVideoReady(new RewardData()) && coins > 0)
		{
			videoButton.onClick.AddListener(VideoButtonClicked);
			videoContainer.SetActive(value: true);
			if (videoCoinsText) videoCoinsText.text = (coins * 2).ToString();
		}
		else
		{
			videoContainer.SetActive(value: false);
			Singleton<VideoAdsManager>.Instance.CacheAllRewardedVideos();
		}
*/
	}

	private void DoDoublerAnim()
	{
		collectText.transform.DOScale(Vector3.one * 1.3f, 0.4f).SetDelay(0.35f).OnComplete(delegate
		{
			collectText.text = coins.ToString();
			collectText.transform.DOScale(Vector3.one, 0.4f);
		});
		videoCoinsText.transform.DOScale(Vector3.one * 1.3f, 0.4f).SetDelay(0.35f).OnComplete(delegate
		{
			videoCoinsText.text = (2 * coins).ToString();
			videoCoinsText.transform.DOScale(Vector3.one, 0.4f);
		});
	}

	public void CollectButtonClicked()
	{
		//animator.SetTrigger("Out");
		if (menuOutAudioSource) menuOutAudioSource.Play();
		if (coins > 0)
		{
			Debug.Log("My log CollectButtonClicked - "+ UI_Ctl.coins);
	 PlayerDataManager.AddCoins(UI_Ctl.coins, showOnUI: false);
            GoToMainMenu();
		}
		else
		{
			GoToMainMenu();
		}
	}

	public void CallAnalytics()
	{
//Singleton<AnalyticsManager>.Instance.GameOver(Gameplay_Ctl.Instance.currentGameplaySession);
	}

	private void GoToMainMenu()
	{
		GameManager.Instance.ChangeScene("MainMenu");
	}

	public void VideoButtonClicked()
	{
		if (!delegateAdded)
		{
			rewardVideoCompleteDelegate = (RewardVideoCompleteDelegate)Delegate.Combine(rewardVideoCompleteDelegate, new RewardVideoCompleteDelegate(VideoAdWatched));
			delegateAdded = true;
		}
		Singleton<VideoAdsManager>.Instance.currentReward = new RewardData("DoubleCoins", 0);
		Singleton<VideoAdsManager>.Instance.PlayRewardVideo(Singleton<VideoAdsManager>.Instance.currentReward, rewardVideoCompleteDelegate);
	}

	private void SetNewHighscore()
	{
		PlayerDataManager.SetHighscore(coins);
		//Review_Ctl.Instance.CheckAndShowReviewFLow();
	}

	private void VideoAdWatched(bool rewardEarned)
	{
		rewardVideoCompleteDelegate = (RewardVideoCompleteDelegate)Delegate.Remove(rewardVideoCompleteDelegate, new RewardVideoCompleteDelegate(VideoAdWatched));
		if (rewardEarned && rewardEarned && Singleton<VideoAdsManager>.Instance.currentReward.RewardType.CompareTo("DoubleCoins") == 0)
		{
			Gameplay_Ctl.Instance.currentGameplaySession.watchAdvertToDoubleScore = true;
	//coins *= 2;
			collectText.text = coins.ToString();
			videoContainer.SetActive(value: false);
		}
	}

	private void SetDebugText()
	{
		charsDeliveredPerMinuteText.text += (float)Gameplay_Ctl.Instance.currentGameplaySession.charactersDelivered / ((float)Gameplay_Ctl.Instance.currentGameplaySession.sessionLeght / 60f);
		charsDeliveredText.text += Gameplay_Ctl.Instance.currentGameplaySession.charactersDelivered;
		totalGameTimeText.text += Gameplay_Ctl.Instance.currentGameplaySession.sessionLeght;
		normalCharsDeliveredText.text += Gameplay_Ctl.Instance.currentGameplaySession.charactersDelivered - Gameplay_Ctl.Instance.currentGameplaySession.specialCharactersDelivered;
		VIPDeliveredText.text += Gameplay_Ctl.Instance.currentGameplaySession.specialCharactersDelivered;
		cateringDebugText.text += Gameplay_Ctl.Instance.currentGameplaySession.totalCoins;
		missionsDebugText.text += Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions;
	}
}
