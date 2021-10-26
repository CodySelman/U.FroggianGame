using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNormalState : GameBaseState
{
    public GameNormalState(GameController gc, StateMachine sm) : base(gc, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        gc.PlayMusicAndAmbience(true);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        gc.ListenForPause();
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