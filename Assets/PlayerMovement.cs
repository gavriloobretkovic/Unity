using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed; //velocit�
    public float walkSpeed;
    public float sprintSpeed;
    public Transform orientation; //orientamento

    public float altezza;
    public float attrito;
    public LayerMask pavimento;
    bool aTerra;

    public float jumpForce;
    public float jumpCooldown;
    public float airMul;  //cosa in più, resistenza dell'aria
    private bool readyToJump = true;

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    private KeyCode JumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    float InputOri;
    float InputVer;

    Vector3 Direzione;

    public MovementState state;

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }

    private void MyInput()
    {
        InputOri = Input.GetAxisRaw("Horizontal");
        InputVer = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(JumpKey) && readyToJump && aTerra)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void SpeedControl()
    {
        Vector3 limitVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (limitVel.magnitude > movementSpeed)
        {
            Vector3 newVel = limitVel.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(newVel.x, rb.linearVelocity.y, newVel.z);
        }
    }

    private void StateHandler()
    {
        if (aTerra && Input.GetKey(SprintKey))
        {
            state = MovementState.sprinting;
            movementSpeed = sprintSpeed;
        }
        else if (aTerra)
        {
            state = MovementState.walking;
            movementSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }

        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            movementSpeed = crouchSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        aTerra = Physics.Raycast(transform.position, Vector3.down, altezza * 0.7f, pavimento);
        MyInput();
        SpeedControl();
        StateHandler();
        if (aTerra)
        {
            rb.linearDamping = attrito;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void MovePlayer()
    {
        Direzione = orientation.forward * InputVer + orientation.right * InputOri;
        if (aTerra)
        {
            rb.AddForce(Direzione.normalized * movementSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(Direzione.normalized * movementSpeed * airMul, ForceMode.Force);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
