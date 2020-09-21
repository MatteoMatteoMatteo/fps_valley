using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public GameObject bullet;
    public GameObject weapon;
    public Transform firePoint;
    public Transform playerBody;
    private float _groundDistance = 0.4f;
    private bool _isGrounded;
    public float speed = 12;
    public float gravity = -9.81f;
    public float jumpHeight = 10;
    private Vector3 _velocity;
    private GameObject _grabbedObj;
    public Transform grabPosition;
    private bool _isGrabbing;
    
    void Update()
    {
        Movement();
        HandleMissiles();
        RotateWeapon();
        Grab();
    }

    private void Movement()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, _groundDistance, groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");

        var transform1 = transform;
        var move = transform1.right * x + transform1.forward * z;

        controller.Move(move * (speed * Time.deltaTime));

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);
    }

    private void HandleMissiles()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(playerBody.position, playerBody.forward, out var hit, 500f))
            {
                if (Vector3.Distance(playerBody.position, hit.point) > 2f)
                {
                    firePoint.LookAt(hit.point);
                }
            }
            else
            {
                firePoint.LookAt(playerBody.position + (playerBody.forward * 30f));
            }
            LaunchMissiles();
        }
    }
    
   private void LaunchMissiles()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

    private void RotateWeapon()
    {
        weapon.transform.Rotate(0, 0, 20 * Time.deltaTime);
    }

    private void Grab()
    {
        if (Input.GetMouseButtonDown(1) && Physics.Raycast(playerBody.position, playerBody.forward , out var hit, 500f) && hit.transform.GetComponent<Rigidbody>() && !_isGrabbing)
        {
            _grabbedObj = hit.transform.gameObject;
            _isGrabbing = true;
        }
        else if (Input.GetMouseButtonDown(1) && _isGrabbing)
        {
            _isGrabbing = false;
            _grabbedObj = null;
        }

        if (!_grabbedObj) return;
        if (!(_grabbedObj is null))
            _grabbedObj.GetComponent<Rigidbody>().velocity =
                10 * (grabPosition.position - _grabbedObj.transform.position);
    }
}
