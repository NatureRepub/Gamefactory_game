using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    public AudioClip FragExp;

    Animator animator;
    Rigidbody2D rigid;

    void Start()
    {   //1ÃÊµÚ ÆøÅº »èÁ¦
        Invoke("DestroyExplosive", 1);
        SoundMgr.instance.SFXPlay10("FragExplosive", FragExp);
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>(); //º¯¼ö ÃÊ±âÈ­ 
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "platform")
            {
                Debug.Log("explosive hit");
                SoundMgr.instance.SFXPlay10("FragExplosive", FragExp);
                Invoke("DestroyExplosive", 0.5f); // ÆøÅº À¯Áö ½Ã°£
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;  //ÆøÅºÀ§Ä¡°íÁ¤
                animator.SetBool("isFragHit", true);
            }
                if (ray.collider.tag == "enemymove")
            {   //¿òÁ÷ÀÌ´Â Àû ÃÑ¾Ë ÇÇ°Ý+Á¡¼ö  
               
                Debug.Log("Explosive drone hit");
                SoundMgr.instance.SFXPlay10("FragExplosive", FragExp);
                EnemyMove enemymove = ray.collider.GetComponent<EnemyMove>();
                enemymove.OnDamaged();

                //ÃÑ¾Ë ÀÓÆåÆ®
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                animator.SetBool("isFragHit", true);              
            }

            if (ray.collider.tag == "enemystay")
            {      //¾È¿òÁ÷ÀÌ´Â Àû ÃÑ¾Ë ÇÇ°Ý+Á¡¼ö
                
                Debug.Log("Explosive turret hit");
                SoundMgr.instance.SFXPlay10("FragExplosive", FragExp);
                EnemyStay enemstay = ray.collider.GetComponent<EnemyStay>();
                enemstay.OnDamaged();

                //ÃÑ¾Ë ÀÓÆåÆ®
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                animator.SetBool("isFragHit", true);

            }
            Invoke("DestroyExplosive", 0.5f);
        }
        //ÆøÅº ¹æÇâ Á¦¾î
        if (transform.rotation.y == 0)
            transform.Translate(transform.right * speed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
    }
    void DestroyExplosive()
    {//ÃÑ¾Ë »èÁ¦ È°¼ºÈ­
        Destroy(gameObject);
    }
  
}
