using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchedState : Player_Base_State
{

    public PlayerCrouchedState(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        Debug.Log("Crouched State");
        ctx.SetCurrentState(this);
        ctx.animator.SetBool("Crouched", true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleCrouching();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(!ctx.isCrouched)
        {
            SwitchState(factory.Walking());
        } 
        else if(ctx.isRollingPressed)
        {
            SwitchState(factory.Rolling());
        }
        else if(ctx.isDashPressed)
        {
            ctx.isCrouched = false;
            ctx.animator.SetBool("Crouched", false);
            SwitchState(factory.Dashing());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    void HandleCrouching()
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
            ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.animator.GetFloat("PlayerVel") / 16);
            
            //Smooth the input vector for animation blend tree to slowly blend between directions
            ctx.smoothAnimationsVector = Vector3.SmoothDamp(ctx.smoothAnimationsVector, new Vector3(ctx.move.x, 0, ctx.move.y), ref ctx.currentVelocity, .2f);
        // } else if(hasWalkedBefore)
        // {
        //     //Inertia slow down
        //     if( ctx.playerVelocity > 0)
        //     {
        //         ctx.playerVelocity -= ctx.velDecrement;
        //     }
        //     if(ctx.playerVelocity > ctx.maxVelocity) ctx.playerVelocity = ctx.maxVelocity;
        //     if(ctx.playerVelocity < 0) ctx.playerVelocity = 0;
        //     ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);

        //     //Move player
        //     ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.animator.GetFloat("PlayerVel") / 5);

        // 
        } else 
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
            ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.animator.GetFloat("PlayerVel") / 16);
        }
    }
}
