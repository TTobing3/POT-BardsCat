using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//애니메이션 바꾸는 애
public class PlayerAnimator : MonoBehaviour
{
    public System.Action ActionCompleteFormChange;

    Animator animator;
    MotionAnimator motionAnimator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        motionAnimator = GetComponent<MotionAnimator>();
    }

    public void ManaSetting()
    {
        animator.SetBool("EmptyMana", GameManager.instance.player.mp == 0);
    }
    public void ReceiveNewForm(int _direction)
    { 
        motionAnimator.FlipSprite(_direction);

        motionAnimator.isChanging = true;
        animator.ResetTrigger("MotionChange");
        animator.SetTrigger("FormChange");
    }

    // called by end of toBattleMotion animation
    void CompleteFormChange(int _form)
    {
        animator.SetInteger("Form", _form);
        motionAnimator.isChanging = false;

        if(ActionCompleteFormChange != null) ActionCompleteFormChange();
    }

}