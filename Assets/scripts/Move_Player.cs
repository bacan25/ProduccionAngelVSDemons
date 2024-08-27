using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Move_Player : MonoBehaviour
{
    [Header("Mov and Jump")]
    private Rigidbody rb;
    [SerializeField] private float movSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    private float horInput, verInput;
    private Vector2 mouseInput;
    public bool isGrounded = false;
    public Transform cameraVer;
    private float localRot;

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 5f;
    public bool isClimbing = false;
    public float sphereCastRadius;
    public bool wallFront;
    public LayerMask whatIsWall;
    public Transform climbRef;

    [Header("Camera")]
    public Camera playerCamera; // Referencia a la cámara del jugador

    private PhotonView photonView;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        // Activar o desactivar la cámara en función de si este jugador es el propietario
        if (!photonView.IsMine)
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = false;
            }
        }
        else
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = true;
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return; // Solo permite que el jugador local controle su propio personaje

        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (isClimbing)
        {
            Climb();
        }
        else
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
        }
        else
        {
            wallFront = false;
        }

        localRot -= mouseInput.y * turnSpeed * Time.deltaTime * 2;
        localRot = Mathf.Clamp(localRot, -30f, 80f);
        cameraVer.localEulerAngles = Vector3.right * localRot;
    }

    public void Walk()
    {
        rb.velocity = transform.rotation * new Vector3
        (
            horInput * movSpeed,
            rb.velocity.y,
            wallFront ? Mathf.Clamp(verInput * movSpeed, -1000, 0) : verInput * movSpeed
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
