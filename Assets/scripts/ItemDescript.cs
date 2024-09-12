using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescript : MonoBehaviour
{

    public Transform slotDescriptText;
    public Slot slot;
    // Start is called before the first frame update
    void Start()
    {
        slotDescriptText = transform.GetChild(0);
    }

    //public void UpdateText()
    //{
    //    slotDescriptText.GetComponent<Text>().text = slot.descript;

    //}
}
