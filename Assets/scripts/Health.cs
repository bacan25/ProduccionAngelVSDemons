using Photon.Pun;
using UnityEngine;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public int currentHealth;
    public int maxHealth;
    private Vector3 respawnPosition;
    public Move_Player player;
    private bool isRespawning = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (player == null)
        {
            player = GetComponent<Move_Player>() ?? GetComponentInChildren<Move_Player>();
            if (player == null)
            {
                Debug.LogError("El componente Move_Player no se encontró en el jugador ni en sus hijos.");
            }
        }

        respawnPosition = transform.position;

        if (photonView.IsMine)
        {
            UpdateUI();
        }
    }


    private void Update()
    {
        if (!photonView.IsMine) return;

        if (currentHealth <= 0 && !isRespawning)
        {
            isRespawning = true;
            RespawnPlayer();
            isRespawning = false;
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (photonView.IsMine)
        {
            UpdateUI();
        }

        if (currentHealth <= 0)
        {
            if (CompareTag("Minion"))
            {
                DeathEnemy();
            }
            else if (CompareTag("Player"))
            {
                // El jugador será respawneado en Update()
            }
        }
    }

    public void TakeFallDamage()
    {
        TakeDamage(currentHealth);
    }

    public void Potion()
    {
        currentHealth += 10;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (photonView.IsMine)
        {
            UpdateUI();
        }
    }

    void RespawnPlayer()
    {
        player.muerto = true;
        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        transform.position = respawnPosition;

        currentHealth = maxHealth;

        if (photonView.IsMine)
        {
            UpdateUI();
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
        player.muerto = false;
    }

    public void DeathEnemy()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void UpdateUI()
    {
        if (photonView.IsMine)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            HUDManager.Instance.UpdateHealth(healthPercent);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }
    }

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }
}
