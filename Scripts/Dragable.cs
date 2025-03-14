using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Dragable : MonoBehaviour, IDragHandler
{
    RectTransform rect;
    public Canvas canvas;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }
}
