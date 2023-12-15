using System;
using UnityEngine;
using UnityEngine.UI;

public class CreditsPopup : MonoBehaviour
{
	public Button closeButton;

	public Button okButton;

	private float timeForSecretRoomUnlock = 5f;

	private bool secretAlreadyUnlocked;

	[SerializeField]
	private static CreditsPopup prefab;

	public event Action OnPopupClosed;

	public static CreditsPopup GetInstance()
	{
		if (prefab == null)
		{
			prefab = Resources.Load<CreditsPopup>("Popups/Popup.Credits");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab);
	}

	public void Init()
	{
		if (PlayerDataManager.GetUnlockedFloors().Contains("Playlab"))
		{
			secretAlreadyUnlocked = true;
		}
		closeButton.onClick.AddListener(ClosePopup);
		okButton.onClick.AddListener(ClosePopup);
	}

	public void ClosePopup()
	{
		if (this.OnPopupClosed != null)
		{
			this.OnPopupClosed();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
