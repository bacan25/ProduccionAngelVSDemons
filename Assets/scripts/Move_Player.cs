using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Move_Player : MonoBehaviour
{
    [Header("Mov and Jump")]
    Rigidbody rb;
    [SerializeField] float movSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float turnSpeed;
    private float startJumpSpeed;

    float horInput, verInput;
    private Vector2 mouseInput;
    public bool isGrounded = false;
    private int jumpCount = 0;
    public Transform cameraVer;
    public float localRot;


    [Header("Climbing")]
    [SerializeField] private float climbSpeed;
    public bool isClimbing = false;
    public float sphereCastRadius;
    public bool wallFront;
    public LayerMask whatIsWall;
    public Transform climbRef;

    public Inventory inventory;

    [SerializeField] private bool dobleSaltoSkill = false;
    private bool climbSkill = false;

    public bool muerto;


    public ChangeAlpha changeAlpha;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();
        startJumpSpeed = jumpSpeed;

    }

    
    void Update()
    {
        if(muerto)
        {
            return;
        }

        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");

        if (inventory.inventoryOnStage || inventory.instructionsOnStage)
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

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 || Input.GetKeyDown(KeyCode.Space) && jumpCount == 1 && dobleSaltoSkill)
        {
            Jump();
        }

        Debug.Log(jumpCount);
        


        Collider[] colliders = Physics.OverlapSphere(climbRef.position, sphereCastRadius, whatIsWall);
        if (colliders.Length > 0 && Input.GetKey(KeyCode.Space) && climbSkill)
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
        if(jumpCount == 0 && isGrounded || jumpCount == 2)
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
            
            Debug.Log(jumpCount);

        } if (jumpCount == 2)
        {
            
            jumpSpeed = jumpSpeed - ((jumpSpeed/ 2f) / 2f);
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
            jumpSpeed = startJumpSpeed;
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DobleSalto"))
        {
            DobleSalto();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Escalar"))
        {
            Escalar();
            Destroy(other.gameObject);
        }

      
    }

    private void DobleSalto()
    {
        dobleSaltoSkill = true;
        changeAlpha.SetDobleSaltoIcon(1f);
        

    }

    private void Escalar()
    {
        climbSkill = true;
        changeAlpha.SetClimbIcon(1f);


    }

    



}
