using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToObject : MonoBehaviour
{
	public GameObject root;
	public string name;
    public Vector3 offset = new Vector3(0f, 0.2f, 0f);

    public bool localOffset = false;

    private bool flipXOffset = false;

    private Transform destination;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destination == null)
		{
            if (string.IsNullOrWhiteSpace(name))
            {
                destination = root.transform;
            }
            else
            {
                Transform obj = root.transform.Find(name);
                if (obj != null)
                {
                    destination = obj;
                }
            }
		}
		
		if (destination != null)
		{
            if (localOffset)
            {
                transform.position = destination.TransformPoint(offset);
            }
            else // local-ish offset
            {
                //transform.position = destination.position + offset; // Doesn't work relative to hand
                //transform.position = destination.InverseTransformPoint(offset); // Weird rotation and scaling issues
                transform.position = destination.position;
                transform.position += new Vector3(0, offset.y, 0);
                transform.position += Vector3.ProjectOnPlane(destination.forward, Vector3.up).normalized * offset.z; // Means closer or further from camera
                transform.position += Vector3.ProjectOnPlane(destination.right, Vector3.up).normalized * (flipXOffset ? -offset.x : offset.x); // Medial/lateral position from hand
            }
        }
    }

	public void SetRoot(GameObject newRoot, bool flipX=false)
	{
		root = newRoot;
		destination = null;
        flipXOffset = flipX;
    }
}
