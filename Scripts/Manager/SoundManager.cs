using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum Sfx { Hit, Death }
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer mixer;
    public AudioSource bgmSource;
    public AudioClip[] bgmClips;
    public AudioClip[] UIClips;

    //

    public Slider bgmSlider, sfxSlider;

    //

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }


    public void BgmVolume(float size) // ������� ���� ����
    {
        mixer.SetFloat("BGM", (size <= -20) ? -80f : size);

        //mixer.SetFloat("Bgm",Mathf.Log10(size) * 20);
        //SaveDataManager.instance.bgmVolume = size;
    }
    public void SfxVolume(float size) // ȿ���� ���� ����
    {
        mixer.SetFloat("SFX", (size <= -30) ? -80f : size);

        //SaveDataManager.instance.sfxVolume = size;
        //mixer.SetFloat("Sfx",Mathf.Log10(size) * 20);
        //mixer.SetFloat("Dice",Mathf.Log10(size) * 20);
    }

    public void UISfxPlay(int _n) // ȿ���� �÷���
    {
        GameObject sfx = new GameObject(_n + "Sound");
        AudioSource audioSource = sfx.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = UIClips[_n];
        audioSource.Play();

        Destroy(sfx, UIClips[_n].length);
    }
    public void SfxPlay(AudioClip _clip) // ȿ���� �÷���
    {
        if (_clip == null) return; 
        GameObject sfx = new GameObject("SfxSound");
        AudioSource audioSource = sfx.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = _clip;
        audioSource.Play();

        Destroy(sfx, _clip.length);
    }
    public void BgSoundPlay(int _n) // ����� �÷���
    {
        bgmSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        bgmSource.clip = bgmClips[_n];
        bgmSource.loop = true;
        bgmSource.volume = 0.1f;
        bgmSource.Play();
    }

}
