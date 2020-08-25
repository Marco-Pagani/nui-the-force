using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logger
{
    private static Logger _instance;
    public static Logger Instance
    {
        get
        {
            if (_instance == null) _instance = new Logger();
            return _instance;
        }
    }

    private string rootLogPath = "./";
    private string logFilename = "";
    private StreamWriter logWriter;

    public Logger()
    {
    }

    public void StartLog(string pid, string condition, string levelNumber)
    {
        StartLog($"Logfile_p{pid}_{condition}_Lvl{levelNumber}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}");
    }

    public void StartLog(string filename)
    {
        StopLog();

        logFilename = $"{filename}.txt";
        string logPath = Path.Combine(rootLogPath, logFilename);
        logWriter = new StreamWriter(logPath);

        // Write headers
        logWriter.WriteLine("timestamp,name,message");
    }

    public void StopLog()
    {
        if (logWriter != null)
        {
            logWriter.Flush();
            logWriter.Close();
            logWriter = null;
        }
    }

    public void Log(string name, string message)
    {
        Log(name, message);
    }

    public void Log(params object[] args)
    {
        if (logWriter == null) return;
        string line = string.Join(", ", args);
        line = $"{DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss")},{line}";
        Debug.Log(line);

        if (logWriter != null)
        {
            logWriter.WriteLine(line);
            logWriter.Flush();
        }
    }
}
