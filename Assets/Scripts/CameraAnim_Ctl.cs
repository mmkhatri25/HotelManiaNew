using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraAnim_Ctl : MonoBehaviour
{
	public static CameraAnim_Ctl Instance;

	public Camera camera;

	[Header("IntroZoom")]
	public GameObject boundsToShowRef;

	private Bounds targetBounds;

	public float animLength = 1.5f;

	[Header("Shake")]
	public Transform camTransform;

	private bool shake;

	private Vector3 originalCamPos;

	private Vector3 startingPos;

	private float startingOrthographicSize;

	public SpriteRenderer fade;

	public float fadeAmount = 0.66f;

	public Tween zoomInTween;

	public Tween moveTween;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void Start()
	{
		startingPos = camera.transform.position;
		startingOrthographicSize = camera.orthographicSize;
		targetBounds = boundsToShowRef.GetComponent<SpriteRenderer>().bounds;
		CameraStartAnim();
	}

	public void CameraStartAnim()
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = targetBounds.size.x / targetBounds.size.y;
		moveTween = camera.transform.DOMove(new Vector3(targetBounds.center.x, targetBounds.center.y, -1f), animLength).SetEase(Ease.OutQuart);
		if (num >= num2)
		{
			zoomInTween = DOTween.To(() => camera.orthographicSize, delegate(float x)
			{
				camera.orthographicSize = x;
			}, targetBounds.size.y / 2f, animLength).SetEase(Ease.OutQuart);
		}
		else
		{
			float num3 = num2 / num;
			zoomInTween = DOTween.To(() => camera.orthographicSize, delegate(float x)
			{
				camera.orthographicSize = x;
			}, targetBounds.size.y / 2f * num3, animLength).SetEase(Ease.OutQuart);
		}
		if (Gameplay_Ctl.Instance != null)
		{
			Gameplay_Ctl.Instance.uI_Ctl.FadeHotelFacade(animLength, Ease.Linear, 0f);
		}
		if (Customize_Ctl.Instance != null)
		{
			Customize_Ctl.Instance.uI_Ctl.FadeHotelFacade(animLength, Ease.Linear, 0f);
			fade.DOFade(0.9f, animLength).SetEase(Ease.Linear);
		}
	}

	public void CameraGameOverAnim()
	{
		if (zoomInTween != null && zoomInTween.IsActive())
		{
			zoomInTween.Kill();
		}
		if (moveTween != null && moveTween.IsActive())
		{
			moveTween.Kill();
		}
		camera.transform.DOMove(new Vector3(startingPos.x, startingPos.y, -1f), animLength).SetEase(Ease.OutQuart);
		DOTween.To(() => camera.orthographicSize, delegate(float x)
		{
			camera.orthographicSize = x;
		}, startingOrthographicSize, animLength).SetEase(Ease.OutQuart);
		Gameplay_Ctl.Instance.uI_Ctl.FadeHotelFacade(animLength, Ease.Linear, 1f);
	}

	public void CloseCustomizationAnim()
	{
		camera.transform.DOMove(new Vector3(startingPos.x, startingPos.y, -1f), animLength).SetEase(Ease.OutQuart);
		DOTween.To(() => camera.orthographicSize, delegate(float x)
		{
			camera.orthographicSize = x;
		}, startingOrthographicSize, animLength).SetEase(Ease.OutQuart);
		Customize_Ctl.Instance.uI_Ctl.FadeHotelFacade(animLength * fadeAmount, Ease.Linear, 1f);
		fade.DOFade(0f, animLength).SetEase(Ease.Linear);
	}

	public IEnumerator DoShake(float shakeDuration, float shakeAmount, float decreaseFactor = 1f)
	{
		originalCamPos = camera.transform.localPosition;
		shake = true;
		float _shakeDuration = shakeDuration;
		float _shakeAmount = shakeAmount;
		while (shake)
		{
			if (_shakeDuration > 0f)
			{
				camTransform.localPosition = originalCamPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
				_shakeDuration -= Time.deltaTime;
				shakeAmount = _shakeAmount * (_shakeDuration / shakeDuration);
			}
			else
			{
				_shakeDuration = 0f;
				camTransform.localPosition = originalCamPos;
				shake = false;
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
