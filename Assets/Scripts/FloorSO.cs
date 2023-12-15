using UnityEngine;

[CreateAssetMenu(fileName = "Floor", menuName = "Game/Floor")]
[SerializeField]
public class FloorSO : ScriptableObject
{
	public string floorName;

	public float speed = 3f;

	public int positions = 10;

	public float gap = 0.48f;

	public int floorPoints = 1;

	public GameObject floorArt;

	public Sprite floorSprite;

	[SerializeField]
	public CharacterSO floorCharacter;

	public Sprite char1;

	public Sprite char2;

	public Sprite char3;

	public string displayName;

	public string displayDesc;

	public bool neverEquipped = true;

	public bool isSecret;
	public bool isBought;
}
