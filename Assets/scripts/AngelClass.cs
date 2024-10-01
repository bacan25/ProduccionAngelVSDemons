using Photon.Pun;
using UnityEngine;

public class AngelClass : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform pivot;

    [Header("Habilidades")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float vel;
    public float basicCooldown;
    private float basicTimer;

    [SerializeField]
    private GameObject power;
    [SerializeField]
    private float velPower;
    [SerializeField]
    private float powerCooldown;
    private float powerTimer;

    private bool basicUnlocked = true;
    private bool powerUnlocked = false;

    public Inventory inventory;
    public Move_Player player;

    void Start()
    {
        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            enabled = false;
            return;
        }

        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
        }

        if (player == null)
        {
            player = GetComponent<Move_Player>();
        }

        // Inicializar habilidades en el HUD
        HUDManager.Instance.UnlockAbility("BasicAttack");
        HUDManager.Instance.LockAbility("PowerAttack");
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (inventory != null && inventory.inventoryUI != null && inventory.inventoryUI.activeSelf)
        {
            return;
        }

        if (player != null && player.muerto)
        {
            return;
        }

        if (basicUnlocked)
        {
            basicTimer += Time.deltaTime;
            float cooldownPercent = Mathf.Clamp01(basicTimer / basicCooldown);
            HUDManager.Instance.UpdateAbilityCooldown("BasicAttack", 1 - cooldownPercent);
        }

        if (powerUnlocked)
        {
            powerTimer += Time.deltaTime;
            float cooldownPercent = Mathf.Clamp01(powerTimer / powerCooldown);
            HUDManager.Instance.UpdateAbilityCooldown("PowerAttack", 1 - cooldownPercent);
        }

        if (Input.GetMouseButton(0) && basicUnlocked)
            BasicAttack();

        if (Input.GetMouseButton(1) && powerUnlocked)
            PowerAttack();
    }

    void BasicAttack()
    {
        if (basicTimer >= basicCooldown)
        {
            GameObject basicAtt = PhotonNetwork.Instantiate(bullet.name, pivot.position, pivot.rotation);
            Rigidbody rb = basicAtt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * vel;
            }
            basicTimer = 0f;
            HUDManager.Instance.UpdateAbilityCooldown("BasicAttack", 1f);
        }
    }

    void PowerAttack()
    {
        if (powerTimer >= powerCooldown)
        {
            GameObject powerAtt = PhotonNetwork.Instantiate(power.name, pivot.position, pivot.rotation);
            Rigidbody rb = powerAtt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * velPower;
            }
            powerTimer = 0f;
            HUDManager.Instance.UpdateAbilityCooldown("PowerAttack", 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        if (other.gameObject.CompareTag("PowerUp"))
        {
            PowerUp();
            PhotonNetwork.Destroy(other.gameObject);
        }
    }

    private void PowerUp()
    {
        powerUnlocked = true;
        powerTimer = powerCooldown;
        HUDManager.Instance.UnlockAbility("PowerAttack");
        HUDManager.Instance.UpdateAbilityCooldown("PowerAttack", 1f);
    }
}
