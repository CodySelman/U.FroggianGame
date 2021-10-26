using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseState : GameBaseState
{
    public GamePauseState(GameController gc, StateMachine sm) : base(gc, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameController.instance.isPaused = true;
        PauseMenuController.instance.ShowPauseMenu(true);
        Time.timeScale = 0;
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
        GameController.instance.isPaused = false;
        PauseMenuController.instance.ShowPauseMenu(false);
        Time.timeScale = 1;
    }
}