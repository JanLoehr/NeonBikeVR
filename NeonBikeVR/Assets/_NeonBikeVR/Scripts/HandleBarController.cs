using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBarController : OVRGrabbable
{
    public Transform Motorcycle;
    public float SpeedModifier;

    public bool IsThrottleLever;

    private Transform _grabbingHand;
    private Vector3 _initialRotation;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsThrottleLever && _grabbingHand != null && _grabbingHand.forward.y > _initialRotation.y)
        {
            Debug.Log(_initialRotation + " " + _grabbingHand.localEulerAngles.x);
            Motorcycle.position += Motorcycle.forward * (_grabbingHand.forward.x - _initialRotation.y) * SpeedModifier * Time.deltaTime;
        }
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;

        if (m_grabbedBy is CustomGrabber grabber)
        {
            grabber.LockPosition();
            grabber.LockRotationAxes(false, true, true);

            grabber.SetDontMoveGrabbable();

            _grabbingHand = grabber.transform;
            _initialRotation = _grabbingHand.forward;
        }
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        if (m_grabbedBy is CustomGrabber grabber)
        {
            grabber.ReleasePosition();
            grabber.ReleaseRotationAxes();

            _grabbingHand = null;
        }

        m_grabbedBy = null;
        m_grabbedCollider = null;
    }
}
