using UnityEngine;
using System.IO;
//using System.Runtime.Remoting.Contexts;

public class LogToFile : MonoBehaviour
{
    private string logFilePath;
    private StreamWriter logWriter;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Define the file path where the log will be saved
        //string filePath = Path.Combine(Application.persistentDataPath, "log.txt");

        //if (File.Exists(filePath))
        //{
        //    // The file exists, so delete it
        //    File.Delete(filePath);
        //    Debug.Log("Deleted file: " + "log.txt" + Application.persistentDataPath);
        //}
        //else
        //{
        //    Debug.Log("File not found: " + "log.txt");
        //}



        logFilePath = Path.Combine(Application.persistentDataPath, "log.txt");
        print("logFilePath.." + logFilePath);
        // Subscribe to the log message event
        Application.logMessageReceived += HandleLog;

        // Create or append to the log file
        logWriter = File.AppendText(logFilePath);
        logWriter.WriteLine("Log file created on: " + System.DateTime.Now);
        logWriter.Flush();
    }

    private void HandleLog(string logText, string stackTrace, LogType type)
    {
        // Write the log message to the log file
        logWriter.WriteLine(type.ToString() + ": " + logText);
        logWriter.WriteLine("Stack Trace:\n" + stackTrace);
        logWriter.Flush();
       // print("log added here.." + logText);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the log message event and close the log file
        Application.logMessageReceived -= HandleLog;
        if (logWriter != null)
        {
            logWriter.Close();
        }
    }
}
