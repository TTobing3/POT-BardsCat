using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


/*
사용법
생성
 StartSceneManager.instance.toolTip.SetBox(item, category); storage 데이터 넘기는 거
 StartSceneManager.instance.toolTip.gameObject.SetActive(true);
삭제
 StartSceneManager.instance.toolTip.SetBox(item, category);
+
 update 에서 나가면 삭제하는거까지
 */

public class Tooptip : MonoBehaviour
{
    public GameObject skillCell;
    public RectTransform rect;

    [Header("Text")]
    public string itemName;
    public string rank, from;
    public List<string> ability, skill;
    public string des, price;

    [Header("TMP")]
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI fromText, rankText, abilityText;
    public List<TextMeshProUGUI> skillText;
    public TextMeshProUGUI desText, priceText;

    [Header("Rect")]
    public RectTransform boxRect;
    public RectTransform sectionRect, footerRect, skillsRect, hr;
    public List<RectTransform> skillCells;

    [Header("Sub")]
    public GameObject subPrefab;
    public List<GameObject> subTooltips;
    public List<RectTransform> activatedSubTooltips;

    public void SetBox(int _item, int _category)
    {
        gameObject.SetActive(true);
        SetInfo(_item, _category);

        SetHeader();
        SetSection();
        SetFooter();


        if (ability.Count == 0 && skill.Count == 0)
        {
            boxRect.sizeDelta = new Vector2(boxRect.sizeDelta.x,
            130 +
            footerRect.sizeDelta.y);
        }
        else
        {

            boxRect.sizeDelta = new Vector2(boxRect.sizeDelta.x,
            130 +
            sectionRect.sizeDelta.y +
            30 +
            footerRect.sizeDelta.y);
        }

        rect.anchoredPosition = new Vector2( Input.mousePosition.x * (1920.0f / Screen.width), Input.mousePosition.y * (1080.0f / Screen.height)) ;

        AdjustPosition();

        foreach(string i in ability)
            AddSubToolTip(i);

    }
    public void SetBox(string _item, int _category)
    {
        gameObject.SetActive(true);
        SetInfo(_item, _category);

        SetHeader();
        SetSection();
        SetFooter();


        if (ability.Count == 0 && skill.Count == 0)
        {
            boxRect.sizeDelta = new Vector2(boxRect.sizeDelta.x,
            130 +
            footerRect.sizeDelta.y);
        }
        else
        {

            boxRect.sizeDelta = new Vector2(boxRect.sizeDelta.x,
            130 +
            sectionRect.sizeDelta.y +
            30 +
            footerRect.sizeDelta.y);
        }

        rect.anchoredPosition = new Vector2(Input.mousePosition.x * (1920.0f / Screen.width), Input.mousePosition.y * (1080.0f / Screen.height));

        AdjustPosition();

        foreach (string i in ability)
            AddSubToolTip(i);

    }

    #region Tooltip Setting

    void ResetInfo()
    {
        itemName = "";
        rank = "";
        from = "";
        ability = new List<string>();
        skill = new List<string>();
        des = ""; 
        price = "";
    }

    void SetInfo(int _item, int _category)
    {
        ResetInfo();

        itemName = DataCarrier.instance.storage.items[_category][_item];

        if ((int)Tab.Weapon == _category)
        {
            var weaponData = DataManager.instance.AllWeaponDatas[itemName];

            rank = weaponData.rank;
            from = weaponData.from;
            des = weaponData.backstory;
            price = weaponData.price + "";

            ability = new List<string>();
            skill = new List<string>();

            foreach (string i in weaponData.abilities) ability.Add(i);
            foreach (string[] i in weaponData.effects) skill.Add(i[0]);
        }
        else if((int)Tab.Armor == _category || (int)Tab.Helmet == _category || (int)Tab.Acc == _category)
        {
            var armorData = DataManager.instance.AllArmorDatas[itemName];

            rank = armorData.rank;
            from = armorData.from;
            des = armorData.backstory;
            price = armorData.price + "";

            ability = new List<string>();

            foreach (string i in armorData.abilities) ability.Add(i);
        }
        else if ((int)Tab.Material == _category || (int)Tab.Gem == _category)
        {
            var chipData = DataManager.instance.AllChipDatas[itemName];

            rank = chipData.rank;
            from = chipData.from;
            des = chipData.backstory;
            price = chipData.price + "";
        }
    }
    void SetInfo(string _item, int _category)
    {
        ResetInfo();

        itemName = _item;

        if ((int)Tab.Weapon == _category)
        {
            var weaponData = DataManager.instance.AllWeaponDatas[itemName];

            rank = weaponData.rank;
            from = weaponData.from;
            des = weaponData.backstory;
            price = weaponData.price + "";

            ability = new List<string>();
            skill = new List<string>();

            foreach (string i in weaponData.abilities) ability.Add(i);
            foreach (string[] i in weaponData.effects) skill.Add(i[0]);
        }
        else if ((int)Tab.Armor == _category || (int)Tab.Helmet == _category || (int)Tab.Acc == _category)
        {
            var armorData = DataManager.instance.AllArmorDatas[itemName];

            rank = armorData.rank;
            from = armorData.from;
            des = armorData.backstory;
            price = armorData.price + "";

            ability = new List<string>();

            foreach (string i in armorData.abilities) ability.Add(i);
        }
        else if ((int)Tab.Material == _category || (int)Tab.Gem == _category)
        {
            var chipData = DataManager.instance.AllChipDatas[itemName];

            rank = chipData.rank;
            from = chipData.from;
            des = chipData.backstory;
            price = chipData.price + "";
        }
    }

