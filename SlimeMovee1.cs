using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovee1 : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    void Awake()
    {
        
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 3); //주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove*2, rigid.velocity.y);  //velocity는 rigid꺼인데 웬만한 물리적인건 Rigidbody에 있음

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.8f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform")); //GetMask(): 레이어 이름에 해당하는 정수값을 리턴하는 함수
        if(rayHit.collider == null){
            Turn(); //방향이 반대가 되도록 하는 함수
        }
    }

    void Think()  //재귀함수
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2); //최대는 포함X)

        //Sprite Animation
        animator.SetInteger("WalkSpeed", nextMove);

        //Flip Sprite
        if(nextMove !=0)
            spriteRenderer.flipX = nextMove == 1; 
            
        //Recursive    
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn(){
        nextMove *= (-1);
        spriteRenderer.flipX = nextMove == 1; 
        CancelInvoke();
        Invoke("Think", 3);
    }
}
