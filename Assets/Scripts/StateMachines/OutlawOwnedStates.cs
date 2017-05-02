using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// States of the Outlaw:  StayInCampStatee, RobBankState, LurkState, DeadState, WalkingState, GlobalState?
/// </summary>
/// <summary>
/// Staying in Campus State
/// </summary>
public sealed class StayInCampState : State<Jesse>
{

    static readonly StayInCampState instance = new StayInCampState();

    public static StayInCampState Instance
    {
        get
        {
            return instance;
        }
    }

    static StayInCampState() { }
    private StayInCampState() { }

    public override void Enter(Jesse agent)
    {
        Debug.Log("Jesse: Going to Camp with money...");
        agent.money = 0;
        agent.hidingTime = 0;

    }

    public override void Execute(Jesse agent)
    {
        int r = Random.Range(1, 70);
        agent.hidingTime++;
        if ((agent.hidingTime % r == 0) && (agent.spawned == true))
        {
            agent.SetPath(LevelManager.Instance.getJessePath(1));
            agent.ChangeState(WalkingState.Instance);
            agent.inCamp = false;
        }
    }

    public override void Exit(Jesse agent)
    {
        Debug.Log("Jesse: ...exiting the camp!");

    }
}

/// <summary>
/// Robbing Bank State
/// </summary>
public sealed class RobBankState : State<Jesse>
{

    static readonly RobBankState instance = new RobBankState();

    public static RobBankState Instance
    {
        get
        {
            return instance;
        }
    }

    static RobBankState() { }
    private RobBankState() { }

    public override void Enter(Jesse agent)
    {
        Debug.Log("Jesse: Going to Rob a bank...");
    }

    public override void Execute(Jesse agent)
    {
        if (!agent.dead)
        {
            if (agent.sheriffInBank)
            {
                agent.SetPath(LevelManager.Instance.getJessePath(11));
                agent.ChangeState(WalkingState.Instance);
            }
            else
            {
                agent.RobBank();
                int r = Random.Range(1, 10);
                agent.money = agent.money + r;
                agent.IncreaseWaitedTime(1);
                if (agent.WaitedLongEnough())
                {
                    agent.inBank = false;
                    Debug.Log("Jesse:...I have got enough money " + agent.money);
                    agent.money = 0;
                    agent.SetPath(LevelManager.Instance.getJessePath(2));
                    agent.ChangeState(WalkingState.Instance);
                }
            }

        }
        else if (agent.dead)
        {
            agent.ChangeState(DeadState.Instance);
        }
    }

    public override void Exit(Jesse agent)
    {
        agent.inBank = false;
        agent.FinishRobbery();
        agent.ResetWaitedTime();
        Debug.Log("Jesse:..I must leave!");

    }
}

/// <summary>
/// Lurking State
/// </summary>
public sealed class LurkState : State<Jesse>
{

    static readonly LurkState instance = new LurkState();

    public static LurkState Instance
    {
        get
        {
            return instance;
        }
    }

    static LurkState() { }
    private LurkState() { }

    public override void Enter(Jesse agent)
    {

        Debug.Log("Running to Cemetery...");
    }

    public override void Execute(Jesse agent)
    {
        if (!agent.sheriffInBank)
        {
            agent.inCemetery = false;
            agent.SetPath(LevelManager.Instance.getJessePath(10));
            agent.ChangeState(WalkingState.Instance);
        }

    }

    public override void Exit(Jesse agent)
    {

    }
}

/// <summary>
/// Dead State
/// </summary>
public sealed class DeadState : State<Jesse>
{

    static readonly DeadState instance = new DeadState();

    public static DeadState Instance
    {
        get
        {
            return instance;
        }
    }

    static DeadState() { }
    private DeadState() { }

    public override void Enter(Jesse agent)
    {
        Debug.Log("Jesse: I am dead X(");
    }

    public override void Execute(Jesse agent)
    {
        if (agent.pickedUp)
        {
            agent.SetPath(LevelManager.Instance.getJessePath(13));
            agent.ChangeState(WalkingState.Instance);
        }
    }

    public override void Exit(Jesse agent)
    {

    }
}

/// <summary>
/// Walking State like transition state
/// </summary>
public sealed class WalkingState : State<Jesse>
{

    static readonly WalkingState instance = new WalkingState();

    public static WalkingState Instance
    {
        get
        {
            return instance;
        }
    }

    static WalkingState() { }
    private WalkingState() { }

    public override void Enter(Jesse agent)
    {

    }

    public override void Execute(Jesse agent)
    {

        if (!agent.pickedUp)
        {
            if (agent.inBank)
            {
                agent.ChangeState(RobBankState.Instance);
            }

            else if (agent.inCamp)
            {

                agent.ChangeState(StayInCampState.Instance);
            }
            else if (agent.inCemetery)
            {

                agent.ChangeState(LurkState.Instance);
            }
            else if (agent.dead)
            {
                agent.ChangeState(DeadState.Instance);
            }
        }

    }

    public override void Exit(Jesse agent)
    {

    }
}

/// <summary>
/// Global State have't implemented yet
/// </summary>
public sealed class GlobalState : State<Jesse>
{

    static readonly GlobalState instance = new GlobalState();

    public static GlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static GlobalState() { }
    private GlobalState() { }

    public override void Enter(Jesse agent)
    {

    }

    public override void Execute(Jesse agent)
    {

    }

    public override void Exit(Jesse agent)
    {

    }
}

