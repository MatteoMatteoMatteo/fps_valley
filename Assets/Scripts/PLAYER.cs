using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER : MonoBehaviour
{
    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12, beamDistance;
    public CharacterController charCon;
    private Vector3 moveInput;
    public Transform camTransform;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;
    private bool canJump;
    private bool canDoubleJump;
    private bool canTripleJump;
    private bool beaming = false;
    private float beamTimer;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    public Animator anim;
    public GameObject bullet;
    public Transform firePoint;

    public WallTransformer wallTransformerScript1;

    public WallTransformer wallTransformerScript2;

    void Start()
    {

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Trap")
        {
            Debug.Log("you dead");
        }
    }

    void Update()
    {
        //Handle player movement with keys
        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horiMove + vertMove;
        moveInput.Normalize();

        //Handle running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * runSpeed;
        }
        //Handle walking
        else
        {
            moveInput = moveInput * moveSpeed;
        }
        //Handle Beaming
        if (Input.GetKeyDown(KeyCode.F) && beaming == false)
        {
            beaming = true;
            beamTimer = 0;
            moveInput = moveInput * beamDistance;
        }
        if (beaming)
        {
            beamTimer += Time.deltaTime;
            Debug.Log("Beam Ability ready in: " + Mathf.RoundToInt(beamTimer));
            if (beamTimer >= 5)
            {
                Debug.Log("You can beam by pressing f");
                beaming = false;
            }
        }

        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        //Handle Gravity/Falling Down
        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        //Handle Jumping
        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;

        if (canJump)
        {
            canDoubleJump = false;
            canTripleJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
        }
        // else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        // {
        //     moveInput.y = jumpPower;
        //     canDoubleJump = false;
        //     canTripleJump = true;
        // }
        // else if (canTripleJump && Input.GetKeyDown(KeyCode.Space))
        // {
        //     moveInput.y = jumpPower;
        //     canTripleJump = false;
        // }

        //Give movement to player
        charCon.Move(moveInput * Time.deltaTime);


        //Control Camera Rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTransform.rotation = Quaternion.Euler(camTransform.transform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        //Handle Shooting
        // if (Input.GetMouseButtonDown(0))
        // {
        //     RaycastHit hit;
        //     if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 50f))
        //     {
        //         if (Vector3.Distance(camTransform.position, hit.point) > 2f)
        //         {
        //             firePoint.LookAt(hit.point);
        //         }
        //     }
        //     else
        //     {
        //         firePoint.LookAt(camTransform.position + (camTransform.forward * 30f));
        //     }
        //     Instantiate(bullet, firePoint.position, firePoint.rotation);
        // }


        //Handle running animation
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);
    }
}
