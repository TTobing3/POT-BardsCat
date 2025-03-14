using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EqTalentDice : MonoBehaviour
{
    [SerializeField] Sprite[] eyes, dices;
    [SerializeField] Image eye, dice;
    [SerializeField] TextMeshProUGUI talentText;

    public void Set(string _talent)
    {
        var talent = DataManager.instance.AllCatTalentDatas[_talent];

        talentText.text = talent.name;

        eye.sprite = eyes[talent.eye - 1];
        eye.SetNativeSize();

        switch (talent.rare)
        {
            case "»Á«‘":
                dice.sprite = dices[1];
                break;
            case "±Õ«‘":
                dice.sprite = dices[2];
                break;
            default:
                dice.sprite = dices[0];
                break;
        }
    }
}
