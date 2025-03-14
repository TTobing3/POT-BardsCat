using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BulletBar : MonoBehaviour
{
    [SerializeField] GameObject[] bullets;

    public void SetMaxBullet()
    {
        foreach(GameObject i in bullets) i.SetActive(false);

        for(int i = 0; i<GameManager.instance.player.weapon.mana; i++)
        {
            bullets[i].gameObject.SetActive(true);
        }

        SetBullet();
    }

    public void SetBullet()
    {
        for (int i = 0; i < GameManager.instance.player.weapon.mana; i++)
        {
            if (i < GameManager.instance.player.mp) 
                bullets[i].transform.GetChild(1).gameObject.SetActive(true);
            else
                bullets[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
