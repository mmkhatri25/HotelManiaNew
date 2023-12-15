using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralAudioController : BaseAudio
{
	[SerializeField]
	private AudioClip gameplayMusic;

	[SerializeField]
	private AudioClip gameplayMusicFast;

	[SerializeField]
	private AudioClip gachaMusic;

	[SerializeField]
	private AudioClip menuMusic;

	[SerializeField]
	private float fadeOutDuration = 2f;

	[SerializeField]
	private float fadeInDuration = 3f;

	private AudioSource gameplayMusicSource;

	private AudioSource gameplayMusicFastSource;

	private AudioSource menuMusicSource;

	private AudioSource gachaMusicSource;

	private float lastSpeedChangeTime;

	private float numberOfSecsPreventSpeedChange = 4f;

	public static GeneralAudioController Instance;

	private void myNewStart()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		SceneManager.sceneLoaded += CheckNewScene;
		gameplayMusicSource = CreateAudioSource(gameplayMusic, AudioChannel.Music, playOnAwake: false, loop: true);
		gameplayMusicFastSource = CreateAudioSource(gameplayMusicFast, AudioChannel.Music, playOnAwake: false, loop: true);
		menuMusicSource = CreateAudioSource(menuMusic, AudioChannel.Music, playOnAwake: false, loop: true);
		gachaMusicSource = CreateAudioSource(gachaMusic, AudioChannel.Music, playOnAwake: false, loop: true);
		CheckNewScene(SceneManager.GetActiveScene().buildIndex);
		Object.DontDestroyOnLoad(base.gameObject);
	}

	void myNewUpdate()
	{
		if(PlayerPrefs.GetInt("myMusic")==0)
		{
			gameplayMusicSource.enabled = false;
            gameplayMusicFastSource.enabled = false;
            menuMusicSource.enabled = false;
            gachaMusicSource.enabled = false;
		}
		else
		{
			gameplayMusicSource.enabled = false;
            gameplayMusicFastSource.enabled = false;
            menuMusicSource.enabled = false;
            gachaMusicSource.enabled = false;
		}
	}

	private void CheckNewScene(Scene scene, LoadSceneMode loadScene)
	{
		CheckNewScene(scene.buildIndex);
	}

	private void CheckNewScene(int buildIndex)
	{
		switch (buildIndex)
		{
		case 0:
			if (gameplayMusicSource.isPlaying)
			{
				gameplayMusicSource.Stop();
			}
			if (!menuMusicSource.isPlaying)
			{
				menuMusicSource.Play();
			}
			if (gachaMusicSource.isPlaying)
			{
				FadeOutGachaToMenu();
			}
			break;
		case 3:
			FadeOutMenuToGacha();
			break;
		default:
			GameManager.Instance.OnGameOver += FadeOutGameplayToMenu;
			break;
		}
	}

	private void FadeOutMenuToGameplay()
	{
		gameplayMusicSource.Stop();
		gameplayMusicFastSource.volume = 0f;
		gameplayMusicSource.volume = 0f;
		gameplayMusicSource.Play();
		gameplayMusicSource.DOFade(1f, fadeInDuration);
		menuMusicSource.DOFade(0f, fadeOutDuration).OnComplete(delegate
		{
			menuMusicSource.Stop();
			menuMusicSource.volume = 1f;
		});
	}

	public void FadeOutMenuToGameplayFullVolumeStart()
	{
		Gameplay_Ctl.Instance.inGameAudioManager.PlayHotelBgAudio();
		gameplayMusicSource.Stop();
		gameplayMusicFastSource.volume = 0f;
		gameplayMusicSource.volume = 1f;
		gameplayMusicSource.Play();
		menuMusicSource.DOFade(0f, fadeOutDuration / 2f).OnComplete(delegate
		{
			menuMusicSource.Stop();
			menuMusicSource.volume = 1f;
		});
	}

	private void FadeOutGameplayToMenu()
	{
		menuMusicSource.volume = 0f;
		gameplayMusicSource.DOFade(0f, fadeOutDuration).OnComplete(delegate
		{
			gameplayMusicSource.Stop();
			gameplayMusicSource.volume = 1f;
			menuMusicSource.Play();
			menuMusicSource.DOFade(1f, fadeInDuration);
		});
		gameplayMusicFastSource.DOFade(0f, fadeOutDuration).OnComplete(delegate
		{
			gameplayMusicSource.Stop();
			gameplayMusicFastSource.volume = 0f;
		});
	}

	public void FadeOutGachaToMenu()
	{
		menuMusicSource.volume = 1f;
		menuMusicSource.Play();
		gachaMusicSource.Stop();
	}

	public void FadeOutMenuToGacha()
	{
		gachaMusicSource.volume = 1f;
		gachaMusicSource.Play();
		menuMusicSource.Stop();
	}

	public void GameplayMusicToSlow()
	{
		if (lastSpeedChangeTime + numberOfSecsPreventSpeedChange < Time.time && gameplayMusicSource.volume < 1f && !gameplayMusicSource.isPlaying)
		{
			float num = gameplayMusicFastSource.time / gameplayMusicFastSource.clip.length;
			gameplayMusicSource.time = gameplayMusicSource.clip.length * num;
			gameplayMusicSource.Play();
			gameplayMusicSource.DOFade(1f, 0.1f);
			gameplayMusicFastSource.DOFade(0f, 0.1f).OnComplete(delegate
			{
				gameplayMusicFastSource.Stop();
			});
			lastSpeedChangeTime = Time.time;
		}
	}

	public void GameplayMusicToFast()
	{
		if (lastSpeedChangeTime + numberOfSecsPreventSpeedChange < Time.time && gameplayMusicFastSource.volume < 1f && !gameplayMusicFastSource.isPlaying)
		{
			float num = gameplayMusicSource.time / gameplayMusicSource.clip.length;
			gameplayMusicFastSource.time = gameplayMusicFastSource.clip.length * num;
			gameplayMusicFastSource.Play();
			gameplayMusicFastSource.DOFade(1f, 0.1f);
			gameplayMusicSource.DOFade(0f, 0.1f).OnComplete(delegate
			{
				gameplayMusicSource.Stop();
			});
			lastSpeedChangeTime = Time.time;
		}
	}

	public void PauseMenuMusic()
	{
		menuMusicSource.Pause();
	}

	public void ResumeMenuMusic()
	{
		UnityEngine.Debug.Log("ResumeMenuMusic");
		menuMusicSource.Play();
	}
}
