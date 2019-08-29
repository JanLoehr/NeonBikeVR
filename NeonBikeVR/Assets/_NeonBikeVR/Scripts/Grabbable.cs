using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : OVRGrabbable
{
    [Header("Parameters")]
    public bool LockPosition;
    public Vector3 LockRotationAxes;

    [Header("Events")]
    public UnityEvent GrabStart;
    public UnityEvent GrabStop;

    public Transform Grabber { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;

        Grabber = m_grabbedBy.transform;

        if (m_grabbedBy is CustomGrabber grabber)
        {
            if (LockPosition)
            {
                grabber.LockPosition();

                grabber.SetDontMoveGrabbable();
            }

            if (LockRotationAxes.magnitude > 1)
            {
                grabber.LockRotationAxes(LockRotationAxes.x > 0 ? true : false,
                                            LockRotationAxes.y > 0 ? true : false,
                                            LockRotationAxes.z > 0 ? true : false);
            }

            GrabStart.Invoke();
        }
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        if (m_grabbedBy is CustomGrabber grabber)
        {
            grabber.ReleasePosition();
            grabber.ReleaseRotationAxes();

            GrabStop.Invoke();
        }

        Grabber = null;
        
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }
}
