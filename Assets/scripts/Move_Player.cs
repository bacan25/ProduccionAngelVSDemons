using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    [Header("Mov and Jump")]
    Rigidbody rb;
    [SerializeField] float movSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float turnSpeed = 100f;

    float horInput, verInput;
    private Vector2 mouseInput;
    public bool isGrounded = false;
    private int jumpCount = 0;
    public Transform cameraVer;
    public float localRot;


    [Header("Climbing")]
    [SerializeField] float climbSpeed = 5f;
    public bool isClimbing = false;
    public float sphereCastRadius;
    public bool wallFront;
    public LayerMask whatIsWall;
    public Transform climbRef;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (isClimbing)
        {
            Climb();
        } else
        {
            Walk();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        Collider[] colliders = Physics.OverlapSphere(climbRef.position, sphereCastRadius, whatIsWall);
        if (colliders.Length > 0)
        {
            wallFront = true;
        } else
        {
            wallFront= false;
        }


        if (!wallFront) 
        {
            isClimbing = false;
        } 

        localRot -= mouseInput.y * turnSpeed * Time.deltaTime * 2;
        localRot = Mathf.Clamp(localRot, -30f, 80f);
        cameraVer.localEulerAngles = Vector3.right * localRot;

    }

    public void Walk()
    {
        rb.velocity =transform.rotation  * new Vector3
            (
            horInput * movSpeed, 
            rb.velocity.y,
            wallFront? Mathf.Clamp(verInput * movSpeed, -1000, 0): verInput * movSpeed
            );
        transform.Rotate(0, mouseInput.x * turnSpeed * Time.deltaTime, 0);

    }

    public void Climb()
    {
        rb.velocity = transform.rotation * new Vector3(horInput * movSpeed, verInput * movSpeed, 0);
        transform.Rotate(0, mouseInput.x * turnSpeed * Time.deltaTime, 0);

    }

    public void Jump()
    {

        if (wallFront)
        {
            isClimbing = !isClimbing;
        }
        rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(climbRef.position, sphereCastRadius);
    }

}
