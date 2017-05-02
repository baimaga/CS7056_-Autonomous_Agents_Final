using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager> {

    [SerializeField]
    private GameObject[] tilePrefabs;
    [SerializeField]
    private Transform map;

    private Point outlawCampSpawn, cemeterySpawn, undertakerSpawn, sheriffsOfficeSpawn, bankSpawn, saloonSpawn;

    [SerializeField]
    private GameObject outlawCampPrefab;
    [SerializeField]
    private GameObject cemeteryPrefab;
    [SerializeField]
    private GameObject undertakerPrefab;
    [SerializeField]
    private GameObject sheriffsOfficePrefab;
    [SerializeField]
    private GameObject bankPrefab;
    [SerializeField]
    private GameObject saloonPrefab;

    private Point mapSize;
    private Stack<Node> pathJesse;
    private Stack<Node> pathWyatt;
    private Stack<Node> pathMark;

    //for Jesse
    public Stack<Node> getJessePath(int index)
    {        
        pathJesse = null;            
        GeneratePath(index);            
        return new Stack<Node>(new Stack<Node>(pathJesse));     
    }

    public Stack<Node> getWyattPath(int index)
    {
        pathWyatt = null;
        GeneratePath(index);
        return new Stack<Node>(new Stack<Node>(pathWyatt));
    }

    public Stack<Node> getMarkPath(int index)
    {
        pathMark = null;
        GeneratePath(index);
        return new Stack<Node>(new Stack<Node>(pathMark));
    }



    public Location OutlawCamp { get; set; }
    public Location Cemetery { get; set; }
    public Location Undertaker { get; set; }
    public Location SheriffSOffice { get; set; }
    public Location Bank { get; set; }
    public Location Saloon { get; set; }


    public Dictionary<Point, TileScript> Tiles { get; set; }

    public float TileSize {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x; }
    }

    // Use this for initialization
    void Start()
    {
        CreateLevel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateLevel() {

        Tiles = new Dictionary<Point, TileScript>();
        string[] mapData = ReadLevelText();

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);  
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;



        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < mapY; y++) {   //the y position

            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)    //the x position
            {
                PlaceTile(newTiles[x].ToString(), x,y, worldStart);
            }
        }
        SpawnLocations();
    }
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart) {

        int tileIndex = int.Parse(tileType);
        
        TileScript newTile = Instantiate(tilePrefabs[tileIndex].GetComponent<TileScript>());    
        newTile.GetComponent<TileScript>().Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);
        
        //Check if tile is water or brick
        if ((tileType == "2")||(tileType == "1"))
        {
            newTile.WalkAble = false;
        }
        
        Tiles.Add(new Point(x, y), newTile);

    }

    private string[] ReadLevelText() {

        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');

    }

    private void SpawnLocations() {
        //Spawn locations 
        //Outlaw camp
        outlawCampSpawn = new Point(0, 0);
        GameObject tmp = Instantiate(outlawCampPrefab, Tiles[outlawCampSpawn].transform.position, Quaternion.identity);
        OutlawCamp = tmp.GetComponent<Location>();
        OutlawCamp.name = "OutlawCamp";

        //Cemetery
        cemeterySpawn = new Point(3, 3);
        GameObject tmp2 = Instantiate(cemeteryPrefab, Tiles[cemeterySpawn].transform.position, Quaternion.identity);         
        Cemetery = tmp2.GetComponent<Location>();
        Cemetery.name = "Cemetery";

        //Undertaker
        undertakerSpawn = new Point(6, 0);
        GameObject tmp3 = Instantiate(undertakerPrefab, Tiles[undertakerSpawn].transform.position, Quaternion.identity);         
        Undertaker = tmp3.GetComponent<Location>();
        Undertaker.name = "Undertaker";
   
        //SheriffsOffice
        sheriffsOfficeSpawn = new Point(10, 4);
        GameObject tmp4 = Instantiate(sheriffsOfficePrefab, Tiles[sheriffsOfficeSpawn].transform.position, Quaternion.identity);         
        SheriffSOffice = tmp4.GetComponent<Location>();
        SheriffSOffice.name = "SheriffSOffice";

        //Bank
        bankSpawn = new Point(2, 6);
        GameObject tmp5 = Instantiate(bankPrefab, Tiles[bankSpawn].transform.position, Quaternion.identity);
        Bank = tmp5.GetComponent<Location>();
        Bank.name = "Bank";

        //Saloon
        saloonSpawn = new Point(7, 2);
        GameObject tmp6 = Instantiate(saloonPrefab, Tiles[saloonSpawn].transform.position, Quaternion.identity);
        Saloon = tmp6.GetComponent<Location>();
        Saloon.name = "Saloon";

    }

    public bool InBounds(Point position) {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y <mapSize.Y;
    }

    public void GeneratePath(int index)
    {
        
        switch (index)
        {
            
            //for Jesse
            case 1:
                pathJesse = AStar.GetPath(outlawCampSpawn, bankSpawn);
                break;
            case 2:
                pathJesse = AStar.GetPath(bankSpawn, outlawCampSpawn);
                break;
            //for Wyatt
            case 3:
                pathWyatt = AStar.GetPath(sheriffsOfficeSpawn, bankSpawn);
                break;
            case 4:
                pathWyatt = AStar.GetPath(bankSpawn, saloonSpawn);
                break;
            case 5:
                pathWyatt = AStar.GetPath(saloonSpawn, sheriffsOfficeSpawn);
                break;
            case 6:
                pathWyatt = AStar.GetPath(bankSpawn, sheriffsOfficeSpawn);
                break;
            //for Mark
            case 7:
                pathMark = AStar.GetPath(undertakerSpawn, bankSpawn);
                break;
            case 8:
                pathMark = AStar.GetPath(bankSpawn, undertakerSpawn);
                break;
            // for Jesse again
            case 9:
                pathJesse = AStar.GetPath(outlawCampSpawn, cemeterySpawn);
                break;
            case 10:
                pathJesse = AStar.GetPath(cemeterySpawn, bankSpawn);
                break;
            case 11:
                pathJesse = AStar.GetPath(bankSpawn, cemeterySpawn);
                break;
            case 12:
                pathWyatt = AStar.GetPath(saloonSpawn, bankSpawn);
                break;
            case 13:
                pathJesse = AStar.GetPath(bankSpawn, undertakerSpawn);
                break;
        }        
    }
}
