using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GamblePage : MonoBehaviour
{
    [Space(10)]
    public RectTransform tableRect;
    public RectTransform backButtonRect, gameTableRect, playerActButtonRect, fateButtonRect;
    public Button battleButton, fateButton;

    [Space(10)]
    public Transform betParents;
    public GameObject betPrefab;
    public List<GameObject> bets = new List<GameObject>();
    public TextMeshProUGUI stakeText, betText;
    public TextMeshProUGUI playerRankText, enemyRankText;

    [Space(10)]
    public List<RectTransform> golds = new List<RectTransform>();
    public RectTransform[] dices;
    public Sprite[] eyeSprites;

    [Space(10)]
    bool isPlayerWin = false;
    bool[] isPlaying = new bool[] { false, false }, handConfirmed = new bool[] { false, false }; // �̰� �Ƿ� ���ٿ��� ����
    int currentState = 0, currentGamble = -1;
    int[] bet = new int[] { 0, 0 }; // playerbet, enemybet
    int[] eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };

    public AudioClip[] gambleClips;

    //��� ���� ctrl m o

    int amount
    {
        get { return bet[0] + bet[1]; }
    }

    //���ӿ� ���� ������ ���� �ճ��m
    //�ǵ��ϰ� ����

    int[,] betSave = new int[,] { { 0, 0 }, { 0, 0 } };
    int[] eyeSave = new int[] { 0, 0, 0, 0, 0, 0 };

    #region ���

    //�ڷΰ��� ��ư
    public void BackButton()
    {
        switch (currentState)
        {
            case 1:
                currentState = 0;
                tableRect.DOAnchorPosY(1200, 0.6f);
                backButtonRect.DOAnchorPosX(-200, 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
                    gameObject.SetActive(false);
                    StartSceneManager.instance.SetButtonsInteractable(true);
                });
                break;
            case 2:
                currentState = 1;
                tableRect.DOAnchorPosY(0, 0.6f);
                gameTableRect.DOAnchorPosY(-1080, 0.6f).OnComplete(() =>
                {
                    foreach (GameObject i in bets) i.SetActive(false);
                    foreach (RectTransform i in dices) i.anchoredPosition = Vector2.zero;
                    playerActButtonRect.anchoredPosition =  Vector2.zero;
                    ResetBet();
                });
                battleButton.interactable = true;
                fateButton.interactable = true;
                break;
        }
    }

    //����
    public void Bet(bool _isPlayer, int _amount)
    {
        if (_isPlayer) DataCarrier.instance.GainGold(-_amount) ;

        var tmpAmount = bet[0] + bet[1];

        bet[_isPlayer ? 0 : 1] += _amount;

        betSave[currentGamble, _isPlayer ? 0 : 1] = bet[_isPlayer ? 0 : 1]; // �ٽ� ������ �� ���

        BetAmount(_isPlayer, _amount);

        betText.DOCounter(0, _amount, 1f);
        stakeText.DOCounter(tmpAmount, amount, 1f);


        StartScene.instance.SoundPlay(gambleClips[1]);
    }
    void BetAmount(bool _isPlayer, int _amount)
    {
        string[,] unit = new string[,] { { "��ȭ", "100" }, { "��ȭ", "10" }, { "��ȭ", "1" } };

        for (int u = 0; u < unit.Length / 2; u++)
        {
            for (int i = 0; i < _amount / int.Parse(unit[u, 1]); i++)
            {
                GameObject bet = null;
                foreach (GameObject j in bets)
                {
                    if (!j.activeSelf && j.name == unit[u, 0])
                    {
                        bet = j;
                        j.SetActive(true);
                        break;
                    }
                }

                if (bet == null)
                {
                    bet = Instantiate(betPrefab, betParents);
                    bet.GetComponent<Bet>().Set(unit[u, 0]);
                    bets.Add(bet);
                }

                bet.GetComponent<Bet>().SetPosition(_isPlayer);
            }
            _amount = _amount % int.Parse(unit[u, 1]);
        }
    }

    //���� �ʱ�ȭ
    void ResetBet()
    {
        bet = new int[] { 0, 0 };
        stakeText.text = 0 + "";
        betText.text = 0 + "";
    }

    //�ֻ��� ����ġ
    void RepositionDice()
    {
        foreach (RectTransform i in dices)
        {
            i.DOAnchorPos(Vector2.zero, 1);
            i.DOScale(new Vector3(1, 1), 1);
        }
    }

    #endregion

    #region ����
    // 0. ���� ���� ���̺� ����
    public void SetGamble(bool _set)
    {
        if( DataCarrier.instance.step == 2 )
        {
            StartSceneManager.instance.PlayTalk("����",
                new List<System.Action>()
                {
                },
                new List<string>()
                {
                    "����, �ּ��� �༮ ȯ���Ѵ�...",
                    "�ֻ��� ������ ���� ��������, �ֻ����� ���� ������ ���� ���Ͽ� ���и� ������.",
                    "�������� �ºθ� ����, �߰��� ���þ��� �ø� �� ���� �׸��� �� �ƴϴ� ������ �����ϴ� �͵� ����ϼ�",
                    "�׷� �Ѳ� ��ܺ����"
                }, 0, true);
        }


        StartScene.instance.SoundPlay(StartScene.instance.uiClips[0]);
        SoundManager.instance.BgSoundPlay(1);

        if (_set)
        {
            currentState = 1;
            tableRect.DOAnchorPosY(0, 0.6f).OnComplete(() => {
                backButtonRect.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutBack);
                battleButton.interactable = true;
                fateButton.interactable = true;
            });
        }
        else
        {
            tableRect.DOAnchorPosY(1200, 0.6f).SetDelay(0.5f);
            backButtonRect.DOAnchorPosX(-200, 0.5f).SetEase(Ease.OutBack);
        }
    }

    //1. ������ ���� ����
    public void SetGame(int _type)
    {
        StartScene.instance.SoundPlay(StartScene.instance.uiClips[0]);
        if (currentState == 0)
            return;

        currentState = 2;
        currentGamble = _type;
        tableRect.DOAnchorPosY(1080, 0.6f);
        backButtonRect.GetComponent<Button>().interactable = false;

        if (isPlaying[_type])
        {
            gameTableRect.DOAnchorPosY(0, 0.6f).SetEase(Ease.Unset).OnComplete(() =>
            {
                StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    StartPreGame
                },
                new List<string>()
                {
                        "�̺�, ���� �߿� ������ �ųʴ� �� �� ��ü���� �����?"
                },
                0.2f);
            });
            return;
        }
        else
        {
            isPlaying[_type] = true;
        }

        gameTableRect.DOAnchorPosY(0, 0.6f).SetEase(Ease.Unset).OnComplete(() =>
        {
            backButtonRect.DOAnchorPosX(-200, 0.5f).SetEase(Ease.OutBack);
            backButtonRect.GetComponent<Button>().interactable = false;

            switch (_type)
            {
                case 0:
                    StartSceneManager.instance.PlayTalk("���ڲ�",
                        new List<System.Action>()
                        {
                            StartGame
                        },
                        new List<string>()
                        {
                            "����, ȣ���ڽ� �� �� ���Ա���!",
                            "���� �ɾƵ� �ǰ���?"
                        },
                        0.2f);
                    break;

                case 1:

                    StartSceneManager.instance.PlayTalk("���ڲ�",
                        new List<System.Action>()
                        {
                            StartGame
                        },
                        new List<string>()
                        {
                            "����, ȣ���ڽ� �� �� ���Ա���!",
                            "���� �ɾƵ� �ǰ���?"
                        },
                        0.2f);
                    break;

            }
        });
    }

    //2. ���ڲ� �ൿ
    public void StartGame()
    {
        eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };
        ResetBet();

        if(currentGamble == 0)
        {
            backButtonRect.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutBack);

            Bet(false, 123);

            StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                SetPlayerTurn
                },
                new List<string>()
                {
                    "������, ��������",
                    "���� " + 123 + "��ŭ ����",
                    "�̺� �ּ���, �ʴ� ��¿��?"
                }, 1);
        }
        else
        {
            RollDice(3);
            SetPlayerDice(0);

            StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    null
                },
                new List<string>()
                {
                    "��������",
                    "�� �� �ֻ����� �������ڰ�",
                }, 1);
        }
    }

    //2-1. ���� ���� �̾
    void StartPreGame()
    {
        ResetBet();
        if (currentGamble == 1) SetPreDice();
        else eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };


        Bet(true, betSave[currentGamble, 0]);
        Bet(false, betSave[currentGamble, 1]);


        SetPlayerTurn();
    }

    //3. ����/�߰�/���� ������ ����
    public void SetPlayerTurn()
    {
        playerActButtonRect.anchoredPosition = new Vector2(0, 0);
        playerActButtonRect.DOAnchorPosY(600, 0.2f);
        backButtonRect.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutBack);
        backButtonRect.GetComponent<Button>().interactable = true;
    }

    //4. �÷��̾� �׼�
    public void ActPlayerTurn(int _n)
    {
        bool isSuccess = false;

        switch (_n)
        {
            case 0: //����
                isSuccess = PlayDuel();
                break;

            case 1: //�߰�
                isSuccess = RaiseBet();
                break;

            case 2: //����
                StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    Die
                },
                new List<string>()
                {
                    "������ �༮, ���� �� ����"
                });

                isSuccess = true;
                break;
        }

        if(isSuccess)
        {
            backButtonRect.DOAnchorPosX(-200, 0.5f).SetEase(Ease.OutBack);
            backButtonRect.GetComponent<Button>().interactable = false;
            playerActButtonRect.DOAnchorPosY(0, 0.2f);

        }
    }

    #region �׼�

    //4-0. ����
    public bool PlayDuel()
    {
        if(DataCarrier.instance.storage.gold == 0)
        {
            StartSceneManager.instance.PlayTalk("���ڲ�",
            new List<System.Action>()
            {
            },
            new List<string>()
            {
                    "���� �����鼭 �� �ϰڴٴ°ž�?"
            });
            return false;
        }

            int tmpBet = bet[1] - bet[0];
        if (DataCarrier.instance.storage.gold < tmpBet)
        {
            StartSceneManager.instance.PlayTalk("���ڲ�",
            new List<System.Action>()
            {
            },
            new List<string>()
            {
                    "���� �� ���ڶ�µ�...",
                    "��, �Ѿ��"
            });
            tmpBet = DataCarrier.instance.storage.gold;
        }
        Bet(true, tmpBet);

        if (currentGamble == 0)
        {
            RollDice(3);
            SetPlayerDice(0);
        }
        else
        {
            SetPlayerDice(1);
        }

        return true;
    }

    //4-1.�߰�
    bool RaiseBet()
    {
        if(DataCarrier.instance.storage.gold < 123)
        {

            StartSceneManager.instance.PlayTalk("���ڲ�",
            new List<System.Action>()
            {
            },
            new List<string>()
            {
                    "�̺� ���� �����鼭 �� �߰��ϰڴٴ°ž�?"
            });
            return false; ;
        }
        Bet(true, 123);
        StartCoroutine(CoRaiseBet());

        return true;
    }
    IEnumerator CoRaiseBet()
    {
        yield return new WaitForSeconds(0.5f);
        Bet(false, 123);
        StartSceneManager.instance.PlayTalk("���ڲ�",
            new List<System.Action>()
            {
                    SetPlayerTurn
            },
            new List<string>()
            {
                    "���� ��������",
                    "�� �����ٰ�?"
            },
            1);
    }

    //4-2. ����
    void Die()
    {
        Bet(true, bet[1] - bet[0]);
        isPlayerWin = false;
        FinishGame();
    }

    #endregion

    //5. �ֻ���
    #region �ֻ��� �׼�

    //5-1. �ֻ��� ����
    void SetPlayerDice(int _n)
    {
        DOTween.Kill(dices[_n]);
        dices[_n].DOAnchorPos(new Vector2(Random.Range(-150, -400), Random.Range(150, 300)), 1);
        dices[_n].DORotate(new Vector3(0, 0, Random.Range(0, 360)), 1).OnComplete(()
            => dices[_n].GetComponent<Button>().interactable = true);
    }
    void ChangeEye(int _n)
    {
        var preEye = eyes_duel[_n];

        int number;
        do number = Random.Range(1, 7);
        while (preEye == number);

        SetDiceEye(_n, number);
    }
    void SetDiceEye(int _n, int _number)
    {
        eyes_duel[_n] = _number;

        if(currentGamble == 1 && eyes_duel[0] != 0 && eyes_duel[3] != 0) eyeSave = eyes_duel;

        dices[_n].GetChild(0).GetComponent<Image>().sprite = eyeSprites[eyes_duel[_n] - 1];
        dices[_n].GetChild(0).GetComponent<Image>().SetNativeSize();
    }
    //5-1. ������
    public void RollDice(int _n)
    {

        StartScene.instance.SoundPlay(gambleClips[0]);

        DOTween.Kill(dices[_n]);

        BounceDice(_n, 2, true);

        //��� �ٽ� ������ ��
        RerollDice();
    }
    void BounceDice(int _n, int _stack, bool _isFirst = false)
    {
        if (_stack == 0) //������
        {
            dices[_n].DORotate(new Vector3(0, 0, Random.Range(0, 360)), 0.5f);
            dices[_n].DOAnchorPos(dices[_n].anchoredPosition + new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)), 0.5f);

            //��� ù ����
            CheckFateDice(_n);

            CheckResult();            
            return;
        }

        Vector2 pos;
        Vector2 size = new Vector2(1 + _stack * 0.3f, 1 + _stack * 0.3f);
        float time = _stack * 0.3f;

        if (_isFirst)
        {
            if (_n < 3)
            {
                pos = new Vector2(Random.Range(-600, -1200), Random.Range(320, 860));
            }
            else
            {
                pos = new Vector2(Random.Range(600, 1200), Random.Range(-320, -860));
            }
        }
        else
        {
            var range = 100f * _stack * 2;
            pos = dices[_n].anchoredPosition + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        }

        dices[_n].DOAnchorPos(pos, time);

        dices[_n].DORotate(new Vector3(0, 0, Random.Range(0, 360)), time);

        dices[_n].DOScale(size, time / 2).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            dices[_n].DOScale(new Vector2(1f, 1f), time / 2).SetEase(Ease.InQuad).OnComplete(() =>
            {
                ChangeEye(_n);
                BounceDice(_n, _stack - 1);
            });
        });

    }

    void SetPreDice()
    {
        dices[0].DOAnchorPos(new Vector2(Random.Range(-600, -1200), Random.Range(320, 860)), 0.5f);
        SetDiceEye(0, eyeSave[0]);
        dices[3].DOAnchorPos(new Vector2(Random.Range(600, 1200), Random.Range(-320, -860)), 0.5f);
        SetDiceEye(3, eyeSave[3]);

    }
    void CheckFateDice(int _n)
    {
        if (currentGamble == 0) return;
        if (handConfirmed[0] && _n < 3) return;
        
        if (_n == 0)
        {
            Bet(false, 123);

            StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                        SetPlayerTurn
                },
                new List<string>()
                {
                        "���� �̸�ŭ �ɾ��"
                });
        }
        else if(_n == 1)
        {

            StartSceneManager.instance.PlayTalk("",
                new List<System.Action>()
                {
                        SetFateButton
                },
                new List<string>()
                {
                        "�� �ֻ��� �� �ϳ��� �ٲܱ�?"
                });
        }
        else if (_n == 4)
        {
            if (eyes_duel[4] == 0) return;
            //��
            if(handConfirmed[1] == false && CompareHand() )
            {
                StartSceneManager.instance.PlayTalk("",
                    new List<System.Action>()
                    {
                        RerollEnemyDice
                    },
                    new List<string>()
                    {
                        "����...",
                        "���� �� �� ���Ҵ�..."
                    });
            }
            else
            {
                handConfirmed[1] = true;
            }
        }
    }

    #endregion

    //0. ���
    void SetFateButton()
    {
        for(int i = 0; i<3; i++) dices[i].GetComponent<Button>().interactable = true;

        fateButtonRect.DOAnchorPosY(200, 0.5f);
    }
    //0-1. ���� (��ư)
    public void KeepDice()
    {
        for (int i = 0; i < 3; i++) dices[i].GetComponent<Button>().interactable = false;
        ConfirmPlayerHand();
    }
    //0-2. ����
    void RerollDice()
    {
        if (!handConfirmed[0] && eyes_duel[0] != 0 && eyes_duel[1] != 0)
        {
            for (int i = 0; i < 3; i++) dices[i].GetComponent<Button>().interactable = false;
            ConfirmPlayerHand();
        }
    }
    void RerollEnemyDice()
    {
        handConfirmed[1] = true;
        RollDice(eyes_duel[3] > eyes_duel[4] ? 4 : 3);
    }
    //0-3. �÷��̾� ���� Ȯ��
    void ConfirmPlayerHand()
    {
        handConfirmed[0] = true;
        fateButtonRect.DOAnchorPosY(0, 0.5f);

        StartSceneManager.instance.PlayTalk("���ڲ�",
            new List<System.Action>()
            {
                RollEnemyDice
            },
            new List<string>()
            {
                "������ ���� �� ���˰�?"
            },1);
    }
    void RollEnemyDice()
    {
        RollDice(4);
    }

    //6. ��� Ȯ��
    void CheckResult()
    {

        if (currentGamble == 0)
        {
            if (eyes_duel[0] == 0 || eyes_duel[3] == 0) return;

            StartCoroutine(CoCheckResult());

        }
        else
        {
            if (handConfirmed[0] == false || handConfirmed[1] == false) return;

            // �� ���� �� 
            StartCoroutine(CoCheckResult());
        }

    }

    IEnumerator CoCheckResult()
    {
        if(currentGamble == 0)
        {

            dices[0].DOAnchorPos(new Vector2(-600, 600), 1f).SetDelay(0.5f).SetEase(Ease.Unset);
            dices[0].DOScale(new Vector3(2, 2), 0.5f).SetDelay(0.5f);
            dices[3].DOAnchorPos(new Vector2(600, -600), 1f).SetDelay(0.5f).SetEase(Ease.Unset);
            dices[3].DOScale(new Vector3(2, 2), 0.5f).SetDelay(0.5f);

            yield return new WaitForSeconds(1.8f);

            if (eyes_duel[0] > eyes_duel[3])
            {
                StartScene.instance.SoundPlay(gambleClips[2]);
                DataCarrier.instance.GainGold(bet[0]+bet[1]);

                isPlayerWin = true;
                StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "����, �� ��..."
                });
            }
            else if (eyes_duel[0] < eyes_duel[3])
            {

                StartScene.instance.SoundPlay(gambleClips[3]);

                isPlayerWin = false;
                StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "������, ����� ������ �� �����±�"
                });
            }
            else
            {
                DataCarrier.instance.GainGold(1); // ���º� ���¿��� ���� �ھҴµ� ���� ���ϴ� ���� ����
                StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    BetDouble
                },
                new List<string>()
                {
                    "��ȣ��, ��屺",
                    "���� ����� ����"
                });
            }
        }
        else
        {

            dices[0].DOAnchorPos(new Vector2(-600, 600), 1f).SetDelay(0.5f).SetEase(Ease.Unset);
            dices[0].DOScale(new Vector3(2, 2), 0.5f).SetDelay(0.5f);
            dices[1].DOAnchorPos(new Vector2(-300, 600), 1f).SetDelay(0.5f).SetEase(Ease.Unset);
            dices[1].DOScale(new Vector3(2, 2), 0.5f).SetDelay(0.5f);

            dices[3].DOAnchorPos(new Vector2(600, -600), 1f).SetDelay(0.5f).SetEase(Ease.Unset);
            dices[3].DOScale(new Vector3(2, 2), 0.5f).SetDelay(0.5f);
            dices[4].DOAnchorPos(new Vector2(300, -600), 1f).SetDelay(0.5f).SetEase(Ease.Unset);
            dices[4].DOScale(new Vector3(2, 2), 0.5f).SetDelay(0.5f);

            playerRankText.gameObject.SetActive(true);
            playerRankText.text = GetRankName(GetRank(eyes_duel[0], eyes_duel[1]));
            playerRankText.color = new Color(1, 1, 1, 0);
            playerRankText.DOFade(1, 0.5f).SetDelay(2f);

            enemyRankText.gameObject.SetActive(true);
            enemyRankText.text = GetRankName(GetRank(eyes_duel[3], eyes_duel[4]));
            enemyRankText.color = new Color(1,1,1,0);
            enemyRankText.DOFade(1, 0.5f).SetDelay(2f);

            yield return new WaitForSeconds(1.8f);

            if(CompareHand())
            {

                StartScene.instance.SoundPlay(gambleClips[2]);
                DataCarrier.instance.GainGold(bet[0] + bet[1]);

                isPlayerWin = true;
                StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "����, �� ��..."
                });
            }
            else
            {
                StartScene.instance.SoundPlay(gambleClips[3]);

                isPlayerWin = false;
                StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "������, ����� ������ �� �����±�"
                });
            }
        }
    }

    //6-1. ���º� �� ���� �����
    void BetDouble()
    {
        eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };

        RepositionDice();
        Bet(false, amount);

        StartSceneManager.instance.PlayTalk("���ڲ�",
             new List<System.Action>()
             {
                    SetPlayerTurn
             },
             new List<string>()
             {
                    "�̺�, �߳��� �뺴",
                    "���� �ο��� ���ϴ� �� �ƴϰ���?"
             },
             1);

    }

    //6-2. �¸� Ȥ�� �й�
    void FinishGame()
    {


        playerRankText.gameObject.SetActive(false);
        enemyRankText.gameObject.SetActive(false);

        RepositionDice();
        betSave[currentGamble, 0] = 0;
        betSave[currentGamble, 1] = 0;
        handConfirmed = new bool[] { false, false };

        GetComponent<RectTransform>().DOAnchorPosX(0, 1).OnComplete(() => {

            StartScene.instance.SoundPlay(gambleClips[4]);
            StartScene.instance.SoundPlay(gambleClips[5]);
            StartScene.instance.SoundPlay(gambleClips[4]);
            StartScene.instance.SoundPlay(gambleClips[5]);
        
        });//�ð� ������ �뵵
        foreach (GameObject i in bets)
            i.GetComponent<RectTransform>().
                DOAnchorPos(new Vector2(0, isPlayerWin ? -600 : 600), Random.Range(0.2f, 1f)).
                SetDelay(1).
                OnComplete(() => i.gameObject.SetActive(false));

        stakeText.DOCounter(amount, 0, 1f).OnComplete(() => {

            StartSceneManager.instance.PlayTalk("���ڲ�",
                new List<System.Action>()
                {
                        StartGame
                },
                new List<string>()
                {
                        "�, �� �� �� ���ٰ�?",
                        "�ּ��̰��� ����ġ�� ����� ����!"
                },
                1f);
        });
        betText.DOCounter(bet[0], 0, 0.5f);
    }


    #endregion


    #region ���� ��

    bool CompareHand()
    {
        bool isPlayerWin;

        if (GetRank(eyes_duel[0], eyes_duel[1]) > GetRank(eyes_duel[3], eyes_duel[4]))
        {
            if (GetRank(eyes_duel[0], eyes_duel[1]) == 3 && GetRank(eyes_duel[3], eyes_duel[4]) == 0)
                isPlayerWin = false;
            else
                isPlayerWin = true;
        }
        else if (GetRank(eyes_duel[0], eyes_duel[1]) < GetRank(eyes_duel[3], eyes_duel[4]))
        {
            if (GetRank(eyes_duel[0], eyes_duel[1]) == 0 && GetRank(eyes_duel[3], eyes_duel[4]) == 3)
                isPlayerWin = true;
            else
                isPlayerWin = false;
        }
        else
        {
            var playerEye = eyes_duel[0] + eyes_duel[1];
            var enemyEye = eyes_duel[3] + eyes_duel[4];
            isPlayerWin = playerEye >= enemyEye;
        }

        return isPlayerWin;
    }

    int GetRank(int _hand_0, int _hand_1)
    {
        var gap = Mathf.Abs(_hand_0 - _hand_1);
        if(gap == 5)
        {
            return 0;
        }
        else if(gap == 0)
        {
            return 3;
        }
        else if (gap == 1)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    string GetRankName(int _n)
    {
        switch(_n)
        {
            case 0:
                return "����";
            case 1:
                return "ģ��";
            case 2:
                return "����";
            default:
                return "�ֵ���";
        }
    }

    #endregion
}
