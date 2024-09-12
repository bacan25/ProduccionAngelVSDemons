using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Health>().TakeDamage(100);
    }
}
