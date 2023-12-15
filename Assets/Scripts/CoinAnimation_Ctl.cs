using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinAnimation_Ctl : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem m_System;

	private ParticleSystem.Particle[] m_Particles;

	private float startTime;

	private Vector3 destination = Vector3.zero;

	private float timeBetweenParticles = 0.002f;

	private float startingGravity;

	public int numParticlesAlive;

	private int numParticles;

	private int pointsPerParticle = 50;

	private int currentScore;

	private int lastGameScore;

	private int addedScoreToVisual;

	public float zPosition;

	private Vector2 screenSize;

	public Transform leftCollider;

	public Transform rightCollider;

	private Vector3 cameraPos;

	private float speedMod = 400f;

	private float speedModOriginal = 1600f;

	private float minWaitTimeForMoveBackInterval = 0.002f;

	private float minTimeBeforeCoinsMove = 1f;

	[SerializeField]
	private AudioSource onBirthSoundAudioSources;

	[SerializeField]
	private AudioSource onDeathCoinLoopSource;

	private void Start()
	{
		cameraPos = Camera.main.transform.position;
		screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f))) * 0.6f;
		screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)), Camera.main.ScreenToWorldPoint(new Vector2(0f, Screen.height))) * 0.55f;
		rightCollider.position = new Vector3(cameraPos.x + screenSize.x, cameraPos.y, zPosition);
		leftCollider.position = new Vector3(cameraPos.x - screenSize.x, cameraPos.y, zPosition);
		startingGravity = m_System.gravityModifier;
		SceneManager.activeSceneChanged += AddScoreWithoutVisual;
	}

	public void SetDestinationForCoins()
	{
		if (destination == Vector3.zero)
		{
			destination = (destination = MainMenu_UI_Ctl.Instance.camera.ScreenToWorldPoint(MainMenu_UI_Ctl.Instance.coinBoxImage.transform.position));
		}
	}

	public void DoAnim(int score, Button pressedButton)
	{
		lastGameScore = score;
		m_System.transform.position = Camera.main.ScreenToWorldPoint(pressedButton.transform.position);
		m_System.gravityModifier = startingGravity;
		StartCoroutine(EmitCoins(lastGameScore));
		speedMod = speedModOriginal;
		addedScoreToVisual = 0;
	}

	private IEnumerator EmitCoins(int score)
	{
		m_System.maxParticles = score / pointsPerParticle;
		ParticleSystem.EmissionModule em = m_System.emission;
		em.enabled = true;
		numParticlesAlive = m_System.maxParticles;
		numParticles = numParticlesAlive - 1;
		onBirthSoundAudioSources.Play();
		float timeWaitedOnCoinSpawn = timeBetweenParticles * ((float)score / ((float)pointsPerParticle * 0.5f));
		for (int i = 0; i < score / pointsPerParticle / 2; i++)
		{
			m_System.Emit(2);
			yield return new WaitForSeconds(timeBetweenParticles);
		}
		m_System.Emit(score / pointsPerParticle / 2 % 2);
		yield return new WaitForSeconds(timeBetweenParticles);
		em.enabled = false;
		if (timeWaitedOnCoinSpawn < minTimeBeforeCoinsMove)
		{
			yield return new WaitForSeconds(minTimeBeforeCoinsMove - timeWaitedOnCoinSpawn);
		}
		GameManager.Instance.ChangeScene("MainMenu");
		UnityEngine.Debug.Log(timeWaitedOnCoinSpawn);
	}

	private void AddScoreWithoutVisual(Scene current, Scene next)
	{
		if (lastGameScore != 0 && next.name.CompareTo("MainMenu") == 0)
		{
			currentScore = PlayerDataManager.Coins;
			MoveToCoinBox();
		}
	}

	public void MoveToCoinBox()
	{
		StartCoroutine(MoveCoins());
	}

	private IEnumerator MoveCoins()
	{
		yield return null;
		PlayerDataManager.AddCoins(lastGameScore, showOnUI: false);
		Color transparent = new Color(0f, 0f, 0f, 0f);
		if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
		{
			m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
		}
		m_System.GetParticles(m_Particles);
		numParticles = numParticlesAlive - 1;
		StartCoroutine(ReduceParticleMovementCap());
		if (numParticlesAlive - 1 > 0)
		{
			onDeathCoinLoopSource.Play();
			while (Vector2.Distance(m_Particles[numParticlesAlive - 1].position, destination) != 0f)
			{
				ParticleSystem.Particle[] array = new ParticleSystem.Particle[m_System.main.maxParticles];
				m_System.GetParticles(array);
				float maxDistanceDelta = Time.deltaTime * speedMod;
				for (int i = 0; i < numParticlesAlive; i++)
				{
					if (i > numParticlesAlive - numParticles)
					{
						continue;
					}
					m_Particles[i].position = Vector3.MoveTowards(m_Particles[i].position, destination, maxDistanceDelta);
					m_Particles[i].velocity = Vector3.zero;
					if (Vector2.Distance(m_Particles[i].position, destination) == 0f && m_Particles[i].color != transparent)
					{
						m_Particles[i].color = transparent;
						addedScoreToVisual += pointsPerParticle;
						if ((bool)MainMenu_UI_Ctl.Instance)
						{
							MainMenu_UI_Ctl.Instance.SetCoinsText(currentScore + addedScoreToVisual);
						}
					}
				}
				for (int j = numParticlesAlive - numParticles + 1; j < numParticlesAlive; j++)
				{
					m_Particles[j] = array[j];
				}
				m_System.SetParticles(m_Particles, numParticlesAlive);
				yield return null;
			}
		}
		yield return new WaitForEndOfFrame();
		onDeathCoinLoopSource.DOFade(0f, 0.8f).OnComplete(delegate
		{
			onDeathCoinLoopSource.Stop();
			onDeathCoinLoopSource.volume = 1f;
		});
		lastGameScore = 0;
		if ((bool)MainMenu_UI_Ctl.Instance)
		{
			MainMenu_UI_Ctl.Instance.SetCoinsText(PlayerDataManager.Coins);
		}
	}

	private IEnumerator ReduceParticleMovementCap()
	{
		while (numParticles != 0)
		{
			numParticles--;
			yield return new WaitForSeconds(minWaitTimeForMoveBackInterval);
		}
	}
}
