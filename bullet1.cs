using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet1 : MonoBehaviour
{
    public float speed;
    public float distance;
    
    public LayerMask isLayer;

    Animator animator;
    void Start()
    {   //2�ʵ� �Ѿ� ����
        Invoke("DestroyBullet", 1);
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "enemymove")
            {   //�����̴� �� �Ѿ� �ǰ�  
               
                Debug.Log("bullet drone hit");
                EnemyMove enemymove = ray.collider.GetComponent<EnemyMove>();
                enemymove.OnDamaged();

                //�Ѿ� ����Ʈ
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);              
            }

            if (ray.collider.tag == "enemymove2")
            {   //�����̴� �� �Ѿ� �ǰ�  

                Debug.Log("bullet dino hit");
                EnemyMove1 enemymove = ray.collider.GetComponent<EnemyMove1>();
                enemymove.OnDamaged();

                //�Ѿ� ����Ʈ
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }

            if (ray.collider.tag == "enemystay")
            {      //�ȿ����̴� �� �Ѿ� �ǰ�

                Debug.Log("bullet turret hit");
                EnemyStay enemstay = ray.collider.GetComponent<EnemyStay>();
                enemstay.OnDamaged();

                //�Ѿ� ����Ʈ
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }
            if (ray.collider.tag == "platform_item")
            {      //�÷��� ����Ʈ
                Debug.Log("platform_item hit");

                //�Ѿ� ����Ʈ
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }

            if (ray.collider.tag == "platform")
            {      //�÷��� ����Ʈ
                Debug.Log("platform hit");

                //�Ѿ� ����Ʈ
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }
            Invoke("DestroyBullet",0.01f);

        }
        //�Ѿ� ���� ����
        if (transform.rotation.y == 0)
            transform.Translate(transform.right * speed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
    }
    void DestroyBullet()
    {//�Ѿ� ���� Ȱ��ȭ
        Destroy(gameObject);
    }
  
}
