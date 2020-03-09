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

    private GlowEffect activeHandGlow;

    public GlowEffect leftHandGlow;

    public GlowEffect rightHandGlow;

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
            activeHandGlow = hand == leftHand ? leftHandGlow : rightHandGlow;
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

    private void SetHandGlow(Color color, GlowEffect handGlow=null)
    {
        if (handGlow == null)
        {
            handGlow = activeHandGlow;
        }

        if (handGlow)
        {
            handGlow.SetColor(color);
        }
    }

    private float GetBoundedPercent(float percent, float min=0, float max=1, float outMin=0, float outMax=1)
    {
        return Mathf.Max(Mathf.Min((percent - min) / (max - min), outMax), outMin);
    }

    private void SetHandGlow(float opacity, GlowEffect handGlow=null)
    {
        if (handGlow == null)
        {
            handGlow = activeHandGlow;
        }

        if (handGlow)
        {
            handGlow.SetOpacity(opacity);
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

                SetHandGlow(Color.white, leftHandGlow);
                SetHandGlow(Color.white, rightHandGlow);
                SetHandGlow(0, leftHandGlow);
                SetHandGlow(0, rightHandGlow);

                break;
            case ProxyState.Grabbed:
                if (proxyManager.castGaze())
                {
                    // Prepare proxy (really tiny right now)
                    originalProxyScale = copyTo.transform.localScale;
                    copyTo.transform.localScale = Vector3.zero;
                    copyTo.SetActive(true);

                    // Reset and prepare glows
                    SetHandGlow(0, leftHandGlow);
                    SetHandGlow(0, rightHandGlow);

                    SetHandGlow(Color.cyan);
                    SetHandGlow(1);

                    DisableHandInteraction(activeHand);
                    Debug.Log("Yea proxy");
                }
                else
                {
                    Debug.Log("No proxy");
                    nextState = ProxyState.Default; // Force quit the gesture recognition
                }
                break;
            case ProxyState.Flipped:
                //copyTo.SetActive(true); // Now we grow and glow instead
                copyTo.transform.localScale = originalProxyScale;
                SetHandGlow(0);
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
                // Update hand glows
                SetHandGlow(leftHand.palmUp ? 0 : GetBoundedPercent(leftHand.graspPercent, 0.15f, 1f), leftHandGlow);
                SetHandGlow(rightHand.palmUp ? 0 : GetBoundedPercent(rightHand.graspPercent, 0.15f, 1f), rightHandGlow);

                // Check for conflicting interaction
                if (activeHand.hand.isGraspingObject || activeHand.hand.isPrimaryHovering) // Don't interfere with grabbing close objects
                {
                    break;
                }

                // Check transition conditions
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
                    //float proxyScaleFactor = Mathf.Max(Mathf.Min((maxGrabPercent - activeHand.graspPercent) / (maxGrabPercent - minGrabPercent), 1), 0);
                    float proxyScaleFactor = 1 - GetBoundedPercent(activeHand.graspPercent, minGrabPercent, maxGrabPercent);
                    Debug.Log(activeHand.graspPercent + " | " + proxyScaleFactor);
                    copyTo.transform.localScale = originalProxyScale * proxyScaleFactor;

                    // Also update glow opacity
                    SetHandGlow(Mathf.Max(1 - proxyScaleFactor * 1.5f, 0));
                }

                // Check transition conditions
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
                // Check transition conditions
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
