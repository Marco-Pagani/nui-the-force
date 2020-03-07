﻿using System.Collections;
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

	public void OnDirectionActivate()
	{
		//Debug.Log("PalmUp");
		palmUp = true;
		OnPalmChanged?.Invoke(true);
    }

	public void OnDirectionDeactivate()
	{
		//Debug.Log("PalmDown");
		palmUp = false;
        OnPalmChanged?.Invoke(false);
	}

	private void Update() {
		if (!hand.isTracked)
		{
            // Reset grasp (sometimes gets out of sync)
            graspPercent = 0;
        }
		else
		{
            // New method to check for hand grasp
            graspPercent = Mathf.Min(hand.leapHand.GrabAngle / Mathf.PI, 1);
        }

        if (graspPercent > graspOnThreshold)
        {
            StartGrasp();
        }
        else if (graspPercent < graspOffThreshold)
        {
            EndGrasp();
        }
	}
}
