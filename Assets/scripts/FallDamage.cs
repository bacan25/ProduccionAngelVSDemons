using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallDamage : MonoBehaviour
{
 
    public Health health;
    


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Health>().TakeDamage(100);
        other.GetComponent<Health>().isRespawn = true;
        
    }
}
