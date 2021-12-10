using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStay : MonoBehaviour
{
    public AudioClip TurretExp;

    SpriteRenderer rend;
    Rigidbody2D rigid;
    Animator animator;
    BoxCollider2D boxCollider;
    
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void FixedUpdate()
    {
        //적 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }

    public void OnDamaged()
    {
        boxCollider.enabled = false;
        animator.SetBool("ishit", true);
        SoundMgr.instance.SFXPlay9("TurretExplosive", TurretExp);

        //Destory
        Invoke("Deactive", 0.5f);
    }
    void Deactive()
    {
        gameObject.SetActive(false);
    }
        
}
   

