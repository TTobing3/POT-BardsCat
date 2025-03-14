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
        // UI ������Ʈ�� ȭ�� ��ǥ�� ����մϴ�.
        Vector2 screenPos = rect.anchoredPosition;

        // ȭ���� ũ�⸦ �����ɴϴ�.
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // UI ������Ʈ�� ũ�⸦ �����ɴϴ�.
        Vector2 size = rect.sizeDelta;

        // UI ������Ʈ�� ȭ�� �ٱ����� ������ ������ Ȯ���մϴ�.
        rect.pivot = new Vector2(0, 0);

        if (screenPos.x + size.x > screenSize.x) // ���������� ������ ���
        {
            rect.pivot = new Vector2(1, rect.pivot.y);
        }
        if (screenPos.y + size.y > screenSize.y) // ���� ������ ���
        {
            rect.pivot = new Vector2(rect.pivot.x, 1);
        }
    }
}
