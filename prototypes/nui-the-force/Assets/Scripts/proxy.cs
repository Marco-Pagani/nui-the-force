using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proxy : MonoBehaviour {

    public Transform copyOrigin; // the point from which the proxy scene was centered
    public Transform sceneObject; // the object that this proxy represents

    void Start () {

    }

    void Update () {
        updatePosition (); //for debugging
    }

    //call this function when the proxy is moved to update the real object
    public void updatePosition () {
        sceneObject.position = gameObject.transform.localPosition + copyOrigin.position;
        sceneObject.localRotation = gameObject.transform.localRotation;

    }
}