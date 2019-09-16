using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableStatic : OVRGrabbable
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
    private Quaternion _oldRotation;

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_grabbedBy != null)
        {
            Quaternion rotation = OVRInput.GetLocalControllerRotation((m_grabbedBy as CustomGrabber).Controller);

            m_grabbedBy.transform.localRotation = Quaternion.Euler(rotation.eulerAngles.x * (1 - LockRotationAxes.x),
                                                                rotation.eulerAngles.y * (1 - LockRotationAxes.y),
                                                                rotation.eulerAngles.z * (1 - LockRotationAxes.z));
        }
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;

        if (LockPosition)
        {
            _oldRotation = hand.transform.localRotation;
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
            m_grabbedBy.transform.localPosition = Vector3.zero;
            m_grabbedBy.transform.localRotation = _oldRotation;
        }

        GrabStop.Invoke(m_grabbedBy);

        _oldGrabberParent = null;
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }
}

[System.Serializable]
public class OVRGrabberEvent : UnityEvent<OVRGrabber> { }
