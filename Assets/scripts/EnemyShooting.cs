using Photon.Pun;
using UnityEngine;

public class EnemyShooting : MonoBehaviourPun
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject bola;
    public float shootCooldown;
    [SerializeField] private float projectileSpeed;

    private float shootTimer;

    void Start()
    {
        shootTimer = shootCooldown;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        shootTimer += Time.deltaTime;
    }

    public void Shoot(Transform target)
    {
        if (shootTimer >= shootCooldown)
        {
            GameObject projectile = PhotonNetwork.Instantiate(bola.name, pivot.position, pivot.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (target.position - pivot.position).normalized * projectileSpeed;
            }
            shootTimer = 0f;
        }
    }
}
