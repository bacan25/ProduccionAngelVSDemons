using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 5f;
    public float lookSpeed = 3f;
    public float jumpForce = 5f;
    public GameObject cameraPrefab;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 20f;
    private Slider healthBar;

    private Rigidbody rb;
    private float currentHealth = 100f;
    private const float maxHealth = 100f;
    private Camera playerCamera;
    private GameManagerFPS gameManager;  // Cambia el tipo de GameManager a GameManagerFPS


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManagerFPS>();  // Usa GameManagerFPS aquí

        if (photonView.IsMine)
        {
            GameObject cameraInstance = Instantiate(cameraPrefab, transform.position, Quaternion.identity);
            playerCamera = cameraInstance.GetComponent<Camera>();
            cameraInstance.transform.SetParent(transform);
        }
        else
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }




    void Update()
    {
        if (!photonView.IsMine) return;

        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleShooting();

        if (currentHealth <= 0)
        {
            Die();
        }

        // Asegúrate de que la cámara esté siempre en la posición correcta
        if (playerCamera != null)
        {
            // Mantén la cámara alineada con la rotación del jugador
            playerCamera.transform.position = transform.position + new Vector3(0, 1.5f, 0); // Ajusta la altura si es necesario
            playerCamera.transform.rotation = Quaternion.Euler(10, transform.eulerAngles.y, 0); // Mantén el eje de rotación correcto
        }
    }

    public void SetHealthBar(Slider healthBar)
    {
        this.healthBar = healthBar;
        this.healthBar.maxValue = maxHealth;
        this.healthBar.value = currentHealth;
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move);
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;

        // Aplica la rotación solo en el eje Y para evitar que la cámara se descontrole
        transform.Rotate(0, mouseX, 0);
    }
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            photonView.RPC("Fire", RpcTarget.All);
        }
    }

    [PunRPC]
    void Fire()
    {
        if (bulletPrefab == null || bulletSpawn == null)
        {
            Debug.LogError("bulletPrefab o bulletSpawn no están asignados.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.velocity = bulletSpawn.forward * bulletSpeed;
        Destroy(bullet, 2.0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !photonView.IsMine)
        {
            photonView.RPC("TakeDamage", RpcTarget.All, 10f);
            Destroy(other.gameObject);
        }
    }

    [PunRPC]
    void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PhotonNetwork.Destroy(gameObject);
        gameManager.CheckForWinner();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
        }
    }
}
