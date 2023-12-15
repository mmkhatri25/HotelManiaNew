using System.Collections.Generic;
using UnityEngine;

public class SpecialCharacterArtOrder : MonoBehaviour
{
	[SerializeField]
	private Character_Ctl character_Ctl;

	private List<int> startingOrder = new List<int>();

	private List<SpriteRenderer> artSprites = new List<SpriteRenderer>();

	private void Start()
	{
		foreach (Transform item in character_Ctl.art.transform)
		{
			SpriteRenderer component = item.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				artSprites.Add(component);
				startingOrder.Add(component.sortingOrder);
			}
			foreach (Transform item2 in item)
			{
				SpriteRenderer component2 = item2.GetComponent<SpriteRenderer>();
				if (component2 != null)
				{
					artSprites.Add(component2);
					startingOrder.Add(component2.sortingOrder);
				}
			}
		}
	}

	public void MoveArtFront()
	{
		for (int i = 0; i < artSprites.Count; i++)
		{
			artSprites[i].sortingOrder = startingOrder[i] + 5;
		}
	}

	public void MoveArtBack()
	{
		for (int i = 0; i < artSprites.Count; i++)
		{
			artSprites[i].sortingOrder = startingOrder[i];
		}
	}
}
