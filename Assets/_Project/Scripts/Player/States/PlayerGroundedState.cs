using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (sm.CurrentState != pm.chargeJumpState 
            && (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        ) {
            sm.ChangeState(pm.chargeJumpState);
        }

        // TODO fall state?
        if (!pm.isGrounded) {
            sm.ChangeState(pm.jumpState);
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