using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Tab { Weapon, Helmet, Armor, Acc, Material, Gem }

[System.Serializable]
public class PlayerData
{
    public string[] item = new string[5];
    public string cat;
}

[System.Serializable]
public class Starage
{

    // 아이템, 타입 아이템  Weapon, Helmet, Armor, Acc, Material, Gem 
    public List<List<string>> items = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() }; //
    public List<string> cats = new List<string>();

    public int[] selectItemInfo = new int[4] { 0, 0, 0, 0 }; //얘네가 storage에 있어야 하나?
    public int selectCatInfo; // 얘네가 storage에 있어야 하나?

    public int gold, honor, level;
    public string[] title, achievement;
}

public class DataCarrier : MonoBehaviour
{
    public System.Action ActionChangeItem, ActionChangeTab, ActionAddToStorage, ActionChangeGold, ActionChangeHonor;

    public static DataCarrier instance;

    public int step = 0;

    public QuestData questData;
    public HuntTargetData huntTargetData;

    public PlayerData playerData = new PlayerData();
    public Starage storage = new Starage();

    public AudioMixer mixer;

    public string story;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        playerData.item[0] = "빈 손";

        AddItemToStorage((int)Tab.Armor, "갑옷");
        AddItemToStorage((int)Tab.Acc, "금강석");

