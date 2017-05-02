using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// States of the Sheriff:  StayInOfficeState, PatrolState, GreetingState, ShootState
/// </summary>
/// <summary>
/// Stay In Office State
/// </summary>
public sealed class StayInOfficeState : State<Wyatt>
{

    static readonly StayInOfficeState instance = new StayInOfficeState();

    public static StayInOfficeState Instance
    {
        get
        {
            return instance;
        }
    }

    static StayInOfficeState() { }
    private StayInOfficeState() { }

    public override void Enter(Wyatt agent)
    {

    }

    public override void Execute(Wyatt agent)
    {
        if (agent.inOffice)
        {
            agent.IncreaseWaitedTime(1);
            if (agent.WaitedLongEnough() && (agent.spawned == true))
            {
                agent.ChangeState(PatrolState.Instance);
                agent.inOffice = false;
            }
        }
    }

    public override void Exit(Wyatt agent)
    {
        Debug.Log("Wyatt: ...exiting the office!");
        agent.SetPath(LevelManager.Instance.getWyattPath(3));

    }
}

/// <summary>
/// Patrol State is like transition state
/// </summary>
public sealed class PatrolState : State<Wyatt>
{

    static readonly PatrolState instance = new PatrolState();

    public static PatrolState Instance
    {
        get
        {
            return instance;
        }
    }
    static PatrolState() { }
    private PatrolState() { }

    public override void Enter(Wyatt agent)
    {

    }

    public override void Execute(Wyatt agent)
    {
        //When enter the Bank
        if (agent.inBank)
        {
            agent.CheckBank();
            if (agent.sawOutlaw)
            {
                agent.ChangeState(ShootState.Instance);
            }
            else
                agent.ChangeState(GreetingState.Instance);
        }

        ///When enter the Saloon
        else if (agent.inSaloon)
        {
            Debug.Log("Wyatt: One whiskey!");            
            agent.IncreaseWaitedTime(1);
            if (agent.WaitedLongEnough())
            {
                agent.inSaloon = false;
                agent.SetPath(LevelManager.Instance.getWyattPath(5));
            }
            
        }

        //When went back to office
        else if (agent.inOffice)
        {
            agent.ResetWaitedTime();
            agent.ChangeState(StayInOfficeState.Instance);
        }

    }

    public override void Exit(Wyatt agent)
    {
        agent.ResetWaitedTime();
    }
}

/// <summary>
/// Greeting State
/// </summary>
public sealed class GreetingState : State<Wyatt>
{

    static readonly GreetingState instance = new GreetingState();

    public static GreetingState Instance
    {
        get
        {
            return instance;
        }
    }

    static GreetingState() { }
    private GreetingState() { }

    public override void Enter(Wyatt agent)
    {
        //
        Debug.Log("Howdy partner...");

    }

    public override void Execute(Wyatt agent)
    {

        if (agent.sawOutlaw)
        {
            agent.ChangeState(ShootState.Instance);
        }
        else //if (agent.inBank)
        {
            agent.IncreaseWaitedTime(1);
            if (agent.WaitedLongEnough())
            {
                agent.inBank = false;
                agent.SetPath(LevelManager.Instance.getWyattPath(4));
                agent.ChangeState(PatrolState.Instance);
            }

        }

    }

    public override void Exit(Wyatt agent)
    {
        agent.LeaveBank();
        agent.ResetWaitedTime();
    }
}

/// <summary>
/// Shooting State
/// </summary>
public sealed class ShootState : State<Wyatt>
{

    static readonly ShootState instance = new ShootState();

    public static ShootState Instance
    {
        get
        {
            return instance;
        }
    }

    static ShootState() { }
    private ShootState() { }

    public override void Enter(Wyatt agent)
    {

    }

    public override void Execute(Wyatt agent)
    {
        Debug.Log("Bang bang!...");
        int r = Random.Range(0, 2);
        if (r == 1)
        {
            agent.KillOutlaw();
            Debug.Log("Wyatt: He’s a goner.");
            if (agent.inBank)
            {
                agent.inBank = false;
                agent.sawOutlaw = false;

                agent.SetPath(LevelManager.Instance.getWyattPath(6));
                agent.ChangeState(PatrolState.Instance);
            }
            else if (agent.inSaloon)
            {
                agent.inSaloon = false;
                agent.sawOutlaw = false;

                agent.SetPath(LevelManager.Instance.getWyattPath(5));
                agent.ChangeState(PatrolState.Instance);
            }

        }
        else
        {
            Debug.Log("Wyatt: Miss");
            if (agent.inBank)
            {
                agent.sawOutlaw = false;
                agent.inBank = false;

                agent.SetPath(LevelManager.Instance.getWyattPath(4));
                agent.ChangeState(PatrolState.Instance);
            }
            else if (agent.inSaloon)
            {
                agent.sawOutlaw = false;
                agent.inSaloon = false;

                agent.SetPath(LevelManager.Instance.getWyattPath(5));
                agent.ChangeState(PatrolState.Instance);
            }

        }
    }

    public override void Exit(Wyatt agent)
    {
        agent.LeaveBank();
    }
}



