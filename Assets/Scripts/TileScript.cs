using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public Point GridPosition { get; private set; }

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }

    public SpriteRenderer SpriteRenderer { get; set; }

    public bool WalkAble { get; set; }

    public bool Debugging { get; set; }

    // Use this for initialization
    void Start () {
        SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent) {
        WalkAble = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
       // LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    public void ColorTile(Color newColor){
        SpriteRenderer.color = newColor; 
    }
}
