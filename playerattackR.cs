using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerattackR : MonoBehaviour
{
    public GameObject bulletR;
    public GameObject FragR;
    public Transform pos;//��ġ ����
    public Text UIFrag;//����ź ���� üũ ������Ʈ

    public AudioClip clipB; // ���� ����(�Ѿ�)
    public AudioClip clipF; // ���� ����(��ô����)

    public float cooltimeBullet; //�Ѿ� ��Ÿ��/�Ѿ� �ӵ�(Round Per Second)
    public int FragCount; // ��ź ��������
    private float curtime; //�Ѿ� �ӵ�(Round Per Second)

    void Update()
    {
        PlayerMove player = GetComponent<PlayerMove>();
        UIFrag.text = FragCount.ToString();//����ź ���� üũ ������Ʈ

        if (curtime <= 0){//ZŰ ������ ������ ���
            if (Input.GetKey(KeyCode.S))
                {
                SoundMgr.instance.SFXPlay2("Bullet", clipB);
                player.animator.SetBool("isShooting", true);
                Instantiate(bulletR, pos.position, transform.rotation);
                curtime = cooltimeBullet;

            }//�̿ܿ��� ��������� ����

            else if (Input.GetKeyUp(KeyCode.S))
            {
                player.animator.SetBool("isShooting", false);
            }
            else
                player.animator.SetBool("isShooting", false);

            if (Input.GetKey(KeyCode.DownArrow))
            {
                pos.transform.localPosition = new Vector3(0.207f, -0.134f, 0);                   
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
                pos.transform.localPosition = new Vector3(0.207f, 0.067f, 0);
            else
                pos.transform.localPosition = new Vector3(0.207f, 0.067f, 0);

        }
        if (curtime <= 0)
        {//XŰ ������ ��ź
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (FragCount > 0)
                {
                    Instantiate(FragR, pos.position, transform.rotation);
                    SoundMgr.instance.SFXPlay("FragPin", clipF);   // ���� ����(��ź)
                    FragCount = FragCount - 1;
                }                             
            }
        }
        curtime -= Time.deltaTime;
    }
}
