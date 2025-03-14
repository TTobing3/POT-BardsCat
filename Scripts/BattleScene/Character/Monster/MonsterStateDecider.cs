using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateDecider : MonoBehaviour
{
    Monster monster;

    MonsterAct monsterAct;
    MonsterMove monsterMove;
    MotionAnimator motionAnimator;

    public PatternData curPattern;

    [HideInInspector] public int curDir = 0;

    Coroutine curThink;

    #region Condition

    public float distanceToPlayer
    {
        get 
        {
            return Mathf.Abs(GameManager.instance.player.transform.position.x - transform.position.x);
        }
    }
    public float directionToPlayer
    {
        get
        {
            return GameManager.instance.player.transform.position.x < transform.position.x ? 0 : 1;
        }
    }


    #endregion

    void Awake()
    {
        monster = GetComponent<Monster>();
        monsterAct = GetComponent<MonsterAct>();
        monsterMove = GetComponent<MonsterMove>();
        motionAnimator = GetComponent<MotionAnimator>();
    }

    public void StartPattern()
    {
        curThink = StartCoroutine(CoActPattern());
    }

    IEnumerator CoActPattern(int _time = -1)
    {
        
        if(_time == -1)
        {
            yield return new WaitForSeconds(monster.monsterData.thinkTime);
        }
        else
        {
            yield return new WaitForSeconds(_time);
        }

        SelectPattern();

        if (curPattern.finishAct) yield break;

        //���� = ���� �ð�
        if (curPattern.degree > 0)
        {
            StopCoPattern();
            curThink = StartCoroutine(CoActPattern(curPattern.degree));
        }
        else
        {
            curThink = StartCoroutine(CoActPattern());
        }
    }

    void SelectPattern()
    {
        if (GameManager.instance.player.isDead) return;

        //���� ������ ���� ����
        var tmpList = ReturnPossiblePattern();

        if (tmpList.Count == 0) return;

        var tmpPattern = tmpList[Random.Range(0, tmpList.Count)];

        ActPattern(tmpPattern);
    }

    #region Check Pattern Condition

    List<string> ReturnPossiblePattern()
    {
        var tmpList = new List<string>();

        foreach (string i in monster.monsterData.patterns)
        {
            if (CheckPattern(i)) tmpList.Add(i);
        }

        return tmpList;
    }

    bool CheckPattern(string _patternName)
    {
        var tmpPattern = DataManager.instance.AllPatternDatas[_patternName];

        foreach (string i in tmpPattern.conditions)
        {
            var tmpContion = i.Split(','); // ��� ���� ���ذ�

            switch (tmpContion[0])
            {
                case "�Ÿ�":
                    if (!CalculateCondition(tmpContion[1], distanceToPlayer, int.Parse(tmpContion[2]))) return false;
                    break;
                case "�� ü��":
                    if (!CalculateCondition(tmpContion[1], monster.hp, int.Parse(tmpContion[2]))) return false;
                    break;
                case "�÷��̾� ü��":
                    if (!CalculateCondition(tmpContion[1], GameManager.instance.player.hp, int.Parse(tmpContion[2]))) return false;
                    break;
                case "����":
                    if (!CalculateCondition(tmpContion[1], GameManager.instance.player.weapon.name, tmpContion[2])) return false;
                    break;
            }

        }

        return true;
    }

    bool CalculateCondition(string _con, float _target, float _value)
    {
        switch (_con)
        {
            case ">": //ũ��
                return _target > _value;
            case "<": //�۴�
                return _target < _value;
            case "=": //����
                return _target == _value;
            case "!"://�ٸ���
                return _target != _value;
        }

        return false;
    }
    bool CalculateCondition(string _con, string _target, string _value)
    {
        switch (_con)
        {
            case "=": //����
                return _target == _value;
            case "!"://�ٸ���
                return _target != _value;
        }

        return false;
    }

    bool StringToPattern(string _patternName)
    {
        switch (_patternName)
        {
            case "Heal":
                break;
            case "Stop":
                break;
            case "Patrol":
                break;
            case "Wander":
                break;
            case "Follow":
                break;
            case "Act":
                break;
            case "Detect":
                break;
            case "RunAway":
                break;
        }

        return false;
    }

    #endregion

    void ActPattern(string _patternName)
    {
        if (monster.isDead) return;
        //���� �����ؼ�
        var tmpPattern = DataManager.instance.AllPatternDatas[_patternName];

        curPattern = tmpPattern;

        SetCurDir();

        switch (tmpPattern.action)
        {
            case "Follow":
                Follow();
                break;

            case "Attack":
                Attack(tmpPattern.motion);
                break;

            case "BackStep":
                BackStep();
                break;
        }
    }

    //called by Animator
    public void FinishAct()
    {
        print("call");
        curThink = StartCoroutine(CoActPattern());
        SelectPattern();
    }
    
    void SetCurDir(bool _reverse = false)
    {
        if (_reverse) curDir = transform.position.x < GameManager.instance.player.transform.position.x ? 0 : 1;
        else curDir = transform.position.x > GameManager.instance.player.transform.position.x ? 0 : 1;
    }

    void Attack(int _motion)
    {
        StopCoPattern();
        monsterMove.StopMove();
        motionAnimator.ReceiveAnimation(_motion, curDir, true); 
    }

    void Follow()
    {
        monsterMove.MoveToPlayer();
        motionAnimator.ReceiveAnimation((int)MonsterMotion.Move, curDir, true);
    }

    void BackStep() // ������ �����;� �ǳ�
    {
        monsterMove.MoveToPlayer(false);
        motionAnimator.ReceiveAnimation((int)MonsterMotion.BackMove, curDir, true);

    }


    public void StopCoPattern() // �� �ൿ�� ������ �ٸ� ������ ����
    {
        StopCoroutine(curThink);
    }

}
