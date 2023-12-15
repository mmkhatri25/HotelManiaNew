using DG.Tweening;
//using PlayFab.ClientModels;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
	[SerializeField]
	private string displayName;

	[SerializeField]
	private int position;

	[SerializeField]
	private int value;

	[SerializeField]
	private Image avatar;

	public Color textColor;

	public Text nameText;

	public Text valueText;

	public Image avatarImage;

	public Text positionText;

	public Image bgImage;

	

	public void Init(string _displayName, int _position, int _value, string _avatar)
	{
		textColor = nameText.color;
		if (_displayName != null)
		{
			displayName = _displayName;
		}
		else
		{
			displayName = "Missing NameXXXX";
		}
		position = _position + 1;
		value = _value;
		SetAvatarImage(_avatar);
		nameText.text = displayName.Substring(0, displayName.Length - 4);
		valueText.text = value.ToString();
		positionText.text = position.ToString();
	}

	private void SetAvatarImage(string _avatar)
	{
		if (_avatar != null && _avatar != string.Empty)
		{
			string[] array = _avatar.Split('/');
			//string path = Path.Combine(Path.Combine(Application.persistentDataPath, PlayfabManager.Instance.avatarPath), array[3]);
			if (true)
			{
				//byte[] data = File.ReadAllBytes(path);
				Texture2D texture2D = new Texture2D(128, 128);
				//texture2D.LoadImage(data);
				Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero, 1f);
				avatarImage.sprite = sprite;
			}
			foreach (Transform item in base.transform)
			{
				item.gameObject.SetActive(value: true);
			}
		}
	}

	public void FadeInEntry()
	{
		HideEntry();
		nameText.DOFade(1f, 0.3f);
		valueText.DOFade(1f, 0.3f);
		avatarImage.DOFade(1f, 0.3f);
		positionText.DOFade(1f, 0.3f);
		bgImage.DOFade(1f, 0.3f);
	}

	public void HideEntry()
	{
		Color color = new Color(textColor.r, textColor.g, textColor.b, 0f);
		nameText.color = color;
		valueText.color = color;
		avatarImage.DOFade(0f, 0f);
		positionText.color = color;
		if (bgImage != null)
		{
			bgImage.DOFade(0f, 0f);
		}
	}

	public void HighLightEntry(bool show)
	{
		bgImage.enabled = show;
	}
}
