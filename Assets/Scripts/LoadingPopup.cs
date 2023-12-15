using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPopup : MonoBehaviour
{
	public static LoadingPopup Instance;

	private static LoadingPopup prefab;

	public Image loadingImage;

	private static LoadingPopup instance;

	public static LoadingPopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<LoadingPopup>("Popups/LoadingPopup");
			Resources.UnloadUnusedAssets();
		}
		if (Instance == null)
		{
			Instance = UnityEngine.Object.Instantiate(prefab, parent, worldPositionStays: false);
		}
		Instance.loadingImage.transform.DOLocalRotate(new Vector3(0f, 0f, -360f), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
		return Instance;
	}

	public void ClosePopup()
	{
		Instance = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			ClosePopup();
		}
	}
}
