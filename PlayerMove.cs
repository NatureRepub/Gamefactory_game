using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; //최대 속력 변수 
    public float jumpPower;
    public GameManager gameManager;
    public AudioClip Item_Frag;//수류탄 아이템 사운드 클립
    public AudioClip Item_gun;//RPM(Round Per Min) 증가 아이템(총알 더 빠르게 쏠수 있음) 사운드 클립
    public AudioClip Bronze;//수류탄 아이템 사운드 클립
    public AudioClip Silver;//수류탄 아이템 사운드 클립
    public AudioClip Gold;//수류탄 아이템 사운드 클립

    Rigidbody2D rigid; //물리이동을 위한 변수 선언 
    SpriteRenderer spriteRenderer; //방향전환을 위한 변수 
    public Animator animator; //애니메이터 조작을 위한 변수 
    CapsuleCollider2D capsuleCollider; 
    BoxCollider2D boxCollider;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        rigid = GetComponent<Rigidbody2D>(); //변수 초기화 
        spriteRenderer = GetComponent<SpriteRenderer>(); // 초기화 
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();     
    }
    void Update()
    {
        // 버튼에서 손을 떄는 등의 단발적인 키보드 입력은 FixedUpdate보다 Update에 쓰는게 키보드 입력이 누락될 확률이 낮아짐
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("isJumping") && !animator.GetBool("isCrouch"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        //Stop speed 
        if (Input.GetButtonUp("Horizontal"))
        { // 버튼에서 손을 때는 경우 
            // normalized : 벡터 크기를 1로 만든 상태 (단위벡터 : 크기가 1인 벡터)
            // 벡터는 방향과 크기를 동시에 가지는데 크기(- : 왼 , + : 오)를 구별하기 위하여 단위벡터(1,-1)로 방향을 알수 있도록 단위벡터를 곱함 
            rigid.velocity = new Vector2(0.5f * rigid.velocity.normalized.x, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        
        // 총구
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
        if (Mathf.Abs(rigid.velocity.x) < 0.2) //속도가 0 == 멈춤 
            animator.SetBool("isWalking", false); //isWalking 변수 : false 
        else// 이동중 
            animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //running
        //isRunning 변수 : false 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
            maxSpeed = 2.5f;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetBool("isCrouch", true); //isRunning 변수 : false 
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
        // 누르면 서있는 충돌 효과 제거 및 앉을때 충돌효과 + 속도 0
        // 떼면 반대로 + 속도 정상복귀
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isCrouch", true); //isRunning 변수 : false 
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

        if (rigid.velocity.x > maxSpeed)  //오른쪽으로 이동 (+) , 최대 속력을 넘으면 
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); //해당 오브젝트의 속력은 maxSpeed 

        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽으로 이동 (-) 
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); //y값은 점프의 영향이므로 0으로 제한을 두면 안됨 

        //Landing Paltform
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); //빔을 쏨(디버그는 게임상에서보이지 않음 ) 시작위치, 어디로 쏠지, 빔의 색 

        
        //빔의 시작위치, 빔의 방향 , 1:distance , ( 빔에 맞은 오브젝트를 특정 레이어로 한정 지어야할 때 사용 ) // RaycastHit2D : Ray에 닿은 오브젝트 클래스 

        //rayHit는 여러개 맞더라도 처음 맞은 오브젝트의 정보만을 저장(?) 
        if (rigid.velocity.y < 0)
        { // 뛰어올랐다가 아래로 떨어질 때만 빔을 쏨 
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 0.65f, LayerMask.GetMask("platform"));
            RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position, Vector3.down, 0.65f, LayerMask.GetMask("platform_item"));
            if (rayHit.collider != null)
            { //빔을 맞은 오브젝트가 있을때  -> 맞지않으면 collider도 생성되지않음 (플랫폼-바닥)
                if (rayHit.distance < 0.5f)
                {
                    animator.SetBool("isJumping", false);
                } //거리가 0.5보다 작아지면 변경               
            }
            if (rayHit.collider != null)
            {//빔을 맞은 오브젝트가 있을때  -> 맞지않으면 collider도 생성되지않음 (플랫폼-아이템)
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
        {//적 한테 피격(직접충돌/움직이는 적들)
            Debug.Log("HIT drone");
            if (transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
                OnDamaged(collision.transform.position);

            }
        }
        if (collision.gameObject.tag == "enemymove2")
        {//적 한테 피격(직접충돌/움직이는 적들)
            Debug.Log("HIT dino");
            if (transform.position.y > collision.transform.position.y)
            {
                OnAttack4(collision.transform);
                OnDamaged(collision.transform.position);
            }
        }
        if (collision.gameObject.tag == "enemystay")
        {//적 한테 피격(직접충돌/안움직이는 적들)
            Debug.Log("HIT turret");   
            if (transform.position.y > collision.transform.position.y)
            { 
              OnAttack2(collision.transform);
              OnDamaged(collision.transform.position);
            }  
        }
       
        if (collision.gameObject.tag == "spike")
        {//적 한테 피격(직접충돌/안움직이는 적들)
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
    public void OnAttack(Transform enemy) //움직이는 적한테 직접 닿았을때 적의 판정
    {
  
        //Reaction Force(위로 튀기)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);

        //Enemy Die(적 사망)
        EnemyMove emenyMove = enemy.GetComponent<EnemyMove>();
        emenyMove.OnDamaged();
    }

    public void OnAttack2(Transform enemy)//안움직이는 적한테 직접 닿았을때 적의 판정
    {
       
        //Reaction Force(위로 튀기)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
        //Enemy Die(적 사망)
        EnemyStay emenyStay = enemy.GetComponent<EnemyStay>();
        emenyStay.OnDamaged();
    }
    public void OnAttack3(Transform enemy)//안움직이는 적한테 직접 닿았을때 적의 판정
    {
        //Reaction Force(위로 튀기)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
    }
    public void OnAttack4(Transform enemy) //움직이는 적한테 직접 닿았을때 적의 판정
    {

        //Reaction Force(위로 튀기)
        rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);

        //Enemy Die(적 사망)
        EnemyMove1 emenyMove = enemy.GetComponent<EnemyMove1>();
        emenyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
        //Change layer(무적활성화)
        gameObject.layer = 11;      

        //알파(투명)값
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //health down(피격)
        gameManager.HealthDown();

        //피격 후 날아감
        float dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*1, ForceMode2D.Impulse);

        //피격애니메이션
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