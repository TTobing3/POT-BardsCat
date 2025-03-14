using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public EqBox eqBox;
    InstantData instantData;

    void Awake()
    {
        instantData = GetComponent<InstantData>();
    }


    void Start()
    {
        DataCarrier.instance.ActionChangeItem += ChangeItem;
        DataCarrier.instance.ActionChangeTab += CheckEquip;
    }


    //called by button 
    //datacarrier.changeItem -> Action / changeItem -> Eqpage.changeitem -> setSprite
    public void SelectItem()
    {
        DataCarrier.instance.ChangeItem(eqBox.curTab, instantData.number);
    }

    //called by pointer enter/exit
    public void SetTooltip(bool _isEnter)
    {
        if (DataCarrier.instance.storage.items[eqBox.curTab].Count <= instantData.number) return;

        if (_isEnter)
        {
            StartSceneManager.instance.toolTip.SetBox(instantData.number, eqBox.curTab);
        }
        else
        {
            StartSceneManager.instance.toolTip.gameObject.SetActive(false);
        }
    }

    void ChangeItem()
    {
        if (DataCarrier.instance.storage.selectItemInfo[eqBox.curTab] == -1)
        {
            transform.GetChild(2).gameObject.SetActive(false);
            return;
        }
        CheckEquip();
    }

    void CheckEquip()
    {
        if (DataCarrier.instance.storage.selectItemInfo[eqBox.curTab] == instantData.number)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }    

    public void SoundPlay(AudioClip _clip)
    {
        SoundManager.instance.SfxPlay(_clip);
    }

    private void OnDestroy()
    {

        DataCarrier.instance.ActionChangeItem -= ChangeItem;
        DataCarrier.instance.ActionChangeTab -= CheckEquip;
    }
}
