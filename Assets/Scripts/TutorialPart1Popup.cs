using UnityEngine;
using UnityEngine.UI;

public class TutorialPart1Popup : MonoBehaviour
{
	[SerializeField]
	public Button nextButton;

	[SerializeField]
	public Text tutoTextField;

	[SerializeField]
	public Text buttonTextField;

	[SerializeField]
	private GameObject characters;

	[SerializeField]
	private GameObject boosters;

	private string[] tutoText = new string[6]
	{
		"Welcome! Ready for your first day?",
		"Great! Use the Up & Down buttons to take the Tenants to their floors.",
		"See? Simple!",
		"Here - have these Power-ups. They may come in handy.",
		"Lastly, lookout for the VIP. They are always problematic...",
		"So, ready to start?"
	};

	private string[] buttonText = new string[6]
	{
		"Yes",
		"Ok",
		"Next",
		"Thanks",
		"Sure",
		"Ready!"
	};

	private int tutorialStep;

	private static TutorialPart1Popup prefab;

	public static TutorialPart1Popup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<TutorialPart1Popup>("Popups/Popup.TutorialPart1");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init(int part = 1)
	{
		nextButton.onClick.AddListener(NextButtonClicked);
		if (part == 2)
		{
			Gameplay_Ctl.Instance.playing = false;
			Gameplay_Ctl.Instance.elevator_Ctl.SetAllowMovement(value: false);
			Gameplay_Ctl.Instance.elevator_Ctl.currentDirection = Direction.NoDirection;
			tutorialStep = 2;
			NextButtonClicked();
			Gameplay_Ctl.Instance.uiCanvas.SetActive(value: false);
		}
		else
		{
			tutoTextField.text = tutoText[0];
			buttonTextField.text = buttonText[0];
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
				if (tutorialStep == 4)
				{
					boosters.SetActive(value: true);
				}
				else
				{
					boosters.SetActive(value: false);
				}
				if (tutorialStep == 5)
				{
					characters.SetActive(value: true);
				}
				else
				{
					characters.SetActive(value: false);
				}
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
