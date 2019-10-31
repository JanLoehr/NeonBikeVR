using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    [Header("Params")]
    public float Acceleration = 10;
    public float MaxSteer = 20;
    public float HeadWeightFactor = 10;

    public float UpForceModifier = 10000;

    [Header("Object Refs")]
    public Transform PlayerController;
    public Transform ThrottleLever;

    public Transform HeadWeight;
    public Transform CenterOfMass;

    public WheelCollider FrontWheel;
    public WheelCollider BackWheel;

    private Transform _trans;
    private Rigidbody _rigidBody;

    private bool _isDriving;

    private Transform _grabber;
    private Vector3 _initialThrottleDir;

    private Transform _headTransform;
    private Vector3 _initialHeadPos;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
        _rigidBody = GetComponent<Rigidbody>();
        _headTransform = Camera.main.transform;

        _rigidBody.centerOfMass = CenterOfMass.localPosition;

        _initialHeadPos = _headTransform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float throttle = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");

        if (_isDriving && _initialThrottleDir.y < _grabber.forward.y)
        {
            throttle = (_grabber.forward.y - _initialThrottleDir.y) * Time.fixedDeltaTime * Acceleration;
        }

        BackWheel.motorTorque = Acceleration * throttle * Time.fixedDeltaTime;
        FrontWheel.steerAngle = MaxSteer * steer;
        HeadWeight.localPosition = new Vector3((_headTransform.localPosition.x - _initialHeadPos.x) * HeadWeightFactor, HeadWeight.localPosition.y, HeadWeight.localPosition.z);

        _rigidBody.AddTorque(_trans.forward * -_trans.localRotation.z * UpForceModifier * _rigidBody.velocity.magnitude);
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
