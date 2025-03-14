using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputManager : MonoBehaviour
{
    StateDecider stateDecider; // input �� ���� ���
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
        //������ ��� �ʱ�ȭ
        //ResetKeepPress();

        // !_isInnerCall �˻�� �� ?  ������ �Է� => ���� �Է� ������ => ���� ����, ������ InnerCall => ������ �Է� => ����
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

        //���� ȣ���� �ƴϸ� prekeycode�� �ٲ�
        //isChanging�� true�� �� prekey �Է��� �����ϸ� ��ٷ� ���� �ԷµǾ ������
        if(!_isInnerCall && !motionAnimator.isChanging) 
            StartCoroutine(ResetPreKeyCode(_curKey));
    }
    void KeyUp(KeyCode _releaseKey)
    {
        ResetKeepPress();

        //���� ���� Ű ����
        //���� �� �ݴ� Ű�� ���� ������ �ش� Ű�� ����ȣ��
        //�ƹ��� Ű�� �� ���� ���¶�� None ȣ��

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
        //Ư�� Ű ��� ������ �ִ� �� ����
        yield return new WaitForSeconds(2f);

        GiveInput(_input);
    }
    IEnumerator ResetPreKeyCode(KeyCode _key)
    {
        //���� �Է� ���������� ���� �ð� prekey ����
        preKey = _key;

        yield return new WaitForSeconds(0.5f);

        preKey = KeyCode.None;
    }
    #endregion

    #region OutCall
    // called by end of animation
    public void SetAutoInput()
    {
        //���� �׼� ������ ���� ���� Ű ����ȣ��
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
        //stateDecider���� ��ǲ ����
        stateDecider.ReceiveInput(_input);
    }
    #endregion

}
