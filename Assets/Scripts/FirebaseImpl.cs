using com.F4A.MobileThird;
using System.Collections.Generic;

public class FirebaseImpl : BaseAnalyticsImpl
{
	public override void Init()
	{
	}

	public override void LogUserData(string propertyName, object value)
	{
		if (value is string)
		{
			string text = (string)value;
			if (text.Length > 36)
			{
				value = text.Substring(0, 36);
			}
		}
		if (propertyName == "userId") FirebaseManager.Instance.SetUserId(value.ToString());
		else FirebaseManager.Instance.SetUserProperty(propertyName, value.ToString());
	}

	public override void LogEvent(string eventName, List<AnalyticsParameter> parameters)
	{
		if (parameters != null)
		{
			Dictionary<string, object> dic = new Dictionary<string, object>();
			foreach (AnalyticsParameter parameter in parameters)
			{
				dic[parameter.key] = parameter.value;
			}
			EventsManager.Instance.LogEvent(eventName, dic);
		}
		else
		{
			EventsManager.Instance.LogEvent(eventName);
		}
	}
}
