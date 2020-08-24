using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;

public class IronmanGesture : MonoBehaviour
{
    // public InteractionHand leftHand;
    // public InteractionHand rightHand;
    // private InteractionHand pinchHand;

    public GameObject indicator;
    private int _layerMask = 1 << 11;
    private bool _debug = false;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        Hand leapH = GetComponent<CapsuleHand>().GetLeapHand();

        if(Physics.Raycast(leapH.PalmPosition.ToVector3(), leapH.PalmNormal.ToVector3(), out hit, Mathf.Infinity, _layerMask))
        {
            if(_debug)
            {
                Debug.DrawRay(leapH.PalmPosition.ToVector3(), leapH.PalmNormal.ToVector3() * hit.distance, Color.green);
                Debug.Log("Hit: " + hit.collider.name + "(" + hit.collider.gameObject.layer + ")");
            }

            indicator.transform.position = leapH.PalmPosition.ToVector3() + leapH.PalmNormal.ToVector3() * hit.distance;
            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            if(_debug)
            {
                Debug.DrawRay(leapH.PalmPosition.ToVector3(), leapH.PalmNormal.ToVector3() * 3, Color.yellow);
            }

            indicator.transform.position = leapH.PalmPosition.ToVector3() + leapH.PalmNormal.ToVector3() * 3;
            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
