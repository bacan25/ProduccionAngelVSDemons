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
    public bool isGrounded = true;
    private int jumpCount = 0;
    public Transform cameraTransform;
    public float verticalRotation;

    [Header("Abilities")]
    [SerializeField] private bool doubleJumpAbility = false;  // Control de la habilidad de doble salto
    private bool climbAbility = false;
    public bool isDead;

    private Vector3 currentVelocity; // Almacena la velocidad actual del jugador

    // Referencia al PlayerCanvas singleton
    private PlayerCanvas playerCanvas;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpForce = 10f; // Ajusta este valor en el Inspector de Unity

        // Obtener referencia al PlayerCanvas singleton
        playerCanvas = PlayerCanvas.Instance;

        if (playerCanvas == null)
        {
            Debug.LogError("PlayerCanvas no encontrado. Asegúrate de que el PlayerCanvas esté en la escena.");
        }

        // Ocultar el cursor del ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!photonView.IsMine || isDead) return;

        // Obtener inputs de rotación de cámara
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        RotatePlayer();

        // Manejar el salto
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (doubleJumpAbility && jumpCount < 2)
            {
                Jump();
            }
        }

        // Movimiento horizontal solo si está en el suelo o tiene doble salto
        if (isGrounded || doubleJumpAbility)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Actualizar la velocidad actual en función de los inputs
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            currentVelocity = moveDirection * moveSpeed;
        }
    }

    void FixedUpdate()
    {
        if (isGrounded || doubleJumpAbility)
        {
            // Aplicar movimiento
            Vector3 velocity = transform.TransformDirection(currentVelocity);
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        // Si está en el aire sin doble salto, no se aplica movimiento horizontal
    }

    private void RotatePlayer()
    {
        // Rotar el jugador y la cámara
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
        // Aplicar la fuerza de salto
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reiniciar la velocidad vertical
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        jumpCount++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;  // Reiniciar el contador de saltos
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
        if (other.CompareTag("DobleSalto"))
        {
            ActivateDoubleJump();
            PhotonView photonViewObj = other.GetComponent<PhotonView>();
            if (photonViewObj != null && photonViewObj.IsMine)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Escalar"))
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

        if (playerCanvas != null)
        {
            playerCanvas.UnlockAbility("DoubleJump");
        }
    }

    private void ActivateClimb()
    {
        climbAbility = true;
        Debug.Log("Habilidad de escalar activada");

        if (playerCanvas != null)
        {
            playerCanvas.UnlockAbility("Climb");
        }
    }
}
