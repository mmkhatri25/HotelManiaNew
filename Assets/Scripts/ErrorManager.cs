using Alg;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
	private string supportEmail = "support@pocketplaylab.com";

	public static ErrorManager Instance;

	public static string lastErrorLog = "";

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnEnable()
	{
		Application.logMessageReceived += LogCallback;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= LogCallback;
	}

	public void SendSupportEmail()
	{
		string subject = "HM Support";
		string defaultMailbody = GetDefaultMailbody();
		//SendEmail(subject, defaultMailbody);
	}

	private void LogCallback(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			lastErrorLog = stackTrace;
		}
	}

	private void SendErrorSupportEmail(string stackTrace)
	{
		string subject = "HM Support";
		string errorMailBody = GetErrorMailBody(stackTrace);
		//SendEmail(subject, errorMailBody);
	}

	private string GetDefaultMailbody()
	{
		string first_name = Singleton<DBConnection_Ctl>.Instance.first_name;
		string last_name = Singleton<DBConnection_Ctl>.Instance.last_name;
		string email = Singleton<DBConnection_Ctl>.Instance.email;
		return string.Format("\n\n\n\n\n\n\n\n" + (string.IsNullOrEmpty(lastErrorLog) ? "" : ("lastErrorStackTrace:" + lastErrorLog + "\n")) + "________________________________\nVersion number: {0}\nOperating System: {1}\nDevice model: {2}\n" + (string.IsNullOrEmpty(first_name) ? "" : "First Name: {3}\n") + (string.IsNullOrEmpty(last_name) ? "" : "Last Name: {4}\n") + (string.IsNullOrEmpty(email) ? "" : "Email: {5}\n"), Application.version, SystemInfo.operatingSystem, SystemInfo.deviceModel, first_name, last_name, email);
	}

	private string GetErrorMailBody(string error)
	{
		return GetDefaultMailbody() + $"Error: {error}\n";
	}

	

	
}
