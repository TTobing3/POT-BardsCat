using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class StoreItem : MonoBehaviour
{
    public StorePage storePage;
    public int number;
    string item;
    int category = -1, price = 0;
    public TextMeshProUGUI priceText;

    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Set(int _category, string _item)
    {
        GetComponent<Image>().raycastTarget = true;
        item = _item;
        category = _category;
        switch (_category)
        {
            case 0: price =  DataManager.instance.AllWeaponDatas[_item].price;break; 
            case 1: price = DataManager.instance.AllArmorDatas[_item].price; ; break;
            case 2: price = DataManager.instance.AllArmorDatas[_item].price; ; break;
            case 3: price = DataManager.instance.AllArmorDatas[_item].price; ; break;
            case 4: price = DataManager.instance.AllChipDatas[_item].price; ; break;
            case 5: price = DataManager.instance.AllChipDatas[_item].price; ; break;
        }

        priceText.text = price + "G";

        GetComponent<SlotImageChanger>().ItemImageChange(_category, _item);

        rect.DOAnchorPos(new Vector2(Random.Range(-800,400), Random.Range(-300, 300)), 1);
    }
    public void SetTooltip(bool _isEnter)
    {

        if (_isEnter)
        {
            StartSceneManager.instance.toolTip.SetBox(item, category);
        }
        else
        {
            StartSceneManager.instance.toolTip.gameObject.SetActive(false);
        }
    }

    public void Buy()
    {

        StartSceneManager.instance.PlayTalk("������ ����",
            new List<System.Action>()
            {
                Check
            },
            new List<string>()
            {
                    "��! ���� ���� �ֱ���?"
            }, 0);
    }

    public void Check()
    {
        if(DataCarrier.instance.storage.gold > price)
        {
            DataCarrier.instance.GainGold(-price);
            DataCarrier.instance.AddItemToStorage(category, item);
            StartSceneManager.instance.PlayTalk("������ ����",
                new List<System.Action>()
                {
                    SoldOut
                },
                new List<string>()
                {
                    "�� �̰� �������� ���� ������!"
                }, 0);
        }
        else
        {

            StartSceneManager.instance.PlayTalk("������ ����",
                new List<System.Action>()
                {
                },
                new List<string>()
                {
                    "���� �����鼭... ���� ���� �峭��?"
                }, 0);
        }
    }

    void SoldOut()
    {
        GetComponent<Image>().raycastTarget = false;

        rect.DOAnchorPos(new Vector2(1300, 0), 0.5f);

        storePage.Sell(number);
    }

}
