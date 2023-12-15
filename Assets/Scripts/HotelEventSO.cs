using UnityEngine;

[CreateAssetMenu(fileName = "HotelEvent", menuName = "Game/HotelEvent")]
public class HotelEventSO : ScriptableObject
{
	public float minTime;

	public float maxTime;

	public int minTaps;

	public int maxTaps;

	public GameObject tapImage;

	public GameObject prefab;
}
