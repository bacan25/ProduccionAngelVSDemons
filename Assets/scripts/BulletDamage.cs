using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    //public Health health;
    public int damage;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject, 5f);


    }

    
}
