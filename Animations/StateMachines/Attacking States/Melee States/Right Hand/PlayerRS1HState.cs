using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Right Sword One Handed
public class PlayerRS1HState : Player_Base_State
{
    private float firstHitTime;
    private float secondHitTime;
    private float secondHitMax = 1f;
    private float thirdHitMax = 1.8f;
    private bool hasBegun = false;

    public PlayerRS1HState(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        ctx.SetCurrentState(this);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleAttackRight();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(ctx.isRS1HAttacking == false)
        {
            SwitchState(factory.Walking());
        }
    }

    public override void InitializeSubState()
    {

    }

    void HandleAttackRight()
    {
        if(ctx.gameManager.GetStamina() >= ctx.dashCost && ctx.isRS1HAttacking)
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
