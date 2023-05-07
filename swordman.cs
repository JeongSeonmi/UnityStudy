using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordman : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*Jump인데요*/
        if (Input.GetButtonDown("Jump") && !animator.GetBool("jumping")){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
        }
        

        /*Stop Speed*/
        if (Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
        }

        /*Direction Sprite(방향 전환)*/
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        /*Animation(달릴 때, 애니메이션)*/
        if (Mathf.Abs(rigid.velocity.x) < 0.3)   //Mathf : 수학 관련 함수를 제공하는 클래스 
            animator.SetBool("moving", false);
        else
            animator.SetBool("moving", true);
        
    }
    void FixedUpdate()
    {
        /*Move Speed*/
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        /*Max Speed*/
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // Right Max Speed
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // Left Max Speed

        /*Landing Platform*/
        if(rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); //DrawRay() : 에디터 상에서만 Ray를 그려주는 함수, 색깔은 그냥 그린
            //빔을 쏴서 맞는걸 확인, 정확한 판단을 위해서 초록바 길이 설정
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position*0.5f, Vector3.down, 1, LayerMask.GetMask("Platform")); //GetMask(): 레이어 이름에 해당하는 정수값을 리턴하는 함수
            if(rayHit.collider != null){
                if(rayHit.distance < 0.6f) animator.SetBool("jumping", false);
            }
        }
    }
}
