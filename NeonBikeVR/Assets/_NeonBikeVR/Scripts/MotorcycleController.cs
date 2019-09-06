using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    [Header("Params")]
    public float SpeedMulti = 10;
    public float RotMulti = 5;

    [Header("Object Refs")]
    public Transform PlayerController;
    public Transform ThrottleLever;

    private Transform _trans;
    
    private bool _isDriving;
    
    private Transform _grabber;
    private Vector3 _initialThrottleDir;

    private Transform _headTransform;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
        _headTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isDriving && _initialThrottleDir.y < _grabber.forward.y)
        {
            _trans.position += _trans.forward * (_grabber.forward.y - _initialThrottleDir.y) * Time.deltaTime * SpeedMulti;

            _trans.Rotate(Vector3.up, _headTransform.localPosition.x * RotMulti);
        }
    }

    public void ThrottleGrabbed(OVRGrabber hand)
    {
        _grabber = hand.transform;

        _initialThrottleDir = _grabber.forward;

        _isDriving = true;
    }

    public void ThrottleReleased(OVRGrabber hand)
    {
        _grabber = null;
        _isDriving = false;
    }
}
