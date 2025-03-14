using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CatInfo : MonoBehaviour
{
    public PlayerSpriteSetter player;

    public TextMeshProUGUI nameText, talentText;
    public TextMeshProUGUI stat_0, stat_1;
    public TextMeshProUGUI backstory;
    public TextMeshProUGUI abilityTitle, ability;

    public void Set()
    {
        nameText.text = "";
        talentText.text = "";

        var cat = DataManager.instance.AllCatDatas[DataCarrier.instance.playerData.cat];

        player.ChangeCat(cat.name[0]);

        nameText.text = cat.name[1];

        foreach(string i in cat.talent)
        {
            var talent = DataManager.instance.AllCatTalentDatas[i];
            talentText.text = $"{talent.name} [ {talent.effect} ]\n";
        }
        talentText.text = talentText.text.TrimEnd('\n');

        stat_0.text = $"���� : {cat.stats[0]}\n���� : {cat.stats[1]}\n�ӵ� : {cat.stats[2]}\nȸ�� : {cat.stats[7]}";
        stat_1.text = $"���� : {cat.stats[3]}\n���� : {cat.stats[4]}\n��� : {cat.stats[5]}\n���� : {cat.stats[6]}";

        backstory.text = cat.story;

        abilityTitle.text = "���� Ư�� : " + cat.ability;
        ability.text = DataManager.instance.AllCatAbilityDatas[cat.ability].des;
    }
}
