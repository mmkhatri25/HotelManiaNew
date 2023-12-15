using UnityEngine;
using UnityEngine.UI;

public class TutorialPart2Popup : MonoBehaviour
{
	[SerializeField]
	public Button nextButton;

	[SerializeField]
	public Text tutoTextField;

	[SerializeField]
	public Text buttonTextField;

	[SerializeField]
	private Animator _animator;

	[Header("ButtonImages")]
	[SerializeField]
	private Image buttonBgImage;

	[SerializeField]
	private Image customizeIconImage;

	[SerializeField]
	private Image customizeButtonImage;

	[SerializeField]
	private Image gachaIconImage;

	[SerializeField]
	private Image gachaButtonImage;

	[SerializeField]
	private Image playIconImage;

	[SerializeField]
	private Image playButtonImage;

	private string[] tutoText = new string[9]
	{
		"Not bad! C’mon, I have a gift for you.",
		"This one is on me.",
		"Oh! Lady Luck likes you!",
		"New Room, new look! Let’s spruce up the Hotel.",
		"Tap any Floor to change its Room.",
		"Here, tap the [insert name here] Room to equip it.",
		"Wow! It looks amazing!",
		"I think that’s good for now.",
		"Bravo, you’ve passed your training! And me, well, I’m off to my vacations. Bye!"
	};

	private string[] buttonText = new string[8]
	{
		"( Next )",
		"-",
		"-",
		"-",
		"-",
		"( Next )",
		"-",
		"Have fun!"
	};

	private int tutorialStep = -1;

	private static TutorialPart2Popup prefab;

	public static TutorialPart2Popup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<TutorialPart2Popup>("Popups/Popup.TutorialPart2");
			Resources.UnloadUnusedAssets();
		}
		UnityEngine.Debug.Log("instantiate");
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init(int part = 1)
	{
		nextButton.onClick.AddListener(NextButtonClicked);
		if (part == 2)
		{
			tutorialStep = 2;
			NextButtonClicked();
			Gameplay_Ctl.Instance.uiCanvas.SetActive(value: false);
		}
	}

	public void NextButtonClicked()
	{
		tutorialStep++;
		if (tutorialStep == 2)
		{
			ClosePopup();
		}
		else if (tutorialStep < 7)
		{
			if (tutorialStep > 2)
			{
				tutoTextField.text = tutoText[tutorialStep - 1];
				buttonTextField.text = buttonText[tutorialStep - 1];
			}
			else
			{
				tutoTextField.text = tutoText[tutorialStep];
				buttonTextField.text = buttonText[tutorialStep];
			}
		}
		else if (tutorialStep == 7)
		{
			PlayerPrefs.SetInt("TutorialDone", 1);
			base.gameObject.SetActive(value: false);
			CameraAnim_Ctl.Instance.CameraGameOverAnim();
			Invoke("GoToMainMenu", 1.2f);
		}
	}

	private void GoToMainMenu()
	{
		GameManager.Instance.ChangeScene("MainMenu");
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void ClosePopup()
	{
		if (!PlayerPrefs.HasKey("TutorialDone"))
		{
			Gameplay_Ctl.Instance.StartGame();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
