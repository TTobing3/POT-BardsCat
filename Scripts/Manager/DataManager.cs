using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public enum WeaponType { Dagger, Sword, LongSword, Pistol, Musket, Wand, Staff, Bow }


[System.Serializable]

public class SpriteAndName
{
    public string name;
    public Sprite sprite;
}

public class RecipeData   
{
    public int number;
    public string name, type, result, resultType;
    public string[] components;

    public RecipeData(int number, string name, string type, string result, string resultType, string[] components)
    {
        this.number = number;
        this.name = name;
        this.type = type;
        this.result = result;
        this.resultType = resultType;
        this.components = components;
    }

    public string[] Check(string[] _components)
    {
        var sub = new string[] { components[1], components[2], components[3] };
        var _sub = new string[] { _components[1], _components[2], _components[3] };
        System.Array.Sort(sub);
        System.Array.Sort(_sub);

        if (components[0] == _components[0] && sub.SequenceEqual(_sub) ) 
            return new string[] { resultType, result };
        else 
            return null;
    }
}

public class WeaponData
{
    //�ܰ� ��
    public int number, type, mana, speed, price;
    public string name, from, rank, backstory;
    public string[][] effects;//�̰� ��̸���Ʈ��
    public string[] abilities;//�̰� ��̸���Ʈ��
    public bool form;
    public bool[] motionLock;
    public float[] damagePoint; 
    public WeaponData(
        int number,
        string name, 
        int type,
        int price,
        string rank,
        string from,
        string[][] effects,
        string[] abilities,
        string backstory,
        int mana,
        int speed,
        float[] damagePoint,
        bool form, 
        bool[] motionLock)
    {
        this.number = number;
        this.name = name;
        this.type = type;
        this.price = price;
        this.rank = rank;
        this.from = from;
        this.effects = effects;
        this.abilities = abilities;
        this.backstory = backstory;
        this.mana = mana;
        this.speed = speed;
        this.damagePoint = damagePoint;
        this.form = form;
        this.motionLock = motionLock;
    }
}

[System.Serializable]
public class QuestData
{
    public int number, level, round;
    public string name, type, goal, target, area, backstory;
    public string[] rewards;

    public QuestData(int number, int level, int round, string name, string type, string goal, string target, string area, string[] rewards, string backstory)
    {
        this.number = number;
        this.level = level;
        this.round = round;
        this.name = name;
        this.type = type;
        this.goal = goal;
        this.target = target;
        this.area = area;
        this.rewards = rewards;
        this.backstory = backstory;
    }
}

public class WaveData
{
    public int number;
    public string name, type, area;
    public string[] monsters;
    public WaveData(int _number, string _name, string[] _monsters, string _type, string _area)
    {
        number = _number;
        name = _name;
        monsters = _monsters;
        type = _type;
        area = _area;
    }
}

public class MonsterData
{
    public int number, exp, worth;
    public int[] stats;
    public float thinkTime;
    public string name, prefab;
    public string[] patterns, equips, items;
    public MonsterData(int _number, string _name, string _prefab, int[] _stats, string[] _equips, string[] _patterns, float _thinkTime, int _exp, string[] _items, int _worth)
    {
        number = _number;
        name = _name;
        prefab = _prefab;
        stats = _stats;
        equips = _equips;
        patterns = _patterns;
        thinkTime = _thinkTime;
        exp = _exp;
        items = _items;
        worth = _worth;
    }
}

[System.Serializable]
public class PatternData
{
    public int number, motion, degree;
    public string name, action, effect;
    public string[] conditions;
    public bool finishAct, superArmor;

    public PatternData(int _number, string _name, string _action, int _motion, string[] _conditions, int _degree, string _effect, bool _finishAct, bool _superArmor)
    {
        number = _number;
        name = _name;
        action = _action;
        motion = _motion;
        conditions = _conditions;
        degree = _degree;
        effect = _effect;
        finishAct = _finishAct;
        superArmor = _superArmor;
    }
}

public class CatData
{
    public int number;
    public int[] stats;
    public string[] name, talent;
    public string ability, story;
    public CatData(int number, string[] name, int[] stats, string[] talent, string ability, string story)
    {
        this.number = number;
        this.stats = stats;
        this.name = name;
        this.talent = talent;
        this.ability = ability;
        this.story = story;
    }

}

public class AreaData
{

    //���� ���� ���� �׳�, ���� ���̺� array select�� ���� ��󳻼� ��
    public int number;
    public string name;
    public string[] waves;

    public AreaData(int _number, string _name, string[] _waves)
    {
        number = _number;
        name = _name;
        waves = _waves;
    }
}

public class ArmorData
{
    public int number, price;
    public string name, type, from, rank, backstory;
    public string[] abilities;

    // ������
    public ArmorData(int number, string name, string type, int price, string from, string rank, string[] abilities, string backstory)
    {
        this.number = number;
        this.name = name;
        this.type = type;
        this.price = price;
        this.from = from;
        this.rank = rank;
        this.abilities = abilities;
        this.backstory = backstory;
    }
}

public class ChipData
{
    public string name, category, detailType, from, rank, backstory;
    public int number, price;

    public ChipData(int number, string name, string category, string detailType, int price, string from, string rank, string backstory)
    {
        this.number = number;
        this.name = name;
        this.category = category;
        this.detailType = detailType;
        this.price = price;
        this.from = from;
        this.rank = rank;
        this.backstory = backstory;
    }
}

//���� Ư��
public class AbilityData
{
    public int number;
    public string name, condition, conditionDetail, effectType, effect, story;
    public AbilityData(int number, string name, string condition, string conditionDetail, string effectType, string effect, string story)
    {
        this.number = number;
        this.name = name;
        this.condition = condition;
        this.conditionDetail = conditionDetail;
        this.effectType = effectType;
        this.effect = effect;
        this.story = story;
    }
}

public class CatTalentData
{
    public int number, eye;
    public string name, rare, effect;
    // ������
    public CatTalentData(int number, string name, int eye, string rare, string effect)
    {
        this.number = number;
        this.eye = eye;
        this.name = name;
        this.rare = rare;
        this.effect = effect;
    }
}
public class CatAbilityData
{
    public int number;
    public string name, effect, des;

    public CatAbilityData(int number, string name, string effect, string des)
    {
        this.number = number;
        this.name = name;
        this.effect = effect;
        this.des = des;
    }
}
public class StroyData
{
    public int number;
    public string name, type;
    public List<string> des;

    // ������
    public StroyData(int number, string name, string type, List<string> des)
    {
        this.number = number;
        this.name = name;
        this.type = type;
        this.des = des;
    }
}

public class ConversationData
{
    public int number;
    public string name, type, speaker;
    public string[] texts;

    // ������
    public ConversationData(int number, string name, string type, string speaker, string[] texts)
    {
        this.number = number;
        this.name = name;
        this.type = type;
        this.speaker = speaker;
        this.texts = texts;
    }
}

public class HuntTargetData
{
    public int number;
    public string name, rank, target;
    public int level;
    public string area;
    public int range;

    // ������
    public HuntTargetData(int number, string name, string rank, string target, int level, string area, int range)
    {
        this.number = number;
        this.name = name;
        this.rank = rank;
        this.target = target;
        this.level = level;
        this.area = area;
        this.range = range;
    }

    public int GetSkullRank()
    {
        var skullRank = SkullRank.Normal;

        switch (rank)
        {
            case "�Ϲ�":
                skullRank = SkullRank.Normal;
                break;
            case "�Ǹ�":
                skullRank = SkullRank.Power;
                break;
            case "����":
                skullRank = SkullRank.Crime;
                break;
            case "����":
                skullRank = SkullRank.Golden;
                break;
            case "����":
                skullRank = SkullRank.Kingdom;
                break;
            case "���":
                skullRank = SkullRank.Rare;
                break;
            case "����":
                skullRank = SkullRank.Nightmare;
                break;
            case "�ܰ�":
                skullRank = SkullRank.Space;
                break;
            case "����":
                skullRank = SkullRank.Boss;
                break;
            default:
                skullRank = SkullRank.Normal;
                break;

        }

        return (int)skullRank;
    }
    public Color GetSkullRankColor()
    {
        var skullRank = Color.white;

        switch (rank)
        {
            case "�Ϲ�":
                skullRank = Color.black;
                break;
            case "�Ǹ�":
                skullRank = new Color(0.6f, 0.6f, 0.6f);
                break;
            case "����":
                skullRank = new Color(0.3f, 0.3f, 0.3f);
                break;
            case "����":
                skullRank = new Color(0.8f, 0.6f, 0);
                break;
            case "����":
                skullRank = new Color(0.5f, 0, 0);
                break;
            case "���":
                skullRank = new Color(0.25f, 0.4f, 0.75f);
                break;
            case "����":
                skullRank = new Color(0.27f, 0.15f, 0.4f);
                break;
            case "�ܰ�":
                skullRank = new Color(0.3f, 0.7f, 0.6f);
                break;
            case "����":
                skullRank = Color.black;
                break;
            default:
                skullRank = Color.white;
                break;

        }

        return skullRank;
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    //Dic : �̸��� ���� ã�� �� ���
    public Dictionary<string, WeaponData> AllWeaponDatas = new Dictionary<string, WeaponData>();
    public Dictionary<string, QuestData> AllQuestDatas = new Dictionary<string, QuestData>();
    public Dictionary<string, WaveData> AllWaveDatas = new Dictionary<string, WaveData>();
    public Dictionary<string, MonsterData> AllMonsterDatas = new Dictionary<string, MonsterData>();
    public Dictionary<string, PatternData> AllPatternDatas = new Dictionary<string, PatternData>();
    public Dictionary<string, CatData> AllCatDatas = new Dictionary<string, CatData>();
    public Dictionary<string, AreaData> AllAreaDatas = new Dictionary<string, AreaData>();
    public Dictionary<string, ArmorData> AllArmorDatas = new Dictionary<string, ArmorData>();
    public Dictionary<string, ArmorData> AllArmorDatasKR = new Dictionary<string, ArmorData>();
    public Dictionary<string, ChipData> AllChipDatas = new Dictionary<string, ChipData>();
    public Dictionary<string, RecipeData> AllRecipeDatas = new Dictionary<string, RecipeData>();
    public Dictionary<string, AbilityData> AllAbilityDatas = new Dictionary<string, AbilityData>();
    public Dictionary<string, CatTalentData> AllCatTalentDatas = new Dictionary<string, CatTalentData>();
    public Dictionary<string, CatAbilityData> AllCatAbilityDatas = new Dictionary<string, CatAbilityData>();
    public Dictionary<string, StroyData> AllStroyDatas = new Dictionary<string, StroyData>();
    public Dictionary<string, ConversationData> AllConversationDatas = new Dictionary<string, ConversationData>();
    public Dictionary<string, HuntTargetData> AllHuntTargetDatas = new Dictionary<string, HuntTargetData>();


    //List : �������� ���� �� ���
    public List<WeaponData> AllWeaponDataList = new List<WeaponData>();
    public List<QuestData> AllQuestDataList = new List<QuestData>();
    public List<WaveData> AllWaveDataList = new List<WaveData>();
    public List<MonsterData> AllMonsterDataList = new List<MonsterData>();
    public List<PatternData> AllPatternDataList = new List<PatternData>();
    public List<CatData> AllCatDataList = new List<CatData>();
    public List<AreaData> AllAreaDataList = new List<AreaData>();
    public List<ArmorData> AllArmorDataList = new List<ArmorData>();
    public List<ChipData> AllChipDataList = new List<ChipData>();
    public List<RecipeData> AllRecipeDataList = new List<RecipeData>();
    public List<AbilityData> AllAbilityDataList = new List<AbilityData>();
    public List<CatAbilityData> AllCatAbilityDataList = new List<CatAbilityData>();
    public List<StroyData> AllStroyDataList = new List<StroyData>();
    public List<ConversationData> AllConversationDataList = new List<ConversationData>();
    public List<HuntTargetData> AllHuntTargetDataList = new List<HuntTargetData>();


    [Header("ScriptableDataList")]

    public List<EffectData> AllEffectDataList = new List<EffectData>();
    public List<OptionData> AllOptionDataList = new List<OptionData>();

    //
    public List<SpriteAndName> AllWeaponIllustList = new List<SpriteAndName>();

    public string[] TextData = new string[] { };

    #region Search

    public EffectData SearchEffectData(string _effectDataName)
    {
        return AllEffectDataList.Find(x => x.effectName == _effectDataName);
    }


    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        string[] line = TextData[0].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var weaponData = new WeaponData(
                int.Parse(e[0]), 
                e[1], 
                int.Parse(e[2]),
                int.Parse(e[3]),
                e[4],
                e[5],
                e[6].Split('/').Select(s => s.Split(',')).ToArray(),
                e[7].Split(','),
                e[8], 
                int.Parse(e[9]), 
                int.Parse(e[10]),
                e[11].Split('/').Select(i => float.Parse(i)).ToArray(),
                e[12] == "TRUE",
                e[13].Split(',').Select(i => int.Parse(i) == 1).ToArray());

            AllWeaponDatas.Add(e[1], weaponData);
            AllWeaponDataList.Add(weaponData);
        }

        line = TextData[1].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var questData = new QuestData(int.Parse(e[0]), int.Parse(e[1]), int.Parse(e[2]),
                e[3], e[4], e[5], e[6], e[7], e[8].Split(','), e[9]);

            AllQuestDatas.Add(e[3], questData);
            AllQuestDataList.Add(questData);
        }

        line = TextData[2].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var waveData = new WaveData(int.Parse(e[0]), e[1], e[2].Split(','), e[3], e[4]);

            AllWaveDatas.Add(e[1],waveData);
            AllWaveDataList.Add(waveData);
        }

        line = TextData[3].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var monsterData = new MonsterData(int.Parse(e[0]), e[1], e[2], e[3].Split('/').Select(i => int.Parse(i)).ToArray(), e[4].Split(','), 
                e[5].Split(','), float.Parse(e[6]), int.Parse(e[7]), e[8].Split(","), int.Parse(e[9]));

            AllMonsterDatas.Add(e[1], monsterData);
            AllMonsterDataList.Add(monsterData);
        }

        line = TextData[4].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var patternData = new PatternData(int.Parse(e[0]), e[1], e[2], int.Parse(e[3]), e[4].Split('/'), int.Parse(e[5]), e[6], e[7] == "T", e[8] == "T");

            AllPatternDatas.Add(e[1], patternData);
            AllPatternDataList.Add(patternData);
        }

        line = TextData[5].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var catData = new CatData(int.Parse(e[0]), e[1].Split("/"), e[2].Split(",").Select(i=>int.Parse(i)).ToArray(), e[3].Split(','),e[4],e[5]);

            AllCatDatas.Add(e[1].Split("/")[0], catData);
            AllCatDataList.Add(catData);
        }

        line = TextData[6].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var areaData = new AreaData(int.Parse(e[0]), e[1], e[2].Split('/'));

            AllAreaDatas.Add(e[1], areaData);
            AllAreaDataList.Add(areaData);
        }

        line = TextData[7].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var armorData = new ArmorData(int.Parse(e[0]), e[1], e[2], int.Parse(e[3]), e[4], e[5], e[6].Split(','), e[7]);

            AllArmorDatas.Add(e[1], armorData);
            AllArmorDataList.Add(armorData);
        }

        line = TextData[8].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var chipData = new ChipData(int.Parse(e[0]), e[1], e[2], e[3], int.Parse(e[4]), e[5], e[6], e[7]);

            AllChipDatas.Add(e[1], chipData);
            AllChipDataList.Add(chipData);
        }

        line = TextData[9].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var recipeData = new RecipeData(int.Parse(e[0]), e[1], e[2], e[3], e[4], new string[] { e[5], e[6], e[7], e[8] });

            AllRecipeDatas.Add(e[1], recipeData);
            AllRecipeDataList.Add(recipeData);
        }

        line = TextData[10].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var abilityData = new AbilityData(int.Parse(e[0]), e[1], e[2], e[3], e[4], e[5], e[6]);

            AllAbilityDatas.Add(e[1], abilityData);
            AllAbilityDataList.Add(abilityData);
        }

        line = TextData[11].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var catTalentData = new CatTalentData(int.Parse(e[0]), e[1], int.Parse(e[2]), e[3], e[4] );

            // Dictionary �� List�� �߰�
            AllCatTalentDatas.Add(e[1], catTalentData);
        }

        line = TextData[12].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var catAbilityData = new CatAbilityData(
                int.Parse(e[0]),  // number
                e[1],             // name
                e[2],             // effect
                e[3]              // des
            );

            // Dictionary �� List�� �߰�
            AllCatAbilityDatas.Add(e[1], catAbilityData); // name�� Ű�� ���
            AllCatAbilityDataList.Add(catAbilityData);
        }

        line = TextData[13].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            // ���ڿ��� ����Ʈ�� ��ȯ
            List<string> descriptions = new List<string>();
            for (int j = 3; j < e.Length; j++)
            {
                descriptions.Add(e[j]);
            }

            // StroyData ��ü ����
            var stroyData = new StroyData(
                int.Parse(e[0]),  // number
                e[1],             // name
                e[2],             // type
                descriptions      // des
            );

            // Dictionary �� List�� �߰�
            AllStroyDatas.Add(e[1], stroyData); // name�� Ű�� ���
            AllStroyDataList.Add(stroyData);
        }

        line = TextData[14].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            // ���ڿ� �迭�� ��ȯ
            string[] texts = new string[e.Length - 4];
            for (int j = 4; j < e.Length; j++)
            {
                texts[j - 4] = e[j];
            }

            // ConversationData ��ü ����
            var conversationData = new ConversationData(
                int.Parse(e[0]),  // number
                e[1],             // name,             
                e[2],             // type,
                e[3],             // speaker,
                texts             // texts
            );

            // Dictionary �� List�� �߰�
            AllConversationDatas.Add(e[1], conversationData); // name�� Ű�� ���
            AllConversationDataList.Add(conversationData);
        }

        line = TextData[15].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            // HuntTargetData ��ü ����
            var huntTargetData = new HuntTargetData(
                int.Parse(e[0]),  // number
                e[1],             // name
                e[2],             // rank
                e[3],             // target
                int.Parse(e[4]),  // level
                e[5],             // area
                int.Parse(e[6])   // range
            );

            // Dictionary �� List�� �߰�
            AllHuntTargetDatas.Add(e[1], huntTargetData); // name�� Ű�� ���
            AllHuntTargetDataList.Add(huntTargetData);
        }
    }

    #region ������ �ҷ�����
    
    const string weaponURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=0";

    const string questURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=1788969359";

    const string waveURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=2031151052";

    const string monsterURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=369349605";

    const string patternURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=622236662";

    const string catURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=256784123";

    const string areaURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=491032984";

    const string armorURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=631043701";

    const string chipURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=180920709";

    const string combURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=1311408988";

    const string abilityURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=254801633";

    const string catTalentURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=689029355";

    const string catAbilityURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=1353488967";

    const string storyURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=1985997685";

    const string conversationURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=412290693";

    const string huntTargetURL = "https://docs.google.com/spreadsheets/d/1LGERXUejrwlo5pZnZ8gZ5PkQXW8aqfCLI3O8SRg_II8/export?format=tsv&gid=1239736201";


    [ContextMenu("Load Data")]
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(weaponURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 0);

        www = UnityWebRequest.Get(questURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 1);
        
        www = UnityWebRequest.Get(waveURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 2);

        www = UnityWebRequest.Get(monsterURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 3);

        www = UnityWebRequest.Get(patternURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 4);

        www = UnityWebRequest.Get(catURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 5);

        www = UnityWebRequest.Get(areaURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 6);

        www = UnityWebRequest.Get(armorURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 7);

        www = UnityWebRequest.Get(chipURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 8);

        www = UnityWebRequest.Get(combURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 9);

        www = UnityWebRequest.Get(abilityURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 10);

        www = UnityWebRequest.Get(catTalentURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 11);

        www = UnityWebRequest.Get(catAbilityURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 12);

        www = UnityWebRequest.Get(storyURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 13);

        www = UnityWebRequest.Get(conversationURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 14);

        www = UnityWebRequest.Get(huntTargetURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 15);
        Debug.Log("Success Load");
    }

    void SetDataList(string tsv, int i)
    {
        TextData[i] = tsv;
    }

    #endregion

}
