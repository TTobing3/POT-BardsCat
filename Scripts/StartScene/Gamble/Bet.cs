using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using DG.Tweening;

public class Bet : MonoBehaviour
{
    public void Set(string _name)
    {
        gameObject.name = _name;

        GetComponent<SpriteResolver>().SetCategoryAndLabel("Bet", _name);
        GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        GetComponent<Image>().SetNativeSize();
    }

    public void SetPosition(bool _isPlayer)
    {
        var rect = GetComponent<RectTransform>();

        var time = Random.Range(0.5f, 1.2f);

        rect.anchoredPosition = new Vector2(0, _isPlayer ? -600 : 600);

        rect.DOAnchorPos(
            new Vector2(Random.Range(-300, 300),
            Random.Range(-150, 150)),
            time).SetEase(Ease.Unset);

        rect.DORotate(
            new Vector3(0, 0, Random.Range(-200, 200)),
            time);
    }
}
