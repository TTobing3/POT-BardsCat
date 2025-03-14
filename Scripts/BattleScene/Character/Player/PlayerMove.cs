using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;

    StateDecider stateDecider;
    MotionAnimator motionAnimator;

    Player player;

    public bool isMove = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        stateDecider = GetComponent<StateDecider>();
        motionAnimator = GetComponent<MotionAnimator>();
    }

    void FixedUpdate()
    {
        if (player.isDead)
            StopPlayer();
        else
        {
            if (stateDecider.motion == Motion.Move || stateDecider.motion == Motion.BackMove)
                MovePlayer();
            else if (!isMove)
                StopPlayer();
        }

    }

    public void MovePlayer()
    { 
        var curSpeed = player.stats[(int)PlayerStat.Speed];

        if (stateDecider.direction == 0)
        {
            rigid.AddForce(Vector2.left * (curSpeed * 0.1f), ForceMode2D.Impulse);
            rigid.velocity = new Vector2(Mathf.Max(rigid.velocity.x, -curSpeed), rigid.velocity.y);
        }
        else
        {
            rigid.AddForce(Vector2.right * (curSpeed * 0.1f), ForceMode2D.Impulse);
            rigid.velocity = new Vector2(Mathf.Min(rigid.velocity.x, curSpeed), rigid.velocity.y);
        }
    }

    public void StopPlayer()
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

    public void Dash(float _power)
    {
        //isMove = true;
        rigid.velocity = new Vector3(0, rigid.velocity.y);
        rigid.AddForce((stateDecider.faceDirection == 0 ? Vector2.left : Vector2.right) * (_power * 0.1f), ForceMode2D.Impulse);
    }

}
