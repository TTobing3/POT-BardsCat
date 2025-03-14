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

        stat_0.text = $"생명 : {cat.stats[0]}\n마나 : {cat.stats[1]}\n속도 : {cat.stats[2]}\n회복 : {cat.stats[7]}";
        stat_1.text = $"물리 : {cat.stats[3]}\n마법 : {cat.stats[4]}\n방어 : {cat.stats[5]}\n저항 : {cat.stats[6]}";

        backstory.text = cat.story;

        abilityTitle.text = "고유 특성 : " + cat.ability;
        ability.text = DataManager.instance.AllCatAbilityDatas[cat.ability].des;
    }
}
