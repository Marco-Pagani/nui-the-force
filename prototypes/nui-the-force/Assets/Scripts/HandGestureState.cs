using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class HandGestureState : MonoBehaviour
{
	public delegate void OnStateChangedHandler(bool state);
    public event OnStateChangedHandler OnGraspChanged;
    public event OnStateChangedHandler OnPalmChanged;

    public InteractionHand hand;

    public float graspOnThreshold = 0.9f;
    public float graspOffThreshold = 0.5f;
    public float graspPercent = 0;

    public float palmUpThreshold = 80f;
    public float palmDownThreshold = 90f;
    public float palmAngle = 0; // Up: 0 deg, Down: 180 deg (normalized both ways)

    public bool isGrasp = false;
	public bool palmUp = false;


    public void StartGrasp()
	{
        if (hand.isTracked)
        {
            //Debug.Log("GrabStart");
            isGrasp = true;
            OnGraspChanged?.Invoke(true);
        }
    }

	public void EndGrasp()
	{
		//Debug.Log("GrabEnd");
		isGrasp = false;
		OnGraspChanged?.Invoke(false);
    }

    public void StartPalmUp()
    {
        Debug.Log("PalmUp");
        palmUp = true;
        OnPalmChanged?.Invoke(true);
    }

    public void EndPalmUp()
    {
        Debug.Log("PalmDown");
        palmUp = true;
        OnPalmChanged?.Invoke(false);
    }

	public void OnDirectionActivate()
	{
        // This code is leftover from the built-in Leap recognition (which is unreliable)

        /*
		//Debug.Log("PalmUp");
		palmUp = true;
		OnPalmChanged?.Invoke(true);
        */
    }

	public void OnDirectionDeactivate()
	{
        // This code is leftover from the built-in Leap recognition (which is unreliable)

        /*
		//Debug.Log("PalmDown");
		palmUp = false;
        OnPalmChanged?.Invoke(false);
        */
    }

    private void Update() {
		if (!hand.isTracked)
		{
            // Reset grasp (sometimes gets out of sync)
            graspPercent = 0;
            palmAngle = 180;
        }
		else
		{
            // Check for recognition grap and palm up
            graspPercent = Mathf.Min(hand.leapHand.GrabAngle / Mathf.PI, 1);
            palmAngle = 180 - Mathf.Abs(180 - (hand.leapHand.PalmNormal.Roll % 360)); // Normalize to [0, 180]
        }

        if (graspPercent > graspOnThreshold)
        {
            StartGrasp();
        }
        else if (graspPercent < graspOffThreshold)
        {
            EndGrasp();
        }

        if (palmAngle < palmUpThreshold)
        {
            StartPalmUp();
        }
        else if (palmAngle > palmDownThreshold)
        {
            EndPalmUp();
        }
	}
}
