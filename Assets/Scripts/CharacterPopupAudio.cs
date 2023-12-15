using UnityEngine;

public class CharacterPopupAudio : MonoBehaviour
{
	[SerializeField]
	private AudioClip voiceClip;

	[SerializeField]
	private AudioClip outClip;

	private AudioSource voiceAudio;

	private AudioSource outAudio;

	private void Start()
	{
		voiceAudio = base.gameObject.AddComponent<AudioSource>();
		voiceAudio.clip = voiceClip;
		voiceAudio.playOnAwake = false;
		voiceAudio.outputAudioMixerGroup = AudioManager.GetAudioMixerGroup("SndFx");
		outAudio = base.gameObject.AddComponent<AudioSource>();
		outAudio.clip = outClip;
		outAudio.playOnAwake = false;
		outAudio.outputAudioMixerGroup = AudioManager.GetAudioMixerGroup("SndFx");
	}

	public void PlayVoiceAudio()
	{
		voiceAudio.Play();
	}

	public void PlayOutAudio()
	{
		outAudio.Play();
	}
}
