using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingState : Player_Base_State
{
    private bool isDashing = true;
    public PlayerDashingState(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        Debug.Log("Dashing State");
        ctx.SetCurrentState(this);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleDash();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(!isDashing)
        {
            SwitchState(factory.Walking());
        }
    }

    public override void InitializeSubState()
    {

    }

    void HandleDash()
    {
        if(ctx.gameManager.GetStamina() >= ctx.dashCost && isDashing)
        {
            StartDashing();
        } else {
            //play dud animation
        }
    }

    public void StartDashing()
    {
        if(isDashing)
        { 
            ctx.animator.SetBool("Dashing", true);
            ctx.gameManager.currentCameraSmooth = 0.1f;
            if(!ctx.timeStamped)
            {
                ctx.startTime = Time.time; // need to remember this to know how long to dash
                ctx.timeStamped = true;
            }

            //Smooth the velocity change for fake inertia
            if(ctx.playerVelocity < (Mathf.Abs(Vector2.Distance(ctx.move, new Vector2(0,0))) * ctx.maxVelocity))
            {
                ctx.playerVelocity += ctx.velIncrement;
            }
            if(ctx.playerVelocity > ctx.maxVelocity) ctx.playerVelocity = ctx.maxVelocity;
            if(ctx.playerVelocity < 0) ctx.playerVelocity = 0;
            ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);

            Vector2 lastLooked = ctx.playerTransform.forward;

            if(Time.time < ctx.startTime + ctx.dashTime)
            {
                if(ctx.move.x != 0 || ctx.move.y != 0)
                {
                    ctx.sideDashRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
                    lastLooked = ctx.move;
                } else {
                    ctx.sideDashRotation = Quaternion.LookRotation(new Vector3(lastLooked.x, 0, lastLooked.y));
                }
                
                ctx.dashTransform.rotation = Quaternion.Slerp(ctx.dashTransform.rotation, ctx.sideDashRotation, ctx.turnSpeed / 3);
                //Wind up
                if(ctx.startTime + 0.15f > Time.time) {
                    if(ctx.isLockedOn) 
                    {
                        //Slow the change in direction
                        ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
                        ctx.lockedMoveTransform.rotation = Quaternion.Slerp(ctx.lockedMoveTransform.rotation, ctx.moveInputRotation, ctx.turnSpeed / 3);
                        //Move player
                        ctx.controller.Move(ctx.lockedMoveTransform.forward * Time.deltaTime * ctx.maxVelocity / 6);
                    } else {
                        //Slow the change in direction
                        ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
                        ctx.playerTransform.rotation = Quaternion.Slerp(ctx.playerTransform.rotation, ctx.cameraRelativeTransformHorizontal.rotation * ctx.moveInputRotation, ctx.turnSpeed / 3);
                        //Move player
                        ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.maxVelocity / 6);
                    }
                }
                //Wait 0.2f seconds and then dash effect
                if(ctx.startTime + 0.15f < Time.time && !(ctx.startTime + ctx.dashTime - 0.15f < Time.time)) 
                {
                    ctx.dashParticles.Play();
                    if(ctx.isLockedOn) 
                    {
                        //Slow the change in direction
                        ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
                        ctx.lockedMoveTransform.rotation = Quaternion.Slerp(ctx.lockedMoveTransform.rotation, ctx.moveInputRotation, ctx.turnSpeed / 3);
                        //Move player
                        ctx.controller.Move(ctx.lockedMoveTransform.forward * Time.deltaTime * ctx.maxVelocity * ctx.dashMultiplier / 8);
                    } else {
                        //Slow the change in direction
                        ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
                        ctx.playerTransform.rotation = Quaternion.Slerp(ctx.playerTransform.rotation, ctx.cameraRelativeTransformHorizontal.rotation * ctx.moveInputRotation, ctx.turnSpeed / 10);
                        //Move player
                        ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.maxVelocity * ctx.dashMultiplier / 5);
                    }
                }
                //Transition into slide
                if(ctx.startTime + ctx.dashTime - 0.15f < Time.time) 
                {
                    ctx.animator.SetBool("Sliding", true);
                    ctx.dashParticles.Stop();
                    if(ctx.isLockedOn && (ctx.move.x != 0 || ctx.move.y != 0)) 
                    {
                        //Slow the change in direction
                        ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
                        ctx.lockedMoveTransform.rotation = Quaternion.Slerp(ctx.lockedMoveTransform.rotation, ctx.moveInputRotation, ctx.turnSpeed / 3);
                        //Move player
                        ctx.controller.Move(ctx.lockedMoveTransform.forward * Time.deltaTime * ctx.maxVelocity / 8);
                    } else if(ctx.move.x != 0 || ctx.move.y != 0) {
                        //Slow the change in direction
                        ctx.moveInputRotation = Quaternion.LookRotation(new Vector3(ctx.move.x, 0, ctx.move.y));
                        ctx.playerTransform.rotation = Quaternion.Slerp(ctx.playerTransform.rotation, ctx.cameraRelativeTransformHorizontal.rotation * ctx.moveInputRotation, ctx.turnSpeed / 3);

                        //Inertia slow down
                        if( ctx.playerVelocity > 0)
                        {
                            ctx.playerVelocity -= ctx.velDecrement / 2;
                        }
                        if(ctx.playerVelocity > ctx.maxVelocity) ctx.playerVelocity = ctx.maxVelocity;
                        if(ctx.playerVelocity < 0) ctx.playerVelocity = 0;
                        ctx.animator.SetFloat("PlayerVel", ctx.playerVelocity);

                        //Move player
                        ctx.controller.Move(ctx.playerTransform.forward * Time.deltaTime * ctx.maxVelocity / 5);
                    } 
                    //Begin wind down
                    ctx.skidParticles.Play();
                    isDashing = false;
                    ctx.timeStamped = false;
                    ctx.ResetCameraSmooth();
                    ctx.animator.SetBool("Dashing", false);
                    //ctx.animator.SetBool("Sliding", false);
                } else 
                {
                    ctx.animator.SetBool("Sliding", false);
                }
            } else 
            {
                ctx.timeStamped = false;
                ctx.ResetCameraSmooth();
                ctx.animator.SetBool("Dashing", false);
                ctx.animator.SetBool("Sliding", false);
            }
        } else 
        {
            isDashing = false;
            ctx.timeStamped = false;
            ctx.animator.SetBool("Dashing", false);
            ctx.animator.SetBool("Sliding", false);
        }
    }
}