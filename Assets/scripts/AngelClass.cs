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
        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            enabled = false;
            return;
        }

        if (playerCanvas == null)
        {
            playerCanvas = GetComponent<PlayerCanvas>();
        }

        playerCanvas.UnlockAbility("BasicAttack");
        playerCanvas.LockAbility("PowerAttack");
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        HandleCooldowns();

        if (Input.GetMouseButton(0) && basicUnlocked && basicTimer >= basicCooldown)
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
        GameObject basicAttack = PhotonNetwork.Instantiate(bulletPrefab.name, pivot.position, pivot.rotation);
        Rigidbody rb = basicAttack.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = pivot.forward * bulletSpeed;
        }

        playerCanvas.UpdateAbilityCooldown("BasicAttack", 0f);
    }

    private void PowerAttack()
    {
        powerTimer = 0f;
        GameObject powerAttack = PhotonNetwork.Instantiate(powerPrefab.name, pivot.position, pivot.rotation);
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
