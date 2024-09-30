using UnityEngine;

[System.Serializable]
public class Item
{
    public int ID;
    public string type;
    public string descript;
    public Sprite icon;

    public Item(int id, string type, string descript, Sprite icon)
    {
        this.ID = id;
        this.type = type;
        this.descript = descript;
        this.icon = icon;
    }
}
