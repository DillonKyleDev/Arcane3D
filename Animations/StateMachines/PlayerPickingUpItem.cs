using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickingUpItem : Player_Base_State
{
    public PlayerPickingUpItem(Player_State_Machine currentContext, Player_State_Factory fact) : base(currentContext, fact)
    {
        //Contstuctor
    }

    public override void EnterState()
    {
        Debug.Log("Picking up State");
        ctx.animator.SetBool("PickUpItem", true);
        ctx.SetCurrentState(this);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandlePickUp();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(!ctx.pickingUpItem)
        {
            SwitchState(factory.Walking());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    void HandlePickUp()
    {

    }
}
