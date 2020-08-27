using Meta.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogStarter : MonoBehaviour
{
    private DateTime startTime;
    //private GameObject metaHeadset;
    
    // Start is called before the first frame update
    private void Start()
    {
        ConditionSelection cs = FindObjectOfType<ConditionSelection>();
        string pid = cs.pidText.GetComponent<Text>().text;
        string condition = cs.techDropdown.GetComponent<Text>().text;
        string level = SceneManager.GetActiveScene().buildIndex.ToString();

        MyLogger.Instance.StartLog(pid, condition, level);
        MyLogger.Instance.Log("Pid", SceneManager.GetActiveScene().name);
        MyLogger.Instance.Log("StartLevel", SceneManager.GetActiveScene().name);
        startTime = DateTime.Now;

        //metaHeadset = FindObjectOfType<MetaCompositor>().gameObject;
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        MyLogger.Instance.Log("EndLevel", SceneManager.GetActiveScene().name);
        MyLogger.Instance.Log("Duration", (DateTime.Now - startTime).Seconds);
        MyLogger.Instance.StopLog();
    }
}
