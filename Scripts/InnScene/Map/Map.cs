using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using TMPro;

public class Map : MonoBehaviour
{
    [Header("Skull")]
    public List<SkullMark> skullMarks, activeSkullMarks;
    int count = 0, index = -1;
    int step = 0;


    [Header("UI")]
    public TextMeshProUGUI title;

    void Update()
    {
        if(step == 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveSkull(true);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveSkull(false);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectSkull();
            }
        }
        if (step == 2)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ConfirmSkull();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CancleSkull();
            }
        }

    }

    #region 뽑기

    //가챠
    public void SetSkull()
    {
        activeSkullMarks = new List<SkullMark>();
        DrawSkull();
    }

    public void DrawSkull()
    {
        if (count > 2)
        {
            FinishSetSkull();
            return;
        }

        while (true)
        {
            int number = Random.Range(0, skullMarks.Count);

            if (!skullMarks[number].gameObject.activeSelf)
            {
                count++;
                skullMarks[number].Set( DataManager.instance.AllHuntTargetDataList[Random.Range(0, 9)].name );
                activeSkullMarks.Add(skullMarks[number]);
                SetTitle(skullMarks[number].huntTargetData.name, skullMarks[number].huntTargetData.GetSkullRankColor());
                break;
            }
        }
    }

    void SetTitle(string _text, Color _color)
    {
        DOTween.Kill(title);

        title.gameObject.SetActive(true);
        title.GetComponentInChildren<ParticleSystem>().Play();
        title.text = _text;

        title.color = _color;
    }
    void OffTitle()
    {

        DOTween.Kill(title);
        title.DOFade(0, 2f).OnComplete(() => title.gameObject.SetActive(false));
    }
    //마무리 선택으로 넘어가야 함
    void FinishSetSkull()
    {
        OffTitle();

        activeSkullMarks = activeSkullMarks.OrderBy(skull => skullMarks.IndexOf(skull)).ToList();

        InnSceneManager.instance.DelayCall(0.2f, () => {
            TextManager.instance.PlayTalk("주인",
                new List<System.Action>() { ()=> 
                {
                    InnSceneManager.instance.DelayCall(0.2f, () => { step = 1; });
                    MoveSkull(false); 
                } },
                new List<string>() { "최근에 재미있는 소식을 들었지", "숲 초입에 고블린 우두머리가 나타났다더군..." }, 0, true);

        });
    }

    #endregion

    #region 선택

    void MoveSkull(bool _isUp)
    {
        if (index != -1)
            activeSkullMarks[index].OffSkull();

        if (_isUp)
        {
            index = index - 1 < 0 ? activeSkullMarks.Count - 1 : index - 1;
        }
        else
        {
            index = index + 1 >= activeSkullMarks.Count ? 0 : index + 1;
        }

        activeSkullMarks[index].OnSkull();
    }

    void SelectSkull()
    {
        step = 0;

        activeSkullMarks[index].SelectSkull();

        InnSceneManager.instance.DelayCall(0.2f, () => {
            TextManager.instance.PlayTalk("주인",
                new List<System.Action>() {()=> {
                InnSceneManager.instance.DelayCall(0.2f, () => { step = 2; });
                } },
                new List<string>() { "흠...그래", $"{activeSkullMarks[index].huntTargetData.name}으로 갈텐가?" }, 0, true);

        });
    }

    /*
      
     */

    void ConfirmSkull()
    {
        step = 0;

        activeSkullMarks[index].ConfirmSkull();

        InnSceneManager.instance.DelayCall(0.2f, () => {
            TextManager.instance.PlayTalk("주인",
                new List<System.Action>() { FinishSelect },
                new List<string>() { "그래, 알겠네" }, 0, true);

        });
    }
    void CancleSkull()
    {
        activeSkullMarks[index].CancleSkull();
        step = 1;
    }

    #endregion

    void FinishSelect()
    {
        DataCarrier.instance.huntTargetData = activeSkullMarks[index].huntTargetData;

        InnSceneManager.instance.DelayCall(0.5f, ()=> { InnSceneManager.instance.MoveMap(false); });
    }

}
