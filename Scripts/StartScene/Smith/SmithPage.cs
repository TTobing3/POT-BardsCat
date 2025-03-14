using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum SmithTab { Weapon, Helmet, Armor, Material, Gem }
public enum AnvilType { Carving, Crafting }
public enum TargetSlotType { Main, Sub, Gem }
public class AnvilSlotData
{
    public int tab, number;
    public AnvilSlotData(int _tab, int _number)
    {
        tab = _tab;
        number = _number;
    }
}

public class SmithPage : MonoBehaviour
{
    public SmithBox smithBox;
    [Space(10)]
    [SerializeField] Transform hitPoint;
    [SerializeField] Transform backTab, frontTab;
    int curTab;

    // Slot
    [Space(10)]
    public GameObject[] subSlot;
    public GameObject mainSlot, gemSlot;
    public int[,] slot = new int[5,2]; // main sub sub sub gem / ī�װ� �ѹ�

    //Anvil
    [Space(10)]
    public RectTransform anvilRect;
    public RectTransform storageRect, backButtonRect, tableRect, craftingButtonRect;
    public RectTransform hammerRect;

    [Space(10)]
    public GameObject NoteLine;


    public AudioClip[] smithClips;


    // UI ����
    public void SetSmith(bool _set)
    {
        ResetSlot();
        SetShadow(_set);


        if (_set)
        {
            SoundPlay(StartScene.instance.uiClips[0]);

            tableRect.DOAnchorPosY(0, 0.6f).OnComplete(() => {
                anvilRect.DOAnchorPosY(-250, 1).SetEase(Ease.OutBack).SetDelay(0.5f);
                storageRect.DOAnchorPosX(500, 1).SetEase(Ease.OutBack);
                backButtonRect.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutBack);

                SoundPlay(smithClips[0]);
                SoundPlay(smithClips[1]);
            });

            if(DataCarrier.instance.step == 1)
            {
                StartSceneManager.instance.PlayTalk("����",
                    new List<System.Action>()
                    {
                    },
                    new List<string>()
                    {
                    "�ʰ� ����� ������ ��翡 ������ �ΰ� ����ϸ� �ȴ�...",
                    "�˾Ƶ������?",
                    "�����, �ٽ� ��Ḧ ���� ����� �ΰ� �����ؾ� �Ѵٴ� �� �˾Ƶε���",
                    "�׸��� ���� ���� ���� �ٽ� ������ �� �� �ִ� �͵� ����ϰ�"
                    }, 1, true);
            }

        }
        else
        {
            SoundPlay(smithClips[0]);
            SoundPlay(smithClips[1]);

            tableRect.DOAnchorPosY(1200, 0.5f).SetDelay(0.5f);
            anvilRect.DOAnchorPosY(-1000, 1).SetEase(Ease.OutBack);
            storageRect.DOAnchorPosX(1500, 1).SetEase(Ease.OutBack);
            backButtonRect.DOAnchorPosX(-200, 1f).SetEase(Ease.OutBack).OnComplete(()=>
            {
                StartSceneManager.instance.SetButtonsInteractable(true);
                gameObject.SetActive(false);

                SoundPlay(StartScene.instance.uiClips[0]);
            });
        }

    }

    //���尣 �κ��丮 �� ����
    public void SelectTab(int _tab)
    {
        curTab = _tab;

        for(int i = 0; i < 5; i++)
        {
            backTab.GetChild(i).gameObject.SetActive(true);
            frontTab.GetChild(i).gameObject.SetActive(false);
        }

        backTab.GetChild(_tab).gameObject.SetActive(false);
        frontTab.GetChild(_tab).gameObject.SetActive(true);

        smithBox.ChangeTab(_tab);
    }

    //��� ���ο� ���� Ȱ��ȭ�Ǵ� ���� ����
    public void SelectType(int _type)
    {
        foreach (GameObject i in subSlot) i.gameObject.SetActive(false);
        gemSlot.gameObject.SetActive(false);

        if (_type == (int)AnvilType.Carving)
        {
            gemSlot.gameObject.SetActive(true);
        }
        else if(_type == (int)AnvilType.Crafting)
        {
            foreach (GameObject i in subSlot) i.gameObject.SetActive(true);
        }
    }

    // ��� ���� �� ����
    public void SetSlot(int _slotLocation, TargetSlotType _slotType, int _tab, int _number)
    {
        var value = TranslateTabNumber(_tab, _number); 
        slot[_slotLocation, 0] = value[0];
        slot[_slotLocation, 1] = value[1];

        if(_slotType == TargetSlotType.Main)
        {
            SetCraftingButton(true);
        }

        if (value[0] == (int)Tab.Material)
        {
            craftingButtonRect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "����";
        }
        else
        {

            craftingButtonRect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "����";
        }
    }

    //���۹�ư ��ġ ����
    void SetCraftingButton(bool _set)
    {
        DOTween.Kill(craftingButtonRect);
        if(_set)
        {
            craftingButtonRect.gameObject.SetActive(true);
            craftingButtonRect.DOAnchorPosY(-350, 0.5f);
        }
        else
        {
            craftingButtonRect.DOAnchorPosY(-650, 0.5f).OnComplete(() 
                => craftingButtonRect.gameObject.SetActive(false));
        }
    }
    
    //��� ���� �̹� �����ϴ��� üũ
    public bool CheckAlready(int _tab, int _number)
    {
        var value = TranslateTabNumber(_tab, _number);

        for(int i = 0; i<5; i++)
        {
            if (slot[i, 0] == value[0] && slot[i, 1] == value[1]) return true;
        }

        return false;
    }

    int[] TranslateTabNumber(int _tab, int _number)
    {
        var curTab = _tab;
        var curNumber = _number;

        if (smithBox.curTab == (int)SmithTab.Armor)
        {
            if (_number >= DataCarrier.instance.storage.items[(int)Tab.Helmet].Count)
            {
                curTab++;
                curNumber -= DataCarrier.instance.storage.items[(int)Tab.Helmet].Count;
            }
        }
        else if (smithBox.curTab > (int)SmithTab.Armor)
        {
            curTab++;
        }

        return new int[] { curTab, curNumber };
    }

    // ��� ���� ����
    public void ResetSlot(bool _isResetMain = true)
    {
        for(int i = 0; i<5; i++)
        {
            if (!_isResetMain && i == 3) continue;
            ResetIndividualSlot(i);
        }

    }

    // ��� ���� ���� ����
    public void ResetIndividualSlot(int _slotNumber)
    {
        slot[_slotNumber, 0] = -1;
        slot[_slotNumber, 1] = -1;
        if(_slotNumber == 3) // main
        {
            mainSlot.transform.GetChild(0).gameObject.SetActive(false);
            SetCraftingButton(false);
        }
        else if(_slotNumber == 4)
        {
            gemSlot.transform.GetChild(0).gameObject.SetActive(false);
        }
        else 
        {
            subSlot[_slotNumber].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // ��� ���� ����
    void SetShadow(bool _set)
    {
        foreach (GameObject i in subSlot) i.SetActive(false);
        mainSlot.SetActive(false);
        gemSlot.SetActive(false);

        if(_set)
        {
            mainSlot.SetActive(true);
            var mainShadowImage = mainSlot.GetComponent<Image>();
            mainShadowImage.color = new Color(1, 1, 1, 0);
            mainShadowImage.DOFade(1, 1).SetDelay(1);
        }
        else
        {
            mainSlot.SetActive(true);
            var mainShadowImage = mainSlot.GetComponent<Image>();
            mainShadowImage.color = new Color(1, 1, 1, 1);
            mainShadowImage.DOFade(0, 0.5f);
        }

    }

    // ���� ��ư
    public void ActCraftingButton()
    {
        if (slot[3, 1] == -1) return;

        Smash();

        //����

        var result = CraftItem();

        if (result == "����")
        {
            SoundPlay(smithClips[3]);
            SoundPlay(smithClips[3]);
            NoteCraftResult(result);
        }
        else
        {
            SoundPlay(smithClips[2]);
            SoundPlay(smithClips[2]);
            NoteCraftResult(result);
        }

        //��� �Һ�

        UseSlotItem();

        smithBox.ChangeTab(curTab);
        
        ResetSlot();
    }

    // ���� �� �Ҹ�
    void UseSlotItem()
    {
        if (slot[3, 0] == (int)Tab.Material)
        {
            if (slot[0, 1] != -1 || slot[1, 1] != -1 || slot[2, 1] != -1)
            {
                DataCarrier.instance.RemoveItemAtStorage
                    ((int)Tab.Material, new int[] { slot[0, 1], slot[1, 1], slot[2, 1], slot[3, 1] });
            }
        }
        else
        {
            if (slot[4, 1] != -1)
            {
                DataCarrier.instance.RemoveItemAtStorage(slot[3, 0], slot[3, 1]);
                DataCarrier.instance.RemoveItemAtStorage((int)Tab.Gem, slot[4, 1]);
            }
        }
    }

    // ���� ���
    string CraftItem()
    {
        if (slot[3, 0] == (int)Tab.Material)
        {
            var components = new string[]
            {
                slot[3, 0] == -1 ? "����" : DataCarrier.instance.storage.items[ slot[3, 0]][slot[3, 1]],
                slot[0, 0] == -1 ? "����" : DataCarrier.instance.storage.items[ slot[0, 0]][slot[0, 1]],
                slot[1, 0] == -1 ? "����" : DataCarrier.instance.storage.items[ slot[1, 0]][slot[1, 1]],
                slot[2, 0] == -1 ? "����" : DataCarrier.instance.storage.items[ slot[2, 0]][slot[2, 1]]
            };

            foreach (RecipeData i in DataManager.instance.AllRecipeDataList)
            {
                var result = i.Check(components);

                if (result != null)
                {
                    DataCarrier.instance.AddItemToStorage(result[0], result[1]);
                    return result[1];
                }
                else
                {
                    //������ ���
                }
            }
        }
        else
        {
            //print(DataCarrier.instance.storage.items[slot[3, 0]][slot[3, 1]]);
            //print(DataCarrier.instance.storage.items[slot[4, 0]][slot[4, 1]]);
            /*
            print(slot);
            print(slot[3,0]);
            print(slot[4, 0]);
            print(DataCarrier.instance.storage.items);
            print(slot[3, 1]);
            //print(DataCarrier.instance.storage.items[slot[3, 0]]);
            //print(DataCarrier.instance.storage.items[slot[4, 0]]);
            print(slot[3, 0] == -1 ? "����" : DataCarrier.instance.storage.items[slot[3, 0]]);
            print(slot[3, 0] == -1 ? "����" : DataCarrier.instance.storage.items[slot[3, 0]][slot[3, 1]]);
            print(slot[3, 0] == -1 ? "����" : DataCarrier.instance.storage.items[slot[3, 0]][slot[3, 1]]);
            print(slot[4, 0] == -1 ? "����" : DataCarrier.instance.storage.items[slot[4, 0]][slot[4, 1]]);
            */
            var tmpSlot = TranslateTab(slot);

            var components = new string[]
            {
                tmpSlot[3, 0] == -1 ? "����" : DataCarrier.instance.storage.items[ tmpSlot[3, 0]] [tmpSlot[3, 1]],
                tmpSlot[4, 0] == -1 ? "����" : DataCarrier.instance.storage.items[ tmpSlot[4, 0]] [tmpSlot[4, 1]],
                "����", "����"
            };

            foreach (RecipeData i in DataManager.instance.AllRecipeDataList)
            {
                var result = i.Check(components);

                if (result != null)
                {
                    DataCarrier.instance.AddItemToStorage(result[0], result[1]);
                    return result[1];
                }
            }
        }

        return "����";
    }

    int[,] TranslateTab(int[,] _slot)
    {
        if(_slot[3,0] == 1 && _slot[3, 1] >= DataCarrier.instance.storage.items[slot[3, 0]].Count)
        {
            _slot[3, 0] = 2;
            _slot[3, 1] = _slot[3, 1] - DataCarrier.instance.storage.items[slot[3, 0]].Count;
        }
        return _slot;
    }

    void NoteCraftResult(string _result)
    {
        if(_result == "����")
        {
            NoteLine.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "�����Ͽ����ϴ�...";
        }
        else
        {
            NoteLine.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                _result + "(��)�� �����ϴµ��� �����Ͽ���.";
        }


        NoteLine.SetActive(true);

        NoteLine.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        NoteLine.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);

        NoteLine.GetComponent<Image>().DOFade(0.5f, 1);
        NoteLine.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(1, 1).OnComplete(()=> 
            {
                NoteLine.GetComponent<Image>().DOFade(0f, 1).SetDelay(0.5f);
                NoteLine.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 1).SetDelay(0.5f).OnComplete(() =>
                NoteLine.SetActive(false));

            });
    }
    public void SoundPlay(AudioClip _clip)
    {
        SoundManager.instance.SfxPlay(_clip);
    }

    void Smash()
    {
        hammerRect.gameObject.SetActive(true);
        hammerRect.GetComponent<Image>().color = Color.white;
        hammerRect.anchoredPosition = new Vector2(0, 800);
        hammerRect.DOAnchorPosY(0, 0.3f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            hammerRect.GetComponent<ParticleSystem>().Play();
            hammerRect.GetComponent<Image>().DOFade(0, 1).SetDelay(0.5f).OnComplete(() => hammerRect.gameObject.SetActive(false));
        }
         );
    }
}
