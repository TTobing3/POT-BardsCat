using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ManaBar : MonoBehaviour
{
    [SerializeField] Slider manaSlider;

    float curMana = 0;

    public void SetMaxMana()
    {
        curMana = GameManager.instance.player.weapon.mana;
        manaSlider.maxValue = curMana;
        manaSlider.value = curMana;
        
        SetMana();
    }

    void Update()
    {
        if(manaSlider.value > curMana) manaSlider.value = manaSlider.value > curMana ? manaSlider.value - ((manaSlider.value - curMana) * 0.1f) : curMana;
        else if (manaSlider.value < curMana) manaSlider.value = manaSlider.value < curMana ? manaSlider.value + ((manaSlider.value + curMana) * 0.005f) : curMana;

    }

    public void SetMana()
    {
        curMana = GameManager.instance.player.mp;
    }
}
