using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingLagState : PlayerGroundedState
{
    private float landingLagTimer = 0f;

    public PlayerLandingLagState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        landingLagTimer = pm.landingLagTime;
        pm.SetVelocityToZero();
        PlayAnimation();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        landingLagTimer -= Time.deltaTime;
        if (landingLagTimer <= 0) {
            sm.ChangeState(pm.GetProperGroundedState());
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
            pm.animator.Play(Constants.ANIM_LAND_CENTER);
        } else {
            pm.animator.Play(Constants.ANIM_LAND_SIDE);
        }
    }
}