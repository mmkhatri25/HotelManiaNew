using UnityEngine;

public class FunctionsToCallFromIntroAC : MonoBehaviour
{
	public AudioSource introSfxAudioSource;

	public void GetMainMenuUI()
	{
		MainMenu_UI_Ctl.Instance.GetInUI();
	}

	public void CallLoop()
	{
		MainMenu_UI_Ctl.Instance.DoLoopAnim();
	}

	public void GoToGameplay()
	{
		MainMenu_UI_Ctl.Instance.PlayButtonPressed();
	}

	public void PlayIntroSfx()
	{
		introSfxAudioSource.Play();
	}
}
