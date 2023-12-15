using System.Collections.Generic;

public abstract class BaseAnalyticsImpl
{
	public abstract void Init();

	public abstract void LogUserData(string propertyName, object value);

	public abstract void LogEvent(string eventName, List<AnalyticsParameter> parameters);
}
