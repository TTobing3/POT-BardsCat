using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Tooltipable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool[] actType = new bool[2];

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!actType[0]) return;

        StartSceneManager.instance.toolTip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!actType[1]) return;

        StartSceneManager.instance.toolTip.gameObject.SetActive(false);
    }

}
