using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; //�ִ� �ӷ� ���� 
    public float jumpPower;
    public GameManager gameManager;
    public AudioClip Item_Frag;//����ź ������ ���� Ŭ��
    public AudioClip Item_gun;//RPM(Round Per Min) ���� ������(�Ѿ� �� ������ ��� ����) ���� Ŭ��
    public AudioClip Bronze;//����ź ������ ���� Ŭ��
    public AudioClip Silver;//����ź ������ ���� Ŭ��
    public AudioClip Gold;//����ź ������ ���� Ŭ��

    Rigidbody2D rigid; //�����̵��� ���� ���� ���� 
    SpriteRenderer spriteRenderer; //������ȯ�� ���� ���� 
    public Animator animator; //�ִϸ����� ������ ���� ���� 
    CapsuleCollider2D capsuleCollider; 
    BoxCollider2D boxCollider;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        rigid = GetComponent<Rigidbody2D>(); //���� �ʱ�ȭ 
        spriteRenderer = GetComponent<SpriteRenderer>(); // �ʱ�ȭ 
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();     
    }
    void Update()
    {
        // ��ư���� ���� ���� ���� �ܹ����� Ű���� �Է��� FixedUpdate���� Update�� ���°� Ű���� �Է��� ������ Ȯ���� ������
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("isJumping") && !animator.GetBool("isCrouch"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        //Stop speed 
        if (Input.GetButtonUp("Horizontal"))
        { // ��ư���� ���� ���� ��� 
            // normalized : ���� ũ�⸦ 1�� ���� ���� (�������� : ũ�Ⱑ 1�� ����)
            // ���ʹ� ����� ũ�⸦ ���ÿ� �����µ� ũ��(- : �� , + : ��)�� �����ϱ� ���Ͽ� ��������(1,-1)�� ������ �˼� �ֵ��� �������͸� ���� 
            rigid.velocity = new Vector2(0.5f * rigid.velocity.normalized.x, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        
        // �ѱ�
        if (Input.GetKey(KeyCode.RightArrow))
            {
            gameObject.GetComponent<playerattackR>().enabled = true;
            gameObject.GetComponent<playerattackL>().enabled = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<playerattackL>().enabled = true;
            gameObject.GetComponent<playerattackR>().enabled = false;
        }

        //Animation control
        if (Mathf.Abs(rigid.velocity.x) < 0.2) //�ӵ��� 0 == ���� 
            animator.SetBool("isWalking", false); //isWalking ���� : false 
        else// �̵��� 
            animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //running
        //isRunning ���� : false 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
            maxSpeed = 2.5f;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetBool("isCrouch", true); //isRunning ���� : false 
                maxSpeed = 0;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                if (Input.GetKey(KeyCode.LeftArrow))            
                    maxSpeed = 0;                    
                if (Input.GetKey(KeyCode.RightArrow))                        
                    maxSpeed = 0;
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            maxSpeed = 1.5f;
        }
        //crouch
        // ������ ���ִ� �浹 ȿ�� ���� �� ������ �浹ȿ�� + �ӵ� 0
        // ���� �ݴ�� + �ӵ� ���󺹱�
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isCrouch", true); //isRunning ���� : false 
            maxSpeed = 0;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            animator.SetBool("isCrouch", false);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)  //���������� �̵� (+) , �ִ� �ӷ��� ������ 
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); //�ش� ������Ʈ�� �ӷ��� maxSpeed 

        else if (rigid.velocity.x < maxSpeed * (-1)) // �������� �̵� (-) 
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); //y���� ������ �����̹Ƿ� 0���� ������ �θ� �ȵ� 

        //Landing Paltform
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); //���� ��(����״� ���ӻ󿡼������� ���� ) ������ġ, ���� ����, ���� �� 

        
        //���� ������ġ, ���� ���� , 1:distance , ( ���� ���� ������Ʈ�� Ư�� ���̾�� ���� ������� �� ��� ) // RaycastHit2D : Ray�� ���� ������Ʈ Ŭ���� 

        //rayHit�� ������ �´��� ó�� ���� ������Ʈ�� �������� ����(?) 
        if (rigid.velocity.y < 0)
        { // �پ�ö��ٰ� �Ʒ��� ������ ���� ���� �� 
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 0.65f, LayerMask.GetMask("platform"));
            RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position, Vector3.down, 0.65f, LayerMask.GetMask("platform_item"));
            if (rayHit.collider != null)
            { //���� ���� ������Ʈ�� ������  -> ���������� collider�� ������������ (�÷���-�ٴ�)
                if (rayHit.distance < 0.5f)
                {
                    animator.SetBool("isJumping", false);
                } //�Ÿ��� 0.5���� �۾����� ����               
            }
            if (rayHit.collider != null)
            {//���� ���� ������Ʈ�� ������  -> ���������� collider�� ������������ (�÷���-������)
                if (rayHit2.distance < 0.5f)
                {
                    Debug.Log("down");
                    animator.SetBool("isJumping", false);
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemymove")
        {//�� ���� �ǰ�(�����浹/�����̴� ����)
            Debug.Log("HIT drone");
            if (transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
                OnDamaged(collision.transform.position);

            }
        }
        if (collision.gameObject.tag == "enemymove2")
        {//�� ���� �ǰ�(�����浹/�����̴� ����)
            Debug.Log("HIT dino");
            if (transform.position.y > collision.transform.position.y)
            {
                OnAttack4(collision.transform);
                OnDamaged(collision.transform.position);
            }
        }
        if (collision.gameObject.tag == "enemystay")
        {//�� ���� �ǰ�(�����浹/�ȿ����̴� ����)
            Debug.Log("HIT turret");   
            if (transform.position.y > collision.transform.position.y)
            { 
              OnAttack2(collision.transform);
              OnDamaged(collision.transform.position);
            }  
        }
       
        if (collision.gameObject.tag == "spike")
        {//�� ���� �ǰ�(�����浹/�ȿ����̴� ����)
            Debug.Log("HIT object");
            if (transform.position.y > collision.transform.position.y)
            {
                OnAttack3(collision.transform);
                OnDamaged(collision.transform.position);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        playerattackR AttackR = GetComponent<playerattackR>();
        playerattackL AttackL = GetComponent<playerattackL>();
        if (collision.gameObject.tag == "Item")
        { //point
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBronze)
            {
                gameManager.StagePoint += 50;
                SoundMgr.instance.SFXPlay5("Bronze", Bronze);
            }
            else if (isSilver)
            {
                gameManager.StagePoint += 100;
                SoundMgr.instance.SFXPlay6("Silver", Silver);
            }
            else if (isGold)
            {
                gameManager.StagePoint += 200;
                SoundMgr.instance.SFXPlay7("Gold", Gold);
            }

            //item detect
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Finish")
        {//Next Stage
            boxCollider.enabled = false;
            gameManager.NextStage();
        }

        if(collision.gameObject.tag == "Item_gun")
        {
            bool isgun_buff = collision.gameObject.name.Contains("gun_buff");
            if (isgun_buff)
            {
                SoundMgr.instance.SFXPlay4("Item_gun", Item_gun);
                AttackR.cooltimeBullet = AttackR.cooltimeBullet - 0.1f;
            }
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Item_Frag")
        {
            bool isFrag = collision.gameObject.name.Contains("FragRefill");
            if (isFrag)
            {
                SoundMgr.instance.SFXPlay3("Item_Frag", Item_Frag);
                AttackR.FragCount = AttackR.FragCount + 1;
            }
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Item_Jump")
        {
            bool isjumpboost = collision.gameObject.name.Contains("Jump_Potion");
            if (isjumpboost)
            {
                SoundMgr.instance.SFXPlay7("Gold", Gold);
                jumpPower = jumpPower + 3;
            }
            collision.gameObject.SetActive(false);
        }
    }
    public void OnAttack(Transform enemy) //�����̴� ������ ���� ������� ���� ����
    {
  
        //Reaction Force(���� Ƣ��)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);

        //Enemy Die(�� ���)
        EnemyMove emenyMove = enemy.GetComponent<EnemyMove>();
        emenyMove.OnDamaged();
    }

    public void OnAttack2(Transform enemy)//�ȿ����̴� ������ ���� ������� ���� ����
    {
       
        //Reaction Force(���� Ƣ��)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
        //Enemy Die(�� ���)
        EnemyStay emenyStay = enemy.GetComponent<EnemyStay>();
        emenyStay.OnDamaged();
    }
    public void OnAttack3(Transform enemy)//�ȿ����̴� ������ ���� ������� ���� ����
    {
        //Reaction Force(���� Ƣ��)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
    }
    public void OnAttack4(Transform enemy) //�����̴� ������ ���� ������� ���� ����
    {

        //Reaction Force(���� Ƣ��)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);

        //Enemy Die(�� ���)
        EnemyMove1 emenyMove = enemy.GetComponent<EnemyMove1>();
        emenyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
        //Change layer(����Ȱ��ȭ)
        gameObject.layer = 11;      

        //����(����)��
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //health down(�ǰ�)
        gameManager.HealthDown();

        //�ǰ� �� ���ư�
        float dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*1, ForceMode2D.Impulse);

        //�ǰݾִϸ��̼�
        animator.SetTrigger("doDamaged");
        Invoke("OffDamaged",2);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        capsuleCollider.enabled = false;
        boxCollider.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
    }
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}