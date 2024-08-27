using System.Collections;
using UnityEngine;
using Photon.Pun;

public class AngelClass : MonoBehaviour
{
    [SerializeField]
    private Transform pivot;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float vel;
    [SerializeField]
    private float basicCooldown;
    private float basicTimer;

    [SerializeField]
    private GameObject power;
    [SerializeField]
    private float velPower;
    [SerializeField]
    private float powerCooldown;
    private float powerTimer;

    public bool gotBasic;
    public bool gotPower;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (gotBasic)
            basicTimer += Time.deltaTime;
        if (gotPower)
            powerTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && gotBasic)
            BasicAttack();
        if (Input.GetMouseButton(1) && gotPower)
            PowerAttack();
    }

    void BasicAttack()
    {
        if (basicTimer >= basicCooldown)
        {
            GameObject basicAtt = Instantiate(bullet, pivot.position, pivot.rotation); //cambiar instantiate por photon instantiate
            Rigidbody rb = basicAtt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * vel;
            }
            Destroy(basicAtt, 3f);

            basicTimer = 0f;
        }
    }

    void PowerAttack()
    {
        if (powerTimer >= powerCooldown)
        {
            GameObject powerAtt = Instantiate(power, pivot.position, pivot.rotation);
            Rigidbody rb = powerAtt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * vel;
            }
            Destroy(powerAtt, 3f);

            powerTimer = 0f;
        }
    }
}