        AddItemToStorage((int)Tab.Material, "고블린 가죽");
        AddItemToStorage((int)Tab.Material, "고블린 가죽");
        AddItemToStorage((int)Tab.Material, "고블린 가죽");
        AddItemToStorage((int)Tab.Material, "철괴");
        AddItemToStorage((int)Tab.Material, "일반 목재");
        AddItemToStorage((int)Tab.Gem, "평범한 보석");
        AddItemToStorage((int)Tab.Helmet, "투구");
        //;
        //
        //AddItemToStorage((int)Tab.Weapon, "목재 지팡이");
        AddItemToStorage((int)Tab.Weapon, "암석 완드");
        AddItemToStorage((int)Tab.Weapon, "두손검");
        AddItemToStorage((int)Tab.Weapon, "목재 지팡이");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "단검");
        AddItemToStorage((int)Tab.Weapon, "완드");
        AddItemToStorage((int)Tab.Weapon, "머스킷 소총");
        AddItemToStorage((int)Tab.Weapon, "머스킷 단총");
        AddItemToStorage((int)Tab.Weapon, "한 손 도끼");
        AddItemToStorage((int)Tab.Weapon, "암석 완드");
        AddItemToStorage((int)Tab.Weapon, "두손검");
        AddItemToStorage((int)Tab.Weapon, "목재 지팡이");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "단검");
        AddItemToStorage((int)Tab.Weapon, "완드");
        AddItemToStorage((int)Tab.Weapon, "머스킷 소총");
        AddItemToStorage((int)Tab.Weapon, "머스킷 단총");
        AddItemToStorage((int)Tab.Weapon, "한 손 도끼");
        AddItemToStorage((int)Tab.Weapon, "암석 완드");
        AddItemToStorage((int)Tab.Weapon, "두손검");
        AddItemToStorage((int)Tab.Weapon, "목재 지팡이");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "단검");
        AddItemToStorage((int)Tab.Weapon, "완드");
        AddItemToStorage((int)Tab.Weapon, "머스킷 소총");
        AddItemToStorage((int)Tab.Weapon, "머스킷 단총");
        AddItemToStorage((int)Tab.Weapon, "한 손 도끼");
        AddItemToStorage((int)Tab.Weapon, "암석 완드");
        AddItemToStorage((int)Tab.Weapon, "두손검");
        AddItemToStorage((int)Tab.Weapon, "목재 지팡이");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "단검");
        AddItemToStorage((int)Tab.Weapon, "완드");
        AddItemToStorage((int)Tab.Weapon, "머스킷 소총");
        AddItemToStorage((int)Tab.Weapon, "머스킷 단총");
        AddItemToStorage((int)Tab.Weapon, "한 손 도끼");
        AddItemToStorage((int)Tab.Weapon, "암석 완드");
        AddItemToStorage((int)Tab.Weapon, "두손검");
        AddItemToStorage((int)Tab.Weapon, "목재 지팡이");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "암정도");
        AddItemToStorage((int)Tab.Weapon, "단검");
        AddItemToStorage((int)Tab.Weapon, "완드");
        AddItemToStorage((int)Tab.Weapon, "머스킷 소총");
        AddItemToStorage((int)Tab.Weapon, "머스킷 단총");
        AddItemToStorage((int)Tab.Weapon, "한 손 도끼");


        AddCatToStorage("BlackCat");
        AddCatToStorage("WhiteCat");

        playerData.cat = Random.Range(0, 2) == 0 ? "BlackCat" : "WhiteCat";


    }


    public void AddItemToStorage(int _category, string _item)
    {
        storage.items[_category].Add(_item);
        if( ActionAddToStorage != null ) ActionAddToStorage();
    }
    public void AddItemToStorage(string _category, string _item)
    {
        int category = 0;
        
        switch(_category)
        {
            case "무기" :
                category = (int)Tab.Weapon;
                break;
            case "투구":
                category = (int)Tab.Helmet;
                break;
            case "갑옷":
                category = (int)Tab.Armor;
                break;
            case "장신구":
                category = (int)Tab.Acc;
                break;
            case "재료":
                category = (int)Tab.Material;
                break;
            case "보석":
                category = (int)Tab.Gem;
                break;
        }

        storage.items[category].Add(_item);
        if (ActionAddToStorage != null) ActionAddToStorage();
    }

    public void AddCatToStorage(string _name)
    {
        storage.cats.Add(_name);
    }

    public void RemoveItemAtStorage(int _category, int _item)
    {
        storage.items[_category].RemoveAt(_item);
    }

    public void RemoveItemAtStorage(int _category, int[] _items)
    {
        foreach (int i in _items)
        {
            if (i == -1) continue;
            storage.items[_category][i] = null;
        }

        storage.items[_category].RemoveAll(x => x == null);


        /* 
            다음과 같은 에러 방지
         
            위치 넘버를 가져다 쓰는거임

            근데 한 번에 여러개를 삭제하면

            리스트 형식일 때

            그러면 삭제하면, 이전에 있던 애의 위치 번호에는 다른애게 위치하게 됨

            그러면 번호를 가져다 쓰는 경우에는 null로 바꿔놓고 한 번에 삭제해야 오류가 없다

         */

    }

    public void ChangeItem(int _tab, int _itemSlotNumber)
    {
        if (storage.items[_tab].Count <= _itemSlotNumber) return;

        if( _tab != 0 && storage.selectItemInfo[_tab] == _itemSlotNumber )
        {
            playerData.item[_tab] = "";
            storage.selectItemInfo[_tab] = -1;
        }
        else
        {
            var tmpItem = storage.items[_tab][_itemSlotNumber];

            playerData.item[_tab] = tmpItem;
            storage.selectItemInfo[_tab] = _itemSlotNumber;
        }

        ActionChangeItem();
    }

    public void ResetPlayerItem(int _tab)
    {
        playerData.item[_tab] = "";
        storage.selectItemInfo[_tab] = -1;
    }

    public void GainGold(int _gold)
    {
        storage.gold += _gold;

        if(ActionChangeGold != null)
            ActionChangeGold();
    }

    public void GainHonor(int _honor)
    {
        storage.honor += _honor;

        if(ActionChangeHonor != null)
            ActionChangeHonor();
    }

    public void CallChangeGold()
    {
        if (ActionChangeGold != null)
            ActionChangeGold();
    }

    public void CallChangeHonor()
    {
        if (ActionChangeHonor != null)
            ActionChangeHonor();
    }

}
