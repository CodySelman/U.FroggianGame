using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        pm.SetVelocityToZero();
        PlayAnimation();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Mathf.Abs(pm.directionalInput.x) >= 0.1f)
        {
            sm.ChangeState(pm.walkState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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
            pm.animator.Play(Constants.ANIM_IDLE);
        } else {
            pm.animator.Play(Constants.ANIM_IDLE_SIDE);
        } 
    }
}