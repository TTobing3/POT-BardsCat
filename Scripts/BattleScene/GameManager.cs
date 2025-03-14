using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject effectObj;
    public Player player;
    public GameObject bonfire;
    public bool isClear = false;

    int remainRound = 0, nextRange = 15;
    bool isTargetArrived = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        StartSet();

        SoundManager.instance.BgSoundPlay(2);
    }

    void Update()
    {
        if(bonfire.gameObject.activeSelf)
        {
            if( Mathf.Abs( bonfire.transform.position.x - player.transform.position.x ) > nextRange )
            {
                nextRange = Random.Range(15, 20);
                RoundStart();
            }
        }
    }

    void StartSet()
    {
        player.Set();
        UIManager.instance.SetMark(DataCarrier.instance.playerData.cat);

        #region Wave

        remainRound = DataCarrier.instance.questData.round;

        ArriveMonsters();

        #endregion
    }

    void ArriveTarget()
    {
        var tmpWave = DataManager.instance.AllWaveDatas[DataCarrier.instance.questData.target];

        foreach (string i in tmpWave.monsters)
        {
            var tmpMonsterData = DataManager.instance.AllMonsterDatas[i];

            var tmpSpawnPosX = Random.Range(10, 20f);
            var tmpMonster = Instantiate(BattleDataManager.instance.SearchMonsterPrefab(tmpMonsterData.prefab), player.transform.position + new Vector3(tmpSpawnPosX, 0.15f), Quaternion.identity).GetComponent<Monster>();
            MosterPositioning(tmpMonster.transform);
            tmpMonster.Set(tmpMonsterData.name);
            //BattleDataManager.instance.targets.Add(tmpMonster);
            BattleDataManager.instance.monsters.Add(tmpMonster);
            isTargetArrived = true;
        }

        UIManager.instance.ShowArriveText("{ "+tmpWave.name +" 등장!"+ " }");
    }

    void ArriveMonsters()
    {
        var tmpWaves = DataManager.instance.AllWaveDataList.FindAll(i => i.area == DataCarrier.instance.questData.area && i.type == "일반");
        var tmpWave = tmpWaves[Random.Range(0, tmpWaves.Count)];

        foreach(string i in tmpWave.monsters)
        {
            var tmpMonsterData = DataManager.instance.AllMonsterDatas[i];
            //var tmpSpawnPosX = Random.Range(0, 2) > 0 ? Random.Range(10, 20f) : Random.Range(10, 20f) * -1;
            var tmpSpawnPosX = Random.Range(10, 20f);
            var tmpMonster = Instantiate(BattleDataManager.instance.SearchMonsterPrefab(tmpMonsterData.prefab), player.transform.position + new Vector3(tmpSpawnPosX, 0.15f), Quaternion.identity).GetComponent<Monster>();


            MosterPositioning(tmpMonster.transform);

            tmpMonster.Set(tmpMonsterData.name);
            BattleDataManager.instance.monsters.Add(tmpMonster);
        }

        UIManager.instance.ShowArriveText(tmpWave.name+"(을)를 마주쳤다!");
    }

    void MosterPositioning(Transform _target)
    {
        var collider2D = _target.GetComponent<Collider2D>();

        float colliderHeight = collider2D.bounds.size.y;
        float newY = -0.8f + (colliderHeight / 2);
        _target.transform.position = new Vector3(_target.transform.position.x, newY, _target.transform.position.z);
    }

    //의뢰 정보 => 지역 정보 => 지역 별 웨이브 목록 => 무작위 하나 택

    public void RoundClear()
    {
        remainRound--;
        isClear = true;

        bonfire.SetActive(true);
        bonfire.transform.position = new Vector3(player.transform.position.x, 0, 0);
        player.GetComponent<BoxCollider2D>().isTrigger = false;

        
        if (isTargetArrived)
        {
            FinishQuest(true);
        }
        else
        {
            UIManager.instance.ShowArriveText("모두 처치했다, 계속 나아가자!");
        }
        //여기서 모닥불 생성하고

    }

    public void RoundStart()
    {
        player.GetComponent<BoxCollider2D>().isTrigger = true;

        bonfire.SetActive(false);

        UIManager.instance.MoveMark(DataCarrier.instance.questData.round, remainRound);
        
        if (remainRound == 0)
        {
            ArriveTarget();
        }
        else if (remainRound > 0)
        {
            ArriveMonsters();
        }

    }

    public void FinishQuest(bool _isClear)
    {
        UIManager.instance.ClearQuest(_isClear);

        GainResult();

        DataCarrier.instance.story = "애송이";
    }

    void GainResult()
    {
        //명예
        DataCarrier.instance.GainHonor(BattleDataManager.instance.TotalValueCalculator(ResultType.Monster));

        //부산물
        DataCarrier.instance.GainGold(BattleDataManager.instance.TotalValueCalculator(ResultType.Chip));

        //장비
        foreach(string[] i in BattleDataManager.instance.reportResultLists[(int)ResultType.Eq])
        {
            DataCarrier.instance.AddItemToStorage(int.Parse(i[1]), i[0]);
        }

        //재료
        foreach (string[] i in BattleDataManager.instance.reportResultLists[(int)ResultType.Material])
        {
            for(int j = 0; j<int.Parse(i[1]); j++ )
                DataCarrier.instance.AddItemToStorage((int)Tab.Material, i[0]);
        }

        //의뢰보상
        foreach(string i in DataCarrier.instance.questData.rewards)
        {
            var itemData = i.Split(":");

            switch(itemData[0])
            {
                case "금화":
                    DataCarrier.instance.GainGold(int.Parse(itemData[1]));
                    break;
                case "명예":
                    DataCarrier.instance.GainHonor(int.Parse(itemData[1]));
                    break;
                default:
                    DataCarrier.instance.AddItemToStorage(int.Parse(itemData[0]), itemData[1]);
                    break;
            }
        }
    }

    public void GoToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

}
