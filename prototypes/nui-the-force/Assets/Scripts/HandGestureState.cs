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

    public float palmUpThreshold = 75f;
    public float palmDownThreshold = 105f;
    public float palmAngle = 0; // Up: 0 deg, Down: 180 deg (normalized both ways)

    public bool isGrasp = false;
	public bool palmUp = false;

    public Camera leapCamera;
    public float outOfRangeThreshold = .95f;

    public void StartGrasp()
	{
        if (hand.isTracked)
        {
            //// Debug.Log("GrabStart");
            isGrasp = true;
            OnGraspChanged?.Invoke(true);
        }
    }

	public void EndGrasp()
	{
		//// Debug.Log("GrabEnd");
		isGrasp = false;
		OnGraspChanged?.Invoke(false);
    }

    public void StartPalmUp()
    {
        // Debug.Log("PalmUp");
        palmUp = true;
        OnPalmChanged?.Invoke(true);
    }

    public void EndPalmUp()
    {
        // Debug.Log("PalmDown");
        palmUp = false;
        OnPalmChanged?.Invoke(false);
    }

	public void OnDirectionActivate()
	{
        // // This code is leftover from the built-in Leap recognition (which is unreliable)

		// //// Debug.Log("PalmUp");
		// palmUp = true;
		// OnPalmChanged?.Invoke(true);
    }

	public void OnDirectionDeactivate()
	{
        // // This code is leftover from the built-in Leap recognition (which is unreliable)

		// //// Debug.Log("PalmDown");
		// palmUp = false;
        // OnPalmChanged?.Invoke(false);
    }

    private void Update() {
		if (!hand.isTracked)
		{
            // graspPercent = 0;

            // Check if hand is leaving frame or is being occluded
            if (leapCamera != null)
            {
                Vector3 viewPos = leapCamera.WorldToViewportPoint(hand.position);
                if (Mathf.Abs(viewPos.x) > outOfRangeThreshold || Mathf.Abs(viewPos.y) > outOfRangeThreshold)
                {
                    // Hand just went out of range
                    graspPercent = 0;
                    palmAngle = 180;
                }
                else
                {
                    // Hand was likely occluded
                }
            }
            else
            {
                // Reset grasp (sometimes gets out of sync)
                //graspPercent = 0;
                //palmAngle = 180;

                // TODO Check if it's actually fine to not reset these values
            }
        }
		else
		{
            // Check for recognition grap and palm up
            graspPercent = Mathf.Min(hand.leapHand.GrabAngle / Mathf.PI, 1);
            float x = hand.leapHand.PalmNormal.Roll * (180 / Mathf.PI);
            palmAngle = Mathf.Abs(180 - (Mathf.Abs(x) % 360)); // Normalize to [0, 180]
        }

        if (graspPercent > graspOnThreshold)
        {
            StartGrasp();
        }
        else if (graspPercent < graspOffThreshold)
        {
            EndGrasp();
        }

        // // Debug.Log("Palm Angle:\t" + palmAngle);
        if (Mathf.Abs(palmAngle) < palmUpThreshold)
        {
            StartPalmUp();
        }
        else if (palmAngle > palmDownThreshold)
        {
            EndPalmUp();
        }
	}
}
