using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinState : GameBaseState
{
    public GameWinState(GameController gc, StateMachine sm) : base(gc, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameController.instance.hasWon = true;
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
        GameController.instance.hasWon = false;
    }
}