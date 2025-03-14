using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAct : MonoBehaviour
{
    Monster monster;
    MonsterStateDecider monsterStateDecider;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        monsterStateDecider = GetComponent<MonsterStateDecider>();
    }

    //called by animation
    void Act()
    {
        InstantiateEffect(monsterStateDecider.curPattern.effect);
    }

    void InstantiateEffect(string _eftName)// 패턴에서 호출하는거지
    {
        if (_eftName == "None") return;
        Effect tmpEft = null;

        foreach (GameObject i in BattleDataManager.instance.effects)
        {
            if (!i.activeSelf)
            {
                tmpEft = i.GetComponent<Effect>();
                break;
            }
        }

        if (tmpEft == null)
        {
            tmpEft = Instantiate(GameManager.instance.effectObj).GetComponent<Effect>();
            BattleDataManager.instance.effects.Add(tmpEft.gameObject);
        }

        tmpEft.Set(transform, false, DataManager.instance.SearchEffectData(monster.monsterData.prefab+'_'+_eftName), monsterStateDecider.curDir, null);
    }
}
