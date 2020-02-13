using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class map_to_proxy : MonoBehaviour
{

    //TODO: these variable names could be better

    // parent of the 'real' objects that are being remotely controlled
    public Transform sceneObjects;

    // the point FROM which the real objects are recreated
    // this point should be set by the user based on where they gesture
    public Transform copyOrigin;

    // the point AT which the proxy objects are created, relative to their distance from the above point
    // this point will be parented to the user's hand so it follows their movements
    public Transform proxyOrigin;

    // scale of the proxy objects relative to the real
    public float scaleFactor = 0.5f;

    // mex distance from the copy origin that objects will be copied
    public float renderDistance = 10f;


    void Start()
    {

    }

    //this should be called when the user reaches for an object
    public void createProxyScene()
    {
        // delete old scene in case it exists
        deleteProxyScene();

        // scale proxy to desired size
        proxyOrigin.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // get objects within range of the copy origin
        //TODO: create some sort of shader to fade them out beyond a certain range
        Collider[] area = Physics.OverlapSphere(copyOrigin.position, renderDistance);

        foreach (Collider c in area)
        {
            var child = c.transform;
            //create clone
            GameObject proxyObj = Instantiate(child, proxyOrigin).gameObject;
            //set clone's position relative to copy origin
            proxyObj.transform.localPosition = copyOrigin.InverseTransformPoint(child.position);
            proxyObj.transform.localRotation = child.localRotation;
            //add proxy component to clone and set vars
            var proxyComp = proxyObj.AddComponent<Proxy>() as Proxy;
            proxyComp.copyOrigin = copyOrigin;
            proxyComp.sceneObject = child;
        }
    }

    public void deleteProxyScene()
    {
        foreach (Transform child in proxyOrigin)
            Destroy(child.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            createProxyScene();
        }
        if (Input.GetKeyDown("r"))
        {
            deleteProxyScene();
        }
    }

}
