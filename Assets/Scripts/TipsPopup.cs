using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TipsPopup : MonoBehaviour
{
	private static TipsPopup instance;

	public Button button;

	public Image darkFader;

	public CanvasGroup canvasGroup;

	public static TipsPopup GetInstance(Transform parent)
	{
		TipsPopup[] array = Resources.LoadAll<TipsPopup>("Popups/LoadingScreens");
		TipsPopup original = array[Random.Range(0, array.Length)];
		Resources.UnloadUnusedAssets();
		return UnityEngine.Object.Instantiate(original, parent, worldPositionStays: false);
	}

	private void Start()
	{
		darkFader.DOColor(new Color(0f, 0f, 0f, 0f), 0.5f);
		if (button != null)
		{
			button.onClick.AddListener(ClosePopup);
		}
	}

	public void ClosePopup()
	{
		canvasGroup.DOFade(0f, 0.5f).OnComplete(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			ClosePopup();
		}
	}
}
