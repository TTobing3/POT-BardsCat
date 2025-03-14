using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmithDragable : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IPointerExitHandler
{
    public SmithBox box;
    RectTransform rect;
    Image image;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.sprite = GetComponent<SpriteRenderer>().sprite;
        image.color = new Color(1, 1, 1, 1);
        image.SetNativeSize();
        Resize();
        box.isSelect = true;
        image.raycastTarget = false;

        StartScene.instance.SoundPlay(StartScene.instance.uiClips[3]);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        box.selectedNumber = -1;
        box.isSelect = false;
        image.raycastTarget = true;
        rect.anchoredPosition = new Vector2(0, 0);

        gameObject.SetActive(false);

        StartScene.instance.SoundPlay(StartScene.instance.uiClips[3]);
    }

    void Resize()
    {
        if(rect.sizeDelta.x < 100 || rect.sizeDelta.y < 100)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x * 2, rect.sizeDelta.y * 2);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!box.isSelect)gameObject.SetActive(false);
    }

}
