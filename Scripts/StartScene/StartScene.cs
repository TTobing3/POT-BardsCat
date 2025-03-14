using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
public class StartScene : MonoBehaviour
{
    public GameObject playerCharacter; // I DONT Know Why Is This Disable,,,,

    public static StartScene instance;

    public GameObject startPage;
    public SmithPage smithPage;
    public GamblePage gamblePage;
    public StorePage storePage;

    public StoryPage storyPage;

    public Image infoPageEvent;

    enum Page { SelectQuest, QuestDetail, Ready }
    Page curPage;

    [SerializeField] RectTransform table;
    [SerializeField] GameObject page;
    [SerializeField] QuestDetailPage detailPage;
    [SerializeField] TextMeshProUGUI warrningText;
    public AudioClip[] uiClips;
    public AudioClip[] voiceClips;

    public RectTransform[] 
        questPages = new RectTransform[3], 
        detailPageUI = new RectTransform[3],  
        eqPages = new RectTransform[4]; // back, quest, eqpage, eqbox

    public Image fade;

    int selectedQuestNumber = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        ResetPlayerSelect();
    }

    void MoveTable(bool _toDown, System.Action _action)
    {

        SoundPlay(uiClips[0]);

        table.gameObject.SetActive(true);
        detailPageUI[0].DOAnchorPos(new Vector2(25, -25), 1);

        if (_toDown)
        {
            table.DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.OutQuint).OnComplete(() => _action());
        }
        else
        {
            table.DOAnchorPos(new Vector2(0, 1200), 1f).SetEase(Ease.OutQuint);
        }
    }

    #region Quest

    void ResetPlayerSelect()
    {
        DataCarrier.instance.playerData.item[0] = "";
        DataCarrier.instance.storage.selectItemInfo[0] = -1;
        if (DataCarrier.instance.playerData.cat == "") DataCarrier.instance.playerData.cat = "BlackCat";
    }

    #region Button Call

    //Start Button 의뢰 / 대장간 / 등등 고르는 거
    public void StartButton(int _n)
    {
        StartSceneManager.instance.SetButtonsInteractable(false);

        StartSceneManager.instance.pages[_n].SetActive(true);

        infoPageEvent.raycastTarget = false;

        switch(_n)
        {
            case 0:
                MoveTable(true, SetNewQuestPage);
                break;
            case 1:
                smithPage.gameObject.SetActive(true);
                smithPage.SetSmith(true);
                smithPage.SelectTab(0);
                break;
            case 2:
                gamblePage.SetGamble(true);
                break;
            case 3:
                storePage.SetStore(true);
                break;
        }
    }

    public void SetDefault()
    {
        infoPageEvent.raycastTarget = true;
    }

    //Back Button in QuestDetailPage
    public void BackButton()
    {
        if(curPage == Page.QuestDetail)
        {
            SetDetailQuestPage(false);
            SetQuestPagePosition();
            curPage = Page.SelectQuest;
        }
        else if (curPage == Page.Ready)
        {
            SelectQuestPage(selectedQuestNumber);
            MoveEqPage(false);
        }
        else if (curPage == Page.SelectQuest)
        {
            ReturnQuestPage();
            MoveTable(false, SetNewQuestPage);
            detailPageUI[0].DOAnchorPos(new Vector2(-200, -25), 1).OnComplete(()=>
            {

                StartSceneManager.instance.SetButtonsInteractable(true);
                startPage.SetActive(false);
            });
        }
    }
    //QuestSelect Button
    public void QuestSelectButton()
    {
        curPage = Page.Ready;

        SetDetailQuestPage(false);
        SetQuestPagePosition(false);

        eqPages[1].gameObject.SetActive(true);
        MoveEqPage(true);
    }
    //QuestStart Button
    public void QuestStartButton()
    {
        if (DataCarrier.instance.playerData.item[0] == "")
        {
            DOTween.Kill(warrningText);
            warrningText.color = new Color(1,0,0,1);
            warrningText.gameObject.SetActive(true);
            warrningText.text = "무기를 선택해주십시오.";
            warrningText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            warrningText.DOFade(0, 1f).SetDelay(0.5f);
            
            return;
        }
        playerCharacter.SetActive(false);

        storyPage.gameObject.SetActive(true);
        storyPage.GetComponent<Image>().DOColor(Color.black, 0.5f).OnComplete(()=> {
            storyPage.Set(
                "고블린 주술사",
                MoveToBattleScene, true);
        });
    }
    void MoveToBattleScene()
    {
        print("씬 이동");
        SceneManager.LoadScene("BattleScene");
        //fade.gameObject.SetActive(true);
        //fade.DOFade(1, 2).OnComplete(() => );
    }
    #endregion

    #region QuestPage
    void SetNewQuestPage()
    {
        if (questPages[0] == null)
        {
            for (int i = 0; i < 3; i++)
            {
                var tmpPage = Instantiate(page, table.transform).GetComponent<QuestPage>();
                tmpPage.Set(DataManager.instance.AllQuestDataList[Random.Range(0, DataManager.instance.AllQuestDataList.Count)], i);

                questPages[i] = tmpPage.GetComponent<RectTransform>();
                questPages[i].anchoredPosition = new Vector3(0, 1000);
            }
        }
        SetQuestPagePosition();
    }
    void SetQuestPagePosition(bool _set = true)
    {
        SoundPlay(uiClips[2]);
        if (_set)
        {
            for (int i = 0; i < 3; i++)
            {
                SoundPlay(uiClips[2]);
                questPages[i].DORotate(new Vector3(0, 0, Random.Range(-10, 10)), 1);
                questPages[i].DOAnchorPos(new Vector2(-600 + i * 600, Random.Range(-10, 10)), 1).SetEase(Ease.OutQuint);//.SetDelay((float)i / 3)
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                questPages[i].DORotate(new Vector3(0, 0, Random.Range(0, 0)), 1);
                questPages[i].DOAnchorPos(new Vector2(0, 1000), 1).SetEase(Ease.OutQuint);//.SetDelay((float)i / 3)
            }
        }
    }
    void SetDetailPage()
    {
        detailPage.Set(DataCarrier.instance.questData);
    }
    #endregion

    #region SelectPage
    public void SelectQuestPage(int _n)
    {
        DataCarrier.instance.questData = questPages[_n].GetComponent<QuestPage>().questData;

        curPage = Page.QuestDetail;
        selectedQuestNumber = _n;

        SetDetailQuestPage(true);

        for (int i = 0; i < 3; i++)
        {
            if (i == _n)
            {
                SoundPlay(uiClips[1]);
                questPages[i].DOAnchorPos(new Vector2(-410, 40), 1);
                questPages[i].DORotate(new Vector3(0, 0, 0), 1);
            }
            else
            {
                SoundPlay(uiClips[2]);
                questPages[i].DOAnchorPos(new Vector2(0, 1000), 1);
            }
        }
    }

    public void ReturnQuestPage()
    {
        for (int i = 0; i < 3; i++)
        {
                SoundPlay(uiClips[1]);
                questPages[i].DOAnchorPos(new Vector2(0, 1000), 1);
                questPages[i].DORotate(new Vector3(0, 0, 0), 1);
        }
    }
    void SetDetailQuestPage(bool _set)
    {
        SetDetailPage();

        if (_set)
        {
            foreach (RectTransform i in detailPageUI) i.gameObject.SetActive(true);

            detailPageUI[0].DOAnchorPos(new Vector2(25, -25), 1);
            detailPageUI[1].DOAnchorPos(new Vector2(550, 100), 1);
            detailPageUI[2].DOAnchorPos(new Vector2(0, 0), 1);
        }
        else
        {
            //detailPageUI[0].DOAnchorPos(new Vector2(-200, -25), 1);
            detailPageUI[1].DOAnchorPos(new Vector2(550, -100), 1);
            detailPageUI[2].DOAnchorPos(new Vector2(1000, 0), 1);
        }
    }
    #endregion

    #region EqPage

    void MoveEqPage(bool _set)
    {
        if(_set)
        {
            eqPages[3].GetComponent<EqBox>().SelectTab(0);

            eqPages[0].DOAnchorPos(new Vector2(25, -25), 1).SetEase(Ease.OutQuint);
            eqPages[1].DOAnchorPos(new Vector2(550, 100), 1).SetEase(Ease.OutQuint);
            eqPages[2].DOAnchorPos(new Vector2(0, 0), 1).SetEase(Ease.OutQuint);
            eqPages[3].DOAnchorPos(new Vector2(-410, -50), 1).SetEase(Ease.OutQuint);
        }
        else
        {
            //eqPages[0].DOAnchorPos(new Vector2(25, -25), 1).SetEase(Ease.OutQuint);
            eqPages[1].DOAnchorPos(new Vector2(550, -100), 1).SetEase(Ease.OutQuint);
            eqPages[2].DOAnchorPos(new Vector2(1000, 0), 1).SetEase(Ease.OutQuint);
            eqPages[3].DOAnchorPos(new Vector2(-1500, -50), 1).SetEase(Ease.OutQuint);
        }
    }

    #endregion

    #endregion

    public void SoundPlay(AudioClip _clip)
    {
        SoundManager.instance.SfxPlay(_clip);
    }
}
