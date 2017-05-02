using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// States of the Undertaker:  CollectingState, HoverState, TrailState
/// </summary>
/// <summary>
/// Collect State is to go for Picking up the body
/// </summary> 
public sealed class CollectingState : State<Mark>
{

    static readonly CollectingState instance = new CollectingState();

    public static CollectingState Instance
    {
        get
        {
            return instance;
        }
    }

    static CollectingState() { }
    private CollectingState() { }

    public override void Enter(Mark agent)
    {

    }

    public override void Execute(Mark agent)
    {
        if (agent.collideBody)
        {
            Debug.Log("Took body");
            agent.SetPath(LevelManager.Instance.getMarkPath(8));
            agent.ChangeState(TrailState.Instance);
        }


    }

    public override void Exit(Mark agent)
    {
        agent.collideBody = false;
        agent.newCorpse = false;
    }
}

/// <summary>
/// Hover State
/// </summary>
public sealed class HoverState : State<Mark>
{

    static readonly HoverState instance = new HoverState();

    public static HoverState Instance
    {
        get
        {
            return instance;
        }
    }

    static HoverState() { }
    private HoverState() { }

    public override void Enter(Mark agent)
    {

    }

    public override void Execute(Mark agent)
    {
        if (agent.newCorpse)
        {
            Debug.Log("Mark: new corpse!!!");
            agent.SetPath(LevelManager.Instance.getMarkPath(7));
            agent.ChangeState(CollectingState.Instance);
        }
    }

    public override void Exit(Mark agent)
    {
        agent.inUndertaker = false;

    }
}

/// <summary>
/// Trailing State
/// </summary>
public sealed class TrailState : State<Mark>
{

    static readonly TrailState instance = new TrailState();

    public static TrailState Instance
    {
        get
        {
            return instance;
        }
    }

    static TrailState() { }
    private TrailState() { }

    public override void Enter(Mark agent)
    {
        Debug.Log("Mark: Carrying");
    }

    public override void Execute(Mark agent)
    {


        if (agent.inUndertaker)
        {
            agent.ChangeState(HoverState.Instance);
        }

    }

    public override void Exit(Mark agent)
    {

    }
}