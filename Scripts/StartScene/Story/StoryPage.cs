using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class StoryPage : MonoBehaviour
{
    System.Action ActionFinish;

    Image image;

    string title;
    List<string> des;

    public TextMeshProUGUI titleText;

    public GameObject desTextPrefab;
    public RectTransform desParents;

    public List<GameObject> deses;
    public GameObject toInnPage;

    public Button button;

    bool finish = false, isLock = false, backOff = false; // backoff : 뒷 까만 화면 안 꺼주는거

    void Awake()
    {
        image = GetComponent<Image>();    
    }

    void Update()
    {
        if (Input.anyKeyDown) Skip();
    }
    public void Set(string _title, System.Action _action, bool _backOff = false)
    {
        image.color = Color.black;
        backOff = _backOff;
        button.interactable = false;

        var _des = DataManager.instance.AllStroyDatas[_title].des;

        SetDetail(_title, _des);

        ActionFinish += _action;
    }

    public void SetDetail(string _title, List<string> _des)
    {

        SetDescription(_des);
        StartCoroutine(CoSetTitle(_title));
    }

    IEnumerator CoSetTitle(string _title)
    {
        DOTween.Kill(titleText);
        titleText.color = Color.clear;
        title = _title;
        titleText.text = "";
        yield return null;
        titleText.gameObject.SetActive(true);
        titleText.text = title;
        titleText.DOColor(Color.white, 1);

        button.interactable = true;

        titleText.rectTransform.anchoredPosition = new Vector2(0, desParents.sizeDelta.y / 2 + 20);
    }

    void SetDescription(List<string> _des)
    {
        des = _des;
        for (int i = 0; i < _des.Count; i++)
        {
            GameObject desObject = GetPooledObject();
            desObject.transform.SetParent(desParents, false);  // 부모 설정
            desObject.SetActive(true);  // 활성화

            // TextMeshProUGUI 컴포넌트에 접근하여 텍스트 설정
            TextMeshProUGUI textComponent = desObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                InitializeText(textComponent, _des[i], i*0.5f+2);
            }
        }

        titleText.rectTransform.anchoredPosition = new Vector2(0, desParents.sizeDelta.y / 2 + 20);
    }
    private void InitializeText(TextMeshProUGUI textComponent, string title, float _delay)
    {
        finish = false;

        DOTween.Kill(textComponent);
        textComponent.color = Color.clear;
        textComponent.text = "";
        textComponent.text = title;
        textComponent.DOColor(Color.white, _delay).OnComplete(()=> finish = true);
    }

    GameObject GetPooledObject()
    {
        // 풀에서 비활성화된 오브젝트 검색
        for (int i = 0; i < deses.Count; i++)
        {
            if (!deses[i].activeInHierarchy)
            {
                return deses[i];
            }
        }

        // 비활성화된 오브젝트가 없으면 새로운 오브젝트 생성
        GameObject newObject = Instantiate(desTextPrefab);
        deses.Add(newObject);
        return newObject;
    }

    //called by button
    public void Skip()
    {
        if(!finish)
        {
            DOTween.Kill(titleText);

            titleText.text = title;
            titleText.color = Color.white;

            for(int i = 0; i<des.Count; i++)
            {
                var tmp = deses[i].GetComponent<TextMeshProUGUI>();
                DOTween.Kill(tmp);
                tmp.text = des[i];
                tmp.color = Color.white;
            }

            finish = true;
        }
        else
        {
            Finish();
        }
    }

    public void Finish()
    {
        if (isLock) return;
        isLock = true;

        DOTween.Kill(titleText);

        titleText.DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(()=> {
            titleText.text = "";
            title = "";

        });

        for (int i = 0; i < des.Count; i++)
        {
            var tmp = deses[i].GetComponent<TextMeshProUGUI>();
            DOTween.Kill(tmp);

            if (i == des.Count-1)
            {
                tmp.DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(() =>
                {
                    tmp.gameObject.SetActive(false);
                    tmp.text = "";

                    if (ActionFinish != null)
                        ActionFinish();
                    ActionFinish = null;
                });
            }
            else
            {
                tmp.DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(() => tmp.gameObject.SetActive(false));
            }
        }

        if(!backOff)
            image.DOColor(Color.clear, 0.5f).SetDelay(0.5f).OnComplete(() =>
            {

                finish = false;
                isLock = false;
                toInnPage.SetActive(true);
                gameObject.SetActive(false);
            });

    }
}
