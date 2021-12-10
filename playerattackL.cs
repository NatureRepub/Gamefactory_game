using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerattackL : MonoBehaviour
{
    public GameObject bulletL;
    public Transform pos; // ��ġ����
    public GameObject FragL;

    private float curtime;
    public AudioClip clipB;
    public AudioClip clipF;// ���� ����(�Ѿ�)

    Animator animator;

    void Update()
    {
        PlayerMove player = GetComponent<PlayerMove>();
        playerattackR playerattack = GetComponent<playerattackR>();
        playerattack.UIFrag.text = playerattack.FragCount.ToString();

        if (curtime <= 0)
        {//ZŰ ������ ���
            if (Input.GetKey(KeyCode.S))
            {
                // ���� ����(�Ѿ�)
                SoundMgr.instance.SFXPlay2("Bullet", clipB);
                player.animator.SetBool("isShooting", true);
                Instantiate(bulletL, pos.position, transform.rotation);
                curtime = playerattack.cooltimeBullet;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                player.animator.SetBool("isShooting", false);
            }
            else
                player.animator.SetBool("isShooting", false);

            if (Input.GetKey(KeyCode.DownArrow))
            {
                pos.transform.localPosition = new Vector3(-0.207f, -0.134f, 0);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
                pos.transform.localPosition = new Vector3(-0.207f, 0.067f, 0);
            else
                pos.transform.localPosition = new Vector3(-0.207f, 0.067f, 0);

        }
        if (curtime <= 0)
        {//XŰ ������ ��ź
            if (Input.GetKeyDown(KeyCode.D))
            {    
                if (playerattack.FragCount > 0) { 
                Instantiate(FragL, pos.position, transform.rotation);
                    SoundMgr.instance.SFXPlay("FragPin", clipF);
                    playerattack.FragCount = playerattack.FragCount - 1;
                }
            }
        }
        curtime -= Time.deltaTime;
    }
}
