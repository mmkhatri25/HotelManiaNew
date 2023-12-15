using UnityEngine;

public class ScientistGeneralAnimation : SpecialCharacterGeneralAnimation
{
	private static ScientistGeneralAnimation Instance;

	[SerializeField]
	private Transform elevatorElectricityContainer;

	[SerializeField]
	public int activeSpecialCharNumber;

	[SerializeField]
	private AudioSource audioSource;

	private void Awake()
	{
		GameManager.Instance.OnGameOver += ((SpecialCharacterGeneralAnimation)this).Disable;
		if (Instance == null)
		{
			Instance = this;
		}
		else if (activeSpecialCharNumber != 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		activeSpecialCharNumber = 0;
		Setup();
	}

	public override SpecialCharacterGeneralAnimation GetInstance()
	{
		return Instance;
	}

	private void OnEnable()
	{
		Setup();
	}

	private void Setup()
	{
		elevatorElectricityContainer.parent = Gameplay_Ctl.Instance.elevator_Ctl.transform;
		elevatorElectricityContainer.localPosition = Vector3.zero;
		elevatorElectricityContainer.gameObject.SetActive(value: true);
		Gameplay_Ctl.Instance.uI_Ctl.EnableScientistVisual();
		audioSource.Play();
	}

	public override int GetActiveSpecialCharNumber()
	{
		return activeSpecialCharNumber;
	}

	public override void ModifyActiveSpecialCharNumber(int i)
	{
		activeSpecialCharNumber += i;
		if (activeSpecialCharNumber % 2 == 0)
		{
			Gameplay_Ctl.Instance.uI_Ctl.FlipButtonRotationToNormal();
			Gameplay_Ctl.Instance.inGameAudioManager.FlipButton2();
		}
		else
		{
			Gameplay_Ctl.Instance.uI_Ctl.FlipButtonRotationUpsideDown();
			Gameplay_Ctl.Instance.inGameAudioManager.FlipButton1();
		}
	}

	public override void Disable()
	{
		Gameplay_Ctl.Instance.uI_Ctl.DisableScientistVisual();
		audioSource.Play();
		elevatorElectricityContainer.parent = base.transform;
		GameManager.Instance.OnGameOver -= ((SpecialCharacterGeneralAnimation)this).Disable;
		PoolManager.Instance.ObjectBackToPool(base.gameObject);
	}
}
