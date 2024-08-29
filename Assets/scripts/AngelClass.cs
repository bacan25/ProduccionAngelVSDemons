using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
            // Instancia el proyectil con PhotonNetwork.Instantiate
            GameObject basicAtt = PhotonNetwork.Instantiate(bullet.name, pivot.position, pivot.rotation);
            Rigidbody rb = basicAtt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Asegúrate de que la gravedad esté habilitada o deshabilitada según lo necesites
                rb.useGravity = true;  // Activa o desactiva según tu necesidad

                // Aplica una fuerza para mover el proyectil
                rb.velocity = pivot.forward * vel;
            }
            Destroy(basicAtt, 3f);  // Destruye el proyectil después de 3 segundos

            basicTimer = 0f;
        }
    }

    void PowerAttack()
    {
        if (powerTimer >= powerCooldown)
        {
            // Instancia el proyectil con PhotonNetwork.Instantiate
            GameObject powerAtt = PhotonNetwork.Instantiate(power.name, pivot.position, pivot.rotation);
            Rigidbody rb = powerAtt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Asegúrate de que la gravedad esté habilitada o deshabilitada según lo necesites
                rb.useGravity = true;  // Activa o desactiva según tu necesidad

                // Aplica una fuerza para mover el proyectil
                rb.velocity = pivot.forward * velPower;
            }
            Destroy(powerAtt, 3f);  // Destruye el proyectil después de 3 segundos

            powerTimer = 0f;
        }
    }
}