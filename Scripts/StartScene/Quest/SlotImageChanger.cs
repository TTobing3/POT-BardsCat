using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class SlotImageChanger : MonoBehaviour
{

    public void ItemImageChange(int _category, int _itemSlotNumber)// 이름을 줘야되나
    {
        if (DataCarrier.instance.storage.items[_category].Count == 0) return;

        string tmpCategory = "";

        switch(_category)
        {
            case 0: tmpCategory = "Weapon"; break;
            case 1: tmpCategory = "Helmet"; break;
            case 2: tmpCategory = "Armor"; break;
            case 3: tmpCategory = "Acc"; break;
            case 4: tmpCategory = "Material"; break;
            case 5: tmpCategory = "Gem"; break;
        }

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<SpriteResolver>().SetCategoryAndLabel(tmpCategory, DataCarrier.instance.storage.items[_category][_itemSlotNumber]);
        transform.GetChild(1).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Resize();

    }
    public void ItemImageChange(int _category, string _item)// 이름을 줘야되나
    {
        if (DataCarrier.instance.storage.items[_category].Count == 0) return;

        string tmpCategory = "";

        switch (_category)
        {
            case 0: tmpCategory = "Weapon"; break;
            case 1: tmpCategory = "Helmet"; break;
            case 2: tmpCategory = "Armor"; break;
            case 3: tmpCategory = "Acc"; break;
            case 4: tmpCategory = "Material"; break;
            case 5: tmpCategory = "Gem"; break;
        }

        
        GetComponent<SpriteResolver>().SetCategoryAndLabel(tmpCategory, _item);
        GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;

        GetComponent<Image>().SetNativeSize();

    }

    public void Resize()
    {
        var size = transform.GetChild(1).GetComponent<Image>().sprite.rect.size;
        var tmpSize = size.x > size.y ? new Vector2(1, size.y / size.x) : new Vector2(size.x / size.y, 1);
        tmpSize = Mathf.Abs(tmpSize.x - tmpSize.y) < 0.2f ? new Vector2(tmpSize.x - 0.4f, tmpSize.y - 0.4f) : tmpSize;
        transform.GetChild(1).GetComponent<RectTransform>().localScale = tmpSize;
    }
}
