using System;
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
    private StartCanvasManager gameCanvasManager;
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

    //점프를 위한 중력
    private float verticalVelocity = 0f;
    [SerializeField] private float minVerticalVelocity;
    [SerializeField] float jumpGravity;

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
    private bool isDie;

    //플레이어 땅 중력 판단
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundGravity;
    private readonly WaitForSeconds fallWait = new WaitForSeconds(2f);


    //플레이어 상태머신
    private IPlayerState curState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerSlideState slideState = new PlayerSlideState();
    public PlayerDeathState deathState = new PlayerDeathState();

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
    public float JumpGravity { get { return jumpGravity; } }
    public float Score { get { return score; } set { score = value; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsGround { get { return isGround; } set { isGround = value; } }
    public bool IsRun { get { return isRun; } set { isRun = value; } }
    public bool IsSlide { get { return isSlide; } set { isSlide = value; } }
    public bool IsJump { get { return isJump; } set { isJump = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    public Vector2 BoxSize { get { return boxSize; } }
    public float RayDistance { get { return rayDistance; } }
    public LayerMask GroundLayer { get { return groundLayer; } }
    public float GroundGravity { get { return groundGravity; } }
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

    public void InitCanvasManager(StartCanvasManager scm)
    {
        gameCanvasManager = scm;
    }

    private void Update()
    {
        if (curState != null)
            curState.UpdateState(this);      
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
    public void OnGravity(float gravity)
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity <= minVerticalVelocity)
            verticalVelocity = minVerticalVelocity;
    }

    public void FallPlayer()
    {
        playerAnim.speed = 0f;
        playerCollider.isTrigger = true;
        playerInputManager.DisableInput();
        Speed = 0f;
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        yield return fallWait;
        Destroy(this.gameObject);
    }


    /// <summary>
    /// 플레이어 일시정지
    /// </summary>
    public void PausePlayer()
    {
        if(gameManager.IsPause)
        {
            playerAnim.speed = 0f;
            gameCanvasManager.OnPauseUi();
        }
        else
        {
            playerAnim.speed = 1f;
            gameCanvasManager.OffPauseUi();
        }
    }
    /// <summary>
    /// 플레이어 상태 변경
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IPlayerState newState)
    {
        if ((curState != null))
        {
            curState.ExitState(this);            
        }
        curState = newState;
        curState.EnterState(this);
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
        if (Hp <= 0)
        {
            ChangeState(deathState);
            return;
        }
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

    IEnumerator PlayerInvincibility(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    public void ApplySlow(float value, float duration)
    {
        StartCoroutine(SlowPlayer(value, duration));
    }

    IEnumerator SlowPlayer(float value, float duration)
    {
        Speed -= value;
        yield return new WaitForSeconds(duration);
        Speed += value;
    }
    #endregion
}
