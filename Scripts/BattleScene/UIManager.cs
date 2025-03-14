using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cinemachine;
using UnityEngine.U2D.Animation;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public ManaBar manaBar;
    public BulletBar bulletBar;
    public HpBar hpBar;
    public ExpBar expBar;
    public EnemyHpBar enemyHpBar;

    [Header("-")]
    public GameObject damagePrefab;
    public Transform damageParents;
    public List<GameObject> damages = new List<GameObject>();

    [Header("-")]
    public GameObject optionPanel;
    public OptionPage[] options;

    [Header("-")]
    public Transform arriveMessage;
    public CinemachineVirtualCamera vcam;
    public Image fade;

    [Header("-")]
    public TextMeshProUGUI playerHpText;
    public TextMeshProUGUI playerExpText;

    [Header("-")]
    public GameObject finishPanel;
    public TextMeshProUGUI[] finishText;
    public TextMeshProUGUI[] rewardTexts;
    public RectTransform rewardFinishButton;
    public ResultPaper resultPaper;

    [Header("-")]
    public GameObject logTextPrefab;
    public Transform logParents;
    public List<GameObject> logs = new List<GameObject>();
    public int logCount = 0;

    [Header("-")]
    public RectTransform mark;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        fade.gameObject.SetActive(true);
        fade.DOFade(0, 1f).OnComplete(()=>fade.gameObject.SetActive(false));

        //

        vcam.Follow = GameManager.instance.player.transform;
    }

    public void ActiveBar()
    {
        bulletBar.gameObject.SetActive(false);
        manaBar.gameObject.SetActive(false);

        var tmpType = GameManager.instance.player.weapon.type;

        if(tmpType == 3 || tmpType == 4)
        {
            bulletBar.gameObject.SetActive(true);
            bulletBar.SetMaxBullet();
        }
        else if(tmpType == 5 || tmpType ==6)
        {
            manaBar.gameObject.SetActive(true);
            manaBar.SetMaxMana();
        }
    }

    public void FloatLog(string _text)
    {
        GameObject tmp = null;

        

        foreach(GameObject i in logs)
        {
            if (i.activeSelf) continue;
            else tmp = i;
        }

        if (tmp == null)
        {
            tmp = Instantiate(logTextPrefab, logParents);
            logs.Add(tmp);
        }

        tmp.gameObject.SetActive(true);
        var tmpRect = tmp.GetComponent<RectTransform>();
        var tmpText = tmp.GetComponent<TextMeshProUGUI>();
        tmpRect.anchoredPosition = new Vector3(0, -100 + -60 * logCount);
        tmpText.color = Color.white;
        tmpText.DOFade(0, 1f).SetDelay(0.5f).OnComplete(()=> {
            tmp.SetActive(false);
            logCount -= 1;});

        tmpText.text ="-"+ _text;

        logCount += 1;
    }

    public void ShowArriveText(string _text)
    {
        arriveMessage.gameObject.SetActive(true);

        var arriveText = arriveMessage.GetChild(1).GetComponent<TextMeshProUGUI>();
        var arriveShadow = arriveMessage.GetChild(0).GetComponent<TextMeshProUGUI>();

        DOTween.Kill(arriveText);
        DOTween.Kill(arriveShadow);

        arriveText.text = _text;
        arriveShadow.text = _text;

        arriveText.color = new Color(1, 1, 1, 1);
        arriveShadow.color = new Color(0, 0, 0, 0.5f);

        arriveText.DOFade(0, 2);
        arriveShadow.DOFade(0, 2).OnComplete(() => arriveMessage.gameObject.SetActive(false));

        
    }

    public void FloatDamage(Transform _canvas, int _damage, int _dir, Color _color, Color _outlineColor)
    {
        var tmpDamage = Instantiate(damagePrefab, _canvas);
        tmpDamage.GetComponent<FloatingDamage>().Set( _damage, _color, _outlineColor);

        damages.Add(tmpDamage);
    }

    public void ShakeCamera(float _power=1, float _time=0.5f)
    {
        CinemachineBasicMultiChannelPerlin tmpVcamShaker = 
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        tmpVcamShaker.m_AmplitudeGain = _power;
        DOTween.To(() => 
        tmpVcamShaker.m_AmplitudeGain, x => 
        tmpVcamShaker.m_AmplitudeGain = x, 0f, _time);
    }

    public void SetOptionPage()
    {
        optionPanel.SetActive(true);

        for(int i = 0; i<3; i++)
        {
            options[i].SetOption(DataManager.instance.AllOptionDataList[Random.Range(0, DataManager.instance.AllOptionDataList.Count)]);
        }
        Time.timeScale = 0;
    }

    //의뢰 성공 실패 텍스트 설정
    public void ClearQuest(bool _isClear)
    {
        finishPanel.SetActive(true);

        vcam.m_Lens.OrthographicSize = 5; 
        DOTween.To(() => vcam.m_Lens.OrthographicSize, x => vcam.m_Lens.OrthographicSize = x, 3f, 3f);

        finishText[0].color = new Color(0.5f, 0.5f, 0.5f, 0);
        finishText[1].color = new Color(1, 1, 1, 0);

        finishText[0].DOFade(1, 2);
        finishText[1].DOFade(1, 2).OnComplete(()=> {

            finishText[0].DOFade(0, 0.5f);
            finishText[1].DOFade(0, 0.5f).OnComplete(()=>
            {
                resultPaper.transform.parent.parent.gameObject.SetActive(true);
                resultPaper.Set();
                resultPaper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
            });
        });



        if (_isClear)
        {
            finishText[0].text = "의뢰 성공";
            finishText[1].text = "의뢰 성공";
        }
        else
        {
            finishText[0].text = "의뢰 실패";
            finishText[1].text = "의뢰 실패";
        }

        //SetResultText();
        
    }


    public void SetMark(string _cat)
    {
        mark.GetComponent<SpriteResolver>().SetCategoryAndLabel("Head", _cat);
        mark.GetComponent<Image>().sprite = mark.GetComponent<SpriteRenderer>().sprite;
    }
    public void MoveMark(int _totalRound, int _currentRound)
    {
        mark.DOAnchorPosX( (1300/_totalRound) * (_totalRound-_currentRound)  , 2f).SetEase(Ease.InOutCubic);
    }
}
