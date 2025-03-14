using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextOutLine : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] bool drawTop, useTextColor = false;
    [SerializeField] float size;
    [SerializeField] Color[] color;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        Set();
    }
    void Set()
    {
        text.outlineWidth = size;
        if(!useTextColor) text.color = color[0];
        text.outlineColor = color[1];
        text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, size);

        if(drawTop) text.fontMaterial.renderQueue = 3001;
    }

}
