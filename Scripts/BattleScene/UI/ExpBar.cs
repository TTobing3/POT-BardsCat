using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider expSlider;

    float curExp = 0;


    public void SetMaxExp(int _maxHp)
    {
        curExp = _maxHp;
        expSlider.maxValue = curExp;
        expSlider.value = curExp;

        SetExp(_maxHp);
    }

    void Update()
    {
        if(expSlider.value > curExp) expSlider.value = expSlider.value > curExp ? expSlider.value - ((expSlider.value - curExp) * 0.1f) : curExp;
        else if (expSlider.value < curExp) expSlider.value = expSlider.value < curExp ? expSlider.value + ((expSlider.value + curExp) * 0.01f) : curExp;
    }

    public void SetExp(int _exp)
    {
        curExp = _exp;
    }
}
