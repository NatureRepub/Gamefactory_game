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
    {   //2ÃÊµÚ ÃÑ¾Ë »èÁ¦
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
            {   //¿òÁ÷ÀÌ´Â Àû ÃÑ¾Ë ÇÇ°Ý  
               
                Debug.Log("bullet drone hit");
                EnemyMove enemymove = ray.collider.GetComponent<EnemyMove>();
                enemymove.OnDamaged();

                //ÃÑ¾Ë ÀÓÆåÆ®
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);              
            }

            if (ray.collider.tag == "enemymove2")
            {   //¿òÁ÷ÀÌ´Â Àû ÃÑ¾Ë ÇÇ°Ý  

                Debug.Log("bullet dino hit");
                EnemyMove1 enemymove = ray.collider.GetComponent<EnemyMove1>();
                enemymove.OnDamaged();

                //ÃÑ¾Ë ÀÓÆåÆ®
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }

            if (ray.collider.tag == "enemystay")
            {      //¾È¿òÁ÷ÀÌ´Â Àû ÃÑ¾Ë ÇÇ°Ý

                Debug.Log("bullet turret hit");
                EnemyStay enemstay = ray.collider.GetComponent<EnemyStay>();
                enemstay.OnDamaged();

                //ÃÑ¾Ë ÀÓÆåÆ®
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }
            if (ray.collider.tag == "platform_item")
            {      //ÇÃ·§Æû ÀÓÆåÆ®
                Debug.Log("platform_item hit");

                //ÃÑ¾Ë ÀÓÆåÆ®
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }

            if (ray.collider.tag == "platform")
            {      //ÇÃ·§Æû ÀÓÆåÆ®
                Debug.Log("platform hit");

                //ÃÑ¾Ë ÀÓÆåÆ®
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("ishit", true);
            }
            Invoke("DestroyBullet",0.01f);

        }
        //ÃÑ¾Ë ¹æÇâ Á¦¾î
        if (transform.rotation.y == 0)
            transform.Translate(transform.right * speed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
    }
    void DestroyBullet()
    {//ÃÑ¾Ë »èÁ¦ È°¼ºÈ­
        Destroy(gameObject);
    }
  
}
