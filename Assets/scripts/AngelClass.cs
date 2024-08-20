using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelClass : MonoBehaviour
{
    [SerializeField]
    private Transform pivot;

    //Basic attack
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float vel;
    [SerializeField]
    private float basicCooldown;
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
    public bool gotPower;


    void Start()
    {
        //basicTimer = basicCooldown;
        //powerTimer = powerCooldown;
    }

    
    void Update()
    {
        if(gotBasic)
            basicTimer += Time.deltaTime;
        if(gotPower)
            powerTimer += Time.deltaTime;

        //print("Basic:" + basicTimer);
        //print("Power:" + powerTimer);

        if(Input.GetMouseButton(0) && gotBasic)
            BasicAttack();
        if(Input.GetMouseButton(1) && gotPower)
            PowerAttack();
        
    }

    void BasicAttack()
    {
        if(basicTimer >= basicCooldown)
        {
            GameObject basicAtt = Instantiate(bullet, pivot.position, pivot.rotation);
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
        if(powerTimer >= powerCooldown)
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
