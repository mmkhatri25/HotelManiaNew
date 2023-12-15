using UnityEngine;

public class BaseAudio : MonoBehaviour
{
	public enum AudioChannel
	{
		Music,
		AmbInGame,
		AmbMenu,
		SndFx
	}

	protected AudioSource CreateAudioSource(AudioClip clip, AudioChannel channel, bool playOnAwake = false, bool loop = false, bool asNewGO = false)
	{
		AudioSource audioSource = null;
		audioSource = (asNewGO ? new GameObject(base.gameObject.name + "Audio").AddComponent<AudioSource>() : base.gameObject.AddComponent<AudioSource>());
		audioSource.playOnAwake = playOnAwake;
		audioSource.loop = loop;
		audioSource.clip = clip;
		audioSource.outputAudioMixerGroup = AudioManager.GetAudioMixerGroup(channel.ToString());
		return audioSource;
	}
}
