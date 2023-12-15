using UnityEngine;

public abstract class SpecialCharacterGeneralAnimation : MonoBehaviour
{
	public abstract void Disable();

	public abstract SpecialCharacterGeneralAnimation GetInstance();

	public abstract int GetActiveSpecialCharNumber();

	public abstract void ModifyActiveSpecialCharNumber(int i);
}
