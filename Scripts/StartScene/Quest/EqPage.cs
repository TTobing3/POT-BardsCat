using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EqPage : MonoBehaviour
{
    public Transform playerCharacter, player;
    public EqBox eqBox;
    public EqTalentDice[] talentDices;
    [SerializeField] Transform[] eqBoxs;
    [SerializeField] TextMeshProUGUI[] catInfoTexts;

    public GameObject weapon; //버그때메 active 여부 확인하려고 넣은 것

    #region 시스템
    void OnEnable()
    {
        DataCarrier.instance.ResetPlayerItem(0);
        eqBox.curTab = 0;
        ChangeItem();
        CoRepair();
    }
    void Start()
    {
        DataCarrier.instance.ActionChangeItem += ChangeItem;
        DataCarrier.instance.ActionChangeItem += CheckCat;
        //SetCat("BlackCat");
        StartCoroutine(CoAfterStart());
    }

    IEnumerator CoAfterStart()
    {
        yield return null;

        SetCat(DataCarrier.instance.playerData.cat);
        SetArmor();

    }

    private void OnDestroy()
    {
        DataCarrier.instance.ActionChangeItem -= ChangeItem;
        DataCarrier.instance.ActionChangeItem -= CheckCat;
    }
    #endregion
    #region Eq

  
    void SetArmor()
    {
        if(DataCarrier.instance.playerData.item[1] != "")
        {
            SetSprite(1);
            eqBoxs[1].transform.GetChild(1).gameObject.SetActive(true);
            eqBoxs[1].transform.GetChild(2).gameObject.SetActive(false);
            eqBoxs[1].GetComponent<SlotImageChanger>().ItemImageChange(1, DataCarrier.instance.storage.selectItemInfo[1]);
        }
        if (DataCarrier.instance.playerData.item[2] != "")
        {
            SetSprite(2);
            eqBoxs[2].transform.GetChild(1).gameObject.SetActive(true);
            eqBoxs[2].transform.GetChild(2).gameObject.SetActive(false);
            eqBoxs[2].GetComponent<SlotImageChanger>().ItemImageChange(2, DataCarrier.instance.storage.selectItemInfo[2]);
        }
        if (DataCarrier.instance.playerData.item[3] != "")
        {
            eqBoxs[3].transform.GetChild(1).gameObject.SetActive(true);
            eqBoxs[3].transform.GetChild(2).gameObject.SetActive(false);
            eqBoxs[3].GetComponent<SlotImageChanger>().ItemImageChange(3, DataCarrier.instance.storage.selectItemInfo[3]);
        }
    }
    
    void ChangeItem()
    {
        if(DataCarrier.instance.storage.selectItemInfo[eqBox.curTab] == -1)
        {
            eqBoxs[eqBox.curTab].transform.GetChild(1).gameObject.SetActive(false);
            eqBoxs[eqBox.curTab].transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            eqBoxs[eqBox.curTab].transform.GetChild(1).gameObject.SetActive(true);
            eqBoxs[eqBox.curTab].transform.GetChild(2).gameObject.SetActive(false);

            eqBoxs[eqBox.curTab].GetComponent<SlotImageChanger>().ItemImageChange(eqBox.curTab, DataCarrier.instance.storage.selectItemInfo[eqBox.curTab]);
        }
        SetSprite(eqBox.curTab);
    }

    public void CheckCat()
    {
        if (!playerCharacter.gameObject.activeSelf)
        {
            print("이상-!");
            playerCharacter.gameObject.SetActive(true);
        }
    }

    void CoRepair()
    {
        playerCharacter.gameObject.SetActive(false);
        weapon.SetActive(true);
        StartCoroutine(CoRepairCat());
    }

    IEnumerator CoRepairCat()
    {
        yield return null;
        playerCharacter.gameObject.SetActive(true);
    }

    void SetSprite(int _tab)
    {
        if(_tab == 0)
        {
            var tmpWeapon = DataCarrier.instance.playerData.item[(int)Tab.Weapon];

            if (tmpWeapon == "") return;
            StartCoroutine(CoWeaponChange(tmpWeapon));

            /*
            playerCharacter.GetComponent<Animator>().SetInteger("Form", 1);
            playerCharacter.GetComponent<Animator>().SetInteger("Weapon", DataManager.instance.AllWeaponDatas[tmpWeapon].type);
            playerCharacter.GetComponent<MotionAnimator>().ReceiveAnimation(0, 0, false);

            playerCharacter.GetComponent<PlayerSpriteSetter>().ChangeWeapon(DataCarrier.instance.playerData.item[eqBox.curTab]);
            playerCharacter.transform.localScale = new Vector3(108, 108);
            */
        }
        else if(_tab == 1 )
        {
            playerCharacter.GetComponent<PlayerSpriteSetter>().ChangeHelmet(DataCarrier.instance.playerData.item[_tab]);
            player.GetComponent<PlayerSpriteSetter>().ChangeHelmet(DataCarrier.instance.playerData.item[_tab]);
        }
        else if(_tab == 2)
        {
            playerCharacter.GetComponent<PlayerSpriteSetter>().ChangeArmor(DataCarrier.instance.playerData.item[_tab]);
            player.GetComponent<PlayerSpriteSetter>().ChangeArmor(DataCarrier.instance.playerData.item[_tab]);
        }
    }

    IEnumerator CoWeaponChange(string tmpWeapon)
    {
        playerCharacter.GetComponent<Animator>().enabled = false;
        yield return new WaitForEndOfFrame();
        playerCharacter.GetComponent<Animator>().enabled = true;
        playerCharacter.GetComponent<Animator>().SetInteger("Form", 1);
        playerCharacter.GetComponent<Animator>().SetInteger("Weapon", DataManager.instance.AllWeaponDatas[tmpWeapon].type);
        playerCharacter.GetComponent<MotionAnimator>().ReceiveAnimation(0, 0, false);

        playerCharacter.GetComponent<PlayerSpriteSetter>().ChangeWeapon(DataCarrier.instance.playerData.item[eqBox.curTab]);
        playerCharacter.transform.localScale = new Vector3(108, 108);
    }
    #endregion
    #region Cat

    //called by button
    public void ChangeCat(bool _isRight)
    {
        if(DataCarrier.instance.storage.cats.Count == 0) DataCarrier.instance.AddCatToStorage("BlackCat");

        if (_isRight)
        {
            DataCarrier.instance.storage.selectCatInfo = DataCarrier.instance.storage.selectCatInfo >= DataCarrier.instance.storage.cats.Count - 1 ?
                0 : DataCarrier.instance.storage.selectCatInfo + 1;
        }
        else
        {
            DataCarrier.instance.storage.selectCatInfo = DataCarrier.instance.storage.selectCatInfo <= 0 ?
                DataCarrier.instance.storage.cats.Count - 1 : DataCarrier.instance.storage.selectCatInfo - 1;
        }

        DataCarrier.instance.playerData.cat = DataCarrier.instance.storage.cats[DataCarrier.instance.storage.selectCatInfo];

        SetCat(DataCarrier.instance.playerData.cat);

        /*
        playerCharacter.GetComponent<PlayerSpriteSetter>().ChangeCat(DataCarrier.instance.playerData.cat);


        string catStat = "";
        foreach (int i in DataManager.instance.AllCatDatas[DataCarrier.instance.playerData.cat].stats) catStat += i + "\n";
        catInfoTexts[0].text = catStat;
        catInfoTexts[1].text = DataManager.instance.AllCatDatas[DataCarrier.instance.playerData.cat].story;
        */
    }

    void ChangePageText()
    {
        var catData = DataManager.instance.AllCatDatas[DataCarrier.instance.playerData.cat];

        catInfoTexts[0].text = catData.name[1];
        catInfoTexts[1].text = "고유 특성 - " + catData.ability;
        catInfoTexts[2].text = catData.story;
    }

    void SetCat(string _cat)
    {
        if (_cat == "") return;
        DataCarrier.instance.storage.selectCatInfo = DataCarrier.instance.storage.cats.IndexOf(_cat);
        DataCarrier.instance.playerData.cat = _cat;

        playerCharacter.GetComponent<PlayerSpriteSetter>().ChangeCat(_cat);
        player.GetComponent<PlayerSpriteSetter>().ChangeCat(_cat);

        ChangePageText();
        SetTalentDices(_cat);
    }

    void SetTalentDices(string _cat)
    {
        var catData = DataManager.instance.AllCatDatas[_cat];

        foreach (EqTalentDice i in talentDices) i.gameObject.SetActive(false);

        for(int i = 0; i<catData.talent.Length; i++)
        {
            talentDices[i].gameObject.SetActive(true);
            talentDices[i].Set(catData.talent[i]);
            talentDices[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-50 * (catData.talent.Length-1) + 100 * i, 0);
        }
    }
    #endregion
}
