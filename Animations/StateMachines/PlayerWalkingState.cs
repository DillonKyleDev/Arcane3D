using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : Player_Base_State
{
    private bool hasWalkedBefore;

    public PlayerWalkingState(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        Debug.Log("Walking State");
        ctx.SetCurrentState(this);
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
        HandleWalking();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(ctx.isLockedOn)
        {
            SwitchState(factory.WalkingLocked());
        }
        if(ctx.IsDashPressed)
        {
            SwitchState(factory.Dashing());
        } else if(ctx.isRollingPressed)
        {
            SwitchState(factory.Rolling());
        }
        else if(ctx.isRS1HAttacking)
        {
            SwitchState(factory.RightSwordOneHanded());
        }
        else if(ctx.isRB1HAttacking)
        {
            SwitchState(factory.RightBluntOneHanded());
        }
        else if(ctx.isCrouched)
        {
            SwitchState(factory.Crouching());
        }
        else if(ctx.pickingUpItem)
        {
            SwitchState(factory.PickingUpItem());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    void HandleWalking()
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
            ctx.playerTransform.rotation = Quaternion.Slerp(ctx.playerTransform.rotation, ctx.cameraRelativeTransformHorizontal.rotation * ctx.moveInputRotation, ctx.turnSpeed / 3);
            //Move player
            ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.animator.GetFloat("PlayerVel") / 5);
            
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
            ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.animator.GetFloat("PlayerVel") / 5);

        } else 
        {
            ctx.playerVelocity = 0;
            ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);
        }
    }
}
