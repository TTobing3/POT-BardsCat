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
    bool[] isPlaying = new bool[] { false, false }, handConfirmed = new bool[] { false, false }; // 이거 의뢰 갔다오면 리셋
    int currentState = 0, currentGamble = -1;
    int[] bet = new int[] { 0, 0 }; // playerbet, enemybet
    int[] eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };

    public AudioClip[] gambleClips;

    //모두 접기 ctrl m o

    int amount
    {
        get { return bet[0] + bet[1]; }
    }

    //게임에 대한 정보가 뭐가 잇나몀
    //판돈하고 베팅

    int[,] betSave = new int[,] { { 0, 0 }, { 0, 0 } };
    int[] eyeSave = new int[] { 0, 0, 0, 0, 0, 0 };

    #region 기능

    //뒤로가기 버튼
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

    //베팅
    public void Bet(bool _isPlayer, int _amount)
    {
        if (_isPlayer) DataCarrier.instance.GainGold(-_amount) ;

        var tmpAmount = bet[0] + bet[1];

        bet[_isPlayer ? 0 : 1] += _amount;

        betSave[currentGamble, _isPlayer ? 0 : 1] = bet[_isPlayer ? 0 : 1]; // 다시 세팅할 때 사용

        BetAmount(_isPlayer, _amount);

        betText.DOCounter(0, _amount, 1f);
        stakeText.DOCounter(tmpAmount, amount, 1f);


        StartScene.instance.SoundPlay(gambleClips[1]);
    }
    void BetAmount(bool _isPlayer, int _amount)
    {
        string[,] unit = new string[,] { { "금화", "100" }, { "은화", "10" }, { "동화", "1" } };

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

    //베팅 초기화
    void ResetBet()
    {
        bet = new int[] { 0, 0 };
        stakeText.text = 0 + "";
        betText.text = 0 + "";
    }

    //주사위 원위치
    void RepositionDice()
    {
        foreach (RectTransform i in dices)
        {
            i.DOAnchorPos(Vector2.zero, 1);
            i.DOScale(new Vector3(1, 1), 1);
        }
    }

    #endregion

    #region 순서
    // 0. 게임 선택 테이블 세팅
    public void SetGamble(bool _set)
    {
        if( DataCarrier.instance.step == 2 )
        {
            StartSceneManager.instance.PlayTalk("주인",
                new List<System.Action>()
                {
                },
                new List<string>()
                {
                    "하하, 애송이 녀석 환영한다...",
                    "주사위 도박은 아주 간단하지, 주사위를 눌러 굴리고 눈을 비교하여 승패를 가른다.",
                    "도전으로 승부를 보고, 추가로 베팅액을 올릴 수 있지 그리고 영 아니다 싶으면 포기하는 것도 방법일세",
                    "그럼 한껏 즐겨보라고"
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

    //1. 선택한 게임 세팅
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
                StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    StartPreGame
                },
                new List<string>()
                {
                        "이봐, 게임 중에 나가는 매너는 숲 속 시체한테 배웠나?"
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
                    StartSceneManager.instance.PlayTalk("도박꾼",
                        new List<System.Action>()
                        {
                            StartGame
                        },
                        new List<string>()
                        {
                            "하하, 호구자식 한 명 들어왔구만!",
                            "여기 앉아도 되겠지?"
                        },
                        0.2f);
                    break;

                case 1:

                    StartSceneManager.instance.PlayTalk("도박꾼",
                        new List<System.Action>()
                        {
                            StartGame
                        },
                        new List<string>()
                        {
                            "하하, 호구자식 한 명 들어왔구만!",
                            "여기 앉아도 되겠지?"
                        },
                        0.2f);
                    break;

            }
        });
    }

    //2. 도박꾼 행동
    public void StartGame()
    {
        eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };
        ResetBet();

        if(currentGamble == 0)
        {
            backButtonRect.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutBack);

            Bet(false, 123);

            StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                SetPlayerTurn
                },
                new List<string>()
                {
                    "으하하, 좋아좋아",
                    "나는 " + 123 + "만큼 걸지",
                    "이봐 애송이, 너는 어쩔래?"
                }, 1);
        }
        else
        {
            RollDice(3);
            SetPlayerDice(0);

            StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    null
                },
                new List<string>()
                {
                    "좋아좋아",
                    "한 번 주사위를 굴려보자고",
                }, 1);
        }
    }

    //2-1. 이전 게임 이어서
    void StartPreGame()
    {
        ResetBet();
        if (currentGamble == 1) SetPreDice();
        else eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };


        Bet(true, betSave[currentGamble, 0]);
        Bet(false, betSave[currentGamble, 1]);


        SetPlayerTurn();
    }

    //3. 도전/추가/포기 선택지 제시
    public void SetPlayerTurn()
    {
        playerActButtonRect.anchoredPosition = new Vector2(0, 0);
        playerActButtonRect.DOAnchorPosY(600, 0.2f);
        backButtonRect.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutBack);
        backButtonRect.GetComponent<Button>().interactable = true;
    }

    //4. 플레이어 액션
    public void ActPlayerTurn(int _n)
    {
        bool isSuccess = false;

        switch (_n)
        {
            case 0: //도전
                isSuccess = PlayDuel();
                break;

            case 1: //추가
                isSuccess = RaiseBet();
                break;

            case 2: //포기
                StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    Die
                },
                new List<string>()
                {
                    "겁쟁이 녀석, 돈은 잘 쓰마"
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

    #region 액션

    //4-0. 결투
    public bool PlayDuel()
    {
        if(DataCarrier.instance.storage.gold == 0)
        {
            StartSceneManager.instance.PlayTalk("도박꾼",
            new List<System.Action>()
            {
            },
            new List<string>()
            {
                    "돈도 없으면서 뭘 하겠다는거야?"
            });
            return false;
        }

            int tmpBet = bet[1] - bet[0];
        if (DataCarrier.instance.storage.gold < tmpBet)
        {
            StartSceneManager.instance.PlayTalk("도박꾼",
            new List<System.Action>()
            {
            },
            new List<string>()
            {
                    "돈이 좀 모자라는데...",
                    "뭐, 넘어가지"
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

    //4-1.추가
    bool RaiseBet()
    {
        if(DataCarrier.instance.storage.gold < 123)
        {

            StartSceneManager.instance.PlayTalk("도박꾼",
            new List<System.Action>()
            {
            },
            new List<string>()
            {
                    "이봐 돈도 없으면서 뭘 추가하겠다는거야?"
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
        StartSceneManager.instance.PlayTalk("도박꾼",
            new List<System.Action>()
            {
                    SetPlayerTurn
            },
            new List<string>()
            {
                    "나도 지지않지",
                    "더 들어올텐가?"
            },
            1);
    }

    //4-2. 포기
    void Die()
    {
        Bet(true, bet[1] - bet[0]);
        isPlayerWin = false;
        FinishGame();
    }

    #endregion

    //5. 주사위
    #region 주사위 액션

    //5-1. 주사위 세팅
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
    //5-1. 굴리기
    public void RollDice(int _n)
    {

        StartScene.instance.SoundPlay(gambleClips[0]);

        DOTween.Kill(dices[_n]);

        BounceDice(_n, 2, true);

        //운명 다시 굴리는 것
        RerollDice();
    }
    void BounceDice(int _n, int _stack, bool _isFirst = false)
    {
        if (_stack == 0) //굴려짐
        {
            dices[_n].DORotate(new Vector3(0, 0, Random.Range(0, 360)), 0.5f);
            dices[_n].DOAnchorPos(dices[_n].anchoredPosition + new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)), 0.5f);

            //운명 첫 굴림
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

            StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                        SetPlayerTurn
                },
                new List<string>()
                {
                        "나는 이만큼 걸어보지"
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
                        "두 주사위 중 하나를 바꿀까?"
                });
        }
        else if (_n == 4)
        {
            if (eyes_duel[4] == 0) return;
            //비교
            if(handConfirmed[1] == false && CompareHand() )
            {
                StartSceneManager.instance.PlayTalk("",
                    new List<System.Action>()
                    {
                        RerollEnemyDice
                    },
                    new List<string>()
                    {
                        "젠장...",
                        "아직 한 번 남았다..."
                    });
            }
            else
            {
                handConfirmed[1] = true;
            }
        }
    }

    #endregion

    //0. 운명
    void SetFateButton()
    {
        for(int i = 0; i<3; i++) dices[i].GetComponent<Button>().interactable = true;

        fateButtonRect.DOAnchorPosY(200, 0.5f);
    }
    //0-1. 유지 (버튼)
    public void KeepDice()
    {
        for (int i = 0; i < 3; i++) dices[i].GetComponent<Button>().interactable = false;
        ConfirmPlayerHand();
    }
    //0-2. 리롤
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
    //0-3. 플레이어 족보 확정
    void ConfirmPlayerHand()
    {
        handConfirmed[0] = true;
        fateButtonRect.DOAnchorPosY(0, 0.5f);

        StartSceneManager.instance.PlayTalk("도박꾼",
            new List<System.Action>()
            {
                RollEnemyDice
            },
            new List<string>()
            {
                "으하하 이제 내 차롄가?"
            },1);
    }
    void RollEnemyDice()
    {
        RollDice(4);
    }

    //6. 결과 확인
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

            // 두 개를 더 
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
                StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "젠장, 내 돈..."
                });
            }
            else if (eyes_duel[0] < eyes_duel[3])
            {

                StartScene.instance.SoundPlay(gambleClips[3]);

                isPlayerWin = false;
                StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "으하하, 행운의 여신이 날 돌보는군"
                });
            }
            else
            {
                DataCarrier.instance.GainGold(1); // 무승부 상태에서 올인 박았는데 도박 못하는 상태 방지
                StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    BetDouble
                },
                new List<string>()
                {
                    "오호라, 비겼군",
                    "묻고 더블로 가지"
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
                StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "젠장, 내 돈..."
                });
            }
            else
            {
                StartScene.instance.SoundPlay(gambleClips[3]);

                isPlayerWin = false;
                StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                    FinishGame
                },
                new List<string>()
                {
                    "으하하, 행운의 여신이 날 돌보는군"
                });
            }
        }
    }

    //6-1. 무승부 시 묻고 더블로
    void BetDouble()
    {
        eyes_duel = new int[] { 0, 0, 0, 0, 0, 0 };

        RepositionDice();
        Bet(false, amount);

        StartSceneManager.instance.PlayTalk("도박꾼",
             new List<System.Action>()
             {
                    SetPlayerTurn
             },
             new List<string>()
             {
                    "이봐, 뜨내기 용병",
                    "설마 싸움을 피하는 건 아니겠지?"
             },
             1);

    }

    //6-2. 승리 혹은 패배
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
        
        });//시간 딜레이 용도
        foreach (GameObject i in bets)
            i.GetComponent<RectTransform>().
                DOAnchorPos(new Vector2(0, isPlayerWin ? -600 : 600), Random.Range(0.2f, 1f)).
                SetDelay(1).
                OnComplete(() => i.gameObject.SetActive(false));

        stakeText.DOCounter(amount, 0, 1f).OnComplete(() => {

            StartSceneManager.instance.PlayTalk("도박꾼",
                new List<System.Action>()
                {
                        StartGame
                },
                new List<string>()
                {
                        "어때, 한 판 더 할텐가?",
                        "애송이같이 도망치지 말라고 하하!"
                },
                1f);
        });
        betText.DOCounter(bet[0], 0, 0.5f);
    }


    #endregion


    #region 족보 비교

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
                return "원수";
            case 1:
                return "친구";
            case 2:
                return "형제";
            default:
                return "쌍둥이";
        }
    }

    #endregion
}
