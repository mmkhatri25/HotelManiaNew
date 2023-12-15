using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class PostMatchMissionsPopup : MonoBehaviour
{
	public Color circleColor;

	public Text mission1Text;

	public Text mission2Text;

	public Text mission3Text;

	public Animator mission1AC;

	public Animator mission2AC;

	public Animator mission3AC;

	public Text mission1AmmountText;

	public Text mission2AmmountText;

	public Text mission3AmmountText;

	public Image mission1Fill;

	public Image mission2Fill;

	public Image mission3Fill;

	public Image completedMissionsFillBar1;

	public Image completedMissionsFillBar2;

	public Image completedMissionsFillBar3;

	public Image completedMissionsCircle1;

	public Image completedMissionsCircle2;

	public Image completedMissionsCircle3;

	public Text hardCurrencyGivenNumber;

	private int doneMissionsOnLoad;

	[SerializeField]
	private AudioSource fillingBarAudioS;

	private int coins;

	[SerializeField]
	private Animator _animator;

	private static PostMatchMissionsPopup prefab;

	public static PostMatchMissionsPopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<PostMatchMissionsPopup>("Popups/Popup.PostMatchMissions");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init()
	{
		SetMissionsText();
		hardCurrencyGivenNumber.text = "+" + GameManager.Instance.gameVars.hardCurrencyRewardForAllMissionsCompleted;
		doneMissionsOnLoad = GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted();
		SetCompleatedMissionsStartStatus();
		StartCoroutine(AnimateMissionProgress());
	}

	public void SetMissionsText()
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

	private IEnumerator AnimateMissionProgress()
	{
		yield return new WaitForSeconds(1f);
		List<Mission> list = new List<Mission>();
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, GameManager.Instance.mission_Ctl.currentMissions);
			memoryStream.Position = 0L;
			list = (List<Mission>)binaryFormatter.Deserialize(memoryStream);
		}
		GameManager.Instance.mission_Ctl.EvaluateMissionsOnGameOver(Gameplay_Ctl.Instance.currentGameplaySession);
		Sequence sequence = DOTween.Sequence();
		if (list[0].progress < GameManager.Instance.mission_Ctl.currentMissions[0].progress || list[1].progress < GameManager.Instance.mission_Ctl.currentMissions[1].progress || list[2].progress < GameManager.Instance.mission_Ctl.currentMissions[2].progress)
		{
			UnityEngine.Debug.Log("Play fillingBarAudioS");
			fillingBarAudioS.Play();
		}
		sequence.Insert(0f, mission1Fill.DOFillAmount(GameManager.Instance.mission_Ctl.currentMissions[0].GetCompletionPropertion(), 0.3f).SetSpeedBased());
		sequence.Insert(0f, mission2Fill.DOFillAmount(GameManager.Instance.mission_Ctl.currentMissions[1].GetCompletionPropertion(), 0.3f).SetSpeedBased());
		sequence.Insert(0f, mission3Fill.DOFillAmount(GameManager.Instance.mission_Ctl.currentMissions[2].GetCompletionPropertion(), 0.3f).SetSpeedBased());
		sequence.OnComplete(delegate
		{
			//fillingBarAudioS.Stop();
			StartCoroutine(CheckMissionOutput());
		});
	}

	private void SetCompleatedMissionsStartStatus()
	{
		switch (doneMissionsOnLoad)
		{
		case 1:
			completedMissionsFillBar1.fillAmount = 1f;
			completedMissionsCircle1.color = circleColor;
			break;
		case 2:
			completedMissionsFillBar1.fillAmount = 1f;
			completedMissionsFillBar2.fillAmount = 1f;
			completedMissionsCircle1.color = circleColor;
			completedMissionsCircle2.color = circleColor;
			break;
		case 3:
			completedMissionsFillBar1.fillAmount = 1f;
			completedMissionsFillBar2.fillAmount = 1f;
			completedMissionsFillBar3.fillAmount = 1f;
			completedMissionsCircle1.color = circleColor;
			completedMissionsCircle2.color = circleColor;
			completedMissionsCircle3.color = circleColor;
			break;
		}
	}

	private IEnumerator CheckMissionOutput()
	{
		yield return new WaitForSeconds(0.5f);
		if (!GameManager.Instance.mission_Ctl.currentMissions[0].rewardsGiven && GameManager.Instance.mission_Ctl.currentMissions[0].IsCompleted())
		{
			Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions += GameManager.Instance.mission_Ctl.GetCoinsFromCurrentMissionsAndMarkAsDone(0);
			Gameplay_Ctl.Instance.currentGameplaySession.totalMissionsCompleted++;
		}
		else
		{
			GameManager.Instance.mission_Ctl.currentMissions[0].ResetIfSingleAndNotCompleted();
		}
		Tween t = mission1Fill.DOFillAmount(GameManager.Instance.mission_Ctl.currentMissions[0].GetCompletionPropertion(), 0.3f).SetSpeedBased();
		if (!GameManager.Instance.mission_Ctl.currentMissions[1].rewardsGiven && GameManager.Instance.mission_Ctl.currentMissions[1].IsCompleted())
		{
			Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions += GameManager.Instance.mission_Ctl.GetCoinsFromCurrentMissionsAndMarkAsDone(1);
			Gameplay_Ctl.Instance.currentGameplaySession.totalMissionsCompleted++;
		}
		else
		{
			GameManager.Instance.mission_Ctl.currentMissions[1].ResetIfSingleAndNotCompleted();
		}
		Tween fill2Tween = mission2Fill.DOFillAmount(GameManager.Instance.mission_Ctl.currentMissions[1].GetCompletionPropertion(), 0.3f).SetSpeedBased();
		if (!GameManager.Instance.mission_Ctl.currentMissions[2].rewardsGiven && GameManager.Instance.mission_Ctl.currentMissions[2].IsCompleted())
		{
			Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions += GameManager.Instance.mission_Ctl.GetCoinsFromCurrentMissionsAndMarkAsDone(2);
			Gameplay_Ctl.Instance.currentGameplaySession.totalMissionsCompleted++;
		}
		else
		{
			GameManager.Instance.mission_Ctl.currentMissions[2].ResetIfSingleAndNotCompleted();
		}
		Tween fill3Tween = mission3Fill.DOFillAmount(GameManager.Instance.mission_Ctl.currentMissions[2].GetCompletionPropertion(), 0.3f).SetSpeedBased();
		yield return t.WaitForCompletion();
		yield return fill2Tween.WaitForCompletion();
		yield return fill3Tween.WaitForCompletion();
		yield return new WaitForSeconds(1.3f);
		Debug.Log("Mission completed here");
		UnityEngine.Debug.Log("GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted(): " + GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted());
		UnityEngine.Debug.Log("doneMissionsOnLoad: " + doneMissionsOnLoad);
		float num = 0f;
        Debug.Log("I have hardCurrency - " + PlayerDataManager.HardCurrency);
        if (GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted() != doneMissionsOnLoad)
		{
            Debug.Log("Mission complete I have hardCurrency - " + PlayerDataManager.HardCurrency);
            switch (GameManager.Instance.mission_Ctl.GetNumberOfMissionsCompleted())
			{
			case 1:
                    Debug.Log("1 . I have hardCurrency - " + PlayerDataManager.HardCurrency);

                    num = 0.7f;
				completedMissionsFillBar1.DOFillAmount(1f, 0.7f).OnComplete(delegate
				{
					completedMissionsCircle1.color = circleColor;
				});
				break;
			case 2:
                    Debug.Log("2 . I have hardCurrency - " + PlayerDataManager.HardCurrency);

                    num = 1.4f;
				completedMissionsFillBar1.DOFillAmount(1f, 0.7f).OnComplete(delegate
				{
					completedMissionsCircle1.color = circleColor;
					completedMissionsFillBar2.DOFillAmount(1f, 0.7f).OnComplete(delegate
					{
						completedMissionsCircle2.color = circleColor;
					});
				});
				break;
			case 3:
                    Debug.Log("2 . I have hardCurrency - " + PlayerDataManager.HardCurrency);

                    num = 2.1f;
				completedMissionsFillBar1.DOFillAmount(1f, 0.7f).OnComplete(delegate
				{
					completedMissionsCircle1.color = circleColor;
					completedMissionsFillBar2.DOFillAmount(1f, 0.7f).OnComplete(delegate
					{
						completedMissionsCircle2.color = circleColor;
						completedMissionsFillBar3.DOFillAmount(1f, 0.7f).OnComplete(delegate
						{
							completedMissionsCircle3.color = circleColor;
							PlayerDataManager.SetHardCurrencyGivenForMissionPack(value: true);
							PlayerDataManager.AddHardCurrency(GameManager.Instance.gameVars.hardCurrencyRewardForAllMissionsCompleted);
						});
					});
				});
				break;
			}
		}
		yield return new WaitForSeconds(1.3f + num);
		Debug.Log("My log- totalCoinsFromMissions " + Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions);
		Gameplay_Ctl.Instance.currentGameplaySession.totalScore = Gameplay_Ctl.Instance.currentGameplaySession.totalCoins + Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions;
		ResultScreenPopup.GetInstance(Gameplay_Ctl.Instance.uI_Ctl.gameUiTransform).Init(Gameplay_Ctl.Instance.currentGameplaySession.totalCoins, Gameplay_Ctl.Instance.currentGameplaySession.totalCoinsFromMissions);
		ClosePopup();
	}

	public void ClosePopup()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
