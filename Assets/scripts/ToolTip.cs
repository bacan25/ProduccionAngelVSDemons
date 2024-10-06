using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<Text>();


    }

    private void ShowTooltip(string tooltipString)
    {

    }
}
