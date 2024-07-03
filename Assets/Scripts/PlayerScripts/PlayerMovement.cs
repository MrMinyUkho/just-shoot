using UnityEngine;
using System;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [Space(height:10)]
    [SerializeField] private float lookSpeed = 2;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float smoothSpeed = 2;
    
    private Rigidbody _rb;
    private Transform _tr;
    private float _rotationX;
    private Vector3 _normal;
    private bool _onGround;

    private const float SQRT2 = 1.41421356f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _tr = _rb.transform;
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        _rotationX += -Input.GetAxisRaw("Mouse Y") * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 85f);
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxisRaw("Mouse X") * lookSpeed, 0);
    }

    private void FixedUpdate()
    {
        var strafe = Input.GetAxisRaw("Horizontal");
        var accelerate = Input.GetAxisRaw("Vertical");
        var jump = Input.GetAxisRaw("Jump");
        
        var moveDir = (Project(_tr.forward) * accelerate + Project(_tr.right) * strafe).normalized;
        
        if(jump != 0f && _onGround)
        {
            _rb.AddForce(Vector3.up * (jumpForce * 10f), ForceMode.Acceleration);
            _onGround = false;
        }

        
        if(moveDir != Vector3.zero)
            _rb.AddForce(moveDir * (Time.fixedDeltaTime * acceleration * 1000f), ForceMode.Acceleration);
        
        var velocityProjectionOnMovementPlane = Vector3.ProjectOnPlane(_rb.velocity, _normal);
        var speed = velocityProjectionOnMovementPlane.magnitude;
        var k = (Erf(SQRT2 / smoothSpeed * (speed - smoothSpeed * 1.5f - maxSpeed)) + 1)/2;

        if (moveDir == Vector3.zero) k = 0.2f;
        _rb.velocity -= velocityProjectionOnMovementPlane * k;
    }

    private Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal.normalized) * _normal.normalized;
    }

    private void OnCollisionStay(Collision other)
    {
        _onGround = false;
        _normal = Vector3.up;
        var maxDot = float.MinValue;
        foreach (var cnt in other.contacts)
        {
            var dot = Vector3.Dot(Vector3.up, cnt.normal);
            if (!(dot > 0.7f) || !(dot > maxDot)) continue;
            _normal = cnt.normal;
            _onGround = true;
            maxDot = dot;
        }
        _normal = _normal.normalized;
    }

    private void OnCollisionExit(Collision other)
    {
        _normal = Vector3.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + _normal * 3);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward) * 3);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Project(transform.right) * 3);
    }
    
    static float Erf(float x)
    {
        // constants
        const float a1 =  0.254829592f;
        const float a2 = -0.284496736f;
        const float a3 =  1.421413741f;
        const float a4 = -1.453152027f;
        const float a5 =  1.061405429f;
        const float p  =  0.3275911f;
 
        // Save the sign of x
        var sign = 1;
        if (x < 0)
            sign = -1;
        x = Math.Abs(x);
 
        // A&S formula 7.1.26
        var t = 1.0f / (1.0f + p*x);
        var y = 1.0f - (((((a5*t + a4)*t) + a3)*t + a2)*t + a1)*t*Math.Exp(-x*x);
 
        return (float)(sign*y);
    }
}
