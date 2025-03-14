using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using DG.Tweening;
using TMPro;
public class StartSceneManager : MonoBehaviour
{
    public static StartSceneManager instance;
    public GameObject[] pages;
    public Canvas canvas;

    public System.Action NextAction; // �ؽ�Ʈ ���� ȣ��
    //System.Action<string, string> ActionTalk;

    //DataCarrier.step���� �ٷ�

    [Header("Player")]
    public Transform player;

    [Header("Guitar")]
    public Image fade;
    public Button[] buttons;

    [Header("StartPage")]
    public GameObject startBack;
    public GameObject toInnButton;

    [Header("InnPage")]
    public GameObject innPage;
    public GameObject innBack;
    public Transform buttonParents;
    public InfoBar infoBar;
    public InfoPage infoPage;

    [Header("Talk")]
    public GameObject talkPage;
    public Image talkIllust;
    public TextMeshProUGUI talkName;
    public TextMeshProUGUI talkText;
    public List<string> nextTalks = new List<string>();

    //

    [Header("ToolTip")]
    public Tooptip toolTip;

    [Header("Sound")]
    bool[] mute = new bool[2];
    public Image sfxButton, bgmButton;


    private void Awake()
    {
        player.GetComponent<PlayerAnimator>().ActionCompleteFormChange += LockPlayerInput;

        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

    }

    private void Start()
    {
        SetStory();

        SetCat();

        SoundManager.instance.BgSoundPlay(0);
    }

    public void SoundButton(bool _isBgm)
    {

        if (_isBgm)
        {
            if (mute[0])
            {
                SoundManager.instance.BgmVolume(10);
                bgmButton.color = Color.white;
                mute[0] = false;
            }
            else
            {
                SoundManager.instance.BgmVolume(-100);
                bgmButton.color = Color.gray;
                mute[0] = true;
            }
        }
        else
        {

            if (mute[1])
            {
                SoundManager.instance.SfxVolume(-15);
                sfxButton.color = Color.white;
                mute[1] = false;
            }
            else
            {
                SoundManager.instance.SfxVolume(-100);
                sfxButton.color = Color.gray;
                mute[1] = true;
            }
        }
    }

    public void StartSceneGame()
    {
        //���� ���� �ڵ� �̵�
        StartCoroutine(CoDelay(MovePlayerToInn));
    }

    void SetStory()
    {
        StartScene.instance.storyPage.gameObject.SetActive(true);
        StartScene.instance.storyPage.Set(
            DataCarrier.instance.story,
            StartSceneGame);
    }

    void SetCat()
    {
        //����� ����
        player.GetComponent<Player>().weapon = DataManager.instance.AllWeaponDatas["�� ��"];
        SetCat(DataCarrier.instance.playerData.cat);
        ChangePlayerWeapon();
        SetArmor();
    }

    public void SetButtonsInteractable(bool _isSetActive)
    {
        foreach (Button i in buttons) i.interactable = _isSetActive;
        StartScene.instance.infoPageEvent.raycastTarget = true;
    }


    #region Fade

    void FadeOut(bool _loadFadeIn = false, float _time = 1, System.Action _action = null)
    {
        fade.gameObject.SetActive(true);
        fade.color = new Color(0, 0, 0, 0);
        fade.DOFade(1, _time).OnComplete(() =>
        {
            if (_action != null) _action();
            if (_loadFadeIn) FadeIn(false, _time);
            //fade.gameObject.SetActive(false);
        });
    }

    void FadeIn(bool _loadFadeOut = false, float _time = 1, System.Action _action = null)
    {
        fade.gameObject.SetActive(true);
        fade.color = new Color(0, 0, 0, 1);
        fade.DOFade(0, _time).OnComplete(() =>
        {
            if (_action != null) _action();
            if (_loadFadeOut) FadeOut(false, _time);
            fade.gameObject.SetActive(false);
        });
    }

    #endregion

    #region Cat

    void SetCat(string _cat)
    {

        if (_cat == "") return;
        DataCarrier.instance.storage.selectCatInfo = DataCarrier.instance.storage.cats.IndexOf(_cat);
        DataCarrier.instance.playerData.cat = _cat;

        player.GetComponent<PlayerSpriteSetter>().ChangeCat(_cat);

        string catStat = "";
        foreach (int i in DataManager.instance.AllCatDatas[DataCarrier.instance.playerData.cat].stats) catStat += i + "\n";
    }

    void SetArmor()
    {
        player.GetComponent<PlayerSpriteSetter>().ChangeHelmet(DataCarrier.instance.playerData.item[1]);
        player.GetComponent<PlayerSpriteSetter>().ChangeArmor(DataCarrier.instance.playerData.item[2]);
    }

