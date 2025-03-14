using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Category : MonoBehaviour
{
    //TextMeshProUGUI[] headText

    [SerializeField] Transform attributeParents, totalAttribute; // 토탈이 계, totalAttribute.GetChild(1)
    [SerializeField] GameObject attributePrefab;

    [SerializeField] TextMeshProUGUI[] texts; // 카테고리명 / 이름 / 가치
    [SerializeField] Sprite[] icons;

    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Set(int _n)
    {
        #region TMP 세팅
        foreach (TextMeshProUGUI i in texts) i.text = "";

        texts[1].text = "이름";

        switch (_n)
        {
            case (int)ResultType.Monster:
                texts[0].text = "처치한 괴수";
                texts[2].text = "명예";
                break;
            case (int)ResultType.Chip:
                texts[0].text = "획득한 부산물";
                texts[2].text = "가치";
                break;
            case (int)ResultType.Material:
                texts[0].text += "획득한 재료"; //및 보석
                texts[2].text = "가치";
                break;
            case (int)ResultType.Eq:
                texts[0].text += "획득한 장비";
                texts[2].text = "가치";
                break;
            case (int)ResultType.Gem:
                texts[0].text += "획득한 보석";
                texts[2].text = "가치";
                break;
        }
        #endregion

        var totalValue = 0;

        for(int i = 0; i< BattleDataManager.instance.reportResultLists[_n].Count; i++)
        {
            var tmp = Instantiate(attributePrefab, attributeParents).transform;

            // 이름
            var itemData = BattleDataManager.instance.reportResultLists[_n][i]; // [1] 0 : 이름 1 : 개수 [2] 0 : 이름 1 : 장비 타입

            if( (int)ResultType.Eq != _n && (int)ResultType.Gem != _n)
                tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemData[0] + "x" + itemData[1];
            else
                tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemData[0];


            // 가치 계산
            var itemValue = BattleDataManager.instance.ValueCalculator(itemData, (ResultType)_n);

            tmp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemValue + "";

            // 토탈 가치 합산
            totalValue += itemValue;

            // 가치 아이콘
            tmp.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = icons[_n];
            //tmp.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = icons[_n];

            //점선
            tmp.transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = 
                new Vector2(400 - tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().preferredWidth, 20);
        }

        totalAttribute.GetChild(1).GetChild(0).GetComponent<Image>().sprite = icons[_n];
        totalAttribute.GetChild(1).GetComponent<TextMeshProUGUI>().text = totalValue.ToString(); //합계

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + (attributeParents.childCount * 40));

        // BattleDataManager.instance.reportResultLists[_n]

    }

}
