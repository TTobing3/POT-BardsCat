using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] texts;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }


    public void Set(int _damage, Color _color, Color _outlineColor)
    {
        texts[0].color = _color;
        texts[0].outlineWidth = 0.2f;
        texts[0].outlineColor = _outlineColor;

        texts[0].fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0.2f);

        texts[0].text = "";
        if (_damage > 0) 
            texts[0].text += "+";
        else if(_damage == 0) 
            texts[0].text += "-";
        texts[0].text +=  _damage.ToString();

        rect.anchoredPosition = Vector2.zero;

        Float();
    }

    public void Float()
    {
        rect.DOAnchorPosY(100,1);
        texts[0].DOFade(0, 1).OnComplete(()=>Destroy(gameObject));
    }
}
