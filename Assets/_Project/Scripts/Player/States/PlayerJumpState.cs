using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    private float minJumpTime = 0.1f;
    private float jumpTime = 0;

    public PlayerJumpState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        PlayAnimation();
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

    private void PlayAnimation() {
        if (pm.playerFacingDir == 0) {
            pm.animator.Play(Constants.ANIM_JUMP_CENTER);
        } else {
            pm.animator.Play(Constants.ANIM_JUMP_SIDE);
        }
    }
}