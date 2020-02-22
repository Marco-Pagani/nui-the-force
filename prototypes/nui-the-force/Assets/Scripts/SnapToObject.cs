using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToObject : MonoBehaviour
{
	public GameObject root;
	public string name;
    public Vector3 offset = new Vector3(0f, 0.2f, 0f);
	
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
			transform.position = destination.position + offset;
		}
    }
}
