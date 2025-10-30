using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : SingletonManager<PlayerInputManager>
{
    private PlayerControll playerControll;
    private Player player;
    private GameManager gameManager;

    private InputAction jumpAction;
    private InputAction slideAction;
    private InputAction pauseAction;

    public Action OnJump;
    public Action OnSlideStart;
    public Action OnSlideEnd;
    public Action OnPause;

    protected override void Awake()
    {
        base.Awake();
        playerControll = new PlayerControll();
        player = FindObjectOfType<Player>();
        gameManager = GameManager.Instance;
        InitInputAction();
        EnableInput();
    }
    private void OnEnable()
    {
        jumpAction.performed += OnJumpPerformed;
        slideAction.performed += OnSlidePerformed;
        slideAction.canceled += OnSlideCanceled;
        pauseAction.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        jumpAction.performed -= OnJumpPerformed;
        slideAction.performed -= OnSlidePerformed;
        slideAction.canceled -= OnSlideCanceled;
        pauseAction.performed -= OnPausePerformed;
        DisableInput();
    }

    private void InitInputAction()
    {
        jumpAction = playerControll.Player.Jump;
        slideAction = playerControll.Player.Slide;
        pauseAction = playerControll.Player.Pause;
    }

    private bool CheckGameManager()
    {
        return (gameManager.IsStart && !gameManager.IsPause); 
    }

    private bool CheckNull()
    {
        return (player == null || gameManager == null);
    }
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (CheckNull()) return;
        if (player.IsSlide || !CheckGameManager()) return;

        OnJump?.Invoke();
    }
    private void OnSlidePerformed(InputAction.CallbackContext ctx)
    {
        if (CheckNull()) return;
        if (player.IsJump || !CheckGameManager()) return;

        OnSlideStart?.Invoke();
    }
    private void OnSlideCanceled(InputAction.CallbackContext ctx)
    {
        if (CheckNull()) return;
        if (player.IsJump || !gameManager.IsStart) return;
        OnSlideEnd?.Invoke();
    }
    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        gameManager.ClickPause();
        OnPause?.Invoke();
    }


    public void EnableInput()
    {
        jumpAction.Enable();
        slideAction.Enable();
        pauseAction.Enable();
    }

    public void DisableInput()
    {
        jumpAction.Disable();
        slideAction.Disable();
        pauseAction.Disable();
    }
}
