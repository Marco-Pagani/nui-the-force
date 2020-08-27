using System;
using System.Collections.Generic;
using UnityEngine;

public class MyScenes : MonoBehaviour
{
    private GameObject[] _allGO;
    private List<GameObject> _techniques = new List<GameObject>(), _levels = new List<GameObject>();

    public MyScenes()
    {
        _allGO = new GameObject[0];
        _techniques = new List<GameObject>();
        _levels = new List<GameObject>();
    }

    public void GetSceneInfo()
    {
        _allGO = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in _allGO) if (go.name.Contains("GestureManager")) _techniques.Add(go);
        foreach (GameObject go in _allGO) if (go.name.Contains("level_") || go.name.Contains("_scene")) _levels.Add(go);
    }

    public List<GameObject> Techniques { get { return _techniques; } }
    public List<GameObject> Levels { get { return _levels; } }
}
