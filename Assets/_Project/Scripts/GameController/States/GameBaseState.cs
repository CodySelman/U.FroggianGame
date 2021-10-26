using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBaseState : State
{
    protected GameController gc;

    protected GameBaseState(GameController gc, StateMachine sm) : base (sm)
    {
        this.gc = gc;
    }

    public override void Enter()
    {

    }

    public override void HandleInput()
    {

    }

    public override void LogicUpdate()
    {

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