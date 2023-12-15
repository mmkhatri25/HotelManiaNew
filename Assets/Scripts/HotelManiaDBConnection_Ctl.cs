using Alg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class HotelManiaDBConnection_Ctl : MonoBehaviour
{
	private ConnectionConstantsSO connectionConstants;

	public static HotelManiaDBConnection_Ctl Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void Start()
	{
		connectionConstants = Singleton<DBConnection_Ctl>.Instance.connectionConstants;
	}

	public void GetGameData(Action action = null, bool justLinked = false)
	{
		//StartCoroutine(GetGameDataIE(Singleton<DBConnection_Ctl>.Instance.fb_id, action, justLinked));
	}



	private void SetCoinsText(int coins)
	{
		throw new NotImplementedException();
	}

	private void GetGameDataFailed()
	{
	}

	

	
}
