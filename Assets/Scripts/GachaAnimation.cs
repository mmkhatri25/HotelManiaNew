using UnityEngine;

public class GachaAnimation : MonoBehaviour
{
	public GameObject particleSystemGO;

	public Gacha_Ctl gacha_Ctl;

	public void EnableParticles()
	{
		particleSystemGO.SetActive(value: true);
	}

	public void FinishedGachaAnim()
	{
		gacha_Ctl.GachaAnimationFinished();
	}

	public void FinishedLockAnim()
	{
		gacha_Ctl.LockAnimationFinished();
	}

	public void SetInfoPanelActive()
	{
		gacha_Ctl.SetInfoPanelActive();
	}

	public void EnableTapButton()
	{
		gacha_Ctl.EnableTapButton();
	}

	public void PlayGachaAudio()
	{
		gacha_Ctl.gachaFXaudioSource.Play();
	}

	public void PlayUnlockAudios()
	{
		DisableTapButton();
		gacha_Ctl.lockFXaudioSource.Play();
	}

	public void DisableTapButton()
	{
		gacha_Ctl.tapButton.gameObject.SetActive(value: false);
	}
}
