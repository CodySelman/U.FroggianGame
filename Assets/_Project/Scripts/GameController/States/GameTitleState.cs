using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTitleState : GameBaseState
{
    public GameTitleState(GameController gc, StateMachine sm) : base(gc, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        gc.PlayMusicAndAmbience(false);
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
}