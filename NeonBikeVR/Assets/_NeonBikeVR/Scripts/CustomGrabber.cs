using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrabber : OVRGrabber
{
    [SerializeField]
    private bool _updatePosition;
    [SerializeField]
    private bool _updateRotation;

    private Vector3 _lockedRotationAxes;

    private Quaternion _lastHandRot;

    private bool _moveGrabbable = true;

    private void FixedUpdate()
    {
        //UpdatePositionAndRotation();

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

    // Hands follow the touch anchors by calling MovePosition each frame to reach the anchor.
    // This is done instead of parenting to achieve workable physics. If you don't require physics on
    // your hands or held objects, you may wish to switch to parenting.
    private void UpdatePositionAndRotation()
    {
        Vector3 destPos = m_lastPos;
        if (_updatePosition && operatingWithoutOVRCameraRig)
        {
            Vector3 handPos = OVRInput.GetLocalControllerPosition(m_controller);
            destPos = m_parentTransform.TransformPoint(m_anchorOffsetPosition + handPos);
            GetComponent<Rigidbody>().MovePosition(destPos);
        }

        Quaternion destRot = m_lastRot;
        if (_updateRotation)
        {
            Quaternion handRot = OVRInput.GetLocalControllerRotation(m_controller);

            //lock axes according lock
            handRot = Quaternion.Euler(_lockedRotationAxes.x == 1 ? _lastHandRot.eulerAngles.x : handRot.eulerAngles.x,
                _lockedRotationAxes.y == 1 ? _lastHandRot.eulerAngles.y : handRot.eulerAngles.y,
                _lockedRotationAxes.z == 1 ? _lastHandRot.eulerAngles.z : handRot.eulerAngles.z);

            destRot = m_parentTransform.rotation * handRot * m_anchorOffsetRotation;
            GetComponent<Rigidbody>().MoveRotation(destRot);

            _lastHandRot = handRot;
        }

        if (!m_parentHeldObject && _moveGrabbable)
        {
            MoveGrabbedObject(destPos, destRot);
        }
        m_lastPos = transform.position;
        m_lastRot = transform.rotation;
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
