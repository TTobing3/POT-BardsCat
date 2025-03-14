using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Linq;
public class MonsterSpriteSetter : MonoBehaviour
{

    float delay = 0.2f;

    Monster monster;

    SpriteResolver[] resolvers;
    SpriteRenderer[] renderers;

    SpriteResolver[] armorResolver = new SpriteResolver[2];

    Coroutine coFlash;

    Material originalMaterial;

    void Awake()
    {
        monster = GetComponent<Monster>();
        resolvers = GetComponentsInChildren<SpriteResolver>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        originalMaterial = renderers[0].material;

        foreach(SpriteResolver i in resolvers)
        {
            if (i.GetCategory() == "Weapon") armorResolver[0] = i;
            if (i.GetCategory() == "Helmet") armorResolver[1] = i;
        }
    }
    public void ChangeArmor()
    {
        StartCoroutine(CoChangeArmor());
    }

    IEnumerator CoChangeArmor()
    {
        yield return new WaitForEndOfFrame();
        armorResolver[0].SetCategoryAndLabel("Weapon", monster.monsterData.equips[0]);
        armorResolver[1].SetCategoryAndLabel("Helmet", monster.monsterData.equips[1]);
    }
    public void FlashWhite()
    {
        if (coFlash != null) StopCoroutine(coFlash);
        coFlash = StartCoroutine(CoFlash());
    }

    IEnumerator CoFlash()
    {
        foreach (SpriteRenderer i in renderers)
        {
            i.material = BattleDataManager.instance.whiteMaterial;
        }

        yield return new WaitForSeconds(delay);

        foreach (SpriteRenderer i in renderers)
        {
            i.material = originalMaterial;
        }

    }
}
