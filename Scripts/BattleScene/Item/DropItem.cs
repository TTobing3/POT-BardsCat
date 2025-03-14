using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D.Animation;

public enum DropItemType { Gold, Chip, Material, Eq, Gem } // Gold Chip Material Eq(:Tab) Gem
public class DropItem : MonoBehaviour
{

    public AudioClip[] clips;

    ParticleSystem particle;
    SpriteRenderer spriteRenderer;
    SpriteResolver spriteResolver;

    public DropItemType type;
    string[] itemData;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteResolver = GetComponent<SpriteResolver>();
    }
    public void Set(Transform _transform, string _name)
    {
        gameObject.SetActive(true);

        //�÷��̾� �������� ���� ���Ͽ� ��ǥ�� ����
        transform.parent = null;
        transform.position = _transform.position;

        //�� �ʱ�ȭ �� ���� ����
        spriteRenderer.color = new Color(1, 1, 1, 1);
        transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));


        //������ ������ �̸�:Ÿ��:����
        itemData = _name.Split(":");

        //Ÿ�� ���� ����
        type = (DropItemType)int.Parse(itemData[1]);


        switch (type)
        {
            case DropItemType.Gold:
                spriteResolver.SetCategoryAndLabel("Gold", itemData[0]);
                StartCoroutine(CoChangeSprite("Gold", itemData[0]));
                break;

            case DropItemType.Chip:
                spriteResolver.SetCategoryAndLabel("Chip", itemData[0]);
                StartCoroutine(CoChangeSprite("Chip", itemData[0]));
                break;

            case DropItemType.Material:
                spriteResolver.SetCategoryAndLabel("Material", itemData[0]);
                StartCoroutine(CoChangeSprite("Material", itemData[0]));
                break;

            case DropItemType.Eq:
                switch(int.Parse(itemData[2]))
                {
                    case (int)Tab.Weapon: 
                        spriteResolver.SetCategoryAndLabel("Weapon", itemData[0]);
                        StartCoroutine(CoChangeSprite("Weapon", itemData[0]));
                        break;
                    case (int)Tab.Helmet: 
                        spriteResolver.SetCategoryAndLabel("Helmet", itemData[0]);
                        StartCoroutine(CoChangeSprite("Helmet", itemData[0]));
                        break;
                    case (int)Tab.Acc: 
                        spriteResolver.SetCategoryAndLabel("Acc", itemData[0]);
                        StartCoroutine(CoChangeSprite("Acc", itemData[0]));
                        break;
                    case (int)Tab.Armor: 
                        spriteResolver.SetCategoryAndLabel("Armor", itemData[0]);
                        StartCoroutine(CoChangeSprite("Armor", itemData[0]));
                        break;
                }
                
                break;

            case DropItemType.Gem:
                spriteResolver.SetCategoryAndLabel("Gem", itemData[0]);
                StartCoroutine(CoChangeSprite("Gem", itemData[0]));
                break;
        }


        transform.DOMove(new Vector2(transform.position.x + Random.Range(-2,2f), transform.position.y + Random.Range(-2, 2f)), 0.5f).OnComplete(()=> MoveToPlayer());
        transform.DORotate(Vector3.zero, 0.5f);
    }

    IEnumerator CoChangeSprite(string _category, string _label)
    {
        yield return new WaitForEndOfFrame();

        spriteResolver.SetCategoryAndLabel(_category, _label);
    }

    void MoveToPlayer()
    {
        transform.DOMove(GameManager.instance.player.transform.position, 0.5f).SetEase(Ease.Unset).OnComplete(()=>MoveToPlayer());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SoundManager.instance.SfxPlay(clips[0]);

            particle.Play();
            spriteRenderer.DOFade(0, 0.2f).OnComplete(()=> Stop());

            Use();
        }
    }

    void Use()
    {
        BattleDataManager.instance.AddResult((int)type, itemData);

        FloatLog();
    }

    void FloatLog()
    {
        var log = "";

        //log �� Ÿ�� �߰�
        switch (type)
        {
            case DropItemType.Gold:
                //UIManager.instance.FloatLog($"{itemData[0]} {itemData[2]} ȹ��");
                return;

            case DropItemType.Chip:
                break;

            case DropItemType.Material:
                log += "[���] ";
                break;

            case DropItemType.Eq:

                switch (int.Parse(itemData[2]))
                {
                    case (int)Tab.Weapon: log += "[����] "; break;
                    case (int)Tab.Helmet: log += "[����] "; break;
                    case (int)Tab.Armor : log += "[����] "; break;
                    case (int)Tab.Acc   : log += "[��ű�] "; break;
                }

                break;

            case DropItemType.Gem:
                log += "[����]";
                break;
        }

        UIManager.instance.FloatLog(log + itemData[0] + " ȹ��");

    }

    void Stop()
    {
        transform.parent = BattleDataManager.instance.dropItemParent.transform;
        transform.position = new Vector3(0, 10);

        DOTween.Kill(transform);
        
        gameObject.SetActive(false);
    }
}
