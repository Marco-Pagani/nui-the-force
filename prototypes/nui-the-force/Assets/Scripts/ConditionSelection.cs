using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConditionSelection : MonoBehaviour
{
    public GameObject techDropdown, levelDropdown, pidText;

    private const int LEVELS = 6, TECHS = 3, OMNI = 1, TRANSITION = 19, END = 20;

    private int _techChoice, _levelChoice, _currScene = 0, _lastScene, _currLevel, _lastLevel;
    private bool _break = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            TransitionScene();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            InitializeScene();
        }
    }

    public void StartLevel()
    {
        _techChoice = techDropdown.GetComponent<Dropdown>().value;
        _levelChoice = levelDropdown.GetComponent<Dropdown>().value;
        _currLevel = _levelChoice - 1;

        if (_techChoice != 0 || _levelChoice != 0)
        {
            // For the case that each scene is it's own level
            // _currScene = SceneManager.GetActiveScene().buildIndex;
            // SceneManager.LoadScene((((_techChoice - 1) * LEVELS) + _levelChoice), LoadSceneMode.Single);

            // For the case that there is one scene that holds every level
            SceneManager.LoadScene(OMNI, LoadSceneMode.Single);
        }
    }

    public void TransitionScene()
    {
        // For the case that each scene is it's own level
        // _lastScene = _currScene;
        // _currScene = SceneManager.GetActiveScene().buildIndex;

        // if(_currScene != TRANSITION && _currScene % 6 != 0)
        // {
        //     SceneManager.LoadScene(TRANSITION, LoadSceneMode.Single);
        // }
        // else if(_currScene != TRANSITION && _currScene % 6 == 0)
        // {
        //     SceneManager.LoadScene(END, LoadSceneMode.Single);
        // }
        // else if(_currScene == END)
        // {
        //     Application.Quit();
        // }
        // else
        // {
        //     SceneManager.LoadScene(_lastScene + 1, LoadSceneMode.Single);
        // }

        // For the case that there is one scene that holds every level
        _break = GameObject.Find("break_scene").activeSelf;

        if (_break)
        {
            _currLevel++;
            GameObject.Find("break_scene").SetActive(false);
            GameObject.Find("level_" + _currLevel).SetActive(true);
        }
        else if (_currLevel != LEVELS - 1)
        {
            GameObject.Find("level_" + _currLevel).SetActive(false);
            GameObject.Find("break_scene").SetActive(true);
        }
        else if (_currLevel == LEVELS - 1)
        {
            GameObject.Find("level_" + _currLevel).SetActive(false);
            GameObject.Find("end_scene").SetActive(true);
        }
        else
        {
            Application.Quit();
        }
    }

    public void InitializeScene()
    {
        GameObject.Find("cover").SetActive(false);
        switch(_techChoice)
        {
            case 1:
                GameObject.Find("[HoloGestureManager]").SetActive(false);
                GameObject.Find("[IronmanGestureManager]").SetActive(false);
                break;
            case 2:
                GameObject.Find("[HoloGestureManager]").SetActive(false);
                GameObject.Find("[IronmanGestureManager]").SetActive(false);
                break;
            case 3:
                GameObject.Find("[ForceGestureManager]").SetActive(false);
                GameObject.Find("[HoloGestureManager]").SetActive(false);
                break;
            default:
                break;
        }

        for (int i = 0; i < LEVELS; i++)
        {
            if(i != _currLevel) GameObject.Find("level_" + i.ToString()).SetActive(false);
        }
    }
}
