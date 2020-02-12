using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proxy : MonoBehaviour
{

    public Transform target;
    public float scale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target.localPosition = gameObject.transform.localPosition * (1/scale);
        
    }
}
