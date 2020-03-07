using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class HandGestureManager : MonoBehaviour
{
    private enum ProxyState
    {
        Default,
        Grabbed,
        Flipped
    }

    private ProxyState curState = ProxyState.Default;

	private HandGestureState activeHand;

    public map_to_proxy proxyManager;
    public GameObject copyTo;

	public HandGestureState leftHand;
	public HandGestureState rightHand;

    private Vector3 originalProxyScale = Vector3.one;
    private float minGrabPercent = 0.5f;
    private float maxGrabPercent = 1f;

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

		activeHand = leftHand;

        // Register events for detecting active hand
        leftHand.OnGraspChanged += delegate (bool state)
        {
            if (state)
            {
                SetActiveHand(leftHand);
            }
        };

        rightHand.OnGraspChanged += delegate (bool state)
        {
            if (state)
            {
                SetActiveHand(rightHand);
            }
        };
    }

	public void SetActiveHand(HandGestureState hand)
	{
		if (curState == ProxyState.Default)
		{
            Debug.Log("SetActiveHand: " + (hand == leftHand ? "left" : "right"));
            copyTo.GetComponent<SnapToObject>().SetRoot(hand.hand.gameObject, hand == leftHand);
            activeHand = hand;
        }
	}

	private void EnableHandInteraction(HandGestureState hand)
	{
        if (hand != null)
        {
    		hand.hand.hoverEnabled = true;
    		hand.hand.contactEnabled = true;
    		hand.hand.graspingEnabled = true;
        }
	}

	private void DisableHandInteraction(HandGestureState hand)
	{
        if (hand != null)
        {
            hand.hand.hoverEnabled = false;
            hand.hand.contactEnabled = false;
            hand.hand.graspingEnabled = false;
        }
	}

    private void TransitionState(ProxyState nextState)
    {
        switch (nextState)
        {
            default:
            case ProxyState.Default:
                EnableHandInteraction(activeHand);
                proxyManager.deleteProxyScene();
                copyTo.SetActive(false);
                break;
            case ProxyState.Grabbed:
                proxyManager.createProxyScene(); // TODO Make method specifically for doing all the things
                //proxyManager.castGaze();
                
                originalProxyScale = copyTo.transform.localScale;
                copyTo.transform.localScale = Vector3.zero;
                copyTo.SetActive(true);

                DisableHandInteraction(activeHand);
                break;
            case ProxyState.Flipped:
                //copyTo.SetActive(true);
                copyTo.transform.localScale = originalProxyScale;
                break;
        }

        Debug.Log(nextState + ": " + (activeHand == leftHand ? "left" : "right"));
        curState = nextState;
    }

    private void Update()
    {
		bool isGrasp = activeHand.isGrasp;
		bool palmUp = activeHand.palmUp;

        switch (curState)
        {
            default:
            case ProxyState.Default:
                if (activeHand.hand.isGraspingObject || activeHand.hand.isPrimaryHovering) // Don't interfere with grabbing close objects
                {
                    Debug.Log("Ignore gesture");
                    break;
                }

                if (isGrasp)
                {
                    if (palmUp) { }
                    else
                    {
                        TransitionState(ProxyState.Grabbed);
                    }
                }
                else
                {
                    if (palmUp) { }
                    else { }
                }
                break;
            case ProxyState.Grabbed:
                // Update proxy size based on grasp percentage
                if (palmUp)
                {
                    float proxyScaleFactor = Mathf.Max(Mathf.Min((maxGrabPercent - activeHand.graspPercent) / (maxGrabPercent - minGrabPercent), 1), 0);
                    copyTo.transform.localScale = originalProxyScale * proxyScaleFactor;
                }

				if (isGrasp)
                {
                    if (palmUp) { }
                    else { }
                }
                else
                {
                    if (palmUp)
                    {
                        TransitionState(ProxyState.Flipped);
                    }
                    else
                    {
                        TransitionState(ProxyState.Default);
                    }
                }
                break;
			case ProxyState.Flipped:
                if (isGrasp)
                {
                    if (palmUp)
                    {
                        TransitionState(ProxyState.Default);
                    }
                    else
                    {
                        TransitionState(ProxyState.Default);
                    }
                }
                else
                {
                    if (palmUp) { }
                    else
                    {
                        TransitionState(ProxyState.Default);
                    }
                }
                break;
        }
    }
}
