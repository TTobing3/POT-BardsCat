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
        //slot ����
        SetSlot();
        DataCarrier.instance.ActionAddToStorage += SetSlot;
    }

    #region ����
    private void Update()
    {
        if(gameObject.activeSelf)
            CheckTooltipOut();
    }
    void CheckTooltipOut()
    {
        // ���콺 ��ġ�� ��ũ�� ��ǥ���� ���� ��ǥ�� ��ȯ�մϴ�.
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        // RectTransformUtility�� ����Ͽ� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ�մϴ�.
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            mousePosition,
            StartSceneManager.instance.canvas.worldCamera,
            out localPoint
        );

        // ���콺�� RectTransform ���ο� �ִ��� Ȯ���մϴ�.
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
        //���� ����
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