using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGestureManager : MonoBehaviour
{
    private enum ProxyState
    {
        Default,
        Grabbed,
        Flipped
    }

    private ProxyState curState = ProxyState.Default;

    public map_to_proxy proxyManager;
    public GameObject copyTo;

    bool proxyCreated = false;

    bool isGrasp = false;
    bool palmUp = false;

    // Start is called before the first frame update
    void Start()
    {
        if (proxyManager == null)
        {
            proxyManager = FindObjectOfType<map_to_proxy>();
        }

        if (copyTo == null)
        {
            copyTo = GameObject.Find("Copy To");
        }
    }

    public void StartGrasp()
    {
        //Debug.Log("GrabStart");
        isGrasp = true;
    }

    public void EndGrasp()
    {
        //Debug.Log("GrabEnd");
        isGrasp = false;
    }

    public void OnDirectionActivate()
    {
        //Debug.Log("PalmUp");
        palmUp = true;
    }

    public void OnDirectionDeactivate()
    {
        //Debug.Log("PalmDown");
        palmUp = false;
    }

    private void Update()
    {
        switch (curState)
        {
            default:
            case ProxyState.Default:
                proxyManager.deleteProxyScene();
                copyTo.SetActive(false);

                if (isGrasp)
                {
                    if (palmUp)
                    {

                    }
                    else
                    {
                        proxyManager.createProxyScene(); // TODO Make method specifically for doing all the things
                        //proxyManager.castGaze();
                        curState = ProxyState.Grabbed;
                    }
                }
                else
                {
                    if (palmUp)
                    {

                    }
                    else
                    {

                    }
                }
                break;
            case ProxyState.Grabbed:
                if (isGrasp)
                {
                    if (palmUp)
                    {

                    }
                    else
                    {

                    }
                }
                else
                {
                    if (palmUp)
                    {
                        curState = ProxyState.Flipped;
                    }
                    else
                    {
                        curState = ProxyState.Default;
                    }
                }
                break;
            case ProxyState.Flipped:
                copyTo.SetActive(true);

                if (isGrasp)
                {
                    if (palmUp)
                    {
                        curState = ProxyState.Default;
                    }
                    else
                    {
                        curState = ProxyState.Default;
                    }
                }
                else
                {
                    if (palmUp)
                    {

                    }
                    else
                    {
                        curState = ProxyState.Default;
                    }
                }
                break;
        }
    }
}
