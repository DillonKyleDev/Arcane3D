public class Player_State_Factory
{
    Player_State_Machine context;

    public Player_State_Factory(Player_State_Machine currentContext)
    {
        context = currentContext;
    }

    public Player_Base_State Idle()
    {
        return new PlayerIdleState(context, this);
    }

    public Player_Base_State Walking()
    {
        return new PlayerWalkingState(context, this);
    }

    public Player_Base_State WalkingLocked()
    {
        return new PlayerWalkingLockedState(context, this);
    }

    public Player_Base_State Dashing()
    {
        return new PlayerDashingState(context, this);
    }

    public Player_Base_State Rolling()
    {
        return new PlayerRollingState(context, this);
    }

    public Player_Base_State Crouching()
    {
        return new PlayerCrouchedState(context, this);
    }

    public Player_Base_State RightBluntOneHanded()
    {
        return new PlayerRB1HState(context, this);
    }
    
    public Player_Base_State LeftBluntOneHanded()
    {
        return new PlayerLB1HState(context, this);
    }
    
    public Player_Base_State RightSwordOneHanded()
    {
        return new PlayerRS1HState(context, this);
    }
    
    public Player_Base_State LeftSwordOneHanded()
    {
        return new PlayerLS1HState(context, this);
    }

    public Player_Base_State PickingUpItem()
    {
        return new PlayerPickingUpItem(context, this);
    }
    
}
