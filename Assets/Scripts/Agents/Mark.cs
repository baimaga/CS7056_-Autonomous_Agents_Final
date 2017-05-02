using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The Undertaker, Mark is the name of famous Wrestler - Undertaker:)  
/// </summary>
public class Mark : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Stack<Node> path;
    public GameObject MarkPrefab;
    private StateMachine<Mark> stateMachine;
    public Point GridPosition { get; set; }
    private State<Mark> currentState;
    private Vector3 destination;
    public int money = 0;
    //for locating on a map
    public bool newCorpse = false;
    public bool collideBody = false;
    public bool inUndertaker = false;      
    
    
    public void Awake()
    {                
        Wyatt.OnKillingOutlaw += CollectBody; //subscribe 
        Wyatt.OnKillingOutlaw -= Hover; //unsubscribe 
        this.stateMachine = new StateMachine<Mark>();
        this.stateMachine.SetCurrentState(this, HoverState.Instance);
    }
    public void Update()
    {
        this.currentState = this.stateMachine.GetCurrentState();
        if ((currentState == CollectingState.Instance) || (currentState == TrailState.Instance))
        {

            Move();
        }
        this.stateMachine.Update();
    }      

    public void ChangeState(State<Mark> state)
    {
        this.stateMachine.ChangeState(state);
    }
   
    public void Spawn()
    {
        transform.position = LevelManager.Instance.Undertaker.transform.position;

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
    public void CollectBody()
    {
        newCorpse = true;
    }

    public void Hover()
    {

    }
    //When Mark prefab collides with locatoins on maps
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Outlaw":                
                //Collide with the outlaw                
                collideBody = true;
                break;
            case "Undertaker":
                //Collide with the UNdertaker                
                inUndertaker = true;
                break;
        }
    }
}
