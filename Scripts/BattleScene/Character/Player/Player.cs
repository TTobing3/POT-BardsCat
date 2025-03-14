using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat { Health, Mana, Speed, Attack, Magic, Defense, Resist, Recover, ManaRecover }
public enum WeaponCode { Small, OneHand, TwoHand, Pistol, Musket, Wand, Staff, None }


public class Player : MonoBehaviour
{
    [HideInInspector] public StateDecider stateDecider;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerSpriteSetter spriteSetter;
    [HideInInspector] public MotionAnimator motionAnimator;
    [HideInInspector] public PlayerAnimator playerAnimator;

    PlayerData playerData;
    AudioSource audioSource;
    public AudioClip[] sfx;
    public Transform canvas;

    public Animator animator;

    public WeaponData weapon;

    public Talent talent;

    //[HideInInspector] 
    //public float[] stats = new float[10]; // 모든 계산이 끝난 결괏값 // speed

    public int[] baseStats = new int[9];

    public int[] plusStats = new int[9] { 0,0,0,0,0, 0,0,0,0 };

    public float[] stats
    {
        get
        {
            float[] result = new float[9];
            for (int i = 0; i < 9; i++)
            {
                result[i] = baseStats[i] + plusStats[i];

                if (i == (int)PlayerStat.Speed)
                {
                    if (stateDecider.form == 0)
                    {
                        result[i] = defaultSpeed[(int)WeaponCode.None] + result[i] * 0.01f; // 5 10 * 0.1
                    }
                    else
                    {
                        result[i] = defaultSpeed[weapon.type] + result[i] * 0.01f - weapon.speed * 0.01f;
                    }
                }
            }
            return result;
        }
    }

    public float[] defaultSpeed = new float[] { 4, 2, 1, 3, 1, 3, 1, 4 };

    public int hp, maxHp, mp, maxMana, exp, maxExp, gold;

    public bool isDead;

    int stack = -1;
    public int Stack
    {
        get { return stack; }
        set
        {
            if (value >= weapon.effects.Length)
                stack = 0;
            else
                stack = value;
        }
    }

