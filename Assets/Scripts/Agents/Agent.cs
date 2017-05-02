using UnityEngine;
using System.Collections;

abstract public class Agent : MonoBehaviour {

    
    abstract public void Update ();

    abstract public void ChangeState(State<Agent> state);

   
   

}