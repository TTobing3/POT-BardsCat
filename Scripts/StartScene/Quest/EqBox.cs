using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class EqBox : MonoBehaviour
{
    [SerializeField] GameObject squarePrefab, layout;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] GameObject[] tabs;
    [SerializeField] List<GameObject> squares = new List<GameObject>();

    public int curTab = 0;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        //slot 세팅
        SetSlot();
        DataCarrier.instance.ActionAddToStorage += SetSlot;
    }

    #region 툴팁
    private void Update()
    {
        if(gameObject.activeSelf)
            CheckTooltipOut();
    }
    void CheckTooltipOut()
    {
        // 마우스 위치를 스크린 좌표에서 월드 좌표로 변환합니다.
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        // RectTransformUtility를 사용하여 스크린 좌표를 로컬 좌표로 변환합니다.
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            mousePosition,
            StartSceneManager.instance.canvas.worldCamera,
            out localPoint
        );

        // 마우스가 RectTransform 내부에 있는지 확인합니다.
        if (!isInside || !rect.rect.Contains(localPoint))
        {
            StartSceneManager.instance.toolTip.gameObject.SetActive(false);
        }
    }
    #endregion

    void SetSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            if (DataCarrier.instance.storage.items[i].Count > squares.Count && DataCarrier.instance.storage.items[i].Count > 15)
            {
                var tmpCount = DataCarrier.instance.storage.items[i].Count % 5 == 0 ? DataCarrier.instance.storage.items[i].Count : (DataCarrier.instance.storage.items[i].Count / 5 + 1) * 5;
                for (int j = squares.Count; j < tmpCount; j++)
                {
                    GenerateSqure(j);
                }
            }
            else
            {
                for (int j = squares.Count; j < 15; j++)
                {
                    GenerateSqure(j);
                }
            }
        }
    }

    void GenerateSqure(int _n, bool _isEmpty = false)
    {
        var tmpSquare = Instantiate(squarePrefab, layout.transform);

        squares.Add(tmpSquare);

        squares[_n].GetComponent<InstantData>().number = _n;
        squares[_n].GetComponent<Square>().eqBox = this;
    }
    //called by tab button
    public void SelectTab(int _n)
    {
        foreach (GameObject i in tabs) i.SetActive(false);

        tabs[_n].SetActive(true);

        ChangeTab(_n);
    }

    void ChangeTab(int _n)
    {
        curTab = _n;
        scrollRect.normalizedPosition = new Vector2(0, 1);
        //슬롯 세팅
        foreach(GameObject i in squares) 
            i.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(false);
        for(int i = 0; i < DataCarrier.instance.storage.items[_n].Count; i++) 
            squares[i].GetComponent<SlotImageChanger>().ItemImageChange(_n, i);

        DataCarrier.instance.ActionChangeTab();
    }

    void OnDestroy()
    {
        DataCarrier.instance.ActionAddToStorage -= SetSlot;
    }

}