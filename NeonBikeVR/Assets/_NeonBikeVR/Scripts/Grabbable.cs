using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : OVRGrabbable
{
    [Header("Lock Hand to this")]

    [Header("Parameters")]
    public bool LockPosition;
    [Tooltip("Put 1 in the axis to lock, zero to not locked")]
    public Vector3 LockRotationAxes;

    [Header("Events")]
    public OVRGrabberEvent GrabStart;
    public OVRGrabberEvent GrabStop;

    private Transform _oldGrabberParent;

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

        if (LockPosition)
        {
            _oldGrabberParent = hand.transform.parent;
            hand.transform.SetParent(transform.parent);
        }

        if (LockRotationAxes.magnitude > 1)
        {

        }

        GrabStart.Invoke(m_grabbedBy);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        if (_oldGrabberParent)
        {
            m_grabbedBy.transform.SetParent(_oldGrabberParent);
        }

        GrabStop.Invoke(m_grabbedBy);

        _oldGrabberParent = null;
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }
}

[System.Serializable]
public class OVRGrabberEvent : UnityEvent<OVRGrabber> { }
