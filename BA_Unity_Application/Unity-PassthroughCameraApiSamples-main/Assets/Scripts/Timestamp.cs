using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Provides functionality to log timestamps with descriptive messages to a file.
/// </summary>
public class Timestamp : MonoBehaviour
{
    // Define the log file name as a constant.
    private const string LogFileName = "timestamp.log";

    // Variable to store the full path of the log file.
    private string logFilePath;

    void Awake()
    {
        // Determine the path for the log file.
        // Application.persistentDataPath is a writable directory on any platform.
        logFilePath = Path.Combine(Application.persistentDataPath, LogFileName);
        Debug.Log($"Timestamp log file will be saved to: {logFilePath}");
    }

    /// <summary>
    /// Logs the current time and an event description to the log file.
    /// </summary>
    /// <param name="eventDescription">The content of the event to log (e.g., "Button Pressed", "Scene Loaded").</param>
    public void LogTimestamp()
    {
        string eventDescription = "LLM Error at: ";
        try
        {
            // Get the current time and convert it to a string with the specified format.
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // Create the message to be written to the log.
            string logEntry = $"{timestamp} - {eventDescription}{Environment.NewLine}";

            // File.AppendAllText creates a new file if it doesn't exist, or appends to the end if it does.
            File.AppendAllText(logFilePath, logEntry);

            Debug.Log($"Timestamp logged: {logEntry.Trim()}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write timestamp to log file: {e.Message}");
        }
    }


}