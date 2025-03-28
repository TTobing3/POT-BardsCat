using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScrolling : MonoBehaviour
{
    //CM Target이 가운데에 오면 좌우에 생성

    public Transform cameraTransform, skyBox;
    public Transform[] curBackgrounds, preBackgrounds;

    //
    void Update()
    {
        skyBox.position = new Vector3(cameraTransform.position.x, - 10, 0);

        if(GameManager.instance.player.playerMove.isMove)
        {

            for (int i = 0; i < 5; i++)
            {
                if (Mathf.Abs(cameraTransform.position.x - curBackgrounds[i].position.x) > 38)
                {
                    var tmp = curBackgrounds[i];
                    curBackgrounds[i] = preBackgrounds[i];
                    preBackgrounds[i] = tmp;
                }
                if (cameraTransform.position.x < curBackgrounds[i].position.x)
                {
                    preBackgrounds[i].position = new Vector3(curBackgrounds[i].position.x - 38, -10, 0);
                }
                else
                {
                    preBackgrounds[i].position = new Vector3(curBackgrounds[i].position.x + 38, -10, 0);
                }
                if (i != 4)
                {
                    var speed = ((4 - i) * 0.0015f);
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        curBackgrounds[i].position = curBackgrounds[i].position + new Vector3(speed, 0);
                        preBackgrounds[i].position = preBackgrounds[i].position + new Vector3(speed, 0);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        curBackgrounds[i].position = curBackgrounds[i].position - new Vector3(speed, 0);
                        preBackgrounds[i].position = preBackgrounds[i].position - new Vector3(speed, 0);
                    }
                }
            }
        }

    }
}
