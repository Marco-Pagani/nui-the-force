using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity;

public class HololensGestureManager : MonoBehaviour
{
    private InteractionHand activeHand;
    public InteractionHand leftHand;
    public InteractionHand rightHand;

    public PinchDetector leftPinch;
    public PinchDetector rightPinch;

    public Camera sourceCamera;
    public GameObject targetReticule;
    private GameObject targetObject;

    private bool pinchActive = false;
    private Vector3 targetStartPos = Vector3.zero;
    private Vector3 handStartPos = Vector3.zero;

    public float movementScaleRate = .1f;
    private float currentScaleFactor = 1;

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
            
            if (pinchActive)
            {
                // Move object
                Vector3 movementOffset = activeHand.transform.position - handStartPos;
                targetObject.transform.position = targetStartPos + movementOffset * currentScaleFactor;
            }
            else
            {
                targetReticule.SetActive(true);
            }
        }
        else
        {
            // Update targetReticule... save targetObject anyways
            RaycastHit hitInfo;
            if (Physics.Raycast(sourceCamera.transform.position, sourceCamera.transform.forward, out hitInfo))
            {
                targetObject = hitInfo.collider.gameObject;
                targetReticule.transform.position = hitInfo.point;
                targetReticule.SetActive(true);
            }
            else
            {
                targetObject = null;
                targetReticule.SetActive(false);
            }

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
                handStartPos = activeHand.transform.position;
                
                float distance = Vector3.Distance(sourceCamera.transform.position, targetObject.transform.position);
                currentScaleFactor = 1 + distance * movementScaleRate;
            }
        }
    }
}
