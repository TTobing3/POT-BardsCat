using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateInput { Left, Right, LeftDouble, RightDouble, None, Keep, LeftKeep, RightKeep }

public class PlayerSetting
{
    public static bool form = true;
    public static bool animationRock = true;
}
public class StateDecider : MonoBehaviour
{
    Animator animator;

    public MotionAnimator motionAnimator;
    public PlayerAnimator playerAnimator;
    PlayerMove playerMove;
    PlayerInputManager playerInputManager;
    public int direction = 0, faceDirection = 0;

    public Motion motion = 0;
    public Form form = 0;

    Player player;

    Coroutine moveSfxCoroutine;

    void Awake()
    {
        player = GetComponent<Player>();
        playerMove = GetComponent<PlayerMove>();
        motionAnimator = GetComponent<MotionAnimator>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerInputManager = GetComponent<PlayerInputManager>();

        animator = GetComponent<Animator>();
    }

    public void ReceiveInput(StateInput _input) // 떼는 것도 감지
    {
        if (motionAnimator.isChanging) return;

        playerMove.isMove = false;

        // Stop ; Idle
        if (_input == StateInput.None)
        {
            GiveMotion(Motion.Idle);
            return;
        }

        // Direction Decide
        if(_input != StateInput.Keep)
        {
            if (((int)_input % 2) == 0)
                direction = 0;
            else
                direction = 1;
        }

        if (form == Form.UnLock)   
        {
            faceDirection = direction;

            if ((int)_input < 2) // Single
            {
                GiveMotion(Motion.Move);
                playerMove.isMove = true;
            }
            else // Double
            {
                if (player.weapon == null) return;

                if(player.weapon.form)
                {
                    GiveNewForm(Form.Lock);
                }
                else
                {
                    GiveMotion(Motion.FrontAct, player.weapon.motionLock[0]);
                }
                //분기점, 아이템에 따라서 폼 변경 혹은 그냥 액트
            }
        }
        else
        {
            if(!player.weapon.form) faceDirection = direction;

            var curDirection = (int)_input % 2 == 0 ? 0 : 1;

            if(_input == StateInput.LeftKeep || _input == StateInput.RightKeep)
            {
                if (curDirection == faceDirection)
                {
                    //앞꾹
                }
                else
                {
                }
            }
            else if ((int)_input < 2) // Single
            {
                if (curDirection == faceDirection)
                {
                    GiveMotion(Motion.Move);
                    playerMove.isMove = true;
                }
                else
                {
                    GiveMotion(Motion.BackMove);
                    playerMove.isMove = true;
                }
            }
            else // Double
            {
                if(curDirection == faceDirection)
                {
                    GiveMotion(Motion.FrontAct, player.weapon.motionLock[(int)Motion.FrontAct]);
                }
                else
                {
                    GiveNewForm(Form.UnLock);
                    //GiveMotion(Motion.BackAct, player.weapon.motionLock[(int)Motion.BackAct]);
                }
            }
        }
    }

    void GiveMotion(Motion _newMotion, bool _isLock = false)
    {
        motion = _newMotion;
        motionAnimator.ReceiveAnimation((int)motion, faceDirection, _isLock);
    }

    //called by
    void CompleteAct()
    {
        motionAnimator.CompleteActAnimation();
        playerInputManager.SetAutoInput();
        motionAnimator.isMotionCheckOkay = false;
    }



    void GiveNewForm(Form _newForm)
    {
        if (motionAnimator.isChanging || !player.weapon.form) return;

        motion = Motion.Idle;
        form = _newForm;
        playerMove.isMove = false;
        playerAnimator.ReceiveNewForm(faceDirection);

        if (_newForm == Form.Lock) 
            SoundManager.instance.SfxPlay(BattleDataManager.instance.newFormAudioClip[player.weapon.type]);
    }

    public void ResetMotion()
    {
        GiveMotion(Motion.Idle);
    }
}