    Coroutine manaRecover, recover;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        spriteSetter = GetComponent<PlayerSpriteSetter>();
        animator = GetComponent<Animator>();
        stateDecider = GetComponent<StateDecider>();
        motionAnimator = GetComponent<MotionAnimator>();
        playerAnimator = GetComponent<PlayerAnimator>();
        talent = GetComponent<Talent>();
        audioSource = GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = DataCarrier.instance.mixer.FindMatchingGroups("SFX")[0];
    }

    public void Set()
    {
        playerData = DataCarrier.instance.playerData; //플레이어데이터에 스텟 달기

        baseStats = DataManager.instance.AllCatDatas[DataCarrier.instance.playerData.cat].stats;

        maxHp =  baseStats[(int)PlayerStat.Health] * 10;
        maxExp = 100;

        UIManager.instance.hpBar.SetMaxHP(maxHp);
        UIManager.instance.expBar.SetMaxExp(maxExp);

        ChangeExp(0);
        ChangeHp(maxHp);

        ChangeWeapon(DataCarrier.instance.playerData.item[0]);

        spriteSetter.ChangeCat(playerData.cat);
        spriteSetter.ChangeHelmet(playerData.item[1]);
        spriteSetter.ChangeArmor(playerData.item[2]);

        maxMana = weapon.mana;

        StartHealthRecover();

        talent.SetAbilities();
    }


    #region Gold
    public void ChangeGold(int _plus)
    {
        if (isDead) return;
        gold = _plus;
    }

    public void GainGold(int _plus)
    {
        UIManager.instance.FloatDamage(canvas, _plus, stateDecider.faceDirection, Color.yellow, Color.black);
        ChangeGold(gold + _plus);
    }
    #endregion
    #region EXP

    void ChangeExp(int _plus)
    {
        if (isDead) return;

        exp = _plus;
        UIManager.instance.expBar.SetExp(exp);
        UIManager.instance.playerExpText.text = exp.ToString();
        if (exp >= maxExp && !GameManager.instance.isClear)
        {
            UpgradeLevel();
        }
    }

    public void GainExp(int _plus)
    {
        UIManager.instance.FloatDamage(canvas, _plus, stateDecider.faceDirection, Color.green, Color.black);
        ChangeExp(exp + _plus);
    }

    void UpgradeLevel()
    {
        UIManager.instance.SetOptionPage();
        ChangeExp(0);
    }

    public void GainTalent(OptionData _optionData)
    {
        talent.GainTalent(_optionData);
    }

    #endregion
    #region HP

    void ChangeHp(int _hp)
    {
        if (isDead) return;
        hp = _hp > maxHp ? maxHp : _hp;
        hp = _hp < 0 ? 0 : hp;
        UIManager.instance.hpBar.SetHP(hp);
        UIManager.instance.playerHpText.text = hp + "/" + maxHp;
        if (hp <= 0) Dead();
    }

    void GainHp(int _hp)
    {
        ChangeHp(hp + _hp);
    }

    public void Hit(float[] _damage)
    {
        spriteSetter.FlashWhite();

        SfxPlay(sfx[(int)Sfx.Hit]);
        var tmpDamage = CalculateDamage(_damage);

        UIManager.instance.FloatDamage(canvas, tmpDamage, stateDecider.faceDirection, Color.white, Color.black);
        GainHp(tmpDamage);
    }

    int CalculateDamage(float[] _damage)
    {
        float[] tmpDamage = new float[3] { 0, 0, 0 };

        tmpDamage[0] = _damage[0] - baseStats[(int)PlayerStat.Defense] < 0 ? 0 : _damage[0] - baseStats[(int)PlayerStat.Defense];
        tmpDamage[1] = _damage[1] - baseStats[(int)PlayerStat.Resist] < 0 ? 0 : _damage[1] - baseStats[(int)PlayerStat.Resist];
        tmpDamage[2] = _damage[2];

        var resultDamage = (int)(tmpDamage[0] + tmpDamage[1] + tmpDamage[2]);
        return -resultDamage;
    }

    public void Dead()
    {
        motionAnimator.ReceiveAnimation((int)Motion.Dead, stateDecider.direction, true);
        isDead = true;

        StopHealthRecover();
        StopManaRecover();

        GameManager.instance.FinishQuest(false);
    }

    #endregion
    #region Mana
    public void UseBullet()
    {
        ChangeMana(--mp);
        UIManager.instance.bulletBar.SetBullet();
    }

    public void ReloadBullet()
    {
        ChangeMana(weapon.mana);
        UIManager.instance.bulletBar.SetBullet();
    }
    
    //

    public void GainMana(int _mana)
    {
        ChangeMana(mp + _mana);
        UIManager.instance.manaBar.SetMana();

    }

    void ChangeMana(int _mana)
    {
        if (isDead) return;
        mp = _mana > weapon.mana ? weapon.mana : _mana;
        playerAnimator.ManaSetting();
    }


    //
    void StartHealthRecover()
    {
        recover = StartCoroutine(CoHealthRecover());
    }

    void StopHealthRecover()
    {
        if (recover == null) return;
        StopCoroutine(recover);
    }

    IEnumerator CoHealthRecover()
    {
        GainHp((int)PlayerStat.Recover);
        yield return new WaitForSeconds(2f);
        recover = StartCoroutine(CoHealthRecover());
    }

    void StartManaRecover()
    {
        manaRecover = StartCoroutine(CoManaRecover());
    }

    void StopManaRecover()
    {
        if (manaRecover == null) return;
        StopCoroutine(manaRecover);
    }

    IEnumerator CoManaRecover()
    {
        GainMana(baseStats[(int)PlayerStat.ManaRecover]);
        yield return new WaitForSeconds(2f);
        manaRecover = StartCoroutine(CoManaRecover());
    }

    #endregion

    #region Weapon
    public void ChangeWeapon(string _weapon)
    {
        weapon = DataManager.instance.AllWeaponDatas[_weapon];
        ChangeDetailWeapon();
    }
    public void ChangeWeapon(int _code)
    {
        weapon = DataManager.instance.AllWeaponDataList[_code];
        ChangeDetailWeapon();
    }
    void ChangeDetailWeapon()
    {
        spriteSetter.ChangeWeapon(weapon.name);
        animator.SetInteger("Weapon", weapon.type);

        ChangeMana(weapon.mana);

        UIManager.instance.ActiveBar();


        if (!weapon.form)
        {
            animator.SetInteger("Form", 1);
            stateDecider.form = Form.Lock;
            stateDecider.ResetMotion();
        }

        if (weapon.type == (int)WeaponCode.Staff || weapon.type == (int)WeaponCode.Wand)
        {   
            StartManaRecover();
        }
        else StopManaRecover();


        //ChangeSpeed();
    }

    #endregion

    public void SfxPlay(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

}
