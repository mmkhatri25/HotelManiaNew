using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Game/Character")]
[SerializeField]
public class CharacterSO : ScriptableObject
{
	public CharacterBaseStatsSO baseStatsSO;

	public bool isSpecialChar;

	public string charClass;

	public GameObject prefab;

	public int positions = 1;

	public float weightMod;

	public float elevatorDirectionMod = 1f;

	public bool hasDestination = true;

	public List<CharacterArtSO> charsArtList = new List<CharacterArtSO>();

	public GameObject specialCharIntroAnimPrefab;

	public Sprite elevatorStatusSprite;

	public SpecialCharacterGeneralAnimation specialCharGeneralAnim;
}
