using Alg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DBConnection_Ctl : Singleton<DBConnection_Ctl>
{
	public ConnectionConstantsSO connectionConstants;

	private bool _linkedFB;

	public string fb_id = "";

	public string first_name = "";

	public string last_name = "";

	public string middle_name = "";

	public string age_range = "";

	public string email = "";

	public string country = "";

	public GameObject _loadingPopUpRef;

	public bool linkedFB
	{
		get
		{
			return _linkedFB;
		}
		set
		{
			_linkedFB = value;
		}
	}

	public event Action AllDataSynced;

	protected override void Init()
	{
		if (PlayerPrefs.HasKey("fb_id"))
		{
			fb_id = PlayerPrefs.GetString("fb_id");
			Singleton<DBConnection_Ctl>.Instance.linkedFB = true;
		}
		base.Init();
	}

#if DEFINE_FACEBOOK_SDK
	public void ProcessFacebookInfo(IResult result)
	{
		if (result != null && result.ResultDictionary != null)
		{
			if (result.ResultDictionary.ContainsKey("id"))
			{
				fb_id = result.ResultDictionary["id"].ToString();
			}
			if (result.ResultDictionary.ContainsKey("first_name"))
			{
				first_name = result.ResultDictionary["first_name"].ToString();
			}
			if (result.ResultDictionary.ContainsKey("last_name"))
			{
				last_name = result.ResultDictionary["last_name"].ToString();
			}
			if (result.ResultDictionary.ContainsKey("middle_name"))
			{
				middle_name = result.ResultDictionary["middle_name"].ToString();
			}
			if (result.ResultDictionary.ContainsKey("age_range"))
			{
				age_range = result.ResultDictionary["age_range"].ToString();
			}
			if (result.ResultDictionary.ContainsKey("email"))
			{
				email = result.ResultDictionary["email"].ToString();
			}
		}
		StartCoroutine(RegisterOrUpdateIE());
	}
#endif

	

	private void RegisterOrUpdateFailed()
	{
	}

	public void DataSynced()
	{
		if (LoadingPopup.Instance != null)
		{
			LoadingPopup.Instance.ClosePopup();
		}
		if (this.AllDataSynced != null)
		{
			this.AllDataSynced();
		}
	}

	public void CloseLoadingPopup()
	{
		if (_loadingPopUpRef != null)
		{
			UnityEngine.Object.Destroy(_loadingPopUpRef);
		}
	}
}
