using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterStat { Health, Speed, Attack, Magic, Defense, Resist, Recover }
public class Monster : MonoBehaviour
{
    public MonsterData monsterData;

    [Tooltip("0:피격\n1:죽음")]
    public AudioClip[] sfx;

    MonsterStateDecider monsterStateDecider;
    MonsterSpriteSetter monsterSpriteSetter;
    MotionAnimator motionAnimator;
    MonsterMove monsterMove;

    ParticleSystem particle;
    AudioSource audioSource;

    [Tooltip("데미지를 표시할 캔버스")]
    public Transform canvas;

    [HideInInspector] public int[] baseStats = new int[10];

    [HideInInspector] public int hp = 100, maxHp = 100;

    [HideInInspector] public bool isDead = false;

    private void Awake()
    {
        monsterStateDecider = GetComponent<MonsterStateDecider>();
        monsterSpriteSetter = GetComponent<MonsterSpriteSetter>();
        motionAnimator = GetComponent<MotionAnimator>();
        monsterMove = GetComponent<MonsterMove>();

        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = DataCarrier.instance.mixer.FindMatchingGroups("SFX")[0];
    }

    public void Set(string _monsterName)
    {
        monsterData = DataManager.instance.AllMonsterDatas[_monsterName];
        monsterSpriteSetter.ChangeArmor();

        baseStats = monsterData.stats;
        maxHp = baseStats[(int)MonsterStat.Health];
        hp = maxHp;

        monsterStateDecider.StartPattern();
        audioSource.clip = sfx[(int)Sfx.Hit];
        
    }

    public void SfxPlay(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    #region Change

    void ChangeHp(int _hp)
    {
        hp = _hp > maxHp ? maxHp : _hp;
        if (hp <= 0) Dead();

        UIManager.instance.enemyHpBar.Set(this);
    }

    void GainHp(int _hp)
    {
        ChangeHp(hp + _hp);
    }

    #endregion

    #region Hit

    public void Hit(float[] _damage)
    {
        if (isDead) return;

        #region Hit Effect

        var tmpDamage = CalculateDamage(_damage);

        particle.Play();
        SfxPlay(sfx[(int)Sfx.Hit]);
        monsterSpriteSetter.FlashWhite();
        //SoundManager.instance.SfxPlay(sfx[(int)Sfx.Hit]);
        UIManager.instance.FloatDamage(canvas, tmpDamage, monsterStateDecider.curDir, Color.white, Color.black);

        #endregion

        if(!monsterStateDecider.curPattern.superArmor)
        {
            monsterMove.StopMove();
            monsterMove.Dash(-20);
            motionAnimator.ReceiveAnimation((int)MonsterMotion.Hit, monsterStateDecider.curDir, true);
        }
        
        monsterStateDecider.StopCoPattern();

        GainHp(tmpDamage);
    }

    int CalculateDamage(float[] _damage)
    {
        float[] tmpDamage = new float[3] { 0, 0, 0 };

        tmpDamage[0] = _damage[0] - baseStats[(int)MonsterStat.Defense] < 0 ? 0 : _damage[0] - baseStats[(int)MonsterStat.Defense];
        tmpDamage[1] = _damage[1] - baseStats[(int)MonsterStat.Resist] < 0 ? 0 : _damage[1] - baseStats[(int)MonsterStat.Resist];
        tmpDamage[2] = _damage[2];

        var resultDamage = (int)(tmpDamage[0] + tmpDamage[1] + tmpDamage[2]);
        return -resultDamage;
    }
    public void Dead()
    {
        SfxPlay(sfx[(int)Sfx.Death]);
        //SoundManager.instance.SfxPlay(sfx[(int)Sfx.Death]);
        monsterStateDecider.StopCoPattern();
        monsterMove.StopMove();
        motionAnimator.ReceiveAnimation((int)MonsterMotion.Dead, monsterStateDecider.curDir, true);
        isDead = true;

        StartCoroutine(CoDestory());

        Drop(monsterData.items[Random.Range(0, monsterData.items.Length)]);
        BattleDataManager.instance.OnMonsterDead(this);
    }

    IEnumerator CoDestory()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    public void Drop(string _name)
    {
        DropItem tmpDropItem = null;

        foreach (GameObject i in BattleDataManager.instance.dropitems)
        {
            if (!i.activeSelf)
            {
                tmpDropItem = i.GetComponent<DropItem>();
                break;
            }
        }

        if (tmpDropItem == null)
        {
            tmpDropItem = Instantiate(BattleDataManager.instance.AllDropItemPrefabList[0]).GetComponent<DropItem>();
            BattleDataManager.instance.dropitems.Add(tmpDropItem.gameObject);
        }
        tmpDropItem.Set(transform, _name);
    }

    #endregion
}
