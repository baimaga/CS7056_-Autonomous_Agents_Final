using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager :  Singleton<GameManager>  {
    
        public ObjectPool Pool { get; set; }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
        
    }

    void Start()
    {
        
    }

    void Update()
    {

    }


    public void StartAgents() {
        StartCoroutine(SpawnAgents());
    }

    private IEnumerator SpawnAgents() {

        string type = "Jesse";
        string type2 = "Mark";
        string type3 = "Wyatt";

        Jesse jesse = Pool.GetObject(type).GetComponent<Jesse>();
        jesse.Spawn();
        Wyatt wyatt = Pool.GetObject(type3).GetComponent<Wyatt>();
        wyatt.Spawn();
        Mark mark = Pool.GetObject(type2).GetComponent<Mark>();
        mark.Spawn();

        yield return new WaitForSeconds(2.5f);
    }

   
}
