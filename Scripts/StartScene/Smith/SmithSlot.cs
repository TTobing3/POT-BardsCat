using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmithSlot : MonoBehaviour
{
    // 대장간 인벤토리 슬롯

    public SmithBox box;
    InstantData instantData;
    RectTransform rect;

    public int item, category;
    private void Awake()
    {
        instantData = GetComponent<InstantData>();
        rect = GetComponent<RectTransform>();
    }

    public void Set()
    {
        if (box.isSelect) return;

        if(transform.GetChild(1).gameObject.activeSelf)
        {
            
            box.itemDrager.SetActive(true);
            var sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            box.itemDrager.GetComponent<SpriteRenderer>().sprite = sprite;
            box.itemDrager.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 110);
            box.itemDrager.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            box.itemDrager.GetComponent<Image>().sprite = null;
            box.itemDrager.GetComponent<RectTransform>().anchoredPosition = ConvertAnchoredPosition();//Input.mousePosition;//Camera.main.ScreenToViewportPoint();
            box.selectedNumber = instantData.number;

            StartSceneManager.instance.toolTip.SetBox(item, category);
        }
        else
        {
            box.selectedNumber = -1;
            StartSceneManager.instance.toolTip.gameObject.SetActive(false);
        }
    }

    Vector2 ConvertAnchoredPosition()
    {
        // 소스 RectTransform의 월드 포지션을 얻습니다.
        Vector3 worldPosition = rect.TransformPoint(new Vector3(rect.rect.width * rect.pivot.x, rect.rect.height * rect.pivot.y, 0));


        // 월드 포지션을 스크린 포지션으로 변환합니다.
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(null, worldPosition);

        // 스크린 포지션을 타겟 RectTransform의 로컬 포지션으로 변환합니다.
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(box.page.GetComponent<RectTransform>(), screenPosition, null, out localPoint);

        var boxRect = box.page.GetComponent<RectTransform>();

        Vector2 anchoredPosition = localPoint - (boxRect.rect.size * boxRect.pivot);

        return anchoredPosition;
    }

    public void SoundPlay(AudioClip _clip)
    {
        SoundManager.instance.SfxPlay(_clip);
    }

}
