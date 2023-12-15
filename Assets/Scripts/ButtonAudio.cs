using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour
{
	[SerializeField]
	private AudioClip buttonAudio;

	private AudioSource audioSource;

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(PlayAudio);
		audioSource = new GameObject(base.gameObject.name + "Audio").AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.clip = buttonAudio;
		audioSource.outputAudioMixerGroup = AudioManager.GetAudioMixerGroup("SndFx");
	}

	public void PlayAudio()
	{
		audioSource.Play();
	}

	private void OnDestroy()
	{
		if (audioSource != null)
		{
			if (audioSource.isPlaying)
			{
				UnityEngine.Object.Destroy(audioSource.gameObject, audioSource.clip.length);
			}
			else
			{
				UnityEngine.Object.Destroy(audioSource.gameObject);
			}
		}
	}
}
