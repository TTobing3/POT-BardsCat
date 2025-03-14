using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipSub : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title, des;
    [SerializeField] RectTransform body;

    [SerializeField] RectTransform rect;

    public void Set(string _title, string _des)
    {
        title.text = _title;
        des.text = _des;
    }

    public void SetSize()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, title.GetComponent<RectTransform>().sizeDelta.y + des.GetComponent<RectTransform>().sizeDelta.y + 20 + 20 + 5);
    }

    void AdjustPosition()
    {
        // UI 오브젝트의 화면 좌표를 계산합니다.
        Vector2 screenPos = rect.anchoredPosition;

        // 화면의 크기를 가져옵니다.
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // UI 오브젝트의 크기를 가져옵니다.
        Vector2 size = rect.sizeDelta;

        // UI 오브젝트가 화면 바깥으로 나가는 방향을 확인합니다.
        rect.pivot = new Vector2(0, 0);

        if (screenPos.x + size.x > screenSize.x) // 오른쪽으로 나가는 경우
        {
            rect.pivot = new Vector2(1, rect.pivot.y);
        }
        if (screenPos.y + size.y > screenSize.y) // 위로 나가는 경우
        {
            rect.pivot = new Vector2(rect.pivot.x, 1);
        }
    }
}
