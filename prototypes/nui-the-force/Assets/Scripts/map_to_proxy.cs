using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class map_to_proxy : MonoBehaviour
{
public Transform mainScene, copyFrom, copyTo ;
public float scaleFactor = 1;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject proxy = Instantiate(gameObject);
        createProxyScene();
    }

    void createProxyScene(){
        //copyTo.scale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        foreach (Transform child in mainScene){
            var proxyPos = child.InverseTransformPoint(copyFrom.position) * scaleFactor;
            GameObject proxyObj = Instantiate(child, copyTo).gameObject;
            proxyObj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            var proxyComp = proxyObj.AddComponent<proxy>() as proxy;
            proxyComp.target = child;
            proxyComp.scale = scaleFactor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
