using UnityEngine;
using UnityEngine.UI;

public class ItemDescript : MonoBehaviour
{
    public Transform slotDescriptText;
    public Slot slot;

    void Start()
    {
        slotDescriptText = transform.GetChild(0);
    }

    // Uncomment and use this method to update the description text when needed
    // public void UpdateText()
    // {
    //     slotDescriptText.GetComponent<Text>().text = slot.descript;
    // }
}
