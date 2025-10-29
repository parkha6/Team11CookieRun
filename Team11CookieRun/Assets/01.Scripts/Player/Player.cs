using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //컴포넌트 참조
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Animator playerAnim;

    //캐릭터 능력치
    [SerializeField] float speed;
    [SerializeField] float jumpPower;

    //캐릭터 점수
    [SerializeField] float score;

    //일시정지
    private Vector2 saveVelocity;

    //플레이어 상태
    private bool isGround;
    private bool isSlide;
    private bool isJump;
    private bool isDoubleJump;
    private bool isDead;

    //프로퍼티
    public float Speed { get { return speed; } set { speed = value; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    public float Score { get { return score; } set { score = value; } }




    private void Start()
    {
        WaitPlayer(true);
    }

    private void FixedUpdate()
    {
        MoveFoward();
    }

    /// <summary>
    /// 초반 대기 함수(매개변수와 반대로 작동)
    /// </summary>
    public void WaitPlayer(bool isWait)
    {
        playerInput.enabled = !isWait;        
    }


    /// <summary>
    /// 앞으로 가는 함수
    /// </summary>
    public void MoveFoward()
    {
        Vector2 velocity = playerRb.velocity;
        velocity.x = speed;
        playerRb.velocity = velocity;
    }
    /// <summary>
    /// 플레이어 일시정지
    /// </summary>
    public void PausePlayer()
    {
        //정지 기능 게임매니저에서 가져와야함
        if(playerRb != null)
        {
            saveVelocity = playerRb.velocity;
            Destroy(playerRb);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Item"))
        {
            //아이템 구현 시 아이템 역할 함수
        }
        else if(collision.gameObject.CompareTag("Coin"))
        {
            Score++; //일단 해놨는데 코인에서 가지고 와야함
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if (isGround == false)
                isGround = true;
        }
    }

    #region InputSystem
    private void OnJump()
    {
        Debug.Log("점프");
        if (isDead || isSlide || isDoubleJump)
            return;
        if (isGround)
            isGround = false;
        Vector2 velocity = playerRb.velocity;
        velocity.y += jumpPower;
        playerRb.velocity = velocity;
    }

    private void OnPause()
    {
        Debug.Log("정지");
        PausePlayer();
    }

    private void OnSlide()
    {
        if (!isGround || isDead)
            return;
        Debug.Log("슬라이드");
    }
    #endregion

}
