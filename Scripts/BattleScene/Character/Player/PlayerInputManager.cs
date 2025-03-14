using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputManager : MonoBehaviour
{
    StateDecider stateDecider; // input 값 전달 대상
    MotionAnimator motionAnimator;

    [SerializeField] KeyCode preKey = 0;
    [SerializeField] KeyCode[] curKeies = new KeyCode[] { KeyCode.None, KeyCode.None }; 

    Coroutine checkKeepPress;

    void Awake()
    {
        stateDecider = GetComponent<StateDecider>();
        motionAnimator = GetComponent<MotionAnimator>();
    }

    void Update()
    {
        CheckKeyInput();
    }

    void CheckKeyInput()
    {

        #region KeyDown

        if (Input.GetKeyDown(KeyCode.LeftArrow)) KeyDown(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow)) KeyDown(KeyCode.RightArrow);

        #endregion

        #region Key

        if (Input.GetKey(KeyCode.LeftArrow)) curKeies[0] = KeyCode.LeftArrow;
        if (Input.GetKey(KeyCode.RightArrow)) curKeies[1] = KeyCode.RightArrow;

        #endregion

        #region KeyUp

        if (Input.GetKeyUp(KeyCode.LeftArrow)) KeyUp(KeyCode.LeftArrow);
        if (Input.GetKeyUp(KeyCode.RightArrow)) KeyUp(KeyCode.RightArrow);

        #endregion
    }

    #region Input
    void KeyDown(KeyCode _curKey, bool _isInnerCall = false)
    {
        //더블인 경우 초기화
        //ResetKeepPress();

        // !_isInnerCall 검사는 왜 ?  오른쪽 입력 => 이전 입력 오른쪽 => 왼쪽 떼고, 오른쪽 InnerCall => 오른쪽 입력 => 더블
        bool isDouble = false;

        if (!_isInnerCall && !motionAnimator.isChanging) isDouble = (_curKey == preKey);

        //Double
        if (isDouble)
        {
            //checkKeepPress = StartCoroutine(CheckKeepPress(_curKey == KeyCode.LeftArrow ? StateInput.LeftKeep : StateInput.RightKeep));
            GiveInput(_curKey == KeyCode.LeftArrow ? StateInput.LeftDouble : StateInput.RightDouble);
        }
        //Single
        else
        {
            GiveInput(_curKey == KeyCode.LeftArrow ? StateInput.Left : StateInput.Right);
        }

        //내부 호출이 아니면 prekeycode를 바꿈
        //isChanging이 true일 때 prekey 입력이 가능하면 곧바로 더블 입력되어서 에러남
        if(!_isInnerCall && !motionAnimator.isChanging) 
            StartCoroutine(ResetPreKeyCode(_curKey));
    }
    void KeyUp(KeyCode _releaseKey)
    {
        ResetKeepPress();

        //현재 눌린 키 수정
        //뗏을 때 반대 키가 눌려 있으면 해당 키를 내부호출
        //아무런 키도 안 눌린 상태라면 None 호출

        if (_releaseKey == KeyCode.LeftArrow)
        {
            curKeies[0] = KeyCode.None;

            if (curKeies[1] != KeyCode.None)
                KeyDown(KeyCode.RightArrow, true);
            else
                GiveInput(StateInput.None);
        }
        if (_releaseKey == KeyCode.RightArrow)
        {
            curKeies[1] = KeyCode.None;

            if (curKeies[0] != KeyCode.None)
                KeyDown(KeyCode.LeftArrow, true);
            else
                GiveInput(StateInput.None);
        }
    }
    #endregion

    #region Addtional
    void ResetKeepPress()
    {
        if (checkKeepPress != null) StopCoroutine(checkKeepPress);
    }
    IEnumerator CheckKeepPress(StateInput _input)
    {
        //특정 키 계속 누르고 있는 거 감지
        yield return new WaitForSeconds(2f);

        GiveInput(_input);
    }
    IEnumerator ResetPreKeyCode(KeyCode _key)
    {
        //더블 입력 감지용으로 일정 시간 prekey 유지
        preKey = _key;

        yield return new WaitForSeconds(0.5f);

        preKey = KeyCode.None;
    }
    #endregion

    #region OutCall
    // called by end of animation
    public void SetAutoInput()
    {
        //강제 액션 끝나고 현재 눌린 키 내부호출
        if (curKeies[0] != KeyCode.None)
        {
            if (stateDecider.motion == Motion.BackAct) checkKeepPress = StartCoroutine(CheckKeepPress(StateInput.LeftKeep));
            KeyDown(KeyCode.LeftArrow, true);
        }
        else  if (curKeies[1] != KeyCode.None)
        {
            if (stateDecider.motion == Motion.BackAct) checkKeepPress = StartCoroutine(CheckKeepPress(StateInput.RightKeep));
            KeyDown(KeyCode.RightArrow, true);
        }
        else
        {
            GiveInput(StateInput.None);
        }
    }
    #endregion

    #region Connect
    void GiveInput(StateInput _input)
    {
        //stateDecider에게 인풋 전달
        stateDecider.ReceiveInput(_input);
    }
    #endregion

}
