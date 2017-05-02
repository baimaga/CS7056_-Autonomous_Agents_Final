public class StateMachine<T>
{

    private T agent;
    private State<T> globalState;
    private State<T> previousState;
    private State<T> currentState;

    public void Awake()
    {
        this.globalState = null;
        this.previousState = null;
        this.currentState = null;
    }

    //public void Init (T agent, State<T> startState) {
    //	this.agent = agent;
    //	this.state = startState;
    //}

    public void SetCurrentState(T agent, State<T> s)
    {
        this.agent = agent;
        this.currentState = s;
    }

    public void SetGlobalState(T agent, State<T> s)
    {
        this.agent = agent;
        this.globalState = s;
    }

    public State<T> GetCurrentState()
    {
        return this.currentState;
    }

    public void SetPreviousState(T agent, State<T> s)
    {
        this.agent = agent;
        this.previousState = s;
    }

    public void Update()
    {
        if (this.globalState != null) this.globalState.Execute(this.agent);
        if (this.currentState != null) this.currentState.Execute(this.agent);
        //if (this.state != null) this.state.Execute(this.agent);
    }

    public void ChangeState(State<T> nextState)
    {
        //keep a record of the previous state
        this.previousState = this.currentState;
        //call the exit method of the existing state
        this.currentState.Exit(this.agent);
        //change state to the new state
        this.currentState = nextState;
        //call the entry method of the new state
        this.currentState.Enter(this.agent);

        //if (this.state != null) this.state.Exit(this.agent);
        //this.state = nextState;
        //if (this.state != null) this.state.Enter(this.agent);
    }

    //change state back to the previous state
    public void RevertToPreviousState()
    {
        ChangeState(this.previousState);
    }

}