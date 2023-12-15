public class RewardData
{
	public string RewardType;

	public int Amount;

	public string Campaing = "Default";

	public RewardData()
	{
	}

	public RewardData(string rewardType, int amount)
	{
		RewardType = rewardType;
		Amount = amount;
		Campaing = "Default";
	}
}
