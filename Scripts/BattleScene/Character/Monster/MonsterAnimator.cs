using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MonsterMotion { Idle, Move, BackMove, Hit, Dead, Act0, Act1, Act2, Act3, Act4 }
public class MonsterAnimator : MonoBehaviour
{
    MotionAnimator motionAnimator;
    RectTransform rect;
    [HideInInspector] public bool isChanging;

    void Awake()
    {
        motionAnimator = GetComponent<MotionAnimator>();

        motionAnimator.flipSprite += FlipHpBar;
    }

    void FlipHpBar(int _direction)
    {
        /*
        if (_direction == 0)
        {
            rect.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rect.localScale = new Vector3(-1, 1, 1);
        }
        */
    }
}