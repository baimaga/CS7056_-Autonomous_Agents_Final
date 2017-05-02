using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The Sheriff
/// </summary>
public class Wyatt : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Stack<Node> path;
    private StateMachine<Wyatt> stateMachine;
    public GameObject WyattPrefab;
    public Point GridPosition { get; set; }
    private Vector3 destination;
    private State<Wyatt> currentState;
    public static int WAIT_TIME = 100;
    public int waitedTime = 0;
    public int money = 0;
    //For messaging or subscribing
    public delegate void checkingBank();
    public static event checkingBank OnCheckingBank;
    public delegate void leavingBank();
    public static event leavingBank OnLeavingBank;
    public delegate void killingOutlaw();
    public static event killingOutlaw OnKillingOutlaw;
    //Here is for locating the agent in a map
    public bool spawned = false;
    public bool inBank = false;
    public bool inOffice = false;
    public bool sawOutlaw = false;
    public bool inSaloon = false;

    public Transform singhtStart, singhtEnd;
    public bool spotted = false;
    Vector3 prevPos = new Vector3(4.16f, 0.3f, 0.0f);
   

    public void Awake()
    {
        this.stateMachine = new StateMachine<Wyatt>();
        Jesse.OnBankRobbery += CatchOutlaw; //subscribe 
        Jesse.OnBankRobbery -= Release; //unsubscribe        
        Jesse.OnFinishRobbery += Release; //subscribe 
        Jesse.OnFinishRobbery -= CatchOutlaw; //unsubscribe
    }
    public void Update()
    {
        Raycasting();        

        currentState = this.stateMachine.GetCurrentState();        
        if (currentState == PatrolState.Instance)      //can only move in Patrol State
        {
            Move();
        }
        this.stateMachine.Update();
    }    
    
    public void ChangeState(State<Wyatt> state)
    {
        this.stateMachine.ChangeState(state);
    }    

    public void Spawn()
    {
        transform.position = LevelManager.Instance.SheriffSOffice.transform.position;
        spawned = true;
        this.stateMachine.SetCurrentState(this, StayInOfficeState.Instance);
    }

    private void Move()
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
    public void CatchOutlaw()
    {
        sawOutlaw = true;
    }

    public void Release()
    {
        sawOutlaw = false;
    }

    public void KillOutlaw()
    {
        OnKillingOutlaw();
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

    public void CheckBank()
    {
        OnCheckingBank();
    }

    public void LeaveBank()
    {
        OnLeavingBank();
    }

    /// <summary>
    /// Implementing the Sight Sense
    /// </summary>
    void Raycasting() {
        Vector3 currPos = transform.position;

        int signX = 1;
        int signY = 1;
        if ((currPos.x - prevPos.x) <0 )
        {
            signX = -1;
        }
        else if ((currPos.x - prevPos.x) > 0)
        {
            signX = 1;
        }
        else
        {
            signX = 0;
        }

        if((currPos.y - prevPos.y) < 0)
        {
            signY = -1;
        }
        else if((currPos.y - prevPos.y) > 0)
        {
            signY = 1;
        }
        else
        {
            signY = 0;
        }

        Debug.DrawLine(transform.position, transform.position + new Vector3(signX * 5.0f, signY * 8.0f, 0.0f), Color.green);

        spotted = Physics2D.Linecast(transform.position, transform.position + new Vector3(signX * 5.0f, signY * 8.0f, 0.0f), 1 << LayerMask.NameToLayer("Jesse"));
        if (spotted)
        {
            Debug.Log("Wyatt: I saw you, Hands up! Outlaw there!");
        }

        prevPos = currPos;
    }

    //When Wyatt prefab collides with locatoins on maps
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Bank":
                //Collide with the bank   
                {
                    this.inBank = true;

                }
                break;

            case "Outlaw":
                //Collide with Jesse                
                this.sawOutlaw = true;
                break;

            case "Office":
                //Collide with the bank   
                {
                    this.inOffice = true;
                }
                break;

            case "Saloon":
                //Collide with the Saloon   
                {
                    this.inSaloon = true;
                }
                break;
        }

    }

}
