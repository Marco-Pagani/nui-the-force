using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConditionSelection : MonoBehaviour
{
    public GameObject techDropdown, levelDropdown, pidText;

    private const int LEVELS = 6, TECHS = 3, START = 0, TRANSITION = 19, END = 20;

    private int _techChoice, _levelChoice, _currScene = 0, _lastScene, _pid;

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
    }

    public void StartLevel()
    {
        _pid = Convert.ToInt32(pidText.GetComponent<Text>().text);
        _techChoice = techDropdown.GetComponent<Dropdown>().value;
        _levelChoice = levelDropdown.GetComponent<Dropdown>().value;

        if (_techChoice != 0 || _levelChoice != 0)
        {
            _currScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene((((_techChoice - 1) * LEVELS) + _levelChoice), LoadSceneMode.Single);
        }
    }

    public void TransitionScene()
    {
        _lastScene = _currScene;
        _currScene = SceneManager.GetActiveScene().buildIndex;

        if(_currScene != TRANSITION && _currScene % 6 != 0)
        {
            SceneManager.LoadScene(TRANSITION, LoadSceneMode.Single);
        }
        else if(_currScene != TRANSITION && _currScene % 6 == 0)
        {
            SceneManager.LoadScene(END, LoadSceneMode.Single);
        }
        else if(_currScene == END)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(_lastScene + 1, LoadSceneMode.Single);
        }
    }
}
