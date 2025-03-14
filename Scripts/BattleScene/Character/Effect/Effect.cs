using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EffectType { Slash, Spark, Arrow, Magic, Drop }
public enum WeaponSfx { Use, Sub }

[System.Serializable]
public class EffectDTO
{
    public int count = 0;

    public EffectDTO DeepCopy(EffectDTO dto)
    {
        return new EffectDTO()
        {
            count = dto.count
        };
    }
}

public class Effect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public ParticleSystem particle;

    EffectDTO dto;

    EffectData effectData;
    WeaponData weaponData;
    Transform owner, root;

    AudioSource audioSource;

    bool fromPlayer;
    int direction;

    //

    List<Collider2D> hitDelayList = new List<Collider2D>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = DataCarrier.instance.mixer.FindMatchingGroups("SFX")[0];
    }

    public void Set(Transform _owner, bool _fromPlayer, EffectData _effectData, int _direction, WeaponData _weapon, EffectDTO _dto = null, Transform _root = null)
    {
        gameObject.SetActive(true);

        effectData = _effectData;
        weaponData = _weapon;

        direction = _direction;

        transform.rotation = Quaternion.Euler(new Vector3(1, 1, 1));

        if (_root == null)
            root = _owner;
        else
            root = _root;
        owner = _owner;
        transform.parent = _owner; //스프라이트 방향
        fromPlayer = _fromPlayer;

        hitDelayList = new List<Collider2D>();

        if(_dto == null)
        {
            dto = new EffectDTO().DeepCopy( effectData.dto );
        }
        else
        {
            dto = _dto;
        }

        //

        SetSprite();
        SetParticle();

        Spawn();
        Move();

        StartCoroutine(CoDestroy());

        transform.SetParent(null);

        particle.Play();

        //

        CallMultiEffect(MulitCallBy.Generate);
    }

    void SetSprite()
    {
        spriteRenderer.sprite = effectData.sprite[Random.Range(0, effectData.sprite.Length-1)];
        spriteRenderer.color = new Color(1, 1, 1, 1);

        spriteRenderer.transform.localScale = new Vector3(1, 1, 1);

        if(effectData.isDetailCollider)
        {
            boxCollider.size = new Vector2(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);
        }
        else
        {
            boxCollider.size = new Vector2(spriteRenderer.bounds.size.x, 10);
        }

        if (effectData.isFadeOut)
            spriteRenderer.DOFade(0, effectData.duration);

        if(!effectData.isNeedToFlip && owner.localScale.x < 0)
            spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);

    }

    void SetParticle()
    {
        particle.Stop();

        var dto_p = effectData.particleDTO;

        var pos = new Vector3((direction == 1 ? dto_p.pos.x * (-1) : dto_p.pos.x), dto_p.pos.y, 1); 
        particle.transform.position = transform.position + pos;

        var main = particle.main;
        main.loop = dto_p.loop;
        main.startColor = dto_p.color;
        main.startSize = dto_p.size + Random.Range(-dto_p.sizeRange / 2, dto_p.sizeRange / 2);
        main.startSpeed = dto_p.speed + Random.Range(-dto_p.speedRange / 2, dto_p.speedRange / 2);
        main.duration = dto_p.lifeTime + Random.Range(-dto_p.lifeTimeRange/2, dto_p.lifeTimeRange / 2);
        main.startLifetime = dto_p.duration + Random.Range(-dto_p.durationRange / 2, dto_p.durationRange / 2);

        var emission = particle.emission;
        var burst = emission.GetBurst(0);
        burst.count = dto_p.count + Random.Range(-dto_p.countRange, dto_p.countRange);

        emission.SetBurst(0, burst);

        var shape = particle.shape;
        shape.radius = effectData.particleDTO.radius + Random.Range(-dto_p.radiusRange / 2, dto_p.radiusRange / 2);
    }

    //생성
    void Spawn()
    {
        //SoundManager.instance.SfxPlay(effectData.sfx[(int)WeaponSfx.Use]);
        SfxPlay(effectData.sfx[(int)WeaponSfx.Use]);

        if (effectData.isShakeCamera) UIManager.instance.ShakeCamera();

        if(effectData.spawnType == PosType.Current)
        {
            var posX = owner.position.x +
                Random.Range(-effectData.spawnRange / 2, effectData.spawnRange / 2) +
                (direction == 1 ? effectData.spawnPos.x * (-1) : effectData.spawnPos.x);
            var posY = owner.position.y + effectData.spawnPos.y;

            var tmpSpawnPos = new Vector3(posX, posY);

            transform.position = tmpSpawnPos;
        }
        else if(effectData.spawnType == PosType.Target)
        {
            transform.position = effectData.targetPos;
        }
    }

    //이동
    void Move()
    {
        if (effectData.movePattern == MovePattern.None)
        {
            return;
        }

        var duration = effectData.duration + Random.Range(-effectData.durationRange / 2, effectData.durationRange / 2);

        if (effectData.targetType == PosType.Current)
        {
            var posX = owner.position.x +
                Random.Range(-effectData.targetRange / 2, effectData.targetRange / 2) + 
                ( direction == 1 ? effectData.targetPos.x * (-1) : effectData.targetPos.x );
            var posY = owner.position.y + effectData.targetPos.y;

            if (effectData.movePattern == MovePattern.Linear)
            {
                transform.DOMove( new Vector3(posX, posY), duration);
            }
            else if(effectData.movePattern == MovePattern.Parabola)
            {
                transform.DOMoveX(posX, duration);

                var high = posY + effectData.targetSub + Random.Range(-effectData.targetSub / 2, effectData.targetSub / 2);
                var totalHightDis = (high - effectData.spawnPos.y + high - effectData.targetPos.y);
                var ratioToHigh = (high - effectData.spawnPos.y) / totalHightDis;
                var ratioToLow = (high - effectData.targetPos.y) / totalHightDis;

                transform.DOMoveY(high, duration * ratioToHigh).OnComplete(()=> 
                transform.DOMoveY(posY, duration * ratioToLow));
            }
            else if (effectData.movePattern == MovePattern.FloatAim)
            {
                var tmpPos = new Vector2(owner.position.x + Random.Range(-effectData.targetSub/2, effectData.targetSub/2f), owner.position.y + Random.Range(0, effectData.targetSub*2f));
                var tmpTime = effectData.duration * 0.6f;

                transform.DORotate(GetTargetDirectionVector(tmpPos), tmpTime, RotateMode.Fast);

                transform.DOMove(tmpPos, tmpTime).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    transform.DORotate(GetTargetDirectionVector(new Vector2(posX, posY)), 0.1f, RotateMode.Fast).OnComplete(() =>
                    transform.DOMove(new Vector3(posX, posY), duration - tmpTime));
                    
                });
            }
        }
        else if(effectData.targetType == PosType.Target)
        {
            transform.DOMove(effectData.targetPos, duration);
        }
    }

    //Spear 방향
    Vector3 GetTargetDirectionVector(Vector2 _target)
    {
        Vector2 direction = (Vector2)transform.position - _target;
        float tmpAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 60;


        return new Vector3(0, 0, tmpAngle);
    }


    //타격
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitDelayList.Contains(collision) || !effectData.attackable) return;

        if (fromPlayer)
        {
            if (collision.gameObject.CompareTag("Monster"))
            {
                collision.GetComponent<Monster>().Hit(CalculateDamage());
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.GetComponent<Player>().Hit(CalculateDamage());
            }
        }

        hitDelayList.Add(collision);
        CoDelayHit(collision);
    }

    IEnumerator CoDelayHit(Collider2D _monster)
    {
        yield return new WaitForSeconds(effectData.hitDelay);
        hitDelayList.Remove(_monster);
    }

    //제거
    IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(effectData.duration );

        CallMultiEffect(MulitCallBy.Destroy);

        DOTween.Kill(transform);
        DOTween.Kill(spriteRenderer);

        gameObject.SetActive(false);
        transform.parent = BattleDataManager.instance.effectParent;
    }

    float[] CalculateDamage()
    {
        float[] tmpDamage = new float[3] { 0, 0, 0 };

        if (fromPlayer)
        {
            //physics

            tmpDamage[0] = GameManager.instance.player.baseStats[(int)PlayerStat.Attack] * effectData.damagePoint[0];

            //magic

            tmpDamage[1] = GameManager.instance.player.baseStats[(int)PlayerStat.Magic] * effectData.damagePoint[1];

            //fix

            tmpDamage[2] = effectData.damagePoint[2];


            if (weaponData != null)
            {
                tmpDamage[0] *= weaponData.damagePoint[0];
                tmpDamage[1] *= weaponData.damagePoint[1];
                tmpDamage[2] += weaponData.damagePoint[2];
            }
        }
        else
        {

            //physics

            tmpDamage[0] = root.GetComponent<Monster>().baseStats[(int)MonsterStat.Attack] * effectData.damagePoint[0];

            //magic

            tmpDamage[1] = root.GetComponent<Monster>().monsterData.stats[(int)MonsterStat.Magic] * effectData.damagePoint[1];

            //fix

            tmpDamage[2] = effectData.damagePoint[2];

        }


        return tmpDamage;

    }

    public void SfxPlay(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    public void InstantiateEffect(string _eftName, bool _byWeapon = false)
    {
        if (_eftName == "None" || _eftName == "") return;

        Effect tmpEft = null;

        foreach (GameObject i in BattleDataManager.instance.effects)
        {
            if (!i.activeSelf)
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

        tmpEft.Set(transform, fromPlayer, DataManager.instance.SearchEffectData(_eftName), direction, _byWeapon ? weaponData : null, dto, root);
    }

    void CallMultiEffect(MulitCallBy _call)
    {
        if(_call == effectData.multiEffectType)
        {
            if (dto.count < 1) return;

            dto.count--;
            InstantiateEffect(effectData.multiEffectName);
        }
    }
}
