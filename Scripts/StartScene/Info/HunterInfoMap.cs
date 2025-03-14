using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HunterInfoMap : MonoBehaviour
{
    RectTransform rect;
    public RectTransform badge;
    public GameObject fade;

    bool zoom = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void SetSize()
    {
        StartScene.instance.SoundPlay(StartScene.instance.uiClips[1]);
        if (zoom)
        {
            zoom = false;
            fade.gameObject.SetActive(false);
            rect.DOScale(new Vector3(1f, 1f), 0.5f);
            rect.DOAnchorPos(new Vector2(0, 0), 0.3f);

            badge.DOAnchorPos(new Vector2(-220, -50), 0.5f).SetEase(Ease.Unset);
        }
        else
        {
            zoom = true;
            fade.gameObject.SetActive(true);
            rect.DOScale(new Vector3(1.5f, 1.5f), 0.5f);
            rect.DOAnchorPos(new Vector2(0, 200), 0.3f);

            badge.DOAnchorPos(new Vector2(-5, 320), 0.5f).SetEase(Ease.Unset);
        }
    }
}
