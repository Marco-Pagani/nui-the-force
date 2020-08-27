using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity;

public class HololensGestureManager : MonoBehaviour
{
    private GameObject activeHand;
    public GameObject leftHand;
    public GameObject rightHand;

    public PinchDetector leftPinch;
    public PinchDetector rightPinch;

    public Camera sourceCamera;
    public GameObject targetReticule;
    private GameObject targetObject;

    private bool startPinch = false;
    private bool pinchActive = false;
    private bool raycastHit = false;
    private Vector3 targetStartPos = Vector3.zero;
    private Vector3 handStartPos = Vector3.zero;

    public float movementScaleRate = .1f;
    private float currentScaleFactor = 1;
    private int layerMask = 1 << 11;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pinchActive)
        {
            pinchActive = activeHand == leftHand ? leftPinch.IsPinching : rightPinch.IsPinching;

            if (pinchActive && raycastHit)
            {
                // Move object
                Vector3 movementOffset = activeHand.GetComponent<CapsuleHand>().GetLeapHand().PalmPosition.ToVector3() - handStartPos;
                targetObject.transform.position = targetStartPos + movementOffset * currentScaleFactor;

                // MyLogger.Instance.Log("Move", targetObject.transform.position);
            }
            else
            {
                targetReticule.GetComponent<MeshRenderer>().material.color = Color.green; // .SetActive(true);
                // MyLogger.Instance.Log("EndMove", targetObject.transform.position);
            }
        }
        else
        {
            // Update targetReticule... save targetObject anyways
            RaycastHit hitInfo;
            targetReticule.SetActive(true);
            if (raycastHit = Physics.Raycast(sourceCamera.transform.position, sourceCamera.transform.forward, out hitInfo, Mathf.Infinity, layerMask))
            {
                targetObject = hitInfo.collider.gameObject;
                targetReticule.transform.position = hitInfo.point;
                targetReticule.GetComponent<MeshRenderer>().material.color = Color.green; // .SetActive(true);

                // Check pinch state
                if (leftPinch.IsPinching)
                {
                    activeHand = leftHand;
                    pinchActive = true;
                }
                else if (rightPinch.IsPinching)
                {
                    activeHand = rightHand;
                    pinchActive = true;
                }

                if (pinchActive)
                {
                    targetReticule.SetActive(false);
                    targetStartPos = targetObject.transform.position;
                    handStartPos = activeHand.GetComponent<CapsuleHand>().GetLeapHand().PalmPosition.ToVector3();
                    
                    float distance = Vector3.Distance(sourceCamera.transform.position, targetObject.transform.position);
                    currentScaleFactor = 1 + distance * movementScaleRate;

                    // MyLogger.Instance.Log("StartMove", targetObject.name);
                }
            }
            else
            {
                targetObject = null;
                targetReticule.transform.position = sourceCamera.transform.position + sourceCamera.transform.forward * 5;
                targetReticule.GetComponent<MeshRenderer>().material.color = Color.red;  // SetActive(false);
            }
        }
    }
}
