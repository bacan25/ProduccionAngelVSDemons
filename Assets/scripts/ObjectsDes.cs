using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectsDes : MonoBehaviour
{
    public GameObject descriptPanel;
    [SerializeField] RectTransform panelRect;
    [SerializeField] float middlePosY, topPosY;
    [SerializeField] float tweenDur;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            descriptPanel.gameObject.SetActive(true);
            PanelIntro();
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            descriptPanel.gameObject.SetActive(false);
            PanelExit();
        }
    }

    private void PanelIntro()
    {
        panelRect.DOAnchorPosY(middlePosY, tweenDur);   
    }

    private void PanelExit()
    {
        panelRect.DOAnchorPosY(topPosY, tweenDur);
    }
}
