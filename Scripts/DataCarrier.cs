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

    // ������, Ÿ�� ������  Weapon, Helmet, Armor, Acc, Material, Gem 
    public List<List<string>> items = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() }; //
    public List<string> cats = new List<string>();

    public int[] selectItemInfo = new int[4] { 0, 0, 0, 0 }; //��װ� storage�� �־�� �ϳ�?
    public int selectCatInfo; // ��װ� storage�� �־�� �ϳ�?

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

        playerData.item[0] = "�� ��";

        AddItemToStorage((int)Tab.Armor, "����");
        AddItemToStorage((int)Tab.Acc, "�ݰ���");

        AddItemToStorage((int)Tab.Material, "��� ����");
        AddItemToStorage((int)Tab.Material, "��� ����");
        AddItemToStorage((int)Tab.Material, "��� ����");
        AddItemToStorage((int)Tab.Material, "ö��");
        AddItemToStorage((int)Tab.Material, "�Ϲ� ����");
        AddItemToStorage((int)Tab.Gem, "����� ����");
        AddItemToStorage((int)Tab.Helmet, "����");
        //;
        //
        //AddItemToStorage((int)Tab.Weapon, "���� ������");
        AddItemToStorage((int)Tab.Weapon, "�ϼ� �ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�μհ�");
        AddItemToStorage((int)Tab.Weapon, "���� ������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "�ܰ�");
        AddItemToStorage((int)Tab.Weapon, "�ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�� �� ����");
        AddItemToStorage((int)Tab.Weapon, "�ϼ� �ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�μհ�");
        AddItemToStorage((int)Tab.Weapon, "���� ������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "�ܰ�");
        AddItemToStorage((int)Tab.Weapon, "�ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�� �� ����");
        AddItemToStorage((int)Tab.Weapon, "�ϼ� �ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�μհ�");
        AddItemToStorage((int)Tab.Weapon, "���� ������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "�ܰ�");
        AddItemToStorage((int)Tab.Weapon, "�ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�� �� ����");
        AddItemToStorage((int)Tab.Weapon, "�ϼ� �ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�μհ�");
        AddItemToStorage((int)Tab.Weapon, "���� ������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "�ܰ�");
        AddItemToStorage((int)Tab.Weapon, "�ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�� �� ����");
        AddItemToStorage((int)Tab.Weapon, "�ϼ� �ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�μհ�");
        AddItemToStorage((int)Tab.Weapon, "���� ������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "������");
        AddItemToStorage((int)Tab.Weapon, "�ܰ�");
        AddItemToStorage((int)Tab.Weapon, "�ϵ�");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�ӽ�Ŷ ����");
        AddItemToStorage((int)Tab.Weapon, "�� �� ����");


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
            case "����" :
                category = (int)Tab.Weapon;
                break;
            case "����":
                category = (int)Tab.Helmet;
                break;
            case "����":
                category = (int)Tab.Armor;
                break;
            case "��ű�":
                category = (int)Tab.Acc;
                break;
            case "���":
                category = (int)Tab.Material;
                break;
            case "����":
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
            ������ ���� ���� ����
         
            ��ġ �ѹ��� ������ ���°���

            �ٵ� �� ���� �������� �����ϸ�

            ����Ʈ ������ ��

            �׷��� �����ϸ�, ������ �ִ� ���� ��ġ ��ȣ���� �ٸ��ְ� ��ġ�ϰ� ��

            �׷��� ��ȣ�� ������ ���� ��쿡�� null�� �ٲ���� �� ���� �����ؾ� ������ ����

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
