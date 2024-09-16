using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class AngelClass : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform pivot;

    //Basic attack
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float vel;
    public float basicCooldown;

    [SerializeField]
    private float basicTimer;

    //Power attack
    [SerializeField]
    private GameObject power;
    [SerializeField]
    private float velPower;
    [SerializeField]
    private float powerCooldown;
    private float powerTimer;

    //Have/Don't have powers
    private bool gotBasic = true;
    private bool gotPower = false;

    public ChangeAlpha changeAlpha;
    public Inventory inventory;
    public Move_Player player;

    void Start()
    {
        if (!photonView.IsMine)
        {
            // Deshabilita componentes que solo deben ser controlados por el propietario
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (inventory.inventoryOnStage || inventory.instructionsOnStage)
        {
            return;
        }

        if (player.muerto)
        {
            return;
        }

        if (gotBasic && basicTimer <= basicCooldown + 0.1f)
            basicTimer += Time.deltaTime;
        if (gotPower && powerTimer <= powerCooldown + 0.1f)
            powerTimer += Time.deltaTime;

        if (basicTimer >= basicCooldown)
        {
            changeAlpha.GreenCoolDownIcon();
        }

        if (Input.GetMouseButton(0) && gotBasic)
            BasicAttack();
        if (Input.GetMouseButton(1) && gotPower)
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
            changeAlpha.RedCoolDownIcon();
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
        gotPower = true;
        changeAlpha.SetPowerUpIcon(1f);
    }
}
