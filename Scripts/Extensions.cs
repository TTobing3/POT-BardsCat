using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;


public static class Light2DExtensions
{
    public static void SetIntensity(this Light2D light2d, float _startIntensity, float _intensity, float _duration)
    {
        DOTween.Kill(light2d);
        light2d.intensity = _startIntensity;
        DOTween.To(() => light2d.intensity, x => light2d.intensity = x, _intensity, _duration);
    }
}

public class Extensions : MonoBehaviour
{

    public static Extensions e;

    public void DelayCall(float delay, System.Action action)
    {
        StartCoroutine(CoDelayCall(action, delay));
    }
    IEnumerator CoDelayCall(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
