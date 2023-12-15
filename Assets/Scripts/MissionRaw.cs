using System;

[Serializable]
public struct MissionRaw
{
	public string missionName;

	public int tier;

	public string missionGroup;

	public string type;

	public string difficulty;

	public string description;

	public int variableA;

	public string variableB;

	public int expRew;

	public int coinRew;

	public bool enabled;

	public MissionRaw(string _missionName, int _tier, string _missionGroup, string _type, string _difficulty, string _description, int _variableA, string _variableB, int _expRew, int _coinRew, bool _enabled)
	{
		missionName = _missionName;
		tier = _tier;
		missionGroup = _missionGroup;
		type = _type;
		difficulty = _difficulty;
		description = _description;
		variableA = _variableA;
		variableB = _variableB;
		expRew = _expRew;
		coinRew = _coinRew;
		enabled = _enabled;
	}
}
