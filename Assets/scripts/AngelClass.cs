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

    public PlayerCanvas playerCanvas;

    void Start()
    {
        // Si el objeto no pertenece a este jugador y no está en modo offline, desactivar el control y componentes no necesarios
        if (!photonView.IsMine && !PhotonNetwork.OfflineMode)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            enabled = false;
            return;
        }

        // Asignar referencia al PlayerCanvas
        if (playerCanvas == null)
        {
            playerCanvas = PlayerCanvas.Instance;
        }

        // Desbloquear habilidad de ataque básico e inicializar la interfaz
        playerCanvas.UnlockAbility("BasicAttack");
        playerCanvas.LockAbility("PowerAttack");
    }

    void Update()
    {
        // Si no es el jugador local ni estamos en offline mode, salir de la actualización
        if (!photonView.IsMine && !PhotonNetwork.OfflineMode) return;

        HandleCooldowns();

        // Comprobar si se puede realizar el ataque básico
        if (Input.GetMouseButton(0) && basicUnlocked && basicTimer >= basicCooldown)
        {
            BasicAttack();
        }

        // Comprobar si se puede realizar el ataque de poder
        if (Input.GetMouseButton(1) && powerUnlocked && powerTimer >= powerCooldown)
        {
            PowerAttack();
        }
    }

    private void HandleCooldowns()
    {
        // Actualizar el cooldown de las habilidades
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

        GameObject basicAttack = null;
        // Si está en modo offline, instanciar el proyectil localmente
        if (PhotonNetwork.OfflineMode || !PhotonNetwork.InRoom)
        {
            basicAttack = Instantiate(bulletPrefab, pivot.position, pivot.rotation);
        }
        else
        {
            // En modo online, usar Photon para instanciar el proyectil
            basicAttack = PhotonNetwork.Instantiate(bulletPrefab.name, pivot.position, pivot.rotation);
        }

        // Verificar que el proyectil se ha instanciado correctamente
        if (basicAttack != null)
        {
            Rigidbody rb = basicAttack.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * bulletSpeed;
            }
            else
            {
                Debug.LogError("El proyectil no tiene un componente Rigidbody.");
            }
        }
        else
        {
            Debug.LogError("No se pudo instanciar el proyectil. Asegúrate de estar en una sala o en offline mode.");
        }

        playerCanvas.UpdateAbilityCooldown("BasicAttack", 0f);
    }

    private void PowerAttack()
    {
        powerTimer = 0f;

        GameObject powerAttack = null;
        // Si está en modo offline, instanciar el proyectil localmente
        if (PhotonNetwork.OfflineMode || !PhotonNetwork.InRoom)
        {
            powerAttack = Instantiate(powerPrefab, pivot.position, pivot.rotation);
        }
        else
        {
            // En modo online, usar Photon para instanciar el proyectil
            powerAttack = PhotonNetwork.Instantiate(powerPrefab.name, pivot.position, pivot.rotation);
        }

        // Verificar que el proyectil se ha instanciado correctamente
        if (powerAttack != null)
        {
            Rigidbody rb = powerAttack.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * powerSpeed;
            }
            else
            {
                Debug.LogError("El proyectil no tiene un componente Rigidbody.");
            }
        }
        else
        {
            Debug.LogError("No se pudo instanciar el proyectil de poder. Asegúrate de estar en una sala o en offline mode.");
        }

        playerCanvas.UpdateAbilityCooldown("PowerAttack", 0f);
    }

    public void UnlockPower()
    {
        powerUnlocked = true;
        playerCanvas.UnlockAbility("PowerAttack");
    }
}
