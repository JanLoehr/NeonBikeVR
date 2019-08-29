using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    [Header("Object Refs")]
    public Transform PlayerController;
    public Transform ThrottleLever;

    private Transform _trans;
    
    private bool _isDriving;
    private Transform _grabber;
    private Vector3 _initialThrottleDir;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDriving && _initialThrottleDir.y < _grabber.forward.y)
        {
            _trans.position += _trans.forward * (_grabber.forward.y - _initialThrottleDir.y) * Time.deltaTime;
        }
    }

    public void ThrottleGrabbed()
    {
        _grabber = ThrottleLever.GetComponent<Grabbable>().Grabber;
        _initialThrottleDir = _grabber.forward;
        PlayerController.parent = transform;

        _isDriving = true;
    }

    public void ThrottleReleased()
    {
        _isDriving = false;
        PlayerController.parent = null;
    }
}
