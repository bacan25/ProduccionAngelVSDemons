using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Move_Player : MonoBehaviourPunCallbacks
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

        if (photonView.IsMine)
        {
            // Enviamos un RPC al Master Client para registrar nuestro Transform
            photonView.RPC("RegisterPlayerWithMaster", RpcTarget.MasterClient, photonView.ViewID);
        }
        else
        {
            // Desactivar componentes en jugadores remotos
            DisableRemotePlayerComponents();
        }
    }

    void DisableRemotePlayerComponents()
    {
        // Desactivar cámara
        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }

        // Desactivar audio listener
        AudioListener audioListener = GetComponentInChildren<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = false;
        }

        // Desactivar scripts específicos
        Inventory inventoryScript = GetComponent<Inventory>();
        if (inventoryScript != null)
        {
            inventoryScript.enabled = false;
        }

        // Desactivar otros componentes si es necesario
    }

    [PunRPC]
    void RegisterPlayerWithMaster(int viewID)
    {
        PhotonView playerView = PhotonView.Find(viewID);
        if (playerView != null)
        {
            Transform playerTransform = playerView.transform;
            InGameManager.Instance.RegisterPlayer(playerTransform);
        }
    }

    void OnDestroy()
    {
        if (photonView.IsMine)
        {
            // Enviamos un RPC al Master Client para eliminar nuestro Transform
            photonView.RPC("UnregisterPlayerWithMaster", RpcTarget.MasterClient, photonView.ViewID);
        }
    }

    [PunRPC]
    void UnregisterPlayerWithMaster(int viewID)
    {
        PhotonView playerView = PhotonView.Find(viewID);
        if (playerView != null)
        {
            Transform playerTransform = playerView.transform;
            InGameManager.Instance.UnregisterPlayer(playerTransform);
        }
    }

    void Update()
    {
        if (!photonView.IsMine || muerto)
        {
            return;
        }

        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");

        if (inventory != null && (inventory.inventoryOnStage || inventory.instructionsOnStage))
        {
            mouseInput = Vector2.zero;
        }
        else
        {
            mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        if (isClimbing)
        {
            Climb();
        }
        else
        {
            Walk();
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 || (Input.GetKeyDown(KeyCode.Space) && jumpCount == 1 && dobleSaltoSkill))
        {
            Jump();
        }

        Collider[] colliders = Physics.OverlapSphere(climbRef.position, sphereCastRadius, whatIsWall);
        if (colliders.Length > 0 && Input.GetKey(KeyCode.Space) && climbSkill)
        {
            wallFront = true;
        }
        else
        {
            wallFront = false;
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
        if (cameraVer != null)
        {
            cameraVer.localEulerAngles = Vector3.right * localRot;
        }
    }

    public void Walk()
    {
        if ((jumpCount == 0 && isGrounded) || jumpCount == 2)
        {
            rb.velocity = transform.rotation * new Vector3
            (
                horInput * movSpeed,
                rb.velocity.y,
                verInput * movSpeed
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

        if (jumpCount == 2)
        {
            jumpSpeed = startJumpSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(climbRef.position, sphereCastRadius);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            jumpSpeed = startJumpSpeed;
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
        if (other.gameObject.CompareTag("DobleSalto"))
        {
            DobleSalto();
            PhotonNetwork.Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Escalar"))
        {
            Escalar();
            PhotonNetwork.Destroy(other.gameObject);
        }
    }

    private void DobleSalto()
    {
        dobleSaltoSkill = true;
        changeAlpha.SetDobleSaltoIcon(1f);

        // Sincronizar con otros jugadores si es necesario
        photonView.RPC("SyncDobleSaltoSkill", RpcTarget.Others, dobleSaltoSkill);
    }

    [PunRPC]
    void SyncDobleSaltoSkill(bool value)
    {
        dobleSaltoSkill = value;
        // Actualizar UI si es necesario
    }

    private void Escalar()
    {
        climbSkill = true;
        changeAlpha.SetClimbIcon(1f);

        // Sincronizar con otros jugadores si es necesario
        photonView.RPC("SyncClimbSkill", RpcTarget.Others, climbSkill);
    }

    [PunRPC]
    void SyncClimbSkill(bool value)
    {
        climbSkill = value;
        // Actualizar UI si es necesario
    }
}
