using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UI_Ctl : MonoBehaviour
{
	public Transform gameUiTransform;

	public TextMesh score_text;

	public Button stopSpawnButton;

	public Button superSnapButton;

	public Button cleanFloorButton;

	[SerializeField]
	private GameObject topButtonBar;

	[SerializeField]
	private GameObject bottomButtonBar;

	[SerializeField]
	private Image stopSpawnCooldownImage;

	[SerializeField]
	private Text stopSpawnAmountText;

	[SerializeField]
	private Image cleanFloorCooldownImage;

	[SerializeField]
	private Text cleanFloorAmountText;

	[SerializeField]
	private Image superSnapCooldownImage;

	[SerializeField]
	private Text superSnapAmountText;

	[SerializeField]
	private SpriteRenderer hotelFacade;

	[SerializeField]
	private Image gameOverGrey;

	[SerializeField]
	private Text gameOverText;

	[SerializeField]
	private GameObject downButtonElectricityImage;

	[SerializeField]
	private GameObject upButtonElectricityImage;

	private Sequence scientistVisualSequence;

	[SerializeField]
	private Button showDevPanelButton;

	[SerializeField]
	private GameObject devPanel;

	[SerializeField]
	private Button addScoreButton;

	[SerializeField]
	private Button gameOverButton;

	public int booster1Amount;
	public int booster2Amount;

	public static int coins;

    private void Start()
	{
		SetUpBoostersVisuals();
		Text text = stopSpawnAmountText;
		PlayerData playerData = PlayerDataManager.GetPlayerData();
		text.text = playerData.booster1Amount.ToString();
		Text text2 = cleanFloorAmountText;
		playerData = PlayerDataManager.GetPlayerData();
		text2.text = playerData.booster2Amount.ToString();
		scientistVisualSequence = DOTween.Sequence();
		showDevPanelButton.onClick.AddListener(ShowDevPanelButton);
		addScoreButton.onClick.AddListener(AddScoreButton);
		gameOverButton.onClick.AddListener(ForceGameOver);
		if (!PlayerPrefs.HasKey("TutorialDone"))
		{
			stopSpawnAmountText.text = "0";
			superSnapAmountText.text = "0";
			cleanFloorAmountText.text = "0";
			DisableBoosterButton(superSnapAmountText.transform.parent.gameObject, superSnapCooldownImage, superSnapButton);
			DisableBoosterButton(stopSpawnAmountText.transform.parent.gameObject, stopSpawnCooldownImage, stopSpawnButton);
			DisableBoosterButton(cleanFloorAmountText.transform.parent.gameObject, cleanFloorCooldownImage, cleanFloorButton);
			score_text.gameObject.SetActive(value: false);
		}

		if (playerData.booster1Amount < 1)
		{
			DisableBoosterButton(stopSpawnAmountText.transform.parent.gameObject, stopSpawnCooldownImage, stopSpawnButton);
			//StartCoroutine(CooldownButton(60f, stopSpawnCooldownImage, stopSpawnButton));
			//PlayerDataManager.SetBooster1(playerData.booster1Amount+=1);
		}

		if (playerData.booster2Amount < 1)
		{
			DisableBoosterButton(cleanFloorAmountText.transform.parent.gameObject, cleanFloorCooldownImage, cleanFloorButton);
			//StartCoroutine(CooldownButton(60f, cleanFloorCooldownImage, cleanFloorButton));
			//PlayerDataManager.SetBooster2(playerData.booster2Amount+=1);
		}
	
	}

	void Update()
	{
		PlayerData playerData = PlayerDataManager.GetPlayerData();
		stopSpawnAmountText.text = (playerData.booster1Amount).ToString();
		cleanFloorAmountText.text = (playerData.booster2Amount).ToString();

        
    }

	public void SetScore(int score)
	{
		score_text.text = "SCORE: " + score;
		coins = score;// PlayerDataManager.Coins + score;
    }

	public void ShowPausePopup()
	{
		PausePopup.GetInstance(gameUiTransform).Init();
	}

	private void SetUpBoostersVisuals()
	{
	}

	public void StartStopSpawnButtonVisual(float cooldownTime, int amount)
	{
		stopSpawnButton.interactable = false;
		if (amount >= 1)
		{
			stopSpawnAmountText.text = amount.ToString();
			StartCoroutine(CooldownButton(cooldownTime, stopSpawnCooldownImage, stopSpawnButton));
			PlayerData playerData = PlayerDataManager.GetPlayerData();
			PlayerDataManager.SetBooster1(playerData.booster1Amount+=1);
		}
		else
		{
			DisableBoosterButton(stopSpawnAmountText.transform.parent.gameObject, stopSpawnCooldownImage, stopSpawnButton);
		}
	}

	public void StartCleanFloorButtonVisual(float cooldownTime, int amount)
	{
		cleanFloorButton.interactable = false;
		if (amount >= 1)
		{
			cleanFloorAmountText.text = amount.ToString();
			StartCoroutine(CooldownButton(cooldownTime, cleanFloorCooldownImage, cleanFloorButton));
			PlayerData playerData = PlayerDataManager.GetPlayerData();
			PlayerDataManager.SetBooster2(playerData.booster2Amount+=1);
		}
		else
		{
			DisableBoosterButton(cleanFloorAmountText.transform.parent.gameObject, cleanFloorCooldownImage, cleanFloorButton);
		}
	}

	public void StartSuperSnapButtonVisual(float cooldownTime, int amount)
	{
		superSnapButton.interactable = false;
		if (amount >= 1)
		{
			superSnapAmountText.text = amount.ToString();
			StartCoroutine(CooldownButton(cooldownTime, superSnapCooldownImage, superSnapButton));
		}
		else
		{
			DisableBoosterButton(superSnapAmountText.transform.parent.gameObject, superSnapCooldownImage, superSnapButton);
		}
	}

	private IEnumerator CooldownButton(float cooldownTime, Image cooldownImage, Button button)
	{
		float time = Time.time;
		cooldownImage.fillAmount = 1f;
		float elapsepTime = 0f;
		while (cooldownTime - elapsepTime > 0f)
		{
			elapsepTime += Time.deltaTime * Time.timeScale;
			cooldownImage.fillAmount = (cooldownTime - elapsepTime) / cooldownTime;
			yield return null;
		}
		SetButtonInteractable(button);
	}

	private void SetButtonInteractable(Button button)
	{
		button.interactable = true;
	}

	private void DisableBoosterButton(GameObject textContainerGO, Image buttonCooldownImage, Button button)
	{
		buttonCooldownImage.fillAmount = 1f;
		button.interactable = false;
		textContainerGO.SetActive(value: false);
	}

	public void FadeHotelFacade(float time, Ease selectedEasing, float value)
	{
		hotelFacade.DOFade(value, time).SetEase(selectedEasing);
	}

	public void FadeToGreyAndShowEndGameText()
	{
		gameOverGrey.gameObject.SetActive(value: true);
		gameOverGrey.DOFade(0.75f, 2f).SetEase(Ease.Linear);
		gameOverText.DOFade(1f, 1f).SetEase(Ease.Linear);
	}

	public void EnableScientistVisual()
	{
		if (scientistVisualSequence.IsPlaying())
		{
			scientistVisualSequence.Kill();
		}
		upButtonElectricityImage.SetActive(value: true);
		downButtonElectricityImage.SetActive(value: true);
	}

	public void DisableScientistVisual()
	{
		upButtonElectricityImage.SetActive(value: false);
		downButtonElectricityImage.SetActive(value: false);
	}

	public void FlipButtonRotationUpsideDown()
	{
		scientistVisualSequence.Append(upButtonElectricityImage.transform.parent.DORotate(Vector3.zero, 720f).SetEase(Ease.Linear).SetSpeedBased()).Insert(0f, downButtonElectricityImage.transform.parent.DORotate(Vector3.zero, 720f).SetEase(Ease.Linear).SetSpeedBased());
	}

	public void FlipButtonRotationToNormal()
	{
		scientistVisualSequence.Append(upButtonElectricityImage.transform.parent.DORotate(Vector3.forward * 180f, 720f).SetEase(Ease.Linear).SetSpeedBased()).Insert(0f, downButtonElectricityImage.transform.parent.DORotate(Vector3.forward * 180f, 720f).SetEase(Ease.Linear).SetSpeedBased());
	}

	public void ShowDevPanelButton()
	{
		devPanel.SetActive(!devPanel.activeInHierarchy);
	}

	public void AddScoreButton()
	{
		Gameplay_Ctl.Instance.AddCoinsToSession(1000);
	}

	public void HideUI()
	{
		topButtonBar.SetActive(value: false);
		bottomButtonBar.SetActive(value: false);
	}

	public void ForceGameOver()
	{
		Gameplay_Ctl.Instance.GameOver();
	}
}
