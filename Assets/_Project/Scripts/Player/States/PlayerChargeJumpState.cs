using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeJumpState : PlayerGroundedState
{
    private Vector2 initialMousePos;
    private bool jumpNextUpdate = false;
    private bool isJumping = false;
    private float jumpTimer = 0f;

    public PlayerChargeJumpState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // TODO play animation
        jumpNextUpdate = false;
        isJumping = false;
        initialMousePos = GetMousePosition();
        PlayAnimation();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (!Input.GetMouseButton(0) && !isJumping) {
            jumpNextUpdate = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        #if UNITY_EDITOR
            Debug.DrawRay(pm.transform.position, GetJumpVector(), Color.black);
        #endif

        pm.SetPlayerFacingDirection(GetVectorDirection(GetJumpVector()));
        PlayAnimation();

        if (isJumping) {
            jumpTimer -= Time.deltaTime;
        }

        if (isJumping && jumpTimer < 0) {
            sm.ChangeState(pm.jumpState);
        }

        // TODO handle sprite
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (jumpNextUpdate) {
            pm.Jump(GetJumpVector());
            jumpTimer = pm.jumpSquatTime;
            isJumping = true;
            jumpNextUpdate = false;
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void PlayAnimation() {
        if (pm.playerFacingDir == 0) {
            pm.animator.Play(Constants.ANIM_CHARGE_JUMP_CENTER);
        } else {
            pm.animator.Play(Constants.ANIM_CHARGE_JUMP_SIDE);
        }
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

    private int GetVectorDirection(Vector2 jumpVector) {
        float jumpDir = Mathf.Sign(jumpVector.x);
        // TODO handle upside down jumps better
        float jumpAngle = jumpDir > 0 ? 
            Vector2.SignedAngle(Vector2.right, jumpVector) :
            Vector2.SignedAngle(Vector2.left, jumpVector) * -1;

        if (jumpAngle > 60 && jumpAngle < 120) {
            return 0;
        } else {
            if (jumpDir > 0) {
                return 1;
            } else {
                return -1;
            }
        }
    }
}