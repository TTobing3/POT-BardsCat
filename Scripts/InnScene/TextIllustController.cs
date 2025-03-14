using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class IllustAndName
{
    public string name;
    public Transform illust;
}

public class TextIllustController : MonoBehaviour
{
    public List<IllustAndName> Illusts;

    void SetIllust(string _name)
    {
        var selectedIllust = Illusts.FirstOrDefault(illustAndName => illustAndName.name == _name);


    }
}