    void MovePlayerToInn() // �̵�
    {
        DOTween.Kill(player);
        player.GetComponent<MotionAnimator>().ReceiveAnimation((int)Motion.Move, 1, true);
        player.position = new Vector3(-10, -2.5f, 0);
        player.DOMoveX(10, 5).SetEase(Ease.Linear).OnComplete(() => OnInnPage());
    }
    void MovePlayerToMaster()
    {
        DOTween.Kill(player);
        player.GetComponent<MotionAnimator>().ReceiveAnimation((int)Motion.Move, 1, true);
        player.position = new Vector3(-10, -1.6f, 0);
        player.DOMoveX(2.5f, 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            player.GetComponent<PlayerAnimator>().ReceiveNewForm(1);
            SetInn();
        });
    }
    #endregion

    #region Page Start

    public void SetInn()
    {
        SetTalk("����", true);

        StartScene.instance.SoundPlay(StartScene.instance.voiceClips[0]);

        if (DataCarrier.instance.step == 0)
        {
            nextTalks.Add("����, ���� ��굢�̰� �ϳ� �߰��ǰڱ�...");
            nextTalks.Add("���� �ּ���, ������ ��û�ϰ� ������ ���� �Ƿڳ� ���ٿ����");

        }

        if (DataCarrier.instance.step == 1)
        {
            nextTalks.Add("���� ��Ƶ��ƿԱ�...");
            nextTalks.Add("�׷�, �� ���ƺ��� ������� �ٲٶ��");
        }

        if (DataCarrier.instance.step == 2)
        {
            nextTalks.Add("��... �� �ϳ� ������");
            nextTalks.Add("�ֻ��� ���� �غ����� ����?");
        }

        if (DataCarrier.instance.step == 3)
        {
            nextTalks.Add("�ϳ� �ټ��� �ִ� �༮�̱���");
            nextTalks.Add("��ħ ������ �鷶���ϱ�, �ʿ��� ������ �ִٸ� ��δ� �� ��õ����");
        }

        if (DataCarrier.instance.step > 3)
        {
            nextTalks.Add("�׷�, �̹� ���� ���̰� �� �Ƴ�?");
        }

        TalkNext();

        NextAction = SetButtons;
    }

    void SetButtons()
    {
        DataCarrier.instance.step += 1;

        SoundManager.instance.SfxPlay(StartScene.instance.uiClips[6]);
        for (int i = 0; i < DataCarrier.instance.step; i++)
        {
            if (i < 4)
            {
                var tmpRect = buttonParents.GetChild(i).GetComponent<RectTransform>();
                tmpRect.anchoredPosition = new Vector2(tmpRect.anchoredPosition.x, 800);
                tmpRect.DOAnchorPosY(280, 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.2f);

            }
            else
            {
                break;
            }
        }
    }
    void ChangePlayerWeapon()
    {
        var tmpPlayer = player.GetComponent<Player>();
        tmpPlayer.spriteSetter.ChangeWeapon(tmpPlayer.weapon.name);
        tmpPlayer.animator.SetInteger("Weapon", tmpPlayer.weapon.type);

        if (!tmpPlayer.weapon.form)
        {
            tmpPlayer.animator.SetInteger("Form", 1);
            tmpPlayer.stateDecider.form = Form.Lock;
            tmpPlayer.stateDecider.ResetMotion();
        }
    }
    void LockPlayerInput()
    {
        player.GetComponent<MotionAnimator>().ReceiveAnimation((int)Motion.Idle, 1, true);
    }

    #endregion

    #region InnPage

    public void OnInnPage()
    {
        toInnButton.gameObject.SetActive(false);
        FadeOut(true, 2f, () =>
        {
            startBack.gameObject.SetActive(false);
            innPage.gameObject.SetActive(true);
            innBack.gameObject.SetActive(true);
            MovePlayerToMaster();
            SetButtons(800);
            infoBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 110);
            infoBar.Set();
        });
    }


    public void SetButtons(float _posY = 280)
    {
        for (int i = 0; i < 4; i++)
        {
            var tmpRect = buttonParents.GetChild(i).GetComponent<RectTransform>();
            tmpRect.anchoredPosition = new Vector2(tmpRect.anchoredPosition.x, 800);
            tmpRect.DOAnchorPosY(_posY, 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.2f);
        }

        StartScene.instance.SoundPlay(StartScene.instance.uiClips[7]);
    }

    #endregion

    #region Talk


    public void PlayTalk(string _name, List<System.Action> _action, List<string> _conversation, float _delay = 0, bool _illust = false)
    {
        //�̰� ���ϴ� ����??
        NextAction = null;

        if (_action.Count != 0)
        {
            NextAction += _action[0];
            for (int i = 1; i < _action.Count - 1; i++)
                NextAction += _action[i];
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

    public void Talk(string _talk)
    {
        talkText.DOKill();
        talkText.text = "";
        talkText.DOText(_talk, 0.1f);
    }

    public void SetTalk(string _name, bool _isIllust = false)
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
        //�Ϸ���Ʈ ���õ�
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

    public void FinishTalk()
    {
        talkPage.gameObject.SetActive(false);
        if (NextAction != null) NextAction();
    }

    #endregion

    #region NextAction


    #endregion

    IEnumerator CoDelay(System.Action _action, float _time = 1)
    {
        yield return new WaitForSeconds(_time);

        _action();
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
