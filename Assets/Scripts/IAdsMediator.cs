public interface IAdsMediator
{
	bool IsAvailable
	{
		get;
	}

	void Init();

	void OnEnable();

	void OnDisable();

	void CacheRewardVideo(string campaingName);

	void PlayRewardVideo(string campaingName, RewardVideoCompleteDelegate completeDelegate);

	void PlayInterstitial();

	bool HasRewardVideo(RewardData rewardData);

	bool IsRewardVideoReady(RewardData rewardData);
}
