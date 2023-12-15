using Alg;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
	public Button restartButton;

	public Button soundButton;

	public Button musicButton;

	public Button unlinkFacebookButton;

	public Button creditsButton;

	public Button supportButton;

	public Button closeButton;

	public GameObject soundMuted;

	public GameObject musicMuted;

	public GameObject soundUnmuted;

	public GameObject musicUnmuted;

	public Text versionText;

	[SerializeField]
	private static SettingsPopup prefab;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private AudioSource menuOutAudioSource;

	public event Action OnPopupClosed;

	public static SettingsPopup GetInstance()
	{
		if (prefab == null)
		{
			prefab = Resources.Load<SettingsPopup>("Popups/Popup.Settings");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab);
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
		if (Singleton<DBConnection_Ctl>.Instance.linkedFB)
		{
			unlinkFacebookButton.onClick.AddListener(delegate
			{
				UnlinkFacebookButtonAction();
			});
		}
		else
		{
			HideUnlinkFacebookButton();
		}
		versionText.text = "v" + Application.version;
		soundButton.onClick.AddListener(ChangeSoundSetting);
		musicButton.onClick.AddListener(ChangeMusicSetting);
		supportButton.onClick.AddListener(SupportClicked);
		creditsButton.onClick.AddListener(CreditsClicked);
		SetSoundButtonGraphic();
		SetMusicButtonGraphic();
		closeButton.onClick.AddListener(ClosePopupButton);
		//PlayfabManager.Instance.FacebookAccountLinkedUnsuccessful += ErrorUnlinkFacebookButton;
		//PlayfabManager.Instance.FacebookAccountUnlinkedSuccessful += HideUnlinkFacebookButton;
	}

	private void SetSoundButtonGraphic()
	{
		soundUnmuted.SetActive(!AudioManager.SndfxMuted);
		soundMuted.SetActive(AudioManager.SndfxMuted);
	}

	private void SetMusicButtonGraphic()
	{
		musicUnmuted.SetActive(!AudioManager.MusicMuted);
		musicMuted.SetActive(AudioManager.MusicMuted);
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

	public void SupportClicked()
	{
		ErrorManager.Instance.SendSupportEmail();
	}

	public void CreditsClicked()
	{
		CreditsPopup instance = CreditsPopup.GetInstance();
		instance.transform.SetParent(MainMenu_UI_Ctl.Instance.canvasTransform, worldPositionStays: false);
		instance.Init();
	}

	public void UnlinkFacebookButtonAction()
	{
		UnityEngine.Debug.Log("UnlinkFacebookButtonAction");
		Singleton<DBConnection_Ctl>.Instance._loadingPopUpRef = LoadingPopup.GetInstance(MainMenu_UI_Ctl.Instance.canvasTransform.transform).gameObject;

	}

	private void HideUnlinkFacebookButton()
	{
		unlinkFacebookButton.gameObject.SetActive(value: false);
	}

	private void ErrorUnlinkFacebookButton()
	{
		Singleton<DBConnection_Ctl>.Instance.CloseLoadingPopup();
	}

	private void ClosePopupButton()
	{
		menuOutAudioSource.Play();
		_animator.SetTrigger("Out");
	}

	public void ClosePopup()
	{
		//PlayfabManager.Instance.FacebookAccountLinkedUnsuccessful -= ErrorUnlinkFacebookButton;
		//PlayfabManager.Instance.FacebookAccountUnlinkedSuccessful -= HideUnlinkFacebookButton;
		if (this.OnPopupClosed != null)
		{
			this.OnPopupClosed();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
