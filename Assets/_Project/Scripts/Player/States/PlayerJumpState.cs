using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    private float minJumpTime = 0.1f;
    private float jumpTime = 0;
    private bool isFalling = false;

    public PlayerJumpState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (Mathf.Sign(pm.rb.velocity.y) > 0) {
            isFalling = false;
            PlayJumpAnimation();
        } else {
            isFalling = true;
            PlayFallAnimation();
        }
        
        // pm.ActivateStunMaterial(true);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (pm.isGrounded) {
            sm.ChangeState(pm.landingLagState);
        }

        if (!isFalling && Mathf.Sign(pm.rb.velocity.y) < 0) {
            isFalling = true;
            PlayFallAnimation();
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
        // pm.ActivateStunMaterial(false);
    }

    private void PlayJumpAnimation() {
        if (pm.playerFacingDir == 0) {
            pm.animator.Play(Constants.ANIM_JUMP_CENTER);
        } else {
            pm.animator.Play(Constants.ANIM_JUMP_SIDE);
        }
    }

    private void PlayFallAnimation() {
        if (pm.playerFacingDir == 0) {
            pm.animator.Play(Constants.ANIM_FALL_CENTER);
        } else {
            pm.animator.Play(Constants.ANIM_FALL_SIDE);
        }
    }
}