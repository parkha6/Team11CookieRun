using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //컴포넌트 참조
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] Animator playerAnim;
    [SerializeField] CapsuleCollider2D playerCollider;
    private PlayerInputManager playerInputManager;
    private GameManager gameManager;
    [SerializeField] SpriteRenderer playerSpriteRenderer;

    //캐릭터 능력치
    [SerializeField] float hp;
    [SerializeField] float maxHp;
    [SerializeField] float speed;
    [SerializeField] float jumpPower;

    //캐릭터 무적관련
    [SerializeField] bool isInvincible;
    [SerializeField] int blinkNum;
    private readonly WaitForSeconds blinkDelay = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds invincibleDelay = new WaitForSeconds(0.5f);

    //캐릭터 피격판정
    private readonly WaitForSeconds hitEffectDelay = new WaitForSeconds(0.4f);

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
    public float Hp { get { return hp; }
        set
        { 
            hp = value;
            if (hp >= maxHp)
                hp = maxHp;
            else if(hp <= 0f)
                hp = 0f;
        }
    }
    public float Speed { get { return speed; } set { speed = value; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    public float Score { get { return score; } set { score = value; } }
    public bool IsInvincible { get { return isInvincible; } }
    public bool IsGround { get { return isGround; } set { isGround = value; } }
    public bool IsRun { get { return isRun; } set { isRun = value; } }
    public bool IsSlide { get { return isSlide; } set { isSlide = value; } }
    public bool IsJump { get { return isJump; } set { isJump = value; } }
    #endregion




    private void Start()
    {
        playerInputManager = PlayerInputManager.Instance;
        gameManager = GameManager.Instance;

        playerInputManager.OnJump += () => IsJump = true;
        playerInputManager.OnSlideStart += () => IsSlide = true;
        playerInputManager.OnSlideEnd += () => IsSlide = false;
        playerInputManager.OnPause += PausePlayer;

        ChangeState(idleState);
    }

    private void Update()
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

    public void PausePlayer()
    {
        if(gameManager.IsPause)
            playerAnim.speed = 0f;
        else
            playerAnim.speed = 1f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if (IsGround == false)
            {
                IsGround = true;
            }
            verticalVelocity = 0f;
        }
    }

    public void Jump()
    {
        IsGround = false;
        verticalVelocity = JumpPower;
    }
    #region ObstacleInteraction
    public void TakeDamage(float damage)
    {
        Hp -= damage;
        StartCoroutine(HitEffect());
        StartCoroutine(HitDelay());
        StartCoroutine(BlinkCharacter(blinkNum));
    }

    IEnumerator HitEffect()
    {
        Color playerColor = playerSpriteRenderer.color;
        playerSpriteRenderer.color = Color.red;
        yield return hitEffectDelay;
        playerSpriteRenderer.color = playerColor;
    }

    IEnumerator HitDelay()
    {
        isInvincible = true;
        yield return invincibleDelay;
        isInvincible = false;
    }

    IEnumerator BlinkCharacter(int countNum)
    {
        int count = 0;

        while(count < countNum)
        {
            SetAlpha(0.5f);
            yield return blinkDelay;
            SetAlpha(1f);
            yield return blinkDelay;
            count++;
        }
        SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
        Color playerColor = playerSpriteRenderer.color;
        playerColor.a = alpha;
        playerSpriteRenderer.color = playerColor;
    }
    #endregion


    #region ItemInteraction
    public void AddScore(int value)
    {
        Score += value;
    }

    public void HealPercent(float value)
    {
        Hp += value;
    }

    public void ActivateInvincibility(float value)
    {
        StartCoroutine(PlayerInvincibility(value));
    }

    IEnumerator PlayerInvincibility(float value)
    {
        isInvincible = true;
        yield return new WaitForSeconds(value);
        isInvincible = false;
    }

    #endregion
}
