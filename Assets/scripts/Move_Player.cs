using UnityEngine;
using Photon.Pun;

public class Move_Player : MonoBehaviourPun
{
    [Header("Movement and Jump")]
    Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;

    private float horizontalInput, verticalInput;
    private Vector2 mouseInput;
    public bool isGrounded = false;
    private int jumpCount = 0;
    public Transform cameraTransform;
    public float verticalRotation;

    [Header("Abilities")]
    [SerializeField] private bool doubleJumpAbility = false;
    private bool climbAbility = false;

    public bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpForce = 10f; // Set default jump force, adjust in Unity Inspector
    }

    void Update()
    {
        if (!photonView.IsMine || isDead) return;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        RotatePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && (jumpCount == 0 || (jumpCount == 1 && doubleJumpAbility)))
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        if (isGrounded || jumpCount < 2)
        {
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + transform.TransformDirection(movement));
        }
    }

    private void RotatePlayer()
    {
        transform.Rotate(0, mouseInput.x * rotationSpeed * Time.deltaTime, 0);
        verticalRotation -= mouseInput.y * rotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);

        if (cameraTransform != null)
        {
            cameraTransform.localEulerAngles = Vector3.right * verticalRotation;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        jumpCount++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DoubleJump"))
        {
            ActivateDoubleJump();
            PhotonView photonViewObj = other.GetComponent<PhotonView>();
            if (photonViewObj != null && photonViewObj.IsMine)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Climb"))
        {
            ActivateClimb();
            PhotonView photonViewObj = other.GetComponent<PhotonView>();
            if (photonViewObj != null && photonViewObj.IsMine)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
        }
    }

    private void ActivateDoubleJump()
    {
        doubleJumpAbility = true;
        Debug.Log("Habilidad de doble salto activada");
    }

    private void ActivateClimb()
    {
        climbAbility = true;
        Debug.Log("Habilidad de escalar activada");
    }
}
