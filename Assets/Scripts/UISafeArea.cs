using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISafeArea : MonoBehaviour
{
	[SerializeField]
	private bool _ignoreTop;

	[SerializeField]
	private bool _ignoreBottom;

	[SerializeField]
	private bool _ignoreLeft;

	[SerializeField]
	private bool _ignoreRight;

	private RectTransform _rectTransform;

	private RectTransform _parentRectTransform;

	private Vector2 _lastMin;

	private Vector2 _lastMax;

	protected void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_parentRectTransform = _rectTransform.parent.GetComponent<RectTransform>();
		UpdateSafeArea();
	}

	protected void OnEnable()
	{
		UpdateSafeArea();
	}

	protected void OnRectTransformDimensionsChange()
	{
		bool activeInHierarchy = base.gameObject.activeInHierarchy;
	}

	public void UpdateSafeArea(bool force = true)
	{
		if (_rectTransform == null || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		Canvas componentInParent = GetComponentInParent<Canvas>();
		if (!(componentInParent != null) || !(_rectTransform.parent != null))
		{
			return;
		}
		Rect safeArea = Screen.safeArea;
		Rect rect = _parentRectTransform.rect;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, safeArea.min, componentInParent.worldCamera, out Vector2 localPoint);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, safeArea.max, componentInParent.worldCamera, out Vector2 localPoint2);
		Vector2 vector = new Vector2(localPoint.x - rect.xMin, localPoint.y - rect.yMin);
		Vector2 vector2 = new Vector2(localPoint2.x - rect.xMax, localPoint2.y - rect.yMax);
		if ((vector != _lastMin || vector2 != _lastMax) | force)
		{
			_lastMin = vector;
			_lastMax = vector2;
			if (_ignoreLeft)
			{
				vector.x = _rectTransform.offsetMin.x;
			}
			if (_ignoreRight)
			{
				vector2.x = _rectTransform.offsetMax.x;
			}
			if (_ignoreBottom)
			{
				vector.y = _rectTransform.offsetMin.y;
			}
			if (_ignoreTop)
			{
				vector2.y = _rectTransform.offsetMax.y;
			}
			_rectTransform.offsetMin = vector;
			_rectTransform.offsetMax = vector2;
		}
	}

	private IEnumerator RoutineDelayUpdate()
	{
		yield return new WaitForEndOfFrame();
		UpdateSafeArea(force: false);
	}
}
