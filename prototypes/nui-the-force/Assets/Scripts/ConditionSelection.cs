using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConditionSelection : MonoBehaviour
{
    public GameObject techDropdown, levelDropdown, pidText;

    private const int LEVELS = 6, TECHS = 3, OMNI = 1, TRANSITION = 19, END = 20;

    private int _techChoice, _levelChoice, _currScene = 0, _lastScene, _currLevel, _lastLevel;
    private bool _break = false;
    private MyScenes _scenes;

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
        // _break = GameObject.Find("break_scene").activeSelf;
        foreach (GameObject go in _scenes.Levels) if (go.name == "break_scene") _break = go.activeSelf;

        if (_break)
        {
            _currLevel++;
            foreach (GameObject go in _scenes.Levels)
            {
                if (go.name == "break_scene") go.SetActive(false);
                else if (go.name == "level_" + _currLevel) go.SetActive(true);
            }
        }
        else if (_currLevel != LEVELS - 1)
        {
            foreach (GameObject go in _scenes.Levels)
            {
                if (go.name == "break_scene") go.SetActive(true);
                else if (go.name == "level_" + _currLevel) go.SetActive(false);
            }
        }
        else if (_currLevel == LEVELS - 1)
        {
            foreach (GameObject go in _scenes.Levels)
            {
                if (go.name == "end_scene") go.SetActive(true);
                else if (go.name == "level_" + _currLevel) go.SetActive(false);
            }
        }
        else
        {
            Application.Quit();
        }
    }

    public void InitializeScene()
    {
        _scenes = new MyScenes();
        _scenes.GetSceneInfo();

        switch (_techChoice)
        {
            case 1:
                foreach (GameObject go in _scenes.Techniques) if (go.name == "[ForceGestureManager]") { go.SetActive(true); break; }
                break;
            case 2:
                foreach (GameObject go in _scenes.Techniques) if (go.name == "[HoloGestureManager]") { go.SetActive(true); break; }
                break;
            case 3:
                foreach (GameObject go in _scenes.Techniques) if (go.name == "[IronmanGestureManager]") { go.SetActive(true); break; }
                break;
            default:
                break;
        }

        foreach (GameObject go in _scenes.Levels) if (go.name == "level_" + _currLevel.ToString()) { go.SetActive(true); break; }
    }
}
