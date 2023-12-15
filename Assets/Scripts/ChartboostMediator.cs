using com.F4A.MobileThird;
using System.Collections.Generic;
using UnityEngine;

public class ChartboostMediator : IAdsMediator
{
	private bool ageGate;

	private bool autocache = true;

	private bool showRewardedVideo = true;

	private RewardVideoCompleteDelegate videoCallback;

    public bool IsAvailable => true;

    public void Init()
	{
	}

	public void OnEnable()
	{
        AdsManager.OnRewardedAdCompleted += AdsManager_OnRewardedAdCompleted;
	}

    public void OnDisable()
	{
        AdsManager.OnRewardedAdCompleted -= AdsManager_OnRewardedAdCompleted;
    }

    public void CacheRewardVideo(string rewardType)
	{
	}

	public void PlayRewardVideo(string rewardType, RewardVideoCompleteDelegate completeDelegate)
	{
		videoCallback = completeDelegate;
		AdsManager.Instance.ShowRewardAds();
	}

	public void PlayInterstitial()
	{
		AdsManager.Instance.ShowInterstitialAds();
	}

	public bool HasRewardVideo(RewardData rewardData)
	{
		return AdsManager.Instance.IsRewardAdsReady();
	}

	public bool IsRewardVideoReady(RewardData rewardData)
	{
		return AdsManager.Instance.IsRewardAdsReady();
	}

    private void AdsManager_OnRewardedAdCompleted(ERewardedAdNetwork adNetwork, string adName, double value)
    {
        if (videoCallback != null)
        {
   //         string rewardType = adNetwork.ToString();
   //         RewardData obj = new RewardData
   //         {
   //             RewardType = rewardType,
   //             Amount = (int)value
			//};
            videoCallback(rewardEarned: true);
            videoCallback = null;
        }
    }
}
