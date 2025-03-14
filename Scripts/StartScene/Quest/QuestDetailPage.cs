using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using TMPro;

public class QuestDetailPage : MonoBehaviour
{

    public TextMeshProUGUI[] detailTexts = new TextMeshProUGUI[] { };


    SpriteResolver[] spriteResolvers;
    TextMeshProUGUI[] textMeshProUGUIs;

    private void Awake()
    {
        spriteResolvers = GetComponentsInChildren<SpriteResolver>();
        textMeshProUGUIs = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void Set(QuestData _data)
    {
        foreach (SpriteResolver i in spriteResolvers)
        {
            var category = i.GetCategory();

            i.SetCategoryAndLabel(category, _data.type);
            i.GetComponent<Image>().sprite = i.GetComponent<SpriteRenderer>().sprite;
        }

        foreach (TextMeshProUGUI i in textMeshProUGUIs)
        {
            switch(_data.type)
            {
                case "일반":
                    i.color = new Color(0.4f, 0.3f, 0.25f);
                    break;
                case "범죄":
                    i.color = Color.white;
                    break;
                case "제국":
                    i.color = Color.white;
                    break;
            }
        }

        detailTexts[0].text = _data.name;
        detailTexts[1].text = _data.goal;

        detailTexts[2].text = _data.type;
        detailTexts[3].text = _data.target;
        detailTexts[4].text = _data.area;

        string tmpRewardText = "";
        foreach (string i in _data.rewards)
        {
            var rewards = i.Split(':');
            if(rewards[0] == "금화" || rewards[0] == "명예")
            {
                tmpRewardText += rewards[0];
            }
            else
            {
                tmpRewardText += rewards[1];
            }
            tmpRewardText += ',';
        }
        detailTexts[5].text = tmpRewardText.TrimEnd(',');

        detailTexts[6].text = _data.backstory;
    }
}
