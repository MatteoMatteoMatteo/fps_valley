﻿using System.Collections;
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

    private float groundDistance = 0.4f;
    bool isGrounded;
    public float speed = 12;
    public float gravity = -9.81f;
    public float jumpHeight = 10;
    Vector3 velocity;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        HandleShooting();
        RotateWeapon();
    }

    void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerBody.position, playerBody.forward, out hit, 50f))
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

    void RotateWeapon()
    {
        weapon.transform.Rotate(0, 0, 20 * Time.deltaTime);
    }
}
