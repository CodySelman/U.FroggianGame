using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        pm.animator.Play(Constants.ANIM_WALK);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (!pm.isPlayerTryingToMoveX)
        {
            sm.ChangeState(pm.idleState);
        }

        if (pm.directionalInput.x > 0) {
            pm.SetPlayerFacingDirection(1);
        } else if (pm.directionalInput.x < 0) {
            pm.SetPlayerFacingDirection(-1);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        pm.Walk();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}