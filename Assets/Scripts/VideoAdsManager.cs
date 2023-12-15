using Alg;
using UnityEngine.SceneManagement;

public class VideoAdsManager : Singleton<VideoAdsManager>
{
	private IAdsMediator mediator;

	public RewardData currentReward;

	public bool IsAvailable => mediator.IsAvailable;

	private void Start()
	{
		Awake();
		Init();
	}

	private new void Init()
	{
		if (mediator == null)
		{
			mediator = new ChartboostMediator();
			mediator.Init();
			CacheAllRewardedVideos();
		}
	}

	private void OnEnable()
	{
		Init();
		mediator.OnEnable();
	}

	private void OnDisable()
	{
		if (mediator != null)
		{
			mediator.OnDisable();
		}
	}

	public void CacheRewardVideo(RewardData rewardData)
	{
		mediator.CacheRewardVideo(rewardData.Campaing);
	}

	public void PlayRewardVideo(RewardData rewardData, RewardVideoCompleteDelegate OnCompleted)
	{
		//GeneralAudioController.Instance.PauseMenuMusic();
		//mediator.PlayRewardVideo(rewardData.RewardType, OnCompleted);
		HandleRewardVideoCompleteDelegate(true);
		//CacheAllRewardedVideos();
	}

	public void PlayInterstitial()
	{
		mediator.PlayInterstitial();
	}

	public bool HasRewardVideo(RewardData rewardData)
	{
		return mediator.HasRewardVideo(rewardData);
	}

	public bool IsRewardVideoReady(RewardData rewardData)
	{
		return mediator.IsRewardVideoReady(rewardData);
	}

	public void CacheAllRewardedVideos()
	{
		RewardData rewardData = new RewardData();
		if (!IsRewardVideoReady(rewardData))
		{
			CacheRewardVideo(rewardData);
		}
	}

	public void HandleRewardVideoCompleteDelegate(bool rewardEarned)
	{
		if ((bool)GeneralAudioController.Instance)
		{
			GeneralAudioController.Instance.ResumeMenuMusic();
		}
		if (rewardEarned)
		{
			RewardManager.GiveReward(Singleton<VideoAdsManager>.Instance.currentReward.RewardType, Singleton<VideoAdsManager>.Instance.currentReward.Amount);
		}
	}

	public void OnVideoAdsCompleted(bool rewardEarned, RewardData rewardData)
	{
	}

	        void LateUpdate()
        {
            if(SceneManager.GetActiveScene().name == "Gameplay")
            {
                Destroy(gameObject);
            }
        }
}
