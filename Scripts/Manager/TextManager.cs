using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using TMPro;
using DG.Tweening;

public class TextManager : MonoBehaviour
{
    public System.Action NextAction; // 텍스트 이후 호출

    public static TextManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            TalkNext();
        }
    }

    [Header("Talk")]
    public GameObject talkPage;
    public Image talkIllust;
    public TextMeshProUGUI talkName;
    public TextMeshProUGUI talkText;
    public List<string> nextTalks = new List<string>();

    #region Talk
    /// <summary>
    /// 
    /// </summary>
    /// <param name="화자 이름"></param>
    /// <param name="대화 종료 시 실행할 함수"></param>
    /// <param name="대사 리스트"></param>
    /// <param name="대사 간 딜레이"></param>
    /// <param name="일러스트 여부"></param>
    public void PlayTalk(string _name, List<System.Action> _action, List<string> _conversation, float _delay = 0, bool _illust = false)
    {
        NextAction = null;

        if (_action.Count != 0)
        {
            for (int i = 0; i < _action.Count; i++) NextAction += _action[i];
        }

        foreach (string i in _conversation) nextTalks.Add(i);

        StartCoroutine(CoPlayTalk(_name, _delay, _illust));
    }

    IEnumerator CoPlayTalk(string _name, float _delay, bool _illust)
    {
        yield return new WaitForSeconds(_delay);

        SetTalk(_name, _illust);
        TalkNext();
    }

    void Talk(string _talk)
    {
        talkText.DOKill();
        talkText.text = "";
        talkText.DOText(_talk, 0.1f);
    }

    void SetTalk(string _name, bool _isIllust = false)
    {
        talkPage.SetActive(true);
        talkName.text = _name;
        talkIllust.GetComponent<SpriteResolver>().SetCategoryAndLabel("Portrait", _name);
        talkIllust.sprite = talkIllust.GetComponent<SpriteRenderer>().sprite;
        talkIllust.SetNativeSize();

        if (_isIllust)
        {
            talkIllust.gameObject.SetActive(true);
        }
        else
        {
            talkIllust.gameObject.SetActive(false);
        }
    }

    public void TalkNext()
    {
        if (nextTalks.Count <= 0)
        {
            FinishTalk();
        }
        else
        {
            Talk(nextTalks[0]);
            nextTalks.RemoveAt(0);
        }
    }

    void FinishTalk()
    {
        talkPage.gameObject.SetActive(false);
        if (NextAction != null)
        {
            NextAction();
            NextAction = null;
        }
    }

    #endregion

}
