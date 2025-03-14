using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultPaper : MonoBehaviour
{
    public Transform categoryParents;
    public GameObject categoryPrefab;

    public RectTransform paperRect, rewardRect;
    public List<RectTransform> categoryRects = new List<RectTransform>();

    [SerializeField] TextMeshProUGUI[] rewards;

    public void Set()
    {
        //카테고리 생성

        //BattleDataManager.instance.reportResultLists

        //페이퍼 크기 <- 카테고리 크기 <- 카테고리 내용

        float tmpCategoryHeightSum = 0;

        for (int i = 0; i < BattleDataManager.instance.reportResultLists.Length; i++)
        {
            var tmpCategory = Instantiate(categoryPrefab, categoryParents);
            tmpCategory.GetComponent<Category>().Set(i);

            categoryRects.Add(tmpCategory.GetComponent<RectTransform>());
            categoryRects[i].anchoredPosition = new Vector2(0, -( 150 + 20*i + tmpCategoryHeightSum ));
            tmpCategoryHeightSum += categoryRects[i].sizeDelta.y;
        }

        //

        //DataCarrier.instance.questData.rewards
        for(int i = 0; i< DataCarrier.instance.questData.rewards.Length; i++)
        {
            if (DataCarrier.instance.questData.rewards[i].Split(':')[0] == "명예")
            {
                rewards[0].text = DataCarrier.instance.questData.rewards[i].Split(':')[1];
            }
            else if(DataCarrier.instance.questData.rewards[i].Split(':')[0] == "금화")
            {
                rewards[1].text = DataCarrier.instance.questData.rewards[i].Split(':')[1];
            }
            else
            {
                if (rewards[2].text != "") rewards[2].text += "\n";
                rewards[2].text += $"{DataCarrier.instance.questData.rewards[i].Split(':')[1]}";
            }
        }
        //

        //

        rewardRect.anchoredPosition = new Vector2(0, rewardRect.anchoredPosition.y + rewards[2].preferredHeight + 20);

        //

        paperRect.sizeDelta = new Vector2(610, tmpCategoryHeightSum + 150 + 20 * BattleDataManager.instance.reportResultLists.Length + 50 + rewards[2].preferredHeight + 240);
        paperRect.anchoredPosition = new Vector2(0, 0);
    }
}
