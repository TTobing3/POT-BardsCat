using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InfoBar : MonoBehaviour
{
    public TextMeshProUGUI goldText, honorText, titleText, rankText;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        DataCarrier.instance.ActionChangeGold += ChangeGold;
        DataCarrier.instance.ActionChangeHonor += ChangeHonor;
        DataCarrier.instance.CallChangeGold();
        DataCarrier.instance.CallChangeHonor();
    }

    public void Set()
    {
        rectTransform.DOAnchorPosY(0, 1f);
    }

    public void ChangeGold()
    {
        goldText.text = DataCarrier.instance.storage.gold+"";
    }
    public void ChangeHonor()
    {
        honorText.text = DataCarrier.instance.storage.honor + "";
    }

}
