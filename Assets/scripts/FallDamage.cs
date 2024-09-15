using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallDamage : MonoBehaviour
{
    [SerializeField] int min, seg;
    [SerializeField] Text tiempo;

    private float restante;
    public bool respawn;
    public Move_Player player;
    public Health health;
    public GameObject respawnPanel;

    private void Awake()
    {
        restante = (min * 60 + seg);
        
    }

    private void Update()
    {
        if (respawn)
        {
            player.muerto = true;
            respawnPanel.SetActive(true);
            restante -= Time.deltaTime;
            if (restante < 1)
            {
                respawn = false;
                respawnPanel.SetActive(false);
                health.DeathPlayer();
                player.muerto = false;
                restante = (min * 60 + seg);
            }
            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            tiempo.text = "Respawn en " + string.Format("{00:00}:{01:00}", tempMin, tempSeg);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Health>().TakeDamage(100);
        respawn = true;

    }
}
