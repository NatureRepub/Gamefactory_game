using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerattackR : MonoBehaviour
{
    public GameObject bulletR;
    public GameObject FragR;
    public Transform pos;//위치 참조
    public Text UIFrag;//수류탄 갯수 체크 업데이트

    public AudioClip clipB; // 사운드 제어(총알)
    public AudioClip clipF; // 사운드 제어(투척무기)

    public float cooltimeBullet; //총알 쿨타임/총알 속도(Round Per Second)
    public int FragCount; // 폭탄 갯수제어
    private float curtime; //총알 속도(Round Per Second)

    void Update()
    {
        PlayerMove player = GetComponent<PlayerMove>();
        UIFrag.text = FragCount.ToString();//수류탄 갯수 체크 업데이트

        if (curtime <= 0){//Z키 누르고 있으면 사격
            if (Input.GetKey(KeyCode.S))
                {
                SoundMgr.instance.SFXPlay2("Bullet", clipB);
                player.animator.SetBool("isShooting", true);
                Instantiate(bulletR, pos.position, transform.rotation);
                curtime = cooltimeBullet;

            }//이외에는 사격포지션 헤제

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
        {//X키 누르면 폭탄
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (FragCount > 0)
                {
                    Instantiate(FragR, pos.position, transform.rotation);
                    SoundMgr.instance.SFXPlay("FragPin", clipF);   // 사운드 제어(폭탄)
                    FragCount = FragCount - 1;
                }                             
            }
        }
        curtime -= Time.deltaTime;
    }
}
