using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionPage : MonoBehaviour
{
    public string number;

    OptionData optionData;

    public void SetOption(OptionData _optionData)
    {
        optionData = _optionData;

        transform.GetChild(0).GetComponent<Image>().sprite = optionData.sprite;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = optionData.optionName;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = optionData.description;
    }

    public void SelectOption()
    {
        Time.timeScale = 1;
        UIManager.instance.optionPanel.SetActive(false);

        GameManager.instance.player.GainTalent(optionData);
    }
}
