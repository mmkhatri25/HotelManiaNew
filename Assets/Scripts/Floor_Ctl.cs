using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_Ctl : MonoBehaviour
{
	public FloorSO floor;

	public int floorNumber;

	public GameObject posMarker;

	public Transform entryPoint;

	public Transform exitPoint;

	public Transform firstPos;

	private int positionOnFloor;

	public float gap;

	public int points;

	public List<PositionAndChar> positions = new List<PositionAndChar>();

	[SerializeField]
	public SpriteRenderer floorDoorSprite;

	[HideInInspector]
	public GameObject siren;

	public void Setup()
	{
		gap = floor.gap;
		positionOnFloor = GameManager.Instance.gameVars.positionsPerFloor;
		points = floor.floorPoints;
		for (int i = 0; i < positionOnFloor; i++)
		{
			PositionAndChar item = new PositionAndChar(new Vector3(firstPos.position.x - (float)i * gap, firstPos.position.y, 0f), null);
			positions.Add(item);
		}
		base.gameObject.name = floor.name + " Floor " + floorNumber;
		Object.Instantiate(floor.floorArt, base.transform).transform.localPosition = new Vector3(0.031f, 0f, 0f);
	}

	public void AddCharacter(Character_Ctl char_Ctl)
	{
		char_Ctl.Setup(this);
		int freePositionIndex = GetFreePositionIndex();
		if (freePositionIndex == -1 || freePositionIndex + char_Ctl.positions > positions.Count)
		{
			Gameplay_Ctl.Instance.GameOver(this);
		}
		else
		{
			for (int i = 0; i < char_Ctl.positions; i++)
			{
				positions[freePositionIndex + i].character = char_Ctl;
			}
			char_Ctl.SetDestinationPosition(GetCharacterPosition(char_Ctl));
		}
		char_Ctl.transform.parent = base.transform;
	}

	public void InsertCharAtFirstPos(Character_Ctl character)
	{
		MoveRowBack(character.positions);
		for (int i = 0; i < character.positions; i++)
		{
			positions[i].character = character;
		}
		character.SetDestinationPosition(GetCharacterPosition(character));
	}

	public void MoveRow(int numberOfGaps)
	{
		List<PositionAndChar> list = new List<PositionAndChar>(positions);
		for (int i = 0; i < list.Count; i++)
		{
			if (i >= list.Count - numberOfGaps)
			{
				list[i].character = null;
			}
			else
			{
				list[i].character = list[i + numberOfGaps].character;
			}
		}
		positions = list;
		for (int j = 0; j < positions.Count; j++)
		{
			if (positions[j].character != null)
			{
				j += positions[j].character.positions - 1;
			}
		}
		for (int k = 0; k < positions.Count; k++)
		{
			if (positions[k].character != null)
			{
				positions[k].character.SetDestinationPosition(GetCharacterPosition(positions[k].character));
			}
		}
	}

	private void MoveRowBack(int numberOfPositions)
	{
		int freePositionIndex = GetFreePositionIndex();
		if (freePositionIndex == -1 || freePositionIndex - 1 + numberOfPositions > positions.Count)
		{
			Gameplay_Ctl.Instance.GameOver(this);
			return;
		}
		for (int i = 0; i < numberOfPositions; i++)
		{
			for (int num = positions.Count - 1; num > -1; num--)
			{
				if (positions[num].character != null)
				{
					positions[num + 1].character = positions[num].character;
					positions[num].character = null;
				}
			}
		}
		for (int j = 0; j < positions.Count; j++)
		{
			if (positions[j].character != null)
			{
				positions[j].character.SetDestinationPosition(GetCharacterPosition(positions[j].character));
			}
		}
	}

	private int GetFreePositionIndex()
	{
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i].character == null)
			{
				return i;
			}
		}
		return -1;
	}

	private int GetCharacterIndex(Character_Ctl character)
	{
		int result = -1;
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i] != null && positions[i].character != null && positions[i].character == character)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public Vector3 GetCharacterPosition(Character_Ctl character)
	{
		int characterIndex = GetCharacterIndex(character);
		if (characterIndex != -1)
		{
			if (character.positions == 1)
			{
				return positions[characterIndex].position;
			}
			return positions[characterIndex].position - Vector3.right * gap * 0.5f;
		}
		UnityEngine.Debug.LogWarning("Error, characted has not position");
		return Vector3.zero;
	}

	public int GetNumberOfCharactersOnFloor()
	{
		int num = 0;
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i].character != null)
			{
				num++;
			}
		}
		return num;
	}

	public void ClearFloorPositions()
	{
		for (int num = positions.Count - 1; num > -1; num--)
		{
			if (positions[num].character != null)
			{
				positions[num].character.SetEnabledCollisionTrigger(value: true);
				positions[num].character = null;
			}
		}
	}

	public IEnumerator BlendCharsInPairs()
	{
		Tween tween = null;
		Gameplay_Ctl.Instance.SetAllowSpawn(value: false);
		for (int i = 0; i < positions.Count - 2 && positions[i].character != null && positions[i + 1].character != null; i++)
		{
			if (!positions[i].character.character.isSpecialChar && !positions[i + 1].character.character.isSpecialChar)
			{
				int e = i + 1;
				tween = positions[e].character.transform.DOMove(positions[e - 1].character.transform.position, 0.5f).OnComplete(delegate
				{
					positions[e].character.BackToPool();
					positions[e].character = null;
				});
				i++;
			}
		}
		if (tween != null)
		{
			yield return tween.WaitForCompletion();
		}
		CompactQueue();
	}

	private void CompactQueue()
	{
		for (int i = 1; i < positions.Count; i++)
		{
			if (positions[i].character == null)
			{
				for (int j = i; j < positions.Count - 1; j++)
				{
					positions[j].character = positions[j + 1].character;
					positions[j + 1].character = null;
				}
			}
		}
		for (int k = 1; k < positions.Count; k++)
		{
			if (positions[k].character != null)
			{
				positions[k].character.SetDestinationPosition(positions[k].position);
			}
		}
		Gameplay_Ctl.Instance.SetAllowSpawn(value: true);
	}

	public void MakeWave(Character_Ctl arrivingChar)
	{
		int num = GetCharacterIndex(arrivingChar);
		float num2 = 8f;
		float duration = 0.23f;
		while (num > -1 && num2 > 0f)
		{
			DOTween.Sequence().Append(positions[num].character.art.transform.DORotate(Vector3.back * num2, duration).SetEase(Ease.OutCubic)).Append(positions[num].character.art.transform.DORotate(Vector3.zero, duration).SetEase(Ease.InCubic));
			num--;
			num2 -= 2f;
		}
	}

	public void EvaluateSiren()
	{
		print("EvaluateSiren here");
		return;
		if (!Gameplay_Ctl.Instance.IsGameOver())
		{
			int freePositionIndex = GetFreePositionIndex();
			if (freePositionIndex > 4 && siren == null)
			{
				siren = PoolManager.Instance.InstantiatePooled(PoolManager.Instance.siren, base.transform.position, Quaternion.identity);
				siren.transform.parent = base.transform;
				siren.transform.localPosition = Vector3.zero;
				siren.SetActive(value: true);
				siren.GetComponent<Animator>().SetTrigger("Show");
                print("EvaluateSiren show");

            }
            if (siren != null && freePositionIndex <= 4 && freePositionIndex != -1)
			{
				siren.GetComponent<Animator>().SetTrigger("Hide");
                print("EvaluateSiren hide");

                siren = null;
			}
		}
	}
}
