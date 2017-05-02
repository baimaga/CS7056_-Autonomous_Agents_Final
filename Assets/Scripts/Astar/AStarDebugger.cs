using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour {
    
    private TileScript start, goal;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	//void Update () {
            
 //       ClickTile();
 //       if (Input.GetKeyDown(KeyCode.Space)) {
 //           AStar.GetPath(start.GridPosition, goal.GridPosition);
 //       }
	//}

    private void ClickTile() {
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("mouse1");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (hit.collider != null){
               
                TileScript tmp = hit.collider.GetComponent<TileScript>();
                if (tmp != null) {
                    if (start == null) {
                        start = tmp;
                        start.SpriteRenderer.color = new Color(252, 132, 0, 255);
                    }
                    else if (goal == null) {
                        goal = tmp;
                        goal.SpriteRenderer.color = new Color(252, 132, 0, 255);
                    }
                }
            }


        }
    }
}
