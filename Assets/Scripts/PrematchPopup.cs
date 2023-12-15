using Alg;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrematchPopup : MonoBehaviour
{
	public static PrematchPopup Instance;

	public GameObject missionsDoneContainer;

	public GameObject missionsContainer;

	public GameObject boostersContainer;

	public Button playButton;

	public Button closeButton;

	public Button addBoosterButton1;

	public Button addBoosterButton2;

	public Text mission1Text;

	public Text mission2Text;

	public Text mission3Text;

	public Text timeToEndMissionsText;

	public Text timeToEndMissionsCompletedText;

	private readonly string doublePointString = ":";

	public DateTime nextMissionsTime;

	public HorizontalLayoutGroup horizontalLayout1;

	public HorizontalLayoutGroup horizontalLayout2;

	public HorizontalLayoutGroup horizontalLayout3;

	public Text mission1AmmountText;

	public Text mission2AmmountText;

	public Text mission3AmmountText;

	public Image mission1Fill;

	public Image mission2Fill;

	public Image mission3Fill;

	public Button replaceMission1;

	public Button replaceMission2;

	public Button replaceMission3;

	public Button replaceMission1Debug;

	public Button replaceMission2Debug;

	public Button replaceMission3Debug;

	public Button completeMission1;

	public Button completeMission2;

	public Button completeMission3;

	public Text booster1Amount;

	[SerializeField]
	public BoosterSO booster1;

	public Text booster2Amount;

	[SerializeField]
	public BoosterSO booster2;

	public Text booster3Amount;

	[SerializeField]
	public BoosterSO booster3;

	[SerializeField]
	private AudioSource menuOutAudioSource;

	private int coins;

	[SerializeField]
	private Animator _animator;

	private static PrematchPopup prefab;

	public static PrematchPopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<PrematchPopup>("Popups/Popup.Prematch");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init(List<Mission> missions = null)
	{
		Instance = this;
		PlayerDataManager.OnBoosterAmmountModified += SetBoostersInfo;
		playButton.onClick.AddListener(PlayButtonClicked);
		closeButton.onClick.AddListener(PlayButtonClicked);
		if (GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted() == 3)
		{
			missionsDoneContainer.SetActive(value: true);
			missionsContainer.SetActive(value: false);
		}
		SetMissionsText(missions);
		replaceMission1.onClick.AddListener(ClickedReplaceMission1);
		replaceMission2.onClick.AddListener(ClickedReplaceMission2);
		replaceMission3.onClick.AddListener(ClickedReplaceMission3);
		replaceMission1Debug.onClick.AddListener(ClickedReplaceMission1Debug);
		replaceMission2Debug.onClick.AddListener(ClickedReplaceMission2Debug);
		replaceMission3Debug.onClick.AddListener(ClickedReplaceMission3Debug);
		if (!Singleton<VideoAdsManager>.Instance.IsRewardVideoReady(new RewardData()))
		{
			UnityEngine.Debug.Log("IsRewardVideoReady false");
			Singleton<VideoAdsManager>.Instance.CacheAllRewardedVideos();
			replaceMission1.gameObject.SetActive(value: false);
			replaceMission2.gameObject.SetActive(value: false);
			replaceMission3.gameObject.SetActive(value: false);
		}
		nextMissionsTime = PlayerDataManager.GetLastMissionTime().AddHours(GameManager.Instance.gameVars.hoursForNewMission);
		SetBoostersInfo();
	}

	private void Update()
	{
        print("nextMissionsTime  - " + nextMissionsTime + ",  "+ MainMenu_UI_Ctl.Instance.timeLeftText.text);
       


        if (DateTime.Now < nextMissionsTime)
		{
			string text = $"{(nextMissionsTime - DateTime.Now).Hours:D2}" + doublePointString + $"{(nextMissionsTime - DateTime.Now).Minutes:D2}" + doublePointString + $"{(nextMissionsTime - DateTime.Now).Seconds:D2}";
			print("final  - "+ text);
			if (timeToEndMissionsText.gameObject.activeInHierarchy)
			{
				timeToEndMissionsText.text = text;
			}
			else
			{
				timeToEndMissionsCompletedText.text = text;
			}


		}
		else if (timeToEndMissionsText.text.CompareTo("00:00:00") != 0)
		{
            print("final  - ");
            nextMissionsTime = nextMissionsTime.AddHours(GameManager.Instance.gameVars.hoursForNewMission);
            if (timeToEndMissionsText.gameObject.activeInHierarchy)
			{
				timeToEndMissionsText.text = "00:00:00";
			}
			else
			{
				timeToEndMissionsCompletedText.text = "00:00:00";
			}
		}
        //timeToEndMissionsCompletedText.text = MainMenu_UI_Ctl.Instance.timeLeftText.text;
        timeToEndMissionsText.text = MainMenu_UI_Ctl.Instance.timeLeftText.text;

    }

	public void PlayButtonClicked()
	{
		_animator.SetTrigger("Out");
		menuOutAudioSource.Play();
	}

	public void GoToBoosterShop()
	{
		CataloguePopup.GetInstance(base.transform.parent).Init(1);
	}

	public void ClickedReplaceMission1()
	{
		AdForMissionPopup.GetInstance(Gameplay_Ctl.Instance.uiCanvas.transform).Init(0);
	}

	public void ClickedReplaceMission2()
	{
		AdForMissionPopup.GetInstance(Gameplay_Ctl.Instance.uiCanvas.transform).Init(1);
	}

	public void ClickedReplaceMission3()
	{
		AdForMissionPopup.GetInstance(Gameplay_Ctl.Instance.uiCanvas.transform).Init(2);
	}

	public void ClickedReplaceMission1Debug()
	{
		GameManager.Instance.mission_Ctl.ReplaceMission(GameManager.Instance.mission_Ctl.currentMissions[0]);
		SetMissionText(0);
	}

	public void ClickedReplaceMission2Debug()
	{
		GameManager.Instance.mission_Ctl.ReplaceMission(GameManager.Instance.mission_Ctl.currentMissions[1]);
		SetMissionText(1);
	}

	public void ClickedReplaceMission3Debug()
	{
		GameManager.Instance.mission_Ctl.ReplaceMission(GameManager.Instance.mission_Ctl.currentMissions[2]);
		SetMissionText(2);
	}

	public void ClickedCompleteMission1()
	{
		AdForMissionPopup.GetInstance(Gameplay_Ctl.Instance.uiCanvas.transform).Init(0);
	}

	public void ClickedCompleteMission2()
	{
		AdForMissionPopup.GetInstance(Gameplay_Ctl.Instance.uiCanvas.transform).Init(1);
	}

	public void ClickedCompleteMission3()
	{
		AdForMissionPopup.GetInstance(Gameplay_Ctl.Instance.uiCanvas.transform).Init(2);
	}

	public void SetMissionsText(List<Mission> missions)
	{
		SetMissionText(0);
		SetMissionText(1);
		SetMissionText(2);
	}

	public void SetMissionText(int missionIndex)
	{
		switch (missionIndex)
		{
		case 0:
			mission1Fill.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			mission1Text.text = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].description;
			mission1AmmountText.text = string.Concat("Reward: +" + GameManager.Instance.mission_Ctl.currentMissions[missionIndex].coinRew.ToString());
			break;
		case 1:
			mission2Fill.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			mission2Text.text = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].description;
			mission2AmmountText.text = string.Concat("Reward: +" + GameManager.Instance.mission_Ctl.currentMissions[missionIndex].coinRew.ToString());
			break;
		case 2:
			mission3Fill.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			mission3Text.text = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].description;
			mission3AmmountText.text = string.Concat("Reward: +" + GameManager.Instance.mission_Ctl.currentMissions[missionIndex].coinRew.ToString());
			break;
		}
	}

	private void SetBoostersInfo()
	{
		Text text = booster1Amount;
		PlayerData playerData = PlayerDataManager.GetPlayerData();
		text.text = playerData.booster1Amount.ToString();
		Text text2 = booster2Amount;
		playerData = PlayerDataManager.GetPlayerData();
		text2.text = playerData.booster2Amount.ToString();
	}

	public void ClosePopup()
	{
	//ingleton<AnalyticsManager>.Instance.ExitMissionDisplay();
		Gameplay_Ctl.Instance.StartGame();
		CameraAnim_Ctl.Instance.CameraStartAnim();
		PlayerDataManager.OnBoosterAmmountModified -= SetBoostersInfo;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetFillAmount(int missionIndex)
	{
		switch (missionIndex)
		{
		case 0:
			mission1Fill.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			break;
		case 1:
			mission2Fill.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			break;
		case 2:
			mission3Fill.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			break;
		}
	}
}
