using Photon.Pun;
using UnityEngine;

public class Items : MonoBehaviourPun
{
    public int ID;
    public string type;
    public string descript;
    public Sprite icon;

    [HideInInspector]
    public bool isUp;

    [HideInInspector]
    public bool equipped;

    public ItemManager itemManager;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }

        if (itemManager == null)
        {
            itemManager = GetComponentInParent<ItemManager>();
            if (itemManager == null)
            {
                Debug.LogError("ItemManager no encontrado en el padre.");
            }
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (equipped && Input.GetKeyDown(KeyCode.E))
        {
            UnequipItem();
        }
    }

    public void ItemUsage()
    {
        switch (type)
        {
            case "potion":
                itemManager.UsePotion();
                break;
            case "accesory":
                itemManager.UseAccessory();
                break;
            default:
                Debug.Log("Tipo de ítem no reconocido.");
                break;
        }

        itemManager.inventory.RemoveItemByID(ID);
        HUDManager.Instance.inventoryUI.GetComponent<InventoryUI>().RemoveItemByID(ID);
    }

    public void EquipItem()
    {
        if (!photonView.IsMine) return;

        equipped = true;
        gameObject.SetActive(true);
    }

    public void UnequipItem()
    {
        if (!photonView.IsMine) return;

        equipped = false;
        gameObject.SetActive(false);
    }
}
