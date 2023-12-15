using UnityEngine;

[SerializeField]
public class PositionAndChar
{
	public Vector3 position;

	public Character_Ctl character;

	public PositionAndChar(Vector3 pos, Character_Ctl charac)
	{
		position = pos;
		character = charac;
	}
}
