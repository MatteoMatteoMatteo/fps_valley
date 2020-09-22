using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform grabHolder;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public GameObject bullet;
    public GameObject weapon;
    public Transform firePoint;
    public Transform playerBody;
    private float _groundDistance = 0.4f;
    private bool _isGrounded;
    public float walkingSpeed = 12f;
    public float sprint = 20f;
    private bool _isSprinting = false;
    private float _dashingPower=1;
    public float dashingPower;
    private bool _isDashing = false;
    public float gravity = -30f;
    public float jumpHeight = 10;
    private Vector3 _velocity;
    private GameObject _grabbedObj;
    public Transform grabPosition;
    private bool _isGrabbing;
    private Vector3 hookShotPosition;
    public float hookShotSpeedMax;
    public GameObject speedParticles;
    public GameObject dashParticles;
    private float _savedGravity;
    private State state;
    private enum  State
    {
        Normal,
        HookshotFlyingPlayer
    }

    private void Awake()
    {
        _savedGravity = gravity;
        state = State.Normal;
    }

    void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                HandleHookShotStart();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
        }
        Movement();
        HandleMissiles();
        RotateWeapon();
        Grab();
        HandleHookShotStart();
    }

    private void Movement()
    {
        //Basic Movement
        _isGrounded = Physics.CheckSphere(groundCheck.position, _groundDistance, groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");
        var transform1 = transform;
        var move = transform1.right * x + transform1.forward * z;
        
        //Dashing
        if (_isDashing)
        {
            _dashingPower -= _dashingPower * 1.5f * Time.deltaTime;
            dashParticles.SetActive(true);
            if (_dashingPower < 1f)
            {
                _isDashing = false;
                _dashingPower = 1f;
                dashParticles.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab) && state == State.Normal)
        {
            _dashingPower = dashingPower;
            _isDashing = true;
        }

        //Walking & Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && state == State.Normal)
        {
            controller.Move(move * (sprint * Time.deltaTime));
            speedParticles.SetActive(true);
            _isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            speedParticles.SetActive(false);
            _isSprinting = false;
        }
        else
        {
            controller.Move(move * (walkingSpeed * _dashingPower * Time.deltaTime)); 
        }
        
        //Jumping & Gravity
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
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }

    private void RotateWeapon()
    {
        weapon.transform.Rotate(0, 0, 20 * Time.deltaTime);
    }

    private void Grab()
    {
        if (Input.GetMouseButtonDown(1) && Physics.Raycast(playerBody.position, playerBody.forward , out var hit) && hit.transform.GetComponent<Rigidbody>() && !_isGrabbing)
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

    private void HandleHookShotStart()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_isSprinting)
        {
            DisableGravity();

            if (Physics.Raycast(playerBody.transform.position, playerBody.transform.forward, out RaycastHit raycastHit))
            {
                grabHolder.position = raycastHit.point - new Vector3(0,2,0);
                hookShotPosition = raycastHit.point;
                state = State.HookshotFlyingPlayer;
            }
        }
    }

    private void HandleHookshotMovement()
    {
        var position = transform.position;
        float hookShotSpeedMin = 20f;
        float hookShotSpeed = Mathf.Clamp(Vector3.Distance(position,hookShotPosition), hookShotSpeedMin, hookShotSpeedMax);
        float hookshotSpeedMultiplier = 2f;
        
        Vector3 hookShotDir = (hookShotPosition - position).normalized;
        controller.Move(hookShotDir * (hookShotSpeed * hookshotSpeedMultiplier * Time.deltaTime));
        
        if (Vector3.Distance(position, hookShotPosition) > 3f)
        {
            speedParticles.SetActive(true);
        }
        else
        {
            speedParticles.SetActive(false);  
        }

        
        if(Input.GetKeyUp(KeyCode.E))
        {
            state = State.Normal;
            EnableGravity();
            speedParticles.SetActive(false); 
        }
    }

    private void DisableGravity()
    {
        gravity = 0;
        _velocity.y = 0;
    }
    private void EnableGravity()
    {
        gravity = _savedGravity;
    }
}
