using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCharacterAnim_Ctl : MonoBehaviour
{
	public static SpecialCharacterAnim_Ctl Instance;

	private Queue<GameObject> animsToShow = new Queue<GameObject>();

	private float animTime = 1.35f;

	private WaitForSeconds waitTime;

	private Coroutine handleAnimsIE;

	private void Start()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
		waitTime = new WaitForSeconds(animTime * 0.9f);
	}

	public void AddToShow(GameObject animPrefab)
	{
		GameObject item = UnityEngine.Object.Instantiate(animPrefab, Vector3.zero, Quaternion.identity, Gameplay_Ctl.Instance.uiCanvas.transform);
		animsToShow.Enqueue(item);
		if (handleAnimsIE == null)
		{
			handleAnimsIE = StartCoroutine(HandleAnimQueue());
		}
	}

	private IEnumerator HandleAnimQueue()
	{
		while (animsToShow.Count > 0 && !Gameplay_Ctl.Instance.IsGameOver())
		{
			animsToShow.Dequeue().SetActive(value: true);
			yield return waitTime;
		}
		handleAnimsIE = null;
	}
}
