using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
	public Button soundButton;

	public Button musicButton;

	public Button qualityButton;

	public Button supportButton;

	public Button quiteGameButton;

	public Button backButton;

	public Image soundImage;

	public Image musicImage;

	public GameObject soundMuted;

	public GameObject musicMuted;

	public GameObject soundUnmuted;

	public GameObject musicUnmuted;

	private static PausePopup prefab;

	[SerializeField]
	private AudioSource menuOutAudioSource;

	[SerializeField]
	private Animator _animator;

	private bool closingPopup;

	public event Action OnPopupClosed;

	public static PausePopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<PausePopup>("Popups/Popup.Pause");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	void Start()
	{
		if(PlayerPrefs.GetInt("myMusic")==0)
		{
			

			musicMuted.SetActive(false);
			musicUnmuted.SetActive(true);
		}
		else
		{
			musicMuted.SetActive(true);
			musicUnmuted.SetActive(false);
		}

		if(PlayerPrefs.GetInt("soundStatus")==0)
		{
			soundMuted.SetActive(false);
			soundUnmuted.SetActive(true);
		}
		else
		{
			soundMuted.SetActive(true);
			soundUnmuted.SetActive(false);
		}
	}

	public void Init()
	{
		//soundUnmuted = soundImage.sprite;
		//musicUnmuted = musicImage.sprite;
		soundButton.onClick.AddListener(ChangeSoundSetting);
		musicButton.onClick.AddListener(ChangeMusicSetting);
		qualityButton.onClick.AddListener(ChangeQualitySetting);
		supportButton.onClick.AddListener(ClickedSupport);
		quiteGameButton.onClick.AddListener(ClickedQuitGame);
		backButton.onClick.AddListener(ClosePopupButton);
		SetSoundButtonGraphic();
		SetMusicButtonGraphic();
		Gameplay_Ctl.Instance.elevator_Ctl.SetAllowMovement(value: false);
		Gameplay_Ctl.Instance.SetAllowSpawn(value: false);
		Gameplay_Ctl.Instance.pauseTime -= Time.time;
	}

	public void StopTime()
	{
		if (!closingPopup)
		{
			Time.timeScale = 0f;
		}
	}

	private void SetSoundButtonGraphic()
	{
		if (AudioManager.SndfxMuted)
		{
			//soundImage.sprite = soundMuted;
		}
		else
		{
			//soundImage.sprite = soundUnmuted;
		}
	}

	private void SetMusicButtonGraphic()
	{
		if (AudioManager.MusicMuted)
		{
			//musicImage.sprite = musicMuted;
		}
		else
		{
			//musicImage.sprite = musicUnmuted;
		}
	}

	private void ChangeSoundSetting()
	{
		//AudioManager.ToggleSndfxVolume();
		//SetSoundButtonGraphic();
			if(PlayerPrefs.GetInt("soundStatus")==0)
		{
			PlayerPrefs.SetInt("soundStatus",1);
			soundMuted.SetActive(true);
			soundUnmuted.SetActive(false);
		}
		else
		{
			PlayerPrefs.SetInt("soundStatus",0);
			soundMuted.SetActive(false);
			soundUnmuted.SetActive(true);
		}
	}

	private void ChangeMusicSetting()
	{
		//AudioManager.ToggleMusicVolume();
		//SetMusicButtonGraphic();
		if(PlayerPrefs.GetInt("myMusic")==0)
		{
			PlayerPrefs.SetInt("myMusic",1);
			musicMuted.SetActive(true);
			musicUnmuted.SetActive(false);
		}
		else
		{
			PlayerPrefs.SetInt("myMusic",0);
			musicMuted.SetActive(false);
			musicUnmuted.SetActive(true);
		}
	}

	private void ChangeQualitySetting()
	{
	}

	private void ClickedSupport()
	{
		ErrorManager.Instance.SendSupportEmail();
	}

	private void ClickedQuitGame()
	{
		Gameplay_Ctl.Instance.currentGameplaySession.quitGame = true;
		closingPopup = true;
		Time.timeScale = 1f;
		Gameplay_Ctl.Instance.currentGameplaySession.forcedGameOver = true;
		Gameplay_Ctl.Instance.GameOver();
		ClosePopupButton();
	}

	public void ClosePopupButton()
	{
		closingPopup = true;
		Gameplay_Ctl.Instance.pauseTime += Time.time;
		Time.timeScale = 1f;
		//menuOutAudioSource.Play();
		_animator.SetTrigger("Out");
	}

	public void ClosePopup()
	{
		if (this.OnPopupClosed != null)
		{
			this.OnPopupClosed();
		}
		Gameplay_Ctl.Instance.elevator_Ctl.SetAllowMovement(value: true);
		Gameplay_Ctl.Instance.SetAllowSpawn(value: true);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
