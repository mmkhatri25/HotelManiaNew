using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
	private static AudioMixer audioMixer;

	private static float sndfxOriginalVolume;

	private static float musicOriginalVolume;

	private static bool isSndFxMuted;

	private static bool isMusicMuted;

	private static bool isInitialized;

	public static bool SndfxMuted => isSndFxMuted;

	public static bool MusicMuted => isMusicMuted;

	public static void Initialize()
	{
		if (!isInitialized)
		{
			audioMixer = Resources.Load<AudioMixer>("Audio/Mixer");
			sndfxOriginalVolume = GetVolume("SndFx");
			musicOriginalVolume = GetVolume("Music");
			if (PlayerPrefs.HasKey("SndFx"))
			{
				SetVolume("SndFx", PlayerPrefs.GetFloat("SndFx"));
			}
			if (PlayerPrefs.HasKey("Music"))
			{
				SetVolume("Music", PlayerPrefs.GetFloat("Music"));
			}
			isSndFxMuted = (GetVolume("SndFx") == -80f);
			isMusicMuted = (GetVolume("Music") == -80f);
			isInitialized = true;
		}
	}

	static AudioManager()
	{
		Initialize();
	}

	public static void ToggleMusicVolume()
	{
		if (isMusicMuted)
		{
			SetAndSaveVolume("Music", musicOriginalVolume);
		}
		else
		{
			SetAndSaveVolume("Music", -80f);
		}
		isMusicMuted = !isMusicMuted;
	}

	public static void ToggleSndfxVolume()
	{
		if (isSndFxMuted)
		{
			SetAndSaveVolume("SndFx", sndfxOriginalVolume);
		}
		else
		{
			SetAndSaveVolume("SndFx", -80f);
		}
		isSndFxMuted = !isSndFxMuted;
	}

	public static AudioMixerGroup GetAudioMixerGroup(string mixerGroup)
	{
		AudioMixerGroup[] array = new AudioMixerGroup[0];
		if (audioMixer)
        {
            array = audioMixer.FindMatchingGroups(mixerGroup);
        }
        if (array.Length == 0)
		{
			UnityEngine.Debug.Log("Couldn't find AudioMixerGroup named " + mixerGroup);
			return null;
		}
		return array[0];
	}

	private static float GetVolume(string parameter)
	{
		float value = 0f;
		if (audioMixer) audioMixer.GetFloat(parameter, out value);
		return value;
	}

	private static void SetAndSaveVolume(string parameter, float volume)
	{
		PlayerPrefs.SetFloat(parameter, volume);
		SetVolume(parameter, volume);
	}

	private static void SetVolume(string parameter, float volume)
	{
		if (audioMixer) audioMixer.SetFloat(parameter, volume);
	}
}
