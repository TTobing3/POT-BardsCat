using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHpBar : MonoBehaviour
{

    public Monster monster;
    [SerializeField] TextMeshProUGUI monsterNameText;
    HpBar hpBar;

    private void Awake()
    {
        hpBar = GetComponent<HpBar>();
    }

    private void Update()
    {
        if (gameObject.activeSelf && hpBar.IsZero()) gameObject.SetActive(false); 
    }

    public void Set(Monster _monster)
    {
        if(monster == _monster)
        {
            hpBar.SetHP(monster.hp);
        }
        else
        {
            gameObject.SetActive(true);

            monster = _monster;

            monsterNameText.text = monster.monsterData.name;

            hpBar.SetHpImmediately(monster.maxHp, monster.hp);
        }
    }
}
