using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingLockedState : Player_Base_State
{
    private bool hasWalkedBefore;

    public PlayerWalkingLockedState(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        ctx.SetCurrentState(this);
        ctx.animator.SetBool("LockedOn", true);
        hasWalkedBefore = false;
        if(ctx.move.x == 0 && ctx.move.y == 0)
        {
            ctx.playerVelocity = 0;   
        } else 
        {
            ctx.playerVelocity = ctx.maxVelocity / 2;
        }
        ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleWalkingLocked();
    }

    public override void ExitState()
    {
        ctx.animator.SetBool("LockedOn", false);
    }

    public override void CheckSwitchStates()
    {
        if(!ctx.isLockedOn)
        {
            SwitchState(factory.Walking());
        }
        if(ctx.IsDashPressed)
        {
            SwitchState(factory.Dashing());
        } else
        {
            if(ctx.isRollingPressed)
            {
                SwitchState(factory.Rolling());
            }
        }
    }

    public override void InitializeSubState()
    {
        
    }

    void HandleWalkingLocked()
    {
        if(ctx.move.x != 0 || ctx.move.y != 0)
        {       
            //Smooth the velocity change for fake inertia
            if(ctx.playerVelocity < (Mathf.Abs(Vector2.Distance(ctx.move, new Vector2(0,0))) * ctx.maxVelocity))
            {
                ctx.playerVelocity += ctx.velIncrement;
            }
            if(ctx.playerVelocity > ctx.maxVelocity) ctx.playerVelocity = ctx.maxVelocity;
            if(ctx.playerVelocity < 0) ctx.playerVelocity = 0;
            ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);
            
            //Slow the change in direction
            ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
            ctx.lockedMoveTransform.rotation = Quaternion.Slerp(ctx.lockedMoveTransform.rotation, ctx.cameraRelativeTransformHorizontal.rotation * ctx.moveInputRotation, ctx.turnSpeed / 3);

            //Move player
            ctx.controller.Move(ctx.lockedMoveTransform.forward * Time.deltaTime * ctx.animator.GetFloat("PlayerVel") / 6);

            //Smooth the input vector for animation blend tree to slowly blend between directions
            ctx.smoothAnimationsVector = Vector3.SmoothDamp(ctx.smoothAnimationsVector, new Vector3(ctx.move.x, 0, ctx.move.y), ref ctx.currentVelocity, .2f);

            ctx.blendForward = Mathf.SmoothDamp(ctx.blendForward, ctx.move.y, ref ctx.currentVelocity.x, 1f);
            ctx.blendRight = Mathf.SmoothDamp(ctx.blendRight, ctx.move.x, ref ctx.currentVelocity.z, 1f);

            if(ctx.blendForward > 1) ctx.blendForward = 1;
            if(ctx.blendForward < -1) ctx.blendForward = -1;
            if(ctx.blendRight > 1) ctx.blendRight = 1;
            if(ctx.blendRight < -1) ctx.blendRight = -1;
            ctx.animator.SetFloat("Forward", ctx.blendForward);
            ctx.animator.SetFloat("Right", ctx.blendRight);
            hasWalkedBefore = true;
        } else if(hasWalkedBefore)
        {
            //Inertia slow down
            if( ctx.playerVelocity > 0)
            {
                ctx.playerVelocity -= ctx.velDecrement;
            }
            if(ctx.playerVelocity > ctx.maxVelocity) ctx.playerVelocity = ctx.maxVelocity;
            if(ctx.playerVelocity < 0) ctx.playerVelocity = 0;
            ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);

            //Move player
            ctx.controller.Move(ctx.lockedMoveTransform.forward * Time.deltaTime * ctx.animator.GetFloat("PlayerVel") / 6);    

        } else 
        {
            ctx.playerVelocity = 0;
            ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);
        }
    }
}
