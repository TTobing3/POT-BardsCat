using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StorePage : MonoBehaviour
{
    public RectTransform traderTransform;
    public RectTransform tableRect;
    public RectTransform backButtonRect;

    public StoreItem[] items;

    WeaponData weapon;
    ArmorData armor;
    ChipData chip;

    bool[] soldout = new bool[3];

    private void Start()
    {
    }

    public void Sell(int _n)
    {
        soldout[_n] = true;
    }

    public void SetStore(bool _set)
    {
        if (_set)
        {
            tableRect.DOAnchorPosY(0, 0.6f).OnComplete(() => {
                backButtonRect.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutBack);
            });
        }
        //무작위

        if(weapon == null)
        {
            WeaponData tmpWeapon;
            do
            {
                tmpWeapon = DataManager.instance.AllWeaponDataList[Random.Range(0, DataManager.instance.AllWeaponDataList.Count)];
            }
            while (tmpWeapon.name == "빈 손");
            weapon = tmpWeapon;
        }
           
        if(armor == null)
            armor = DataManager.instance.AllArmorDataList[Random.Range(0, DataManager.instance.AllArmorDataList.Count)];
        if(chip == null)
        {
            ChipData tmpChip;
            do
            {
                tmpChip = DataManager.instance.AllChipDataList[Random.Range(0, DataManager.instance.AllChipDataList.Count)];
            }
            while (tmpChip.category == "부산물");

            chip = tmpChip;
        }

        //템세팅
        traderTransform.anchoredPosition = new Vector3(500, -380, 0);
        traderTransform.DOAnchorPosX(-100, 0.5f).SetEase(Ease.InOutBack);

        StartSceneManager.instance.PlayTalk("떠돌이 상인",
            new List<System.Action>()
            {
            },
            new List<string>()
            {
                    "어이, 물건 좀 사가는 거 어때?"
            }, 0);

        if (!soldout[0]) items[0].Set(0, weapon.name);
        if (!soldout[1])
        {
            if (armor.type == "Helmet")
                items[1].Set(1, armor.name);
            if (armor.type == "Armor")
                items[1].Set(2, armor.name);
            if (armor.type == "Acc")
                items[1].Set(3, armor.name);
        }
        if (!soldout[2])
        {
            if (chip.category == "재료")
                items[2].Set(4, chip.name);
            if (chip.category == "보석")
                items[2].Set(5, chip.name);
        }

    }
    public void BackButton()
    {
        tableRect.DOAnchorPosY(1200, 0.6f);
        backButtonRect.DOAnchorPosX(-200, 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
            gameObject.SetActive(false);
            StartSceneManager.instance.SetButtonsInteractable(true);
        });
        traderTransform.DOAnchorPosX(500, 0.5f).SetEase(Ease.InOutBack);
        foreach (StoreItem i in items)
            i.GetComponent<RectTransform>().DOMove(new Vector2(1300, 0), 0.5f);
    }

}
