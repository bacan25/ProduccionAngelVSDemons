using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

    public Inventory inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();

    }

    
    void Update()
    {
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");

        if (inventory.inventoryOnStage)
        {
            mouseInput = new Vector2(0, 0);


        } else 
        {
            mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        

        if (isClimbing)
        {
            Climb();
        } else
        {
            Walk();
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 || Input.GetKeyDown(KeyCode.Space) && jumpCount == 1)
        {
            Jump();
        }

        


        Collider[] colliders = Physics.OverlapSphere(climbRef.position, sphereCastRadius, whatIsWall);
        if (colliders.Length > 0 && Input.GetKey(KeyCode.Space))
        {
            wallFront = true;
        } else
        {
            wallFront= false;
        }

        if (wallFront)
        {
            isClimbing = !isClimbing;
        }

        if (!wallFront) 
        {
            isClimbing = false;
        } 

        localRot -= mouseInput.y * turnSpeed * Time.deltaTime * 2;
        localRot = Mathf.Clamp(localRot, -50f, 80f);
        cameraVer.localEulerAngles = Vector3.right * localRot;

        

    }

    public void Walk()
    {
        if(jumpCount == 0 || jumpCount == 2)
        {
            rb.velocity = transform.rotation * new Vector3
            (
            horInput * movSpeed,
            rb.velocity.y,
            wallFront ? Mathf.Clamp(verInput * movSpeed, -1000, 0) : verInput * movSpeed
            );
            
        }

        transform.Rotate(0, mouseInput.x * turnSpeed * Time.deltaTime, 0);




    }

    public void Climb()
    {
        rb.velocity = transform.rotation * new Vector3(horInput * movSpeed, verInput * movSpeed, 0);
        transform.Rotate(0, mouseInput.x * turnSpeed * Time.deltaTime, 0);
        jumpCount = 0;

    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        jumpCount++;
       

        if (jumpCount == 1)
        {
            
            jumpSpeed = 10f;
            Debug.Log(jumpCount);

        } if (jumpCount == 2)
        {
            
            jumpSpeed = 5f;
            Debug.Log(jumpCount);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(climbRef.position, sphereCastRadius);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.tag == "Ground")
        {
            isGrounded = true;
            jumpCount = 0;
            jumpSpeed = 5f;
        }


    }

}
