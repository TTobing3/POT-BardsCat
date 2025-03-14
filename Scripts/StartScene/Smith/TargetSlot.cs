using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



// ��� ����
public class TargetSlot : MonoBehaviour, IDropHandler
{
    public SmithPage page;
    public SmithBox box;
    public int slotNumber;
    RectTransform rect;
    Image itemImage;


    public TargetSlotType type;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        
        //public enum Tab { Weapon, Helmet, Armor, Acc, Material, Gem }
        //public enum Tab { Weapon, Helmet, Armor, Material, Gem } ���� ���� - ���� �������� �������� ����
        if (box.selectedNumber == -1 || eventData.pointerDrag == null || eventData.pointerDrag.tag != "Dragable") return;

        if (page.CheckAlready(box.curTab, box.selectedNumber)) return;

        if (type == TargetSlotType.Main && box.curTab != 4)
        {
            SetImage(eventData);
            page.ResetSlot(false);
            page.SetSlot(slotNumber, type, box.curTab, box.selectedNumber);

            //���� ���Կ� ���� �����ۿ� ���� ���� Ȥ�� ����
            if (box.curTab == 3) page.SelectType((int)AnvilType.Crafting);
            else if (box.curTab >= 0 && box.curTab < 3) page.SelectType((int)AnvilType.Carving);

        }
        else if (type == TargetSlotType.Sub && box.curTab == 3)
        {
            SetImage(eventData);
            page.SetSlot(slotNumber, type, box.curTab, box.selectedNumber);
        }
        else if (type == TargetSlotType.Gem && box.curTab == 4)
        {
            SetImage(eventData);
            page.SetSlot(slotNumber, type, box.curTab, box.selectedNumber);
        }
    }

    void SetImage(PointerEventData eventData)
    {
        var targetRect = eventData.pointerDrag.GetComponent<RectTransform>();
        targetRect.anchoredPosition = rect.anchoredPosition;

        itemImage.sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
        itemImage.gameObject.SetActive(true);
        itemImage.SetNativeSize();
    }
}
