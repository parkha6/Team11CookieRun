using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : SingletonManager<PlayerInputManager>
{
    private PlayerControll playerControll;

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


    private void OnJumpPerformed(InputAction.CallbackContext ctx) => OnJump?.Invoke();
    private void OnSlidePerformed(InputAction.CallbackContext ctx) => OnSlideStart?.Invoke();
    private void OnSlideCanceled(InputAction.CallbackContext ctx) => OnSlideEnd?.Invoke();
    private void OnPausePerformed(InputAction.CallbackContext ctx) => OnPause?.Invoke();


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
