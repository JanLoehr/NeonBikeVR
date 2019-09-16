using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrabber : OVRGrabber
{
    [SerializeField]
    private bool _updatePosition;
    [SerializeField]
    private bool _updateRotation;

    public OVRInput.Controller Controller => m_controller;

    private Vector3 _lockedRotationAxes;

    private Quaternion _lastHandRot;

    private bool _moveGrabbable = true;

    protected override void Awake()
    {
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;

        // If we are being used with an OVRCameraRig, let it drive input updates, which may come from Update or FixedUpdate.

        OVRCameraRig rig = null;
        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.parent != null)
            rig = transform.parent.parent.parent.GetComponent<OVRCameraRig>();

        if (rig != null)
        {
            operatingWithoutOVRCameraRig = false;
        }
    }

    private void FixedUpdate()
    {
        CheckGrabState();
    }

    public void SetDontMoveGrabbable()
    {
        _moveGrabbable = false;
    }

    public void LockPosition()
    {
        _updatePosition = false;
    }

    public void ReleasePosition()
    {
        _updatePosition = true;
    }

    public void LockRotationAxes(bool x, bool y, bool z)
    {
        _lockedRotationAxes = new Vector3(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0);
    }

    public void ReleaseRotationAxes()
    {
        _lockedRotationAxes = Vector3.zero;
    }

    private void CheckGrabState()
    {
        float prevFlex = m_prevFlex;
        // Update values from inputs
        m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);

        CheckForGrabOrRelease(prevFlex);

        if (m_grabbedObj == null)
        {
            _moveGrabbable = true;
        }
    }
}
