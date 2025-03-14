using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.U2D.Animation;

public class QuestPage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] GameObject[] levelIcons;

    public QuestData questData;

    SpriteResolver[] spriteResolvers;

    int number;

    private void Awake()
    {
        spriteResolvers = GetComponentsInChildren<SpriteResolver>();
    }

    public void Set(QuestData _data, int _n)
    {
        questData = _data;
        number = _n;

        foreach(SpriteResolver i in spriteResolvers)
        {
            var category = i.GetCategory();

            if (category == "Illust")
            {
                i.SetCategoryAndLabel(category, _data.target);
            }
            else
            {
                i.SetCategoryAndLabel(category, _data.type);

                if (category == "Lv") 
                    foreach(GameObject j in levelIcons) 
                        j.GetComponent<Image>().sprite = i.GetComponent<SpriteRenderer>().sprite;
      
            }
            i.GetComponent<Image>().sprite = i.GetComponent<SpriteRenderer>().sprite;
        }

        questName.text = questData.name;
        SetLevelIcon();
    }

    void SetLevelIcon()
    {
        for(int i = 0; i< questData.level; i++)
        {
            levelIcons[i].SetActive(true);
        }
    }

    //called by button
    public void SelectQuest()
    {
        StartScene.instance.SelectQuestPage(number);
    }
}
