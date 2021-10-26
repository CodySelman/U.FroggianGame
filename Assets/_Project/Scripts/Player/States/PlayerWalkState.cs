using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    private float walkTimer = 0;
    public PlayerWalkState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        pm.animator.Play(Constants.ANIM_WALK);
        pm.rb.velocity = Vector2.zero;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (!pm.isPlayerTryingToMoveX)
        {
            sm.ChangeState(pm.idleState);
        }

        if (pm.directionalInput.x > 0) {
            pm.SetPlayerFacingDirection(1);
        } else if (pm.directionalInput.x < 0) {
            pm.SetPlayerFacingDirection(-1);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        walkTimer -= Time.deltaTime;
        if (walkTimer < 0) {
            walkTimer = pm.walkSoundTimer;
            int ran = Random.Range(0, 4);
            if (ran == 0) {
                GameController.instance.PlayAudio(SoundName.SfxFootstep1);
            } else if (ran == 1) {
                GameController.instance.PlayAudio(SoundName.SfxFootstep2);
            } else if (ran == 2) {
                GameController.instance.PlayAudio(SoundName.SfxFootstep3);
            } else {
                GameController.instance.PlayAudio(SoundName.SfxFootstep4);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        pm.Walk();
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