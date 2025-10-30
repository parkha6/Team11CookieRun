using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //컴포넌트 참조
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] Animator playerAnim;
    private PlayerInputManager playerInputManager;

    //캐릭터 능력치
    [SerializeField] float hp;
    [SerializeField] float speed;
    [SerializeField] float jumpPower;

    //이동을 위한 중력
    private float verticalVelocity = 0f;
    [SerializeField] private float minVerticalVelocity;
    [SerializeField] float gravity;

    //캐릭터 점수
    [SerializeField] float score;

    //일시정지
    private Vector2 saveVelocity;

    //플레이어 상태조건  
    private bool isGround;
    private bool isRun;
    private bool isSlide;
    private bool isJump;
    private bool isDoubleJump;
    private bool isDead;

    //플레이어 상태머신
    private IPlayerState curState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerSlideState slideState = new PlayerSlideState();

    #region Property
    public Animator PlayerAnim { get { return playerAnim; } set { playerAnim = value; } }
    public float Hp { get { return hp; } set { hp = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    public float Score { get { return score; } set { score = value; } }
    public bool IsGround { get { return isGround; } set { isGround = value; } }
    public bool IsRun { get { return isRun; } set { isRun = value; } }
    public bool IsSlide { get { return isSlide; } set { isSlide = value; } }
    public bool IsJump { get { return isJump; } set { isJump = value; } }
    #endregion




    private void Start()
    {
        playerInputManager = PlayerInputManager.Instance;

        playerInputManager.OnJump += () => IsJump = true;
        playerInputManager.OnSlideStart += () => IsSlide = true;
        playerInputManager.OnSlideEnd += () => IsSlide = false;

        ChangeState(idleState);
    }

    private void FixedUpdate()
    {
        if (curState != null)
            curState.Update(this);
    }


    /// <summary>
    /// 앞으로 가는 함수
    /// </summary>
    public void MoveFoward()
    {
        Vector3 moveVec = new Vector3(Speed, verticalVelocity, 0f);
        transform.position += moveVec * Time.deltaTime;
    }

    /// <summary>
    /// 중력작용
    /// </summary>
    public void OnGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity <= minVerticalVelocity)
            verticalVelocity = minVerticalVelocity;
    }

    /// <summary>
    /// 플레이어 상태 변경
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IPlayerState newState)
    {
        if ((curState != null))
        {
            curState.Exit(this);            
        }
        curState = newState;
        curState.Enter(this);
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
            {
                isGround = true;
            }
            verticalVelocity = 0f;
        }
    }

    public void Jump()
    {
        verticalVelocity = JumpPower;
    }

    #region ItemInteraction

    #endregion
}
