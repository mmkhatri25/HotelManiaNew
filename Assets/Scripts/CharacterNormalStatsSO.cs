using UnityEngine;

[CreateAssetMenu(fileName = "CharacterNormalStatsSO", menuName = "Game/CharacterNormalStatsSO")]
[SerializeField]
public class CharacterNormalStatsSO : ScriptableObject
{
	public float speed;

	public int baseScore = 1;

	public int matchedScore = 1;

	public int familyScore = 1;
}
