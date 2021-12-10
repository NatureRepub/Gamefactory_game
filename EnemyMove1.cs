using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove1 : MonoBehaviour
{
    public int nextMove;

    SpriteRenderer rend;
    Rigidbody2D rigid;
    Animator animator;
    CapsuleCollider2D capsuleCollider;

    void Awake()
    {
        Application.targetFrameRate = 30;
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think", 3); //3초뒤 Think 호출
    }
    void FixedUpdate()
    {
        //적 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //지형체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f , rigid.position.y);
        
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));//빔을 쏨(디버그는 게임상에서보이지 않음 ) 시작위치, 어디로 쏠지, 빔의 색 
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));
        if (rayHit.collider == null)
            { //낭떠러지 확인시 방향역으로
            turn();
             }
        }
    //재귀함수
    void Think() {
        //다음 활동 세팅(벽에 튕김)
        nextMove = Random.Range(-1, 2);
        //적의 머리 방향
        if (nextMove != 0)
            rend.flipX = nextMove == 1;
        //재귀함수(Recursive)
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", 3);
    }

    void turn()
    {
        nextMove *= -1;
        rend.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 3);
    }
    public void OnDamaged()
    {   
        capsuleCollider.enabled = false;
        animator.SetBool("ishit", true);
        //Destory
        Invoke("Deactive",0.5f);
    }
    void Deactive()
    {
        gameObject.SetActive(false);
    }
        
}
   

