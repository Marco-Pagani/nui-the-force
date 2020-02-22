using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGestureManager : MonoBehaviour
{
    public map_to_proxy proxyManager;

    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        if (proxyManager == null)
        {
            proxyManager = FindObjectOfType<map_to_proxy>();
        }
    }

    public void StartGrasp()
    {
        if (!active)
        {
            proxyManager.createProxyScene();
            active = true;
        }
    }

    public void EndGrasp()
    {
        if (active)
        {
            proxyManager.deleteProxyScene();
            active = false;
        }
    }
}
