using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using static FIMSpace.FProceduralAnimation.LegsAnimator;

public class Health : MonoBehaviourPunCallbacks
{
    public int currentHealth;
    public int maxHealth;
    [SerializeField] Transform respawn;
    public Slider healthBar;

    [SerializeField] int min, seg;
    [SerializeField] Text tiempo;

    private float restante;
    public bool isRespawn;
    public Move_Player player;
    public Health health;
    public GameObject respawnPanel;

    private void Awake()
    {
        restante = (min * 60 + seg);

    }


    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    private void Update()
    {
        if (isRespawn)
        {
            player.muerto = true;
            respawnPanel.SetActive(true);
            restante -= Time.deltaTime;
            if (restante < 1)
            {
                isRespawn = false;
                respawnPanel.SetActive(false);
                DeathPlayer();
                player.muerto = false;
                restante = (min * 60 + seg);
            }
            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            tiempo.text = "Respawn en " + string.Format("{00:00}:{01:00}", tempMin, tempSeg);
        }
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth < 1 && this.gameObject.CompareTag("Minion")) //Cambiar tag a Enemy
        {
            DeathEnemy();
        }else if (currentHealth < 1 && this.gameObject.CompareTag("Player"))
        {
            //DeathPlayer();
        }

        UpdateUI();
    }

    public void Potion()
    {
        currentHealth += 10;
        UpdateUI();
    }

    public void DeathEnemy()
    {
        Destroy(gameObject);
    }

    public void DeathPlayer()
    {
        this.gameObject.transform.position = respawn.position;
        currentHealth = maxHealth;
        UpdateUI();

    }
}
