using UnityEngine;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour
{
    public int maxLogs = 5; // Maximum number of logs to display
    public float displayDuration = 3f; // Duration in seconds to display each log
    public Vector2 logPosition = new Vector2(10f, 10f); // Position of the log on the screen
    public Vector2 logSize = new Vector2(200f, 20f); // Size of the log box

    private Text logText; // Reference to the UI Text component
    private string[] logs; // Array to store the logs
    private float[] logTimestamps; // Array to store the timestamps of the logs

    private void Awake()
    {
        // Get the Text component from the UI object
        logText = GetComponent<Text>();

        logs = new string[maxLogs];
        logTimestamps = new float[maxLogs];
    }

    private void OnEnable()
    {
        Application.logMessageReceived += LogMessageReceived;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= LogMessageReceived;
    }

    private void LogMessageReceived(string logText, string stackTrace, LogType logType)
    {
        // Shift the logs and timestamps
        for (int i = maxLogs - 1; i > 0; i--)
        {
            logs[i] = logs[i - 1];
            logTimestamps[i] = logTimestamps[i - 1];
        }

        // Store the new log and timestamp
        logs[0] = logText;
        logTimestamps[0] = Time.time;

        // Update the displayed logs
        UpdateLogDisplay();
    }

    private void UpdateLogDisplay()
    {
        // Build the log display text
        string logDisplayText = "";
        for (int i = 0; i < maxLogs; i++)
        {
            if (logs[i] != null)
            {
                float logAge = Time.time - logTimestamps[i];

                // Check if the log has expired
                if (logAge >= displayDuration)
                {
                    logs[i] = null; // Clear the log
                }
                else
                {
                    // Append the log to the display text
                    logDisplayText += logs[i] + "\n";
                }
            }
        }

        // Update the UI Text component with the log display text
        logText.text = logDisplayText;
    }
}
