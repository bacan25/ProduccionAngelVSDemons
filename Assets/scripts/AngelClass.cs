using Photon.Pun;
using UnityEngine;

public class AngelClass : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    public float basicCooldown = 2f;
    private float basicTimer;

    [SerializeField] private GameObject powerPrefab;
    [SerializeField] private float powerSpeed;
    public float powerCooldown = 5f;
    private float powerTimer;

    private bool basicUnlocked = true;
    private bool powerUnlocked = false;

    private PlayerCanvas playerCanvas;

    void Start()
    {
        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            enabled = false;
            return;
        }

        // Obtener el PlayerCanvas como singleton
        playerCanvas = PlayerCanvas.Instance;

        if (playerCanvas != null)
        {
            playerCanvas.UnlockAbility("BasicAttack");
            playerCanvas.LockAbility("PowerAttack");
        }
        else
        {
            Debug.LogError("PlayerCanvas no encontrado. Asegúrate de que esté en la escena.");
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        HandleCooldowns();
        basicTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && basicTimer >= basicCooldown)
        {
            BasicAttack();
        }
        if (Input.GetMouseButton(1) && powerUnlocked && powerTimer >= powerCooldown)
        {
            PowerAttack();
        }
    }

    private void HandleCooldowns()
    {
        // Control de cooldown de habilidades en la UI
        if (basicUnlocked)
        {
            basicTimer += Time.deltaTime;
            playerCanvas.UpdateAbilityCooldown("BasicAttack", Mathf.Clamp01(basicTimer / basicCooldown));
        }

        if (powerUnlocked)
        {
            powerTimer += Time.deltaTime;
            playerCanvas.UpdateAbilityCooldown("PowerAttack", Mathf.Clamp01(powerTimer / powerCooldown));
        }
    }


    private void BasicAttack()
    {
        basicTimer = 0f;

        if (bulletPrefab != null && pivot != null)
        {
            GameObject bullet;

            // Verificar si estamos en modo offline o conectados a una sala
            if (PhotonNetwork.OfflineMode)
            {
                // Instanciar usando el método normal de Unity en modo offline
                bullet = Instantiate(bulletPrefab, pivot.position, pivot.rotation);
            }
            else if (PhotonNetwork.InRoom)
            {
                // Instanciar usando Photon si estamos conectados a una sala
                bullet = PhotonNetwork.Instantiate(bulletPrefab.name, pivot.position, pivot.rotation);
            }
            else
            {
                Debug.LogError("No puedes instanciar el proyectil, el cliente no está en una sala.");
                return;
            }

            // Aplicar la velocidad al proyectil
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * bulletSpeed;
            }
        }
        else
        {
            Debug.LogError("El prefab del proyectil o el pivot no están asignados correctamente.");
        }
    }

    private void PowerAttack()
    {
        powerTimer = 0f;
        GameObject powerAttack = PhotonNetwork.IsConnected ? PhotonNetwork.Instantiate(powerPrefab.name, pivot.position, pivot.rotation) : Instantiate(powerPrefab, pivot.position, pivot.rotation);
        Rigidbody rb = powerAttack.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = pivot.forward * powerSpeed;
        }

        playerCanvas.UpdateAbilityCooldown("PowerAttack", 0f);
    }

    public void UnlockPower()
    {
        powerUnlocked = true;
        playerCanvas.UnlockAbility("PowerAttack");
    }
}