    void SetHeader()
    {
        itemText.text = itemName;
        fromText.text = from;
        rankText.text = rank;
    }

    void SetSection()
    {
        SetAbility();
        SetSkill();

        sectionRect.sizeDelta = new Vector2(
            sectionRect.sizeDelta.x,
            abilityText.preferredHeight + skillsRect.sizeDelta.y +
            20);
    }

    void SetAbility()
    {
        abilityText.text = "";

        for (int i = 0; i < ability.Count; i++)
        {
            abilityText.text += '['+ability[i]+ "] ";
        }
    }

    void SetSkill()
    {
        skillsRect.anchoredPosition = new Vector2(0, -abilityText.preferredHeight);
        skillsRect.sizeDelta = new Vector2(0, 0);

        foreach (string i in skill)
        {
            RectTransform cell = null;

            foreach(RectTransform j in skillCells)
            {
                if(!j.gameObject.activeSelf)
                {
                    cell = j;
                    break;
                }
            }

            if (cell == null)
            {
                cell = Instantiate(skillCell, skillsRect.transform).GetComponent<RectTransform>();
                skillCells.Add(cell);
            }
            //기존에 만들어놓은 

            cell.anchoredPosition = new Vector2(0, -(skillsRect.sizeDelta.y + 20));
            cell.GetComponent<SkillCell>().Set(i, DataManager.instance.SearchEffectData(i).effectDes);

            skillsRect.sizeDelta = new Vector2(skillsRect.sizeDelta.x, skillsRect.sizeDelta.y + cell.sizeDelta.y + 20);
        }

    }

    void SetFooter()
    {
        if (ability.Count == 0 && skill.Count == 0)
        {
            hr.gameObject.SetActive(false);
            desText.fontSize = 32;
            desText.fontStyle = FontStyles.Normal;
            desText.color = Color.white;
        }
        else
        {
            hr.gameObject.SetActive(true);
            desText.fontSize = 24;
            desText.fontStyle = FontStyles.Italic;
            desText.color = new Color(0.65f, 0.65f, 0.65f);
        }


        desText.text = des;
        priceText.text = price + "G";
        footerRect.sizeDelta = new Vector2(footerRect.sizeDelta.x, 10 + 4 + 30 + desText.preferredHeight + 30);
        footerRect.anchoredPosition = new Vector2(footerRect.anchoredPosition.x, 30);
    }

    void AdjustPosition()
    {
        // UI 오브젝트의 화면 좌표를 계산합니다.
        Vector2 screenPos = rect.anchoredPosition;

        // 화면의 크기를 가져옵니다.
        Vector2 screenSize = new Vector2(1920, 1080);

        // UI 오브젝트의 크기를 가져옵니다.
        Vector2 size = rect.sizeDelta;

        // UI 오브젝트가 화면 바깥으로 나가는 방향을 확인합니다.
        rect.pivot = new Vector2(0, 0);

        if (screenPos.x + size.x > screenSize.x) // 오른쪽으로 나가는 경우
        {
            rect.pivot = new Vector2(1, rect.pivot.y);
        }
        if (screenPos.y + size.y > screenSize.y) // 위로 나가는 경우
        {
            rect.pivot = new Vector2(rect.pivot.x, 1);
        }
    }

    #endregion

    void AddSubToolTip(string _ability)
    {
        GameObject sub = null;
        foreach(GameObject i in subTooltips)
        {
            if (!i.activeSelf)
            {
                sub = i;
                break;
            }
        }
        if(sub == null)
        {
            sub = Instantiate(subPrefab, transform);
            subTooltips.Add(sub);
        }
        sub.SetActive(true);

        sub.GetComponent<TooltipSub>().Set(_ability, DataManager.instance.AllAbilityDatas[_ability].story);

        StartCoroutine( CoSetSubPosition(sub.GetComponent<RectTransform>()) );

    }
    IEnumerator CoSetSubPosition(RectTransform _sub)
    {
        yield return new WaitForEndOfFrame();

        _sub.GetComponent<TooltipSub>().SetSize();

        var poxX = rect.sizeDelta.x;
        var poxY = 0f;

        if (CheckSubOut()) 
            poxX = -_sub.sizeDelta.x;
        foreach (RectTransform i in activatedSubTooltips) 
            poxY -= i.sizeDelta.y;

        _sub.anchoredPosition = new Vector2(poxX, poxY);

        activatedSubTooltips.Add(_sub);
    }
    void SetSubPosition(RectTransform _sub)
    {
    }

    bool CheckSubOut()
    {
        // UI 오브젝트의 화면 좌표를 계산합니다.
        Vector2 screenPos = rect.anchoredPosition;

        // 화면의 크기를 가져옵니다.
        Vector2 screenSize = new Vector2(1920, 1080);

        // UI 오브젝트의 크기를 가져옵니다.
        Vector2 size = rect.sizeDelta;

        if (screenPos.x + size.x + 360 > screenSize.x) // 오른쪽으로 나가는 경우
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDisable()
    {
        foreach (RectTransform i in skillCells) i.gameObject.SetActive(false);
        foreach (GameObject i in subTooltips) i.gameObject.SetActive(false);
        activatedSubTooltips.Clear();
    }

}
