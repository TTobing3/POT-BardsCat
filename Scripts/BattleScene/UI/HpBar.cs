using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HpBar : MonoBehaviour
{

    public Slider hpSlider;

    float curHp = 0;


    public void SetMaxHP(int _maxHp)
    {
        curHp = _maxHp;
        hpSlider.maxValue = curHp;
        hpSlider.value = curHp;

        SetHP(_maxHp);
    }

    public bool IsZero()
    {
        return hpSlider.value <= 0.5f;
    }

    public void SetHpImmediately(int _maxHp, int _hp)
    {
        curHp = _maxHp;
        hpSlider.maxValue = curHp;
        hpSlider.value = curHp;

        SetHP(_hp);
    }

    void Update()
    {
        if (hpSlider.value > curHp) hpSlider.value = hpSlider.value > curHp ? hpSlider.value - ((hpSlider.value - curHp) * 0.1f) : curHp;
        else if (hpSlider.value < curHp ) hpSlider.value = hpSlider.value < curHp ? hpSlider.value + ((hpSlider.value + curHp) * 0.005f) : curHp;

    }

    public void SetHP(int _hp)
    {
        curHp = _hp;
    }
}
