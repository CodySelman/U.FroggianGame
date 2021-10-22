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
        // TODO play animation
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (!pm.isMovingX)
        {
            sm.ChangeState(pm.idleState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        pm.Walk();
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
}