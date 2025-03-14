using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCell : MonoBehaviour
{
    public TextMeshProUGUI skillName, des;

    RectTransform rect;


    public void Set(string _name, string _des)
    {
        skillName.text = _name;
        des.text = _des;
        SetSize();

        gameObject.SetActive(true);
    }

    void SetSize()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, skillName.preferredHeight + des.preferredHeight);
    }
}
