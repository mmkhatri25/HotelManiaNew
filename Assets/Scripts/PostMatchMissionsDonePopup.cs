using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PostMatchMissionsDonePopup : MonoBehaviour
{
	public Text timer;

	[SerializeField]
	private Animator _animator;

	private DateTime nextMissionsTime;

	private int doneMissionsOnLoad;

	private readonly string doublePointString = ":";

	private int coins;

	[SerializeField]
	private AudioSource menuOutAudioSource;

	private static PostMatchMissionsDonePopup prefab;

	public static PostMatchMissionsDonePopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<PostMatchMissionsDonePopup>("Popups/Popup.PostMatchMissionsDone");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init()
	{
		nextMissionsTime = PlayerDataManager.GetLastMissionTime().AddHours(GameManager.Instance.gameVars.hoursForNewMission);
		StartCoroutine(WaitAndClose());
	}

	private void Update()
	{
		if (DateTime.Now < nextMissionsTime)
		{
			string text = $"{(nextMissionsTime - DateTime.Now).Hours:D2}" + doublePointString + $"{(nextMissionsTime - DateTime.Now).Minutes:D2}" + doublePointString + $"{(nextMissionsTime - DateTime.Now).Seconds:D2}";
			timer.text = text;
		}
		else if (timer.text.CompareTo("00:00:00") != 0)
		{
			timer.text = "00:00:00";
		}
	}

	private IEnumerator WaitAndClose()
	{
		yield return new WaitForSeconds(2f);
		_animator.SetTrigger("Out");
		menuOutAudioSource.Play();
		ResultScreenPopup.GetInstance(Gameplay_Ctl.Instance.uI_Ctl.gameUiTransform).Init(Gameplay_Ctl.Instance.currentGameplaySession.totalCoins, Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions);
	}

	public void ClosePopup()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
