using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class MissionManager
{
	private static TextAsset csvFile;

	private static bool isReady = false;

	private static string filePath = "TextRawData/Missions";

	private static string[,] missionsGrid;

	public static List<MissionRaw> missionRawList;

	public static void Init()
	{
		csvFile = (Resources.Load(filePath) as TextAsset);
		CreateMissionGrid();
		CreateMissionRawList();
	}

	private static void CreateMissionRawList()
	{
		missionRawList = new List<MissionRaw>();
		for (int i = 1; i < missionsGrid.GetUpperBound(1); i++)
		{
			string missionName = missionsGrid[0, i];
			int tier = int.Parse(missionsGrid[1, i]);
			string missionGroup = missionsGrid[2, i];
			string type = missionsGrid[3, i];
			string difficulty = missionsGrid[4, i];
			string description = missionsGrid[5, i];
			int variableA = int.Parse(missionsGrid[6, i]);
			string variableB = missionsGrid[7, i];
			int result = 0;
			int.TryParse(missionsGrid[8, i], out result);
			int result2 = 0;
			int.TryParse(missionsGrid[9, i], out result2);
			int result3 = 0;
			int.TryParse(missionsGrid[10, i], out result3);
			bool enabled = result3 == 1;
			missionRawList.Add(new MissionRaw(missionName, tier, missionGroup, type, difficulty, description, variableA, variableB, result, result2, enabled));
		}
	}

	private static void CreateMissionGrid()
	{
		missionsGrid = SplitCsvGrid(csvFile.text);
	}

	private static string[,] SplitCsvGrid(string csvText)
	{
		string[] array = csvText.Split("\n"[0]);
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = SplitCsvLine(array[i]);
			num = Mathf.Max(num, array2.Length);
		}
		string[,] array3 = new string[num + 1, array.Length + 1];
		for (int j = 0; j < array.Length; j++)
		{
			string[] array4 = SplitCsvLine(array[j]);
			for (int k = 0; k < array4.Length; k++)
			{
				array3[k, j] = array4[k];
				array3[k, j] = array3[k, j].Replace("\"\"", "\"");
			}
		}
		return array3;
	}

	private static string[] SplitCsvLine(string line)
	{
		return (from Match m in Regex.Matches(line, "(((?<x>(?=[,\\r\\n]+))|\"(?<x>([^\"]|\"\")+)\"|(?<x>[^,\\r\\n]+)),?)", RegexOptions.ExplicitCapture)
			select m.Groups[1].Value).ToArray();
	}

	private static void DebugList()
	{
		for (int i = 0; i < missionRawList.Count; i++)
		{
			UnityEngine.Debug.Log(missionRawList[i].missionName);
			UnityEngine.Debug.Log(missionRawList[i].tier);
			UnityEngine.Debug.Log(missionRawList[i].type);
			UnityEngine.Debug.Log(missionRawList[i].difficulty);
			UnityEngine.Debug.Log(missionRawList[i].description);
			UnityEngine.Debug.Log(missionRawList[i].variableA);
			UnityEngine.Debug.Log(missionRawList[i].variableB);
			UnityEngine.Debug.Log(missionRawList[i].expRew);
			UnityEngine.Debug.Log(missionRawList[i].coinRew);
			UnityEngine.Debug.Log(missionRawList[i].enabled);
			UnityEngine.Debug.Log("-------------------");
		}
	}
}
