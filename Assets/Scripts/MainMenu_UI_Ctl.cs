using Alg;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenu_UI_Ctl : MonoBehaviour
{
	private const float _introSkipNormalizedTime = 0.55f;

	public static MainMenu_UI_Ctl Instance;

	public Transform canvasTransform;

	public Image fadeImage;

	public GameObject devCanvas;

	public Button devButton;

	public Button playButton;

	public Button settingsButton;

	public Button facebookButton;

	public Button getFreeCoins;

	public Button gachaButton;

	public Button shopButton;

	public Button showMissionsButton;

	public Button hideMissionsButton;

	public Button clearPlayerDataButton;

	public Button goToCustomization;

	public Button goToChilindo;

	public Text coinsText;

	public Text hardCurrencyText;

	public Image coinBoxImage;

	public Camera camera;

	[SerializeField]
	private Animator mainMenuIntroAC;

	[SerializeField]
	private Animator missionPanelAC;

	private LeaderboardPopup leaderboardPopup;

	public GameObject notificationGachaGO;

	public GameObject notificationCustomizeGO;

	[Header("MissionPanel")]
	[SerializeField]
	public Text missionTextCounter;

	[SerializeField]
	public Text timeLeftText;

	[SerializeField]
	public GameObject mission1ActiveGO;

	[SerializeField]
	public GameObject mission1FinishedGO;

	[SerializeField]
	public Text mission1Text;

	[SerializeField]
	public Image fillBarMission1;

	[SerializeField]
	public Button ReplaceMission1Button;

	[SerializeField]
	public Text mission1Reward;

	[SerializeField]
	public GameObject mission2ActiveGO;

	[SerializeField]
	public GameObject mission2FinishedGO;

	[SerializeField]
	public Text mission2Text;

	[SerializeField]
	public Image fillBarMission2;

	[SerializeField]
	public Button ReplaceMission2Button;

	[SerializeField]
	public Text mission2Reward;

	[SerializeField]
	public GameObject mission3ActiveGO;

	[SerializeField]
	public GameObject mission3FinishedGO;

	[SerializeField]
	public Text mission3Text;

	[SerializeField]
	public Image fillBarMission3;

	[SerializeField]
	public Button ReplaceMission3Button;

	[SerializeField]
	public Text mission3Reward;

	private DateTime nextMissionsTime;

	private bool showingMissionPanel;

	private string doublePointString = ":";

	private bool _insideIntroAnim;

	public enableOnClick enableOnClick;

	

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
		//MissionsButtonPressed();
		//if (enableOnClick != null)
		//{ enableOnClick.myOnClickOn(); }
        //Application.targetFrameRate = 30;
        devButton.onClick.AddListener(OpenDevCanvas);
		playButton.onClick.AddListener(PlayButtonPressed);
		getFreeCoins.onClick.AddListener(OpenShop);
		facebookButton.onClick.AddListener(LoginWithFacebook);
		settingsButton.onClick.AddListener(OpenSettings);
		shopButton.onClick.AddListener(ShopButtonPressed);
		gachaButton.onClick.AddListener(GoToGachaScene);
		clearPlayerDataButton.onClick.AddListener(ClearPlayerData);
		goToCustomization.onClick.AddListener(CustomizationButtonPressed);
		goToChilindo.onClick.AddListener(ChilindoButtonClicked);
		showMissionsButton.onClick.AddListener(MissionsButtonPressed);
		hideMissionsButton.onClick.AddListener(HideMissionsButtonPressed);
		SetCoinsText(PlayerDataManager.Coins);
		hardCurrencyText.text = PlayerDataManager.HardCurrency.ToString();
        Debug.Log("My LOG - On main I have hardCurrency - " + PlayerDataManager.HardCurrency);
  //      PlayerDataManager.AddHardCurrency(10000);
		//PlayerDataManager.AddCoins(10000);
        SetUpMissionPanel();
		AudioManager.Initialize();
		PlayerDataManager.OnCoinChanged += OnCoinChanged;
		PlayerDataManager.OnHardCurrencyChanged += OnHardCurrencyChanged;
		if (!PlayerPrefs.HasKey("TutorialDone"))
		{
			GameManager.Instance.introAnimPlayed = true;
			DoAnimToTuto();
		}
		else if (!GameManager.Instance.introAnimPlayed)
		{
			GameManager.Instance.introAnimPlayed = true;
			DoIntroAnim();
		}
		else
		{
			DoLoopAnim();
			GetInUI();
		}
        MissionsButtonPressed();
        if (enableOnClick != null)
        { enableOnClick.myOnClickOn(); }
        //HideMissionsButtonPressed();
        if (enableOnClick != null)
        { enableOnClick.myOnClickOff(); }

    }

    private void Update()
	{
		if (_insideIntroAnim && mainMenuIntroAC.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.55f && Input.GetMouseButtonDown(0))
		{
			SkipIntroAnim();
		}
		if (showingMissionPanel)
		{
			if (DateTime.Now < nextMissionsTime)
			{
				//string text = $"{(nextMissionsTime - DateTime.Now).Hours:D2}" + doublePointString + $"{(nextMissionsTime - DateTime.Now).Minutes:D2}" + doublePointString + $"{(nextMissionsTime - DateTime.Now).Seconds:D2}";
                string text = $"{(nextMissionsTime - DateTime.Now).Minutes:D2}" + doublePointString + $"{(nextMissionsTime - DateTime.Now).Seconds:D2}";
				timeLeftText.text = text;
			}
			else
			{
				nextMissionsTime = nextMissionsTime.AddHours(GameManager.Instance.gameVars.hoursForNewMission);
				GameManager.Instance.mission_Ctl.ReplaceAllMissions();
				SetUpMissionPanel();
			}
		}

		

    }

	public void SetGachaExclamation()
	{
		if (PlayerDataManager.CanPlayerUnlockRoom() || (GameManager.Instance.firstMatchPlayed && !GameManager.Instance.freeGachaUsed))
		{
			notificationGachaGO.SetActive(value: true);
		}
	}

	public void SetCustomizeExclamation()
	{
		if (PlayerDataManager.HasNeverEquippedFloors())
		{
			notificationCustomizeGO.SetActive(value: true);
		}
	}

	public void SetCoinsText(int value)
	{
		coinsText.text = value.ToString();
	}

	public void OnCoinChanged(int amount)
	{
		SetCoinsText(PlayerDataManager.Coins);
	}

	public void OnHardCurrencyChanged()
	{
		hardCurrencyText.text = PlayerDataManager.HardCurrency.ToString();
	}

	public void PlayButtonPressed()
	{
	/*
		Singleton<AnalyticsManager>.Instance.PressPlayButton();
		fadeImage.gameObject.SetActive(value: true);
		mainMenuIntroAC.SetTrigger("GetOut");
		fadeImage.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			//GameManager.Instance.ChangeScene("Gameplay");
		});
	*/
	}

	private void OpenShop()
	{
		ShopPopup.GetInstance(canvasTransform);
	}

	public void CustomizationButtonPressed()
	{
		//Singleton<AnalyticsManager>.Instance.FirstTimeCustomizationMenu();
		GameManager.Instance.ChangeScene("CustomizationMenu");
	}

	private void ShopButtonPressed()
	{
		CataloguePopup.GetInstance(canvasTransform).Init(0);
		//Singleton<AnalyticsManager>.Instance.FirstTimeCatalogueMenu();
	}

	private void MissionsButtonPressed()
	{
		//if (GameManager.Instance.gameVars.hoursForNewMission > 1)
		//	GameManager.Instance.gameVars.hoursForNewMission = 1;
        print("GameManager.Instance.gameVars.hoursForNewMission - "+ GameManager.Instance.gameVars.hoursForNewMission + ", PlayerDataManager.GetLastMissionTime() -  "+ PlayerDataManager.GetLastMissionTime());
		nextMissionsTime = PlayerDataManager.GetLastMissionTime().AddHours(GameManager.Instance.gameVars.hoursForNewMission);
		showingMissionPanel = true;
		missionPanelAC.SetTrigger("MissionPanelIn");
	}

	private void HideMissionsButtonPressed()
	{
		showingMissionPanel = false;
		missionPanelAC.SetTrigger("MissionPanelOut");
	}

	private void LoginWithFacebook()
	{
		if (!Singleton<DBConnection_Ctl>.Instance.linkedFB)
		{
			
		}
		else
		{
			ShowLeaderboard();
		}
	}

	public void ShowLeaderboard()
	{
		if (leaderboardPopup == null)
		{
			leaderboardPopup = LeaderboardPopup.GetInstance();
			leaderboardPopup.transform.SetParent(canvasTransform, worldPositionStays: false);
			leaderboardPopup.Init();
			leaderboardPopup.OnPopupClosed += delegate
			{
			};
		}
	}

	private void OpenSettings()
	{
		SettingsPopup instance = SettingsPopup.GetInstance();
		instance.transform.SetParent(canvasTransform, worldPositionStays: false);
		instance.Init();
	}

	public void GetInUI()
	{
		canvasTransform.gameObject.SetActive(value: true);
	}

	public void CheckNotificationsOnMainMenu()
	{
		SetGachaExclamation();
		SetCustomizeExclamation();
	}

	public void DoIntroAnim()
	{
		mainMenuIntroAC.SetTrigger("StartIntro");
		_insideIntroAnim = true;
	}

	private void SkipIntroAnim()
	{
		AnimatorStateInfo currentAnimatorStateInfo = mainMenuIntroAC.GetCurrentAnimatorStateInfo(0);
		mainMenuIntroAC.Play(currentAnimatorStateInfo.fullPathHash, 0, 0.55f);
		_insideIntroAnim = false;
	}

	public void DoLoopAnim()
	{
		mainMenuIntroAC.SetTrigger("Loop");
		_insideIntroAnim = false;
	}

	public void DoAnimToTuto()
	{
		mainMenuIntroAC.SetTrigger("Tutorial");
	}

	private void OnDestroy()
	{
		PlayerDataManager.OnCoinChanged -= OnCoinChanged;
		PlayerDataManager.OnHardCurrencyChanged -= OnHardCurrencyChanged;
	}

	private void OpenDevCanvas()
	{
		devCanvas.gameObject.SetActive(value: true);
	}

	public void GoToGachaScene()
	{
		//ngleton<AnalyticsManager>.Instance.FirstTimeGachaButton();
		fadeImage.gameObject.SetActive(value: true);
		mainMenuIntroAC.SetTrigger("GetOut");
		fadeImage.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			GameManager.Instance.ChangeScene("GachaScene");
		});
	}

	public void ClearPlayerData()
	{
		PlayerPrefs.DeleteAll();
		PlayerDataManager.ClearData();
	}

	private void ChilindoButtonClicked()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.chilindo&hl=en");
	}

	private void SetUpMissionPanel()
	{
		if (GameManager.Instance.mission_Ctl.currentMissions == null || GameManager.Instance.mission_Ctl.currentMissions.Count == 0)
		{
			GameManager.Instance.mission_Ctl.CreateMissionList();
		}
		missionTextCounter.text = GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted() + "/3";
		SetMissionText(0);
		SetMissionText(1);
		SetMissionText(2);
		if (GameManager.Instance.mission_Ctl.currentMissions[0].IsCompleted())
		{
			mission1ActiveGO.SetActive(value: false);
			mission1FinishedGO.SetActive(value: true);
		}
		else
		{
			mission1ActiveGO.SetActive(value: true);
			mission1FinishedGO.SetActive(value: false);
			ReplaceMission1Button.onClick.AddListener(ClickedReplaceMission1);
		}
		if (GameManager.Instance.mission_Ctl.currentMissions[1].IsCompleted())
		{
			mission2ActiveGO.SetActive(value: false);
			mission2FinishedGO.SetActive(value: true);
		}
		else
		{
			mission2ActiveGO.SetActive(value: true);
			mission2FinishedGO.SetActive(value: false);
			ReplaceMission2Button.onClick.AddListener(ClickedReplaceMission2);
		}
		if (GameManager.Instance.mission_Ctl.currentMissions[2].IsCompleted())
		{
			mission3ActiveGO.SetActive(value: false);
			mission3FinishedGO.SetActive(value: true);
		}
		else
		{
			mission3ActiveGO.SetActive(value: true);
			mission3FinishedGO.SetActive(value: false);
			ReplaceMission3Button.onClick.AddListener(ClickedReplaceMission3);
		}
	}

	public void ClickedReplaceMission1()
	{
		AdForMissionPopup.GetInstance(canvasTransform).Init(0);
	}

	public void ClickedReplaceMission2()
	{
		AdForMissionPopup.GetInstance(canvasTransform).Init(1);
	}

	public void ClickedReplaceMission3()
	{
		AdForMissionPopup.GetInstance(canvasTransform).Init(2);
	}

	public void SetMissionText(int missionIndex)
	{
		print(" missionIndex - "+ missionIndex + ", GameManager.Instance.mission_Ctl.currentMissions[missionIndex] - "+ GameManager.Instance.mission_Ctl.currentMissions[missionIndex].missionName);
		switch (missionIndex)
		{
		case 0:
			fillBarMission1.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			mission1Text.text = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].description;
			mission1Reward.text = "+" + GameManager.Instance.mission_Ctl.currentMissions[missionIndex].coinRew;
			break;
		case 1:
			fillBarMission2.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			mission2Text.text = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].description;
			mission2Reward.text = "+" + GameManager.Instance.mission_Ctl.currentMissions[missionIndex].coinRew;
			break;
		case 2:
			fillBarMission3.fillAmount = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].GetCompletionPropertion();
			mission3Text.text = GameManager.Instance.mission_Ctl.currentMissions[missionIndex].description;
			mission3Reward.text = "+" + GameManager.Instance.mission_Ctl.currentMissions[missionIndex].coinRew;
			break;
		}
	}

	public void myPlayButtonPressed()
	{
		//ngleton<AnalyticsManager>.Instance.PressPlayButton();
		fadeImage.gameObject.SetActive(value: true);

		AnimatorControllerParameter[] parameters = mainMenuIntroAC.parameters;
        bool hasParameter = false;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].name == "GetOut")
            {
                hasParameter = true;
                break;
            }
        }


		if(hasParameter)
		{
			mainMenuIntroAC.SetTrigger("GetOut");
		}
		
		fadeImage.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			GameManager.Instance.ChangeScene("Gameplay");
		});
	}
}
