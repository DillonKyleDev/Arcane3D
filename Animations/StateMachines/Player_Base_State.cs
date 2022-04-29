public abstract class Player_Base_State
{
    protected Player_State_Machine ctx;
    protected Player_State_Factory factory;
    public Player_Base_State(Player_State_Machine currentContext, Player_State_Factory fact)
    {
        ctx = currentContext;
        factory = fact;
    }

    //Abstract methods
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();


    //Concrete methods
    void UpdateStates() 
    {

    }

    protected void SwitchState(Player_Base_State newState)
    {
        //Exit current state
        this.ExitState();

        //New state enters state
        newState.EnterState();

        //Switch current state of context
        ctx.SetCurrentState(newState);
    }


    protected void SetSuperState()
    {

    }

    protected void SetSubState()
    {

    }
    
}
