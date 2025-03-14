using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    bool[] mute = new bool[2];
    public Image sfxButton, bgmButton;
    public void SoundButton(bool _isBgm)
    {

        if (_isBgm)
        {
            if (mute[0])
            {
                SoundManager.instance.BgmVolume(10);
                bgmButton.color = Color.white;
                mute[0] = false;
            }
            else
            {
                SoundManager.instance.BgmVolume(-100);
                bgmButton.color = Color.gray;
                mute[0] = true;
            }
        }
        else
        {

            if (mute[1])
            {
                SoundManager.instance.SfxVolume(-15);
                sfxButton.color = Color.white;
                mute[1] = false;
            }
            else
            {
                SoundManager.instance.SfxVolume(-100);
                sfxButton.color = Color.gray;
                mute[1] = true;
            }
        }
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
