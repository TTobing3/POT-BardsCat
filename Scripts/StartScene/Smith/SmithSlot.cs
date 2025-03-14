using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmithSlot : MonoBehaviour
{
    // ���尣 �κ��丮 ����

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
        // �ҽ� RectTransform�� ���� �������� ����ϴ�.
        Vector3 worldPosition = rect.TransformPoint(new Vector3(rect.rect.width * rect.pivot.x, rect.rect.height * rect.pivot.y, 0));


        // ���� �������� ��ũ�� ���������� ��ȯ�մϴ�.
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(null, worldPosition);

        // ��ũ�� �������� Ÿ�� RectTransform�� ���� ���������� ��ȯ�մϴ�.
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
