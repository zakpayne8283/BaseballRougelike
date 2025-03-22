using System.IO;
using UnityEngine;

public class CustomLogger : MonoBehaviour
{
    private string logFilePath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void WriteLog(string logMessage)
    {
        setDefaultFilePath();

        string logEntry = $"{logMessage}\n";
        File.AppendAllText(logFilePath, logEntry);
    }

    private void setDefaultFilePath()
    {   
        if (logFilePath == null)
        {
            logFilePath = "defaultlog.log";

            // Set the log file path to Unity's persistent data directory
            logFilePath = Path.Combine(Application.persistentDataPath, logFilePath);
        }
    }
}
