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
        // TODO play idle animation
        // pm.SetVelocityToZero();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Mathf.Abs(Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL)) >= 0.01f)
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
}