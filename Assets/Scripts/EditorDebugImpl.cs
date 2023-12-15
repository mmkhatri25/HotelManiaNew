using System.Collections.Generic;
using UnityEngine;

public class EditorDebugImpl : BaseAnalyticsImpl
{
	public override void Init()
	{
		UnityEngine.Debug.Log("<color=cyan> Analytics.Init</color>");
	}

	public override void LogUserData(string propertyName, object value)
	{
		UnityEngine.Debug.Log("<color=cyan> Analytics.LogUserData >> " + propertyName + " : " + value + "</color>");
	}

	public override void LogEvent(string eventName, List<AnalyticsParameter> parameters)
	{
		UnityEngine.Debug.Log("<color=cyan> Analytics.LogEvent >> " + eventName + "</color>");
		if (parameters != null)
		{
			foreach (AnalyticsParameter parameter in parameters)
			{
				UnityEngine.Debug.Log("<color=cyan> Analytics.LogEvent Parameter >> " + parameter.key + " : " + parameter.value + "</color>");
			}
		}
	}
}
