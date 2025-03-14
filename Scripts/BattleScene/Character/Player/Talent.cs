using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Talent : MonoBehaviour
{
    Player player;
    PlayerAct playerAct;

    public List<OptionData> talent;
    public List<string> abilities; // ¹«±â Àåºñ

    private void Awake()
    {
        player = GetComponent<Player>();
        playerAct = GetComponent<PlayerAct>();
    }
    public void GainTalent(OptionData _optionData)
    {
        talent.Add(_optionData);

        switch(_optionData.optionName)
        {
            case "½À°ü¼º ºÒµ¢ÀÌ":
                // 5ÃÊ ¸¶´Ù / ºÒµ¢ÀÌ / »ý¼ºÇÑ´Ù
                StartCoroutine(ActLoopTalent(GenerateEffect, "¼Ò¿î¼®", 5));
                break;
        }
    }
    public void SetAbilities()
    {
        
        abilities.AddRange(DataManager.instance.AllWeaponDatas[DataCarrier.instance.playerData.item[0]].abilities.ToList());
        //Çï¸ä
        //°©¿Ê
        //Àå½Å±¸

        foreach (string i in abilities)
        {
            var tmpAbility = DataManager.instance.AllAbilityDatas[i];
            if ( tmpAbility.condition == "È¹µæ" )
            {
                if (tmpAbility.effectType == "´É·ÂÄ¡")
                {

                }
            }

        }
    }

    void SetAbility(AbilityData _abilityData)
    {

    }

    IEnumerator ActLoopTalent(System.Action<string> _action, string _effectName,  int _loopTime)
    {
        _action(_effectName);
        yield return new WaitForSeconds(_loopTime);
        StartCoroutine(ActLoopTalent(_action, _effectName, _loopTime));
    }

    void GenerateEffect(string _effectName)
    {
        playerAct.InstantiateEffect(_effectName);
    }
}

