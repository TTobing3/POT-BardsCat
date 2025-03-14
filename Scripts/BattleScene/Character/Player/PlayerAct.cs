using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : MonoBehaviour
{
    StateDecider stateDecider;
    PlayerMove playerMove;
    Player player;

    void Awake()
    {
        stateDecider = GetComponent<StateDecider>();
        playerMove = GetComponent<PlayerMove>();
        player = GetComponent<Player>();
    }

    #region Act

    public void InstantiateEffect(string _eftName, bool _byWeapon = false)
    {
        if (_eftName == "None") return;

        Effect tmpEft = null;

        foreach(GameObject i in BattleDataManager.instance.effects)
        {
            if(!i.activeSelf)
            {
                tmpEft = i.GetComponent<Effect>();
                break;
            }
        }

        if (tmpEft == null)
        {
            tmpEft = Instantiate(GameManager.instance.effectObj).GetComponent<Effect>();
            BattleDataManager.instance.effects.Add(tmpEft.gameObject);
        }

        tmpEft.Set(transform,true,  DataManager.instance.SearchEffectData(_eftName), stateDecider.faceDirection, _byWeapon ? player.weapon : null);
    }

    //called by animation
    public void Act()
    {
        player.Stack++; 
        switch(player.weapon.type)
        {
            case (int)WeaponType.Dagger:
                DaggerAct();
                break;

            case (int)WeaponType.Sword:
                SwordAct();
                break;

            case (int)WeaponType.LongSword:
                LongSwordAct();
                break;

            case (int)WeaponType.Pistol:
                PistolAct();
                break;

            case (int)WeaponType.Musket:
                MusketAct();
                break;

            case (int)WeaponType.Wand:
                WandAct();
                break;

            case (int)WeaponType.Staff:
                StaffAct();
                break;

            case (int)WeaponType.Bow:
                BowAct();
                break;
        }

        //백스템이면 뒤로
        //슬레쉬면 앞으로
    }

    //Lock일때 BackAct는 -1 곱해주기
    void DaggerAct()
    {
        if (stateDecider.motion == Motion.FrontAct)
        {
            playerMove.Dash(50);

            foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);

        }
        else if (stateDecider.motion == Motion.BackAct)
        {
           
        }
    }
    
    void SwordAct()
    {
        if (stateDecider.motion == Motion.FrontAct)
        {
            foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);
        }
        else if (stateDecider.motion == Motion.BackAct)
        {
        }
    }

    void LongSwordAct()
    {
        if (stateDecider.motion == Motion.FrontAct)
        {
            foreach (string i in player.weapon.effects[player.Stack])InstantiateEffect(i, true);
        }
        else if (stateDecider.motion == Motion.BackAct)
        {
            playerMove.Dash(-50);
        }

        if(stateDecider.motion == Motion.Idle)
        {
            foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);
        }
    }

    void MusketAct()
    {
        if (stateDecider.motion == Motion.FrontAct)
        {

            if (player.mp == 0)
            {
                player.ReloadBullet();
                SoundManager.instance.SfxPlay(BattleDataManager.instance.weaponSubAudioClip[player.weapon.type]);
            }
            else
            {
                playerMove.Dash(-20);

                foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);

                player.UseBullet();
            }

        }
        else if (stateDecider.motion == Motion.BackAct)
        {
            //player.ReloadBullet();
        }
    }

    void PistolAct()
    {
        if (stateDecider.motion == Motion.FrontAct)
        {
            if(player.mp == 0)
            {
                player.ReloadBullet();
                SoundManager.instance.SfxPlay(BattleDataManager.instance.weaponSubAudioClip[player.weapon.type]);
            }
            else
            {
                playerMove.Dash(-10);

                foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);

                player.UseBullet();
            }
        }
        else if (stateDecider.motion == Motion.BackAct)
        {
        }
    }

    void WandAct()
    {

        if (stateDecider.motion == Motion.FrontAct)
        {

            if (player.mp != 0) // 최소 필요 마나보다 크다면으로 바꿔야 함
            {
                foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);
                player.GainMana(-1);
            }

        }
        else
        {

        }
    }

    void StaffAct()
    {
        foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);
    }

    void BowAct()
    {
        foreach (string i in player.weapon.effects[player.Stack]) InstantiateEffect(i, true);
    }

    #endregion

}
