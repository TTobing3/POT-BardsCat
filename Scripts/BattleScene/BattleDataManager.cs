using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ResultType { Monster, Chip, Material, Eq, Gem } //

[System.Serializable]
public class DropItemSpriteName
{
    public string name;
    public DropItemType type;
    public Sprite sprite;
}

public class BattleDataManager : MonoBehaviour
{
    public static BattleDataManager instance;


    [Header("DataList")]

    public List<GameObject> AllMonsterPrefabList = new List<GameObject>();
    public List<GameObject> AllDropItemPrefabList = new List<GameObject>();

    [Header("-")]

    public AudioClip[] newFormAudioClip;
    public AudioClip[] weaponSubAudioClip;

    public List<GameObject> effects = new List<GameObject>();
    public List<GameObject> dropitems = new List<GameObject>();

    public Transform effectParent, dropItemParent;

    public List<Monster> monsters = new List<Monster>(), targets = new List<Monster>();
    public int monsterCount { get { return monsters.Count; } }

    public Material whiteMaterial;

    [Header("Gain")]
    
    //Gold Chip Material Eq(:Tab) Gem
    // item count

    public List<string[]>[] reportResultLists = new List<string[]>[] 
    { new List<string[]>(), new List<string[]>(), new List<string[]>(), new List<string[]>(), new List<string[]>() }; 
   

    [Header("Reward")]
    public int rewardGold;
    public int rewardHonor;
    public List<string> rewardItems = new List<string>();

    [Header("New")]
    public List<string> newAchieve = new List<string>();
    public List<string> newTitle = new List<string>();
    public List<string> newUnLock = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    #region Search

    public GameObject SearchMonsterPrefab(string _monsterName)
    {
        return AllMonsterPrefabList.Find(x => x.gameObject.name == _monsterName);
    }

    #endregion

    public void OnMonsterDead(Monster _monster)
    {
        monsters.Remove(_monster);

        AddResult((int)ResultType.Monster, _monster.monsterData.name);

        //여기서 다 처리하면

        if (monsters.Count == 0) GameManager.instance.RoundClear();
    }

    #region 보상 관련
    public void AddResult(int _type, string[] _data)
    {
        //골드 - 몬스터로 대체

        if(_type == (int)DropItemType.Gold)
        {
            return;
        }

        //장비 보상

        if (_type == (int)DropItemType.Eq) 
        {
            reportResultLists[_type].Add(new string[2] { _data[0], _data[2] });
            return;
        }

        //그 외 보상 등록 : 부산물, 재료, 보석(보석 그냥 재료에 편입시키는 방향 모색)

        if (reportResultLists[_type].Any(item => item[0] == _data[0])) //보상 목록에 이미 존재
        {
            foreach (string[] i in reportResultLists[_type])
                i[1] = (int.Parse(i[1]) + 1).ToString();
        }
        else
        {
            reportResultLists[_type].Add(new string[2] { _data[0], "1" });
        }
    }

    void AddResult(int _type, string _data)
    {
        if (reportResultLists[_type].Any(item => item[0] == _data)) //보상 목록에 이미 존재
        {
            foreach (string[] i in reportResultLists[_type])
                i[1] = (int.Parse(i[1]) + 1).ToString();
        }
        else
        {
            reportResultLists[_type].Add(new string[2] { _data, "1" });
        }
    }

    public int ValueCalculator(string[] itemData, ResultType type)
    {

        // 가치
        var itemValue = 0;

        switch (type)
        {
            case ResultType.Monster:
                itemValue = DataManager.instance.AllMonsterDatas[itemData[0]].worth * int.Parse(itemData[1]);
                break;

            case ResultType.Chip:
                itemValue = DataManager.instance.AllChipDatas[itemData[0]].price * int.Parse(itemData[1]);
                break;

            case ResultType.Material:
                itemValue = DataManager.instance.AllChipDatas[itemData[0]].price * int.Parse(itemData[1]);
                break;

            case ResultType.Eq:
                if (int.Parse(itemData[1]) == (int)Tab.Weapon)
                {
                    itemValue = DataManager.instance.AllWeaponDatas[itemData[0]].price;
                }
                else
                {
                    itemValue = DataManager.instance.AllArmorDatas[itemData[0]].price;
                }
                break;
        }

        return itemValue;
    }

    
    public int TotalValueCalculator(ResultType type)
    {
        var totalValue = 0;
        var _n = (int)type;

        for (int i = 0; i < reportResultLists[_n].Count; i++)
        {
            // 이름
            var itemData = reportResultLists[_n][i]; // [1] 0 : 이름 1 : 개수 [2] 0 : 이름 1 : 장비 타입

            // 가치 계산
            var itemValue = ValueCalculator(itemData, (ResultType)_n);

            // 토탈 가치 합산
            totalValue += itemValue;

        }

        return totalValue;
    }
    #endregion
}
