using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletDamage : MonoBehaviour
{
    //public Health health;
    
    public int playerBulletDamage;

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Minion"))
        {
            other.GetComponent<Health>().TakeDamage(playerBulletDamage);
            Destroy(this.gameObject);

        }

        Destroy(this.gameObject, 5f);
    }
}
