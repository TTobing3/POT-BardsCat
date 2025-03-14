using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using TMPro;

enum SkullRank { Normal, Power, Crime, Golden, Kingdom, Rare, Nightmare, Space, Boss }

public class SkullMark : MonoBehaviour
{
    public HuntTargetData huntTargetData;

    Image image;
    Light2D light2d;
    ParticleSystem[] particleSystems; // 0 : »Ò±Õ »πµÊ 1 : º±≈√

    public List<Sprite> skullImages;
    public TextMeshProUGUI nameText;

    public List<GameObject> arrows;

    void Awake()
    {
        light2d = GetComponentInChildren<Light2D>();
        image = GetComponent<Image>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }
    #region Set
    public void Set(string _data)
    {
        huntTargetData = DataManager.instance.AllHuntTargetDatas[_data];

        gameObject.SetActive(true);
        light2d.SetIntensity(10, 1, 0.5f);

        SetImage();
        SetParticle();
        SetName();

        InnSceneManager.instance.DelayCall(1, ()=> {
            TextManager.instance.PlayTalk("¡÷¿Œ",
                new List<System.Action>() { InnSceneManager.instance.map.DrawSkull, OffSkull },
                DataManager.instance.AllConversationDatas[ huntTargetData.name ].texts.ToList(), 0, true);
        });
    }

    void SetParticle()
    {
        particleSystems[0].Play();
    }

    void SetImage()
    {
        image.sprite = skullImages[huntTargetData.GetSkullRank()];
    }

    void SetName()
    {
        nameText.text = huntTargetData.name;
        nameText.color = huntTargetData.GetSkullRankColor();
    }
    #endregion

    public void SetArrow(int _step)
    {
        arrows[0].SetActive(false);
        arrows[1].SetActive(false);

        if (_step == -1) return;

        if (_step == 0)
        {
            arrows[1].SetActive(true);
            arrows[1].GetComponentInChildren<TextMeshProUGUI>().text = "º±≈√";
        }
        else
        if (_step == 1)
        {
            arrows[0].SetActive(true);
            arrows[0].GetComponentInChildren<TextMeshProUGUI>().text = "√Îº“";
            arrows[1].SetActive(true);
            arrows[1].GetComponentInChildren<TextMeshProUGUI>().text = "»Æ¿Œ";
        }
    }

    public void OffSkull()
    {
        light2d.SetIntensity(light2d.intensity, 0, 0.2f);
        nameText.gameObject.SetActive(false);

        SetArrow(-1);
    }

    public void OnSkull()
    {
        light2d.SetIntensity(0, 1, 0.2f);
        nameText.gameObject.SetActive(true);

        SetArrow(0);

        particleSystems[1].Play();
    }

    public void SelectSkull()
    {
        OnSkull();

        SetArrow(1);

        nameText.gameObject.SetActive(false);
        particleSystems[1].Play();
    }

    public void ConfirmSkull()
    {
        SetArrow(-1);
        particleSystems[1].Play();
    }

    public void CancleSkull()
    {
        OnSkull();
    }
}
