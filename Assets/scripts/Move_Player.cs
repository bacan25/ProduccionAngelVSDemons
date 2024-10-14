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

    [Header("Ground Check")]
    [SerializeField] private Transform[] groundCheckPoints; // Puntos para verificar si el jugador está en el suelo
    [SerializeField] private float groundCheckRadius = 0.3f; // Radio de verificación para el suelo
    [SerializeField] private float rayLength = 1.2f; // Longitud del raycast para detectar el suelo

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 5f;
    public bool isClimbing = false;
    [SerializeField] private float sphereCastRadius = 0.5f; // Radio para detectar paredes escalables
    [SerializeField] private LayerMask whatIsWall;
    public Transform climbRef; // Referencia para verificar si hay una pared en frente

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

        // Actualizar el estado de isGrounded con un Raycast o verificación de puntos
        CheckGroundStatus();

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

        // Lógica de escalada
        CheckClimbStatus();

        if (isClimbing)
        {
            Climb();
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

    private void CheckGroundStatus()
    {
        isGrounded = false;

        // Utilizar Raycast para verificar si está tocando el suelo
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                jumpCount = 0; // Reiniciar el contador de saltos
                return;
            }
        }

        // Verificar cada punto en el suelo para asegurarse de que el jugador está grounded
        foreach (Transform checkPoint in groundCheckPoints)
        {
            if (Physics.CheckSphere(checkPoint.position, groundCheckRadius, LayerMask.GetMask("Ground")))
            {
                isGrounded = true;
                jumpCount = 0;
                return;
            }
        }
    }

    private void CheckClimbStatus()
    {
        // Verificar si hay una pared en frente para escalar
        Collider[] colliders = Physics.OverlapSphere(climbRef.position, sphereCastRadius, whatIsWall);
        if (colliders.Length > 0 && Input.GetKey(KeyCode.Space) && climbAbility)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }
    }

    private void Climb()
    {
        // Aplicar movimiento de escalada
        rb.velocity = new Vector3(horizontalInput * climbSpeed, verticalInput * climbSpeed, 0);
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
