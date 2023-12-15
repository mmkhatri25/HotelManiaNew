using UnityEngine;
using UnityEngine.UI;

public class RateUsPanel_Ctl : MonoBehaviour
{
	public string ANDROID_RATE_URL = "market://details?id=ppl.unity.hotelmania";

	public string FEEDBACK_EMAIL = "market://details?id=ppl.unity.hotelmania";

	private string supportEmail = "support@pocketplaylab.com";

	private string subject = "Hotel Mania Feedback";

	public GameObject rateUsIntro;

	public GameObject rateUsPositive;

	public GameObject rateUsNegative;

	public Button introPositiveAnswer;

	public Button introNegativeAnswer;

	public Button rateUsPositivePositiveAnswer;

	public Button rateUsPositiveNegativeAnswer;

	public Button rateUsNegativePositiveAnswer;

	public Button rateUsNegativeNegativeAnswer;

	private void Awake()
	{
		introPositiveAnswer.onClick.AddListener(GoToPositivePanel);
		introNegativeAnswer.onClick.AddListener(GoToNegativePanel);
		rateUsPositivePositiveAnswer.onClick.AddListener(ShowFinalRatingPopup);
		rateUsPositiveNegativeAnswer.onClick.AddListener(CloseRateUs);
		rateUsNegativePositiveAnswer.onClick.AddListener(ShowSupportPopup);
		rateUsNegativeNegativeAnswer.onClick.AddListener(CloseRateUs);
	}

	private void GoToPositivePanel()
	{
		rateUsIntro.SetActive(value: false);
		rateUsPositive.SetActive(value: true);
	}

	private void GoToNegativePanel()
	{
		rateUsIntro.SetActive(value: false);
		rateUsNegative.SetActive(value: true);
	}

	private void CloseRateUs()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void ShowFinalRatingPopup()
	{
		Application.OpenURL(ANDROID_RATE_URL);
		CloseRateUs();
	}

	private void ShowSupportPopup()
	{
		//string str = WWW.EscapeURL(subject);
		//Application.OpenURL("mailto:" + supportEmail + "?subject=" + str);
		CloseRateUs();
	}
}
