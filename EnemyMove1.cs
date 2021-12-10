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

        Invoke("Think", 3); //3�ʵ� Think ȣ��
    }
    void FixedUpdate()
    {
        //�� ������
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //����üũ
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f , rigid.position.y);
        
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));//���� ��(����״� ���ӻ󿡼������� ���� ) ������ġ, ���� ����, ���� �� 
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));
        if (rayHit.collider == null)
            { //�������� Ȯ�ν� ���⿪����
            turn();
             }
        }
    //����Լ�
    void Think() {
        //���� Ȱ�� ����(���� ƨ��)
        nextMove = Random.Range(-1, 2);
        //���� �Ӹ� ����
        if (nextMove != 0)
            rend.flipX = nextMove == 1;
        //����Լ�(Recursive)
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
   

