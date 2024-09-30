using Photon.Pun;
using UnityEngine;

public class ItemManager : MonoBehaviourPun
{
    public GameObject activeItem = null;
    public int itemID;
    public string itemType;
    public int potionNum;
    public int slotInum;

    public Health health;
    public Inventory inventory;
    public AngelClass angelClass;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }

        if (health == null)
        {
            health = GetComponent<Health>();
            if (health == null)
            {
                Debug.LogError("El componente Health no está asignado y no se encontró en el GameObject.");
            }
        }

        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
        }

        if (angelClass == null)
        {
            angelClass = GetComponent<AngelClass>();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.U) && itemType == "accesory")
        {
            UseAccessory();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UsePotion();
        }
    }

    public void UsePotion()
    {
        if (health != null && potionNum > 0)
        {
            health.Potion();
            potionNum--;
            HUDManager.Instance.UpdatePotionCount(potionNum);

            inventory.ClearSlotByNumber(slotInum);

            itemID = 0;
        }
        else
        {
            Debug.Log("No tienes pociones.");
        }
    }

    public void UseAccessory()
    {
        if (itemID == 2 && angelClass != null)
        {
            angelClass.basicCooldown = 0.3f;

            inventory.ClearSlotByNumber(slotInum);

            itemID = 0;
        }
    }
}
