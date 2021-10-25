using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : PlayerAirborneState
{
    private float bonkTimer;
    private bool isBall = false;
    private bool turnIntoBallNextFrame = false;

    public PlayerStunState(PlayerMovement pm, StateMachine sm) : base(pm, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        PlayBonkAnimation();
        isBall = false;
        pm.TurnIntoBall();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isBall) {
            RotatePlayerSprite();
        }

        if (CheckForExitConditions()) {
            sm.ChangeState(pm.GetProperGroundedState());
        }
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
        pm.TurnIntoBall(false);
        ResetPlayerSpriteRotation();
    }

    private void PlayBonkAnimation() {
        if (pm.playerFacingDir == 0) {
            pm.animator.Play(Constants.ANIM_BONK_CENTER);
        } else {
            pm.animator.Play(Constants.ANIM_BONK_SIDE);
        }
    }

    private void PlayBallAnimation() {
        pm.animator.Play(Constants.ANIM_BALL);
        if (pm.playerFacingDir == 0) {
            pm.animator.Play(Constants.ANIM_BALL);
        } else {
            pm.animator.Play(Constants.ANIM_BALL_SIDE);
        }
    }

    private void RotatePlayerSprite() {
        pm.spriteRenderer.gameObject.transform.Rotate(
            0,
            0,
            pm.spriteRenderer.gameObject.transform.rotation.z 
                + (pm.ballSpinSpeed 
                * pm.rb.velocity.x
                * Time.deltaTime
                * -1)
        );
    }

    private void ResetPlayerSpriteRotation() {
        pm.spriteRenderer.gameObject.transform.rotation = Quaternion.identity;
    }

    private bool CheckForExitConditions() {
        // Debug.Log("CheckForExitcondition");
        // Debug.Log("isGrounded: " + pm.isGrounded);
        // Debug.Log("vel: " + pm.rb.velocity.y);

        return pm.isGrounded && pm.rb.velocity.y <= pm.ballExitVelocity;
    }

    public void GetBonked() {
        Debug.Log("GetBonked");
        if (!isBall) {
            pm.animator.Play(Constants.ANIM_BALL);
            isBall = true;
            pm.TurnIntoBall(true);
        }
    }
}