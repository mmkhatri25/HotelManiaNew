public struct AnalyticsParameter
{
	public string key;

	public object value;

	public AnalyticsParameter(string key, object value)
	{
		this.key = key;
		this.value = value;
	}
}
