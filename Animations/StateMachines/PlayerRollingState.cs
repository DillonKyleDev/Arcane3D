using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : Player_Base_State
{
    private float rollVel;
    private float rollFromStanding;
    private bool initialSpeedReached;
    public PlayerRollingState(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        Debug.Log("Rolling State");
        ctx.SetCurrentState(this);
        ctx.playerVelocity = ctx.maxVelocity * 2f;
        rollVel = ctx.animator.GetFloat("PlayerVel");
        rollFromStanding = 15f;
        ctx.animator.SetBool("Rolling", true);
        initialSpeedReached = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleRolling();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(!ctx.isRollingPressed && !ctx.animator.GetBool("Rolling"));
        {
            if(ctx.isCrouched)
            {
                SwitchState(factory.Crouching());
            }
            else SwitchState(factory.Walking());
        }

    }

    public override void InitializeSubState()
    {
        
    }

    void HandleRolling()
    {
        if(ctx.move.x != 0 || ctx.move.y != 0)
        {                   
            //Rotate character toward input direction
            ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
            ctx.playerTransform.rotation = Quaternion.Slerp(ctx.playerTransform.rotation, ctx.cameraRelativeTransformHorizontal.rotation * ctx.moveInputRotation, ctx.turnSpeed / 10f);

            if(initialSpeedReached == false)
            {
                //Inertia speed up
                if( rollVel < ctx.maxVelocity)
                {
                    rollVel += ctx.velIncrement;
                }
                else
                {
                    initialSpeedReached = true;
                }
                ctx.animator.SetFloat("PlayerVel", rollVel);
            } 
            else
            {
                //Inertia slow down
                if( rollVel > 0)
                {
                    rollVel -= ctx.velDecrement;
                }
                //if(rollVel < 20f) rollVel = 20f;
                ctx.animator.SetFloat("PlayerVel", rollVel);
            }

            //Move player
            if(ctx.animator.GetBool("LockedOn"))
            {
                ctx.controller.Move(ctx.lockedMoveTransform.forward * Time.deltaTime * rollVel / 8);    
            }
                ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * rollVel / 5);


        } else
        {   
            //Inertia slow down
            if(rollFromStanding > 0)
            {
                rollFromStanding -= ctx.velDecrement;
            }            
            ctx.animator.SetFloat("PlayerVel", rollFromStanding);
            //Move player
            if(ctx.animator.GetBool("LockedOn"))
            {
                ctx.controller.Move(ctx.lockedMoveTransform.forward * Time.deltaTime * rollVel / 8);    
            }
                ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * rollVel / 5);
        }
    }
}
