using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Right Sword One Handed
public class PlayerRB1HState : Player_Base_State
{
    private float firstHitTime;
    private float secondHitTime;
    private float secondHitMax = 1f;
    private float thirdHitMax = 1.8f;
    private bool hasBegun = false;

    public PlayerRB1HState(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        Debug.Log("Right Hand Blunt State");
        ctx.SetCurrentState(this);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleAttackRight();
    }

    public override void ExitState()
    {
        ctx.animator.SetBool("RB1HAttack", false);
        ctx.animator.SetBool("LeaveAnimationState", false);
        ctx.isRB1HAttacking = false;
    }

    public override void CheckSwitchStates()
    {
        if(ctx.animator.GetBool("LeaveAnimationState"))
        {
            SwitchState(factory.Walking());
        }
    }

    public override void InitializeSubState()
    {

    }

    void HandleAttackRight()
    {
        if(ctx.gameManager.GetStamina() >= ctx.dashCost && ctx.isRB1HAttacking)
        {
            StartAttacking();
        } else {
            //play dud animation
        }
    }
    public void StartAttacking()
    {
        
    }
}
