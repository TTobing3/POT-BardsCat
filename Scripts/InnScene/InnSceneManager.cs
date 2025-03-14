using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class InnSceneManager : MonoBehaviour
{
    public static InnSceneManager instance;
    [Header("Player")]
    public Transform player;

    [Header("UI")]
    public Image screenShadow;
    public RectTransform mapRect;

    [Header("Map")]
    public Map map;

    #region 기능
    public void DelayCall(float delay, System.Action action)
    {
        StartCoroutine(CoDelayCall(action, delay));
    }
    IEnumerator CoDelayCall(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
    void MovePlayer(float _duration, float endX, float startX = 0)
    {
        if (startX == 0) startX = player.position.x;

        var direction = startX > endX ? 0 : 1;

        var playerAnimator = player.GetComponent<MotionAnimator>();
        playerAnimator.ReceiveAnimation((int)Motion.Move, direction, true);

        player.position = new Vector3(startX, -2.5f, 0);
        player.DOMoveX(endX, _duration).SetEase(Ease.Linear).OnComplete(() => playerAnimator.ReceiveAnimation((int)Motion.Idle, direction, true));
    }
    void SetShadow(float _value, float _duration)
    {
        screenShadow.gameObject.SetActive(true);
        screenShadow.color = new Color(0, 0, 0, 0);
        screenShadow.DOFade(_value, _duration);
    }
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        StartInn();
    }

    void StartInn()
    {
        MovePlayer(2, 10, -5);
        CameraManager.instance.Zoom(5, 2);

        DelayCall(2, ToInn);
    }

    void ToInn()
    {
        SetShadow(0.7f, 1);

        DelayCall(1, SetMap);
    }

    void SetMap()
    {
        TextManager.instance.PlayTalk("주인",
            new List<System.Action>() { ()=> { MoveMap(true);  } },
            DataManager.instance.AllConversationDatas["처음"].texts.ToList(), 0, true);

    }
    //시작 움직임
    
    public void MoveMap(bool _on = true)
    {
        if(_on)
        {
            mapRect.gameObject.SetActive(true);
            mapRect.anchoredPosition = new Vector2(2000, 0);
            mapRect.DOAnchorPosX(0, 1).SetEase(Ease.OutBack).OnComplete(() => map.SetSkull());
        }
        else
        {
            mapRect.anchoredPosition = new Vector2(0, 0);
            mapRect.DOAnchorPosX(2000, 1).SetEase(Ease.OutBack).OnComplete(() =>
            {
                mapRect.gameObject.SetActive(false);
                FinishMap();
            });
        }
    }

    void FinishMap()
    {
        SetShadow(0f, 1f);

        DelayCall(1f, () => { SetShadow(0.8f, 0.5f); });
        DelayCall(1f, () => {

            print("call");
            TextManager.instance.PlayTalk("도넛",
                new List<System.Action>(),
                DataManager.instance.AllConversationDatas["도넛의 환영"].texts.ToList(), 0, true);

        });


    }
}
