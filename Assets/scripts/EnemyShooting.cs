using Photon.Pun;
using UnityEngine;

public class EnemyShooting : MonoBehaviourPun
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject bola;
    public float shootCooldown;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private EnemyManager enemyManager;

    private float shootTimer;

    void Start()
    {
        enemyManager = gameObject.GetComponentInParent<EnemyManager>();
        shootTimer = shootCooldown;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        shootTimer += Time.deltaTime;
    }

    public void Shoot()
    {
        if (shootTimer >= shootCooldown)
        {
            GameObject projectile = PhotonNetwork.Instantiate(bola.name, pivot.position, pivot.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (enemyManager.playerDetected.position - pivot.position).normalized * projectileSpeed;
            }
            shootTimer = 0f;
        }
    }

    
}
