using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gacha_Ctl : MonoBehaviour
{
	private const float _skipNormalizedTime = 0.55f;

	private const bool Value = false;

	public GameObject gachaGO;

	public GameObject lockGO;

	public Image fadeImage;

	public Animator gacha_AC;

	public Animator lock_AC;

	public Button gachaButton;

	public GameObject noMoneyText;

	public Text priceText;

	public Text coinsText;

	public Button backButton;

	public SpriteRenderer floorSprite;

	public GameObject inforPanel;

	public Text floorName;

	public TextMeshProUGUI floorDesc;

	public SpriteRenderer character1;

	public SpriteRenderer character2;

	public SpriteRenderer character3;

	public ParticleSystem gachaParticles;

	public ParticleSystem lockParticles;

	public List<GameObject> toDisableOnGachaEnd = new List<GameObject>();

	private FloorSO unlockedFloor;

	[Header("Gacha Present Randomization")]
	public SpriteRenderer gachaTopSriteR;

	public SpriteRenderer gachaBottomSriteR;

	public List<Sprite> gachaTopVariants = new List<Sprite>();

	public List<Sprite> gachaBottomVariants = new List<Sprite>();

	public AudioSource gachaFXaudioSource;

	public AudioSource lockFXaudioSource;

	private int numberOfReqTaps = 6;

	private int currentTaps;

	public Button tapButton;

	private bool launchedTapAnim;

	private bool _insideGachaAnim;

	[SerializeField]
	private List<AudioSource> knockAudioSources = new List<AudioSource>();

	private void Start()
	{
		fadeImage.DOColor(new Color(0f, 0f, 0f, 0f), 0.5f).OnComplete(delegate
		{
			fadeImage.gameObject.SetActive(value: false);
		});
		gachaButton.onClick.AddListener(GachaButtonClicked);
		backButton.onClick.AddListener(GoToMainMenu);
		tapButton.onClick.AddListener(TapButtonPressed);
		SetGachaPriceText();
		EvaluateBuyButton();
		UpdateCoinsText();
	}

	private void Update()
	{
		if (_insideGachaAnim && gacha_AC.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.55f && Input.GetMouseButtonDown(0))
		{
			SkipAnim();
		}
	}

	public void SetGachaPriceText()
	{
		if (GameManager.Instance.firstMatchPlayed && !GameManager.Instance.freeGachaUsed)
		{
			priceText.text = "FREE";
		}
		else
		{
			priceText.text = GameManager.Instance.gameVars.gatchaPrice.ToString();
		}
	}

	public void GachaButtonClicked()
	{
		RandomizeGachaPresentArt();
		gachaButton.gameObject.SetActive(value: false);
		backButton.gameObject.SetActive(value: false);
		gacha_AC.SetTrigger("Clicked");
		_insideGachaAnim = true;
		if (GameManager.Instance.firstMatchPlayed && !GameManager.Instance.freeGachaUsed)
		{
			GameManager.Instance.freeGachaUsed = true;
			PlayerPrefs.SetInt("freeGachaUsed", 1);
		}
		else
		{
			PlayerDataManager.AddCoins(-GameManager.Instance.gameVars.gatchaPrice);
		}
		unlockedFloor = PlayerDataManager.UnlockRandomFloor();
		SetRoomInfo(unlockedFloor);
		UpdateCoinsText();
	}

	private void RandomizeGachaPresentArt()
	{
		gachaTopSriteR.sprite = gachaTopVariants[Random.Range(0, gachaTopVariants.Count)];
		gachaBottomSriteR.sprite = gachaBottomVariants[Random.Range(0, gachaBottomVariants.Count)];
	}

	private void SetRoomInfo(FloorSO unlockedFloor)
	{
		floorSprite.sprite = unlockedFloor.floorSprite;
		floorName.text = unlockedFloor.displayName;
		floorDesc.text = unlockedFloor.displayDesc;
		character1.sprite = unlockedFloor.char1;
		character2.sprite = unlockedFloor.char2;
		character3.sprite = unlockedFloor.char3;
	}

	public void GachaAnimationFinished()
	{
		fadeImage.gameObject.SetActive(value: true);
		fadeImage.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			for (int i = 0; i < toDisableOnGachaEnd.Count; i++)
			{
				toDisableOnGachaEnd[i].SetActive(value: false);
			}
			gachaGO.SetActive(value: false);
			lockGO.SetActive(value: true);
			EnableTapButton();
			fadeImage.DOColor(new Color(0f, 0f, 0f, 0f), 0.5f).OnComplete(delegate
			{
				fadeImage.gameObject.SetActive(value: false);
			});
		});
	}

	public void LockAnimationFinished()
	{
		SetGachaPriceText();
		backButton.transform.localScale = Vector3.zero;
		backButton.gameObject.SetActive(value: true);
		backButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
		backButton.onClick.RemoveAllListeners();
		backButton.onClick.AddListener(GoToGacha);
	}

	public void EnableTapButton()
	{
		tapButton.gameObject.SetActive(value: true);
		launchedTapAnim = false;
	}

	public void TapButtonPressed()
	{
		if (!launchedTapAnim)
		{
			launchedTapAnim = true;
			lock_AC.SetTrigger("Start");
		}
		PlayRandomKnockSound();
	}

	private void PlayRandomKnockSound()
	{
		if (!knockAudioSources[0].isPlaying)
		{
			knockAudioSources[0].Play();
		}
		else if (!knockAudioSources[1].isPlaying)
		{
			knockAudioSources[1].Play();
		}
		else if (!knockAudioSources[2].isPlaying)
		{
			knockAudioSources[2].Play();
		}
	}

	private void EvaluateBuyButton()
	{
		if (PlayerDataManager.CanPlayerUnlockRoom() || (GameManager.Instance.firstMatchPlayed && !GameManager.Instance.freeGachaUsed))
		{
			gachaButton.gameObject.SetActive(value: true);
			noMoneyText.gameObject.SetActive(value: false);
		}
		else
		{
			gachaButton.gameObject.SetActive(value: false);
			noMoneyText.gameObject.SetActive(value: true);
		}
	}

	public void SetInfoPanelActive()
	{
		inforPanel.SetActive(value: true);
	}

	private void GoToGacha()
	{
		gachaGO.SetActive(value: false);
		tapButton.gameObject.SetActive(value: false);
		gachaParticles.gameObject.SetActive(value: false);
		lockParticles.gameObject.SetActive(value: false);
		fadeImage.gameObject.SetActive(value: true);
		fadeImage.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			EvaluateBuyButton();
			gachaGO.SetActive(value: true);
			lockGO.SetActive(value: false);
			inforPanel.SetActive(value: false);
			backButton.onClick.RemoveAllListeners();
			backButton.onClick.AddListener(GoToMainMenu);
			fadeImage.DOColor(new Color(0f, 0f, 0f, 0f), 0.5f).OnComplete(delegate
			{
				fadeImage.gameObject.SetActive(value: false);
			});
		});
	}

	private void GoToMainMenu()
	{
		fadeImage.gameObject.SetActive(value: true);
		fadeImage.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			GameManager.Instance.ChangeScene("MainMenu");
		});
	}

	private void UpdateCoinsText()
	{
		coinsText.text = PlayerDataManager.Coins.ToString();
	}

	private void SkipAnim()
	{
		AnimatorStateInfo currentAnimatorStateInfo = gacha_AC.GetCurrentAnimatorStateInfo(0);
		gacha_AC.Play(currentAnimatorStateInfo.fullPathHash, 0, 0.55f);
		_insideGachaAnim = false;
	}
}
