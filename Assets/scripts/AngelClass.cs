using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static UnityEditor.FilePathAttribute;


public class AngelClass : MonoBehaviour
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
    public bool gotBasic;
    public bool gotPower = false;

    public ChangeAlpha changeAlpha;
    



    void Start()
    {
        //basicTimer = basicCooldown;
        //powerTimer = powerCooldown;

    }


    void Update()
    {
        if(gotBasic && basicTimer <= basicCooldown + 0.1f)
            basicTimer += Time.deltaTime;
        if(gotPower && powerTimer <= powerCooldown + 0.1f)
            powerTimer += Time.deltaTime;

        //print("Basic:" + basicTimer);
        //print("Power:" + powerTimer);

       /*  if(basicTimer >= basicCooldown)
        {
            changeAlpha.GreenCoolDownIcon();
        } */

        if(Input.GetMouseButton(0) && gotBasic)
            BasicAttack();
        if (Input.GetMouseButton(1) && gotPower)
            PowerAttack();

        
    }
    void BasicAttack()
    {
        if (basicTimer >= basicCooldown)
        {
            GameObject basicAtt;

            // Si estás en una sala de Photon, instancia a través de Photon
            if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
            {
                basicAtt = PhotonNetwork.Instantiate(bullet.name, pivot.position, pivot.rotation);
            }
            else
            {
                // Si no estás conectado a Photon, instancia localmente
                basicAtt = Instantiate(bullet, pivot.position, pivot.rotation);
            }

            // Añadir la velocidad a la bala
            Rigidbody rb = basicAtt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pivot.forward * vel;
            }

            // Destruir la bala después de 3 segundos
            Destroy(basicAtt, 3f);

            // Reiniciar el temporizador de ataque básico
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            PowerUp();
            Destroy(other.gameObject);
        }

       
    }

    private void PowerUp()
    {
        gotPower = true;
        changeAlpha.SetPowerUpIcon(1f);


    }

  
}
