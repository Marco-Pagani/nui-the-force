using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    public GameObject lookAt;

    private Material glowMat;

    // Start is called before the first frame update
    void Start()
    {
        glowMat = GetComponent<MeshRenderer>().material;
        SetOpacity(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(lookAt.transform); // Billboard effect; always face camera
        transform.Rotate(Vector3.right, 90);
    }

    public void SetColor(Color color)
    {
        glowMat.color = color;
    }

    public void SetOpacity(float opacity)
    {
        Color newColor = glowMat.color;
        newColor.a = opacity;
        glowMat.color = newColor;
    }
}
