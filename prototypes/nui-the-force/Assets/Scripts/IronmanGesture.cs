using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;

public class IronmanGesture : MonoBehaviour
{
    public GameObject indicator;
    public GameObject leftH;
    public GameObject rightH;

    private GameObject _activeH = null;
    private int _layerMask = 1 << 11;
    private bool _raycastHit = false, _debug = false;

    private Collider targetCollider = null;
    private float _scaleFactor = 2f, _distFromPalm = 0f;
    private Vector3 _startPalmPos, _startTargetPos, _offset;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if(_activeH != null)
        {
            Hand leapH = _activeH.GetComponent<CapsuleHand>().GetLeapHand();
            if(leapH.IsPinching() && _raycastHit)
            {
                if(_debug)
                {
                    Debug.Log("Target Start Position:\t" + _startTargetPos);
                    Debug.Log("Target Current Position:\t" + hit.collider.gameObject.transform.position);
                }

                indicator.SetActive(false);
                _offset = leapH.PalmPosition.ToVector3() - _startPalmPos;
                hit.collider.gameObject.transform.position = _startTargetPos + _offset * _scaleFactor;
            }
            else
            {
                indicator.SetActive(true);
                if (_raycastHit = Physics.Raycast(leapH.PalmPosition.ToVector3(), leapH.PalmNormal.ToVector3(), out hit, Mathf.Infinity, _layerMask))
                {
                    if (_debug)
                    {
                        Debug.DrawRay(leapH.PalmPosition.ToVector3(), leapH.PalmNormal.ToVector3() * hit.distance, Color.green);
                        Debug.Log("Hit: " + hit.collider.name + "(" + hit.collider.gameObject.layer + ")");
                    }

                    indicator.transform.position = leapH.PalmPosition.ToVector3() + leapH.PalmNormal.ToVector3() * hit.distance;
                    indicator.GetComponent<MeshRenderer>().material.color = Color.green;

                    if(leapH.IsPinching())
                    {
                        _startPalmPos = leapH.PalmPosition.ToVector3();
                        _startTargetPos = leapH.PalmPosition.ToVector3() + leapH.PalmNormal.ToVector3() * hit.distance;
                        _scaleFactor = 1 + hit.distance * 1f;
                    }
                }
                else
                {
                    if (_debug)
                    {
                        Debug.DrawRay(leapH.PalmPosition.ToVector3(), leapH.PalmNormal.ToVector3() * 3, Color.yellow);
                    }

                    indicator.transform.position = leapH.PalmPosition.ToVector3() + leapH.PalmNormal.ToVector3() * 3;
                    indicator.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
        else
        {
            if(leftH.activeSelf) _activeH = leftH;
            else if(rightH.activeSelf) _activeH = rightH;
            else _activeH = null;
        }        
    }
}
