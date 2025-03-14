using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerSpriteSetter : MonoBehaviour
{

    // SetCategoryAndLabel(_weapon.category, _weapon.name+ForB(_weapon)[1]);

    SpriteResolver[] resolvers;
    SpriteRenderer[] renderers;

    float delay = 0.2f;
    Coroutine coFlash;

    Material originalMaterial;

    void Awake()
    {
        resolvers = GetComponentsInChildren<SpriteResolver>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        originalMaterial = renderers[0].material;
    }


    public void ChangeWeapon(string _name)
    {
        if (_name == "") return;


        var tmpWeaponData = DataManager.instance.AllWeaponDatas[_name];
        foreach(SpriteResolver i in resolvers)
        {
            if(i.GetCategory() == "Weapon")
            {
                i.SetCategoryAndLabel("Weapon", tmpWeaponData.name);
            }
        }
    }

    public void ChangeArmor(string _name)
    {
        if (_name == "") _name = "None";

        foreach (SpriteResolver i in resolvers)
        {
            if (i.GetCategory() == "Armor")
            {
                i.SetCategoryAndLabel(i.GetCategory(), _name);
            }

        }
    }

    public void ChangeHelmet(string _name)
    {
        if (_name == "") _name = "None";

        foreach (SpriteResolver i in resolvers)
        {
            if (i.GetCategory() == "Helmet")
            {
                i.SetCategoryAndLabel(i.GetCategory(), _name);
            }
        }
    }

    public void ChangeCat(string _cat)
    {
        foreach (SpriteResolver i in resolvers)
        {
            var category = i.GetCategory();
            switch (category)
            {
                case "Arm1B": i.SetCategoryAndLabel(category, _cat); break;
                case "Arm1F": i.SetCategoryAndLabel(category, _cat); break;
                case "Arm2B": i.SetCategoryAndLabel(category, _cat); break;
                case "Arm2F": i.SetCategoryAndLabel(category, _cat); break;
                case "Arm3B": i.SetCategoryAndLabel(category, _cat); break;
                case "Arm3F": i.SetCategoryAndLabel(category, _cat); break;

                case "Leg1B": i.SetCategoryAndLabel(category, _cat); break;
                case "Leg1F": i.SetCategoryAndLabel(category, _cat); break;
                case "Leg2B": i.SetCategoryAndLabel(category, _cat); break;
                case "Leg2F": i.SetCategoryAndLabel(category, _cat); break;
                case "Leg3B": i.SetCategoryAndLabel(category, _cat); break;
                case "Leg3F": i.SetCategoryAndLabel(category, _cat); break;

                case "Head": i.SetCategoryAndLabel(category, _cat); break;
                case "Body": i.SetCategoryAndLabel(category, _cat); break;
                case "Tail": i.SetCategoryAndLabel(category, _cat); break;
            }

        }
    }

    #region WhiteFlash

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

    #endregion
}
