using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MonsterMove : MonoBehaviour
{
    // 거리가 가까워질때까지 이동
    // 
    MonsterStateDecider monsterStateDecider;
    Monster monster;
    Rigidbody2D rigid;

    bool isMove;

    void Awake()
    {
        monsterStateDecider = GetComponent<MonsterStateDecider>();
        monster = GetComponent<Monster>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(!isMove)
        {
            Stop();
        }
    }

    public void Stop()
    {
        if (rigid.velocity.x < 0 && rigid.velocity.x > -0.1)
        {
            rigid.velocity = new Vector3(0, rigid.velocity.y);
        }
        else if (rigid.velocity.x > 0 && rigid.velocity.x < 0.1)
        {
            rigid.velocity = new Vector3(0, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector3(rigid.velocity.x / 1.1f, rigid.velocity.y);
        }
    }
    public void MoveToPlayer(bool _toPlayer = true)
    {
        isMove = true;

        var tmpSpeed = monster.monsterData.stats[(int)MonsterStat.Speed];

        if (GameManager.instance.player.transform.position.x < transform.position.x)
        {
            rigid.velocity = new Vector2(_toPlayer ? -tmpSpeed : tmpSpeed, 0);        
        }
        else
        {
            rigid.velocity = new Vector2(_toPlayer ? tmpSpeed : -tmpSpeed, 0);
        }
    }

    public void StopMove()
    {
        isMove = false;
    }
    public void Dash(float _power)
    {
        //isMove = true;
        rigid.velocity = new Vector3(0, rigid.velocity.y);
        rigid.AddForce((monsterStateDecider.curDir == 0 ? Vector2.left : Vector2.right) * (_power * 0.1f), ForceMode2D.Impulse);
    }
}
