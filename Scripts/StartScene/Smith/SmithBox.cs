using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class SmithBox : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab, layout;
    [SerializeField] List<GameObject> slots = new List<GameObject>();

    public GameObject itemDrager;
    public int selectedNumber =-1;
    public bool isSelect = false;
    public int defaultCount = 42, maxRow = 6, curTab = 0;

    RectTransform rect;

    public SmithPage page;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        SetSlot();
    }

    private void Update()
    {
        CheckTooltipOut();
    }

    void CheckTooltipOut()
    {
        // 마우스 위치를 스크린 좌표에서 월드 좌표로 변환합니다.
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        // RectTransformUtility를 사용하여 스크린 좌표를 로컬 좌표로 변환합니다.
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            mousePosition,
            StartSceneManager.instance.canvas.worldCamera,
            out localPoint
        );

        // 마우스가 RectTransform 내부에 있는지 확인합니다.
        if (!isInside || !rect.rect.Contains(localPoint))
        {
            StartSceneManager.instance.toolTip.gameObject.SetActive(false);
            // 이거는 스미쓰 페이지 일때만
            if (!isSelect) itemDrager.SetActive(false);
        }
    }

    void SetSlot()
    {
        int slotCount = 0;
        for (int i = 0; i < 6; i++)
        {
            if(i==1)
            {
                var tmpSumCount = DataCarrier.instance.storage.items[(int)Tab.Helmet].Count + DataCarrier.instance.storage.items[(int)Tab.Armor].Count;
                if (tmpSumCount > slotCount) slotCount = tmpSumCount;
                i++;
                continue;
            }

            if (DataCarrier.instance.storage.items[i].Count > slotCount) slotCount = DataCarrier.instance.storage.items[i].Count;
        }


        if (slotCount > slots.Count && slotCount > defaultCount)
        {
            var tmpCount = slotCount % maxRow == 0 ? slotCount : (slotCount / maxRow + 1) * maxRow;
            for (int j = slots.Count; j < tmpCount; j++)
            {
                GenerateSqure(j);
            }
        }
        else
        {
            for (int j = slots.Count; j < defaultCount; j++)
            {
                GenerateSqure(j);
            }
        }
    }
    void GenerateSqure(int _n)
    {
        var tmpSlot = Instantiate(slotPrefab, layout.transform);

        slots.Add(tmpSlot);

        slots[_n].GetComponent<InstantData>().number = _n;
        slots[_n].GetComponent<SmithSlot>().box = this;

    }
    public void ChangeTab(int _n)
    {
        //public enum Tab { Weapon, Helmet, Armor, Acc, Material, Gem }
        SetSlot();
        curTab = _n;
        layout.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        foreach (GameObject i in slots)
        {
            i.transform.GetChild(1).gameObject.SetActive(false);
            //i.transform.GetChild(3).gameObject.SetActive(false);
        }

        if (_n == 1) // 헬멧과 아머는 같은 탭에
        {
            var helmetCount = DataCarrier.instance.storage.items[(int)Tab.Helmet].Count;
            
            //슬롯 세팅 헬멧
            for (int i = 0; i < helmetCount; i++)
            {
                slots[i].GetComponent<SlotImageChanger>().ItemImageChange((int)Tab.Helmet, i);

                slots[i].GetComponent<SmithSlot>().category = (int)Tab.Helmet;
                slots[i].GetComponent<SmithSlot>().item = i;
            }

            var armorCount = DataCarrier.instance.storage.items[(int)Tab.Armor].Count;

            //슬롯 세팅 아머
            for (int i = 0; i < armorCount; i++)
            {
                slots[i + helmetCount].GetComponent<SlotImageChanger>().ItemImageChange((int)Tab.Armor, i);

                slots[i + helmetCount].GetComponent<SmithSlot>().category = (int)Tab.Armor;
                slots[i + helmetCount].GetComponent<SmithSlot>().item = i;
            }

        }
        else if(_n > 1)//헬멧 다음일 경우 +1
        {
            _n++;
        }

        //슬롯 세팅
        for (int i = 0; i < DataCarrier.instance.storage.items[_n].Count; i++)
        {
            slots[i].GetComponent<SlotImageChanger>().ItemImageChange(_n, i);

            slots[i].GetComponent<SmithSlot>().category = _n;
            slots[i].GetComponent<SmithSlot>().item = i;
        }
    }


}
