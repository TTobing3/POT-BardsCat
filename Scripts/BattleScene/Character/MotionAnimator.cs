using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Motion { Idle, Move, BackMove, FrontAct, BackAct, Keep, Hit, Dead }
public enum Form { UnLock, Lock }

// ŵ 2�ʴϱ� �ִϸ��̼� 2�� �����ϸ� �� �ѱ��

public class MotionAnimator : MonoBehaviour
{
    public System.Action<int> flipSprite;
    public System.Action<float> ActionReciveAnimation;

    Animator animator;

    RectTransform canvas;

    public bool isChanging = false;
    [HideInInspector] public bool isMotionCheckOkay = false;


    void Awake()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
    }

    void Update()
    {
            
    }

    public void ReceiveAnimation(int _motion, int _direction, bool _motionLock)
    {
        isChanging = _motionLock; // ���� ��Ǻ��� �� ���� ���� 

        FlipSprite(_direction);
        animator.SetInteger("Motion", (int)_motion);
        animator.ResetTrigger("MotionChange");
        animator.SetTrigger("MotionChange");

    }

    public void FlipSprite(int _direction)
    {
        if (_direction == 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if(canvas != null) canvas.localScale = new Vector3(Mathf.Abs( canvas.localScale.x ), canvas.localScale.y, canvas.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (canvas != null) canvas.localScale = new Vector3(Mathf.Abs(canvas.localScale.x) * -1, canvas.localScale.y, canvas.localScale.z);
        }
        if (flipSprite!=null) flipSprite(_direction);
    }


    // called by end of animation
    public void CompleteActAnimation()
    {
        print("call completeActAnimation");
        isChanging = false;
    }

    // ���⿡ ���� ������
}

