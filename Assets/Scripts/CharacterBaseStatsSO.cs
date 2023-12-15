using UnityEngine;

[CreateAssetMenu(fileName = "CharacterBaseStats", menuName = "Game/CharacterBaseStats")]
[SerializeField]
public class CharacterBaseStatsSO : ScriptableObject
{
	public float speed = 2f;

	public int baseScore = 1;

	public int matchedScore = 5;

	public int familyScore = 10;
}
