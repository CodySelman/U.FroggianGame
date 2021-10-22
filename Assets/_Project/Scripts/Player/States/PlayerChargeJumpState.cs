using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeJumpState : PlayerGroundedState
{
    private Vector2 initialMousePos;
    private bool isJumping = false;
    private float jumpTimer = 0f;

    public PlayerChargeJumpState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // TODO play animation
        isJumping = false;
        initialMousePos = GetMousePosition();
        // pm.SetVelocityToZero();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (!Input.GetMouseButton(0) && !isJumping) {
            pm.Jump(GetJumpVector());
            jumpTimer = pm.jumpSquatTime;
            isJumping = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        #if UNITY_EDITOR
            Debug.DrawRay(pm.transform.position, GetJumpVector(), Color.black);
        #endif

        if (isJumping) {
            jumpTimer -= Time.deltaTime;
        }

        if (isJumping && jumpTimer < 0) {
            sm.ChangeState(pm.jumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private Vector2 GetMousePosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return new Vector2(worldPos.x, worldPos.y);
    }

    private Vector2 GetJumpVector() {
        Vector2 currentMousePos = GetMousePosition();
        return initialMousePos - currentMousePos;
    }
}