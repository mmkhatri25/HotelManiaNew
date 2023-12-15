using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSound : MonoBehaviour
{
	[SerializeField]
	private List<AudioClip> audioClips;

	private AudioSource audioSource;

	public bool playOnAwake;

	private void OnEnable()
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
		audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
		if (playOnAwake)
		{
			audioSource.Play();
		}
	}
}
