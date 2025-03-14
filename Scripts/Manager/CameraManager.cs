using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    [Header("Camera")]
    public CinemachineVirtualCamera vCam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Zoom(float _degree = 5, float _duration = 1)
    {
        DOTween.To(() => vCam.m_Lens.OrthographicSize, x => vCam.m_Lens.OrthographicSize = x, _degree, _duration);
    }

    public void Shake(float _power = 1, float _duration = 0.5f)
    {
        CinemachineBasicMultiChannelPerlin tmpVcamShaker =
            vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        tmpVcamShaker.m_AmplitudeGain = _power;
        DOTween.To(() =>
        tmpVcamShaker.m_AmplitudeGain, x =>
        tmpVcamShaker.m_AmplitudeGain = x, 0f, _duration);
    }
}
