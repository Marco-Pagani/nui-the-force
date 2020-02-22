using Leap.Unity.Interaction;
using Meta;
using Meta.HandInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCalibration : MonoBehaviour
{
    private readonly string LEAP_NAME = "Leap Rig";
    private readonly string PALM_NAME = "Palm Transform";
    private readonly string LEFT_NAME = "LeapL";
    private readonly string RIGHT_NAME = "LeapR";

    public bool calibrationMode = true;
    public bool applyCalibration = true;

    public GameObject origin;
    public GameObject leapRig;
    public InteractionHand leftHand;
    public InteractionHand rightHand;

    // List of target objects to be grabbed during calibration
    public GrabInteraction[] targets;
    private int targetIndex = -1;

    public Vector3 calibPosition;
    public Vector3 calibRotation;
    public Vector3 calibScale = new Vector3(1, 1, 1);

    // Extra offset to be applied (for Meta vs Leap hand center location)
    public Vector3 offsetPosition;

    private List<Vector3> metaPoints = new List<Vector3>();
    private List<Vector3> leapPoints = new List<Vector3>();

    void Start()
    {
        // Reconcile missing references
        if (leapRig == null)
        {
            leapRig = GameObject.Find(LEAP_NAME);
        }

        // Get targets
        if (targets.Length == 0)
        {
            // Find all children GrabInteractions
            targets = GetComponentsInChildren<GrabInteraction>(true);
        }
        
        // Set up targets
        foreach (GrabInteraction target in targets)
        {
            target.gameObject.SetActive(false);
        }

        // Start calibration
        targetIndex = -1;
        ShowNextTarget();
    }

    private void OnEnable()
    {
        if (!calibrationMode && applyCalibration)
        {
            ApplyCalibration();
        }
    }

    private void ShowNextTarget()
    {
        if (!calibrationMode || targets.Length == 0) return;

        // Hide previous target
        if (targetIndex >= 0 && targetIndex < targets.Length)
        {
            targets[targetIndex].Events.Engaged.RemoveListener(OnGrabbedTarget);
            targets[targetIndex].gameObject.SetActive(false);
        }
        targetIndex++;

        // Show next target
        if (targetIndex < targets.Length)
        {
            targets[targetIndex].gameObject.SetActive(true);
            targets[targetIndex].Events.Engaged.AddListener(OnGrabbedTarget); // Needed to detect when grabbed
        }
        else
        {
            calibrationMode = false;
            ComputeCalibration();
            ApplyCalibration();
        }
    }

    private void OnGrabbedTarget(MetaInteractionData data)
    {
        // Store calibration data
        Hand handData = data.MotionHandFeature.Hand;
        metaPoints.Add(origin.transform.InverseTransformPoint(handData.Palm.Position));
        leapPoints.Add(origin.transform.InverseTransformPoint(handData.HandType == HandType.Left ? leftHand.position : rightHand.position));

        // Prepare the next object
        ShowNextTarget();
    }

    private void ComputeCalibration()
    {
        Vector3 metaCentroid = new Vector3();
        Vector3 leapCentroid = new Vector3();

        Vector3 metaMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 metaMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        Vector3 leapMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 leapMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        // Compute total, min, max
        for (int i = 0; i < metaPoints.Count; i++)
        {
            Vector3 curMeta = metaPoints[i];
            Vector3 curLeap = leapPoints[i];

            metaCentroid += curMeta;
            leapCentroid += curLeap;

            UpdateMin(ref metaMin, curMeta);
            UpdateMax(ref metaMax, curMeta);

            UpdateMin(ref leapMin, curLeap);
            UpdateMax(ref leapMax, curLeap);
        }

        metaCentroid /= metaPoints.Count;
        leapCentroid /= leapPoints.Count;

        Vector3 metaExtent = (metaMax - metaMin);
        Vector3 leapExtent = (leapMax - leapMin);

        // Get offsets
        calibPosition = metaCentroid - leapCentroid;
        //calibRotation = new Vector3(Vector3.SignedAngle(leapCentroid, metaCentroid, Vector3.right), 0, 0);
        calibScale = new Vector3(metaExtent.x / leapExtent.x, metaExtent.y / leapExtent.y, metaExtent.z / leapExtent.z);
    }

    private void UpdateMin(ref Vector3 prev, Vector3 cur)
    {
        prev.x = Math.Min(prev.x, cur.x);
        prev.y = Math.Min(prev.y, cur.y);
        prev.z = Math.Min(prev.z, cur.z);
    }

    private void UpdateMax(ref Vector3 prev, Vector3 cur)
    {
        prev.x = Math.Max(prev.x, cur.x);
        prev.y = Math.Max(prev.y, cur.y);
        prev.z = Math.Max(prev.z, cur.z);
    }

    private void ApplyCalibration()
    {
        if (!applyCalibration) return;

        leapRig.transform.localPosition = calibPosition;
        //leapRig.transform.localRotation = Quaternion.Euler(calibRotation);
        leapRig.transform.localScale = calibScale;
    }
}
