using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : State
{
    protected PlayerMovement pm;

    protected PlayerBaseState(PlayerMovement pm, StateMachine sm) : base (sm)
    {
        this.pm = pm;
    }

    public override void Enter()
    {

    }

    public override void HandleInput()
    {

    }

    public override void LogicUpdate()
    {
        if (pm.playerFacingDir < 0) {
            pm.spriteRenderer.flipX = true;
        } else {
            pm.spriteRenderer.flipX = false;
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Exit()
    {

    }
}