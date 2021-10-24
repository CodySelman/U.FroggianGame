using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : PlayerAirborneState
{
    public PlayerStunState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
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
            // play center animation
        } else {
            // play side animation
        }
    }
}