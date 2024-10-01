using Photon.Pun;
using UnityEngine;

public class EnemyShooting : MonoBehaviourPun
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject bola;
    
    [SerializeField] private float projectileSpeed;
    [SerializeField] private EnemyManager enemyManager;


    void Start()
    {
        enemyManager = gameObject.GetComponentInParent<EnemyManager>();
       
    }

    public void Shoot()
    {
           GameObject projectile = PhotonNetwork.Instantiate(bola.name, pivot.position, pivot.rotation);
           Rigidbody rb = projectile.GetComponent<Rigidbody>();
           if (rb != null)
           {
               rb.velocity = (enemyManager.playerDetected.position - pivot.position).normalized * projectileSpeed;
           }
              
    }

    
}
