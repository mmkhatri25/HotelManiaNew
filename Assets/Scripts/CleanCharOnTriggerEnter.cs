using UnityEngine;

public class CleanCharOnTriggerEnter : MonoBehaviour
{
	[SerializeField]
	private BoxCollider2D _boxCollider2D;

	[SerializeField]
	private AudioSource _audioSource;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.transform.parent.GetComponent<Character_Ctl>() != null)
		{
			PoolManager.Instance.ObjectBackToPool(collision.gameObject.transform.parent.gameObject);
			_audioSource.Play();
		}
	}
}
