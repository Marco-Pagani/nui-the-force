﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class map_to_proxy : MonoBehaviour
{

    //these variable names could be better

    //the look direction of the meta headset
    public Transform gaze;

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

    // max distance from the copy origin that objects will be copied
    public float renderDistance = 10f;

    // layermask for raycasting
    public LayerMask mask;

    public Shader miniObjectShader;


    //this should be called when the user reaches for an object
    public bool castGaze()
    {
        RaycastHit hit;

        if (Physics.SphereCast(gaze.position, 1f, gaze.TransformDirection(Vector3.forward), out hit, Mathf.Infinity /*,mask.value*/ ))
        {
            //Debug.DrawRay(gaze.position, gaze.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            copyOrigin.transform.position = hit.point;
            createProxyScene();
            return true;
        }
        else
            return false;
    }


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
            if (c.transform != null && c.transform.IsChildOf(sceneObjects))
            {
                var child = c.transform;
                //create clone
                GameObject proxyObj = Instantiate(child, proxyOrigin).gameObject;
                //set clone's position relative to copy origin
                proxyObj.transform.localPosition = copyOrigin.InverseTransformPoint(child.position);
                proxyObj.transform.localRotation = child.localRotation;
                //set shader of proxy object
                proxyObj.GetComponent<Renderer>().material.shader = miniObjectShader;
                //add proxy component to clone and set vars
                var proxyComp = proxyObj.AddComponent<Proxy>() as Proxy;
                proxyComp.copyOrigin = copyOrigin;
                proxyComp.sceneObject = child;
            }
        }
    }

    public void deleteProxyScene()
    {
        foreach (Transform child in proxyOrigin)
            Destroy(child.gameObject);
    }

}
