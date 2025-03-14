using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InfoPage : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] Transform[] talents;

    public List<GameObject> pages;
    int pageIndex = 0;

    public CatInfo catInfo;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Set(bool _up)
    {
        if (_up)
        {
            rectTransform.DOAnchorPosY(700, 0.5f);
            pageIndex = 0;

            foreach (GameObject i in pages) i.SetActive(false);
            pages[pageIndex].SetActive(true);
            catInfo.Set();

        }
        else
            rectTransform.DOAnchorPosY(20, 0.5f);


    }

    public void Next(bool _isNext)
    {
        StartScene.instance.SoundPlay(StartScene.instance.uiClips[3]);
        if(_isNext)
        {
            pages[pageIndex].SetActive(false);
            pageIndex = (pageIndex + 1) % pages.Count;
            pages[pageIndex].SetActive(true);
        }
        else
        {
            pages[pageIndex].SetActive(false);
            pageIndex = (pageIndex - 1) < 0 ? pages.Count - 1 : pageIndex - 1;
            pages[pageIndex].SetActive(true);
        }
    }
}
