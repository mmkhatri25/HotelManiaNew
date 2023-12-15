using UnityEngine;

[CreateAssetMenu(fileName = "Booster", menuName = "Game/Boosters")]
[SerializeField]
public class BoosterSO : ScriptableObject
{
	public int boosterNumber;

	public float effectTime = 5f;

	public float cooldownTime = 30f;

	public ScriptableObject scriptableObjReplacement;

	public GameObject animationPrefab;
}
