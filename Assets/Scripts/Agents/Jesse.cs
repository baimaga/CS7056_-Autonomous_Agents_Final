using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Outlaw
/// </summary>
public class Jesse : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Stack<Node> path;
    public GameObject JessePrefab;
    private StateMachine<Jesse> stateMachine;
    private State<Jesse> currentState;
    public Point GridPosition { get; set; }
    private Vector3 destination;
    public bool spawned = false;
    public static int ENOUGH_MONEY = 100;
    public int money = 0;   
    public static int WAIT_TIME = 50;
    public int waitedTime = 0;
    public int hidingTime = 0;
    //For messaging or subscribing
    public delegate void bankRobbery();
    public static event bankRobbery OnBankRobbery;
    public delegate void finishRobbery();
    public static event finishRobbery OnFinishRobbery;
    //for locating agent on map
    public bool sheriffInBank = false;    
    public bool dead = false;
    public bool pickedUp = false;
    public bool inBank = false;
    public bool inCamp = false;
    public bool inCemetery = false;    

    

    // Use this for initialization
    void Awake()
    {
        this.stateMachine = new StateMachine<Jesse>();
        this.stateMachine.SetGlobalState(this, GlobalState.Instance);
        Wyatt.OnCheckingBank += Hide; //subscribe 
        Wyatt.OnCheckingBank -= Unhide; //unsubscribe
        Wyatt.OnLeavingBank += Free; //subscribe 
        Wyatt.OnLeavingBank -= Unfree; //unsubscribe
        Wyatt.OnKillingOutlaw += Die; //subscribe 
        Wyatt.OnKillingOutlaw -= Hide; //unsubscribe 
    }

    // Update is called once per frame
    private void Update()
    {
        this.currentState = this.stateMachine.GetCurrentState();
        if (currentState == WalkingState.Instance)    //can only move in Walking State
        {
            Move();
        }

        this.stateMachine.Update();
    }

    public void ChangeState(State<Jesse> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public void Spawn()
    {

        transform.position = LevelManager.Instance.OutlawCamp.transform.position;

        spawned = true;
        stateMachine.SetCurrentState(this, StayInCampState.Instance);

    }

    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if (transform.position == destination)
        {
            if (path != null && path.Count > 0)
            {
                GridPosition = path.Peek().GridPosition;
                destination = path.Pop().WorldPosition;
            }
        }
    }

    public void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            this.path = newPath;
            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }

    }

    public void Hide()
    {
        sheriffInBank = true;
    }

    public void Unhide()
    {
        sheriffInBank = false;
    }

    public void Free()
    {
        sheriffInBank = false;
    }

    public void Unfree()
    {
        sheriffInBank = true;
    }
    public void Die()
    {
        dead = true;
    }

    public void RobBank()
    {
        OnBankRobbery();
    }
    public void FinishRobbery()
    {
        OnFinishRobbery();
    }

    public void IncreaseWaitedTime(int amount)
    {
        this.waitedTime += amount;
    }

    public bool WaitedLongEnough()
    {
        return this.waitedTime >= WAIT_TIME;
    }

    public void ResetWaitedTime()
    {
        this.waitedTime = 0;
    }

    public void Respawn()
    {
        Debug.Log("Jesse: Respawn");
        pickedUp = false;
        dead = false;
        transform.position = LevelManager.Instance.OutlawCamp.transform.position;
        stateMachine.SetCurrentState(this, StayInCampState.Instance);
    }

    //When Jesse prefab collides with locatoins on maps
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Bank":
                //Collide with the bank
                this.inBank = true;
                break;

            case "OutlawCamp":
                //Collide with the camp
                this.inCamp = true;
                break;

            case "Cemetery":
                //Collide with the Cemetery
                this.inCemetery = true;
                break;

            case "Mark":
                //Collide with Mark
                this.pickedUp = true;
                break;

            case "Undertaker":
                //Collide with Undertaker
                Respawn();
                break;
        }

    }
}
