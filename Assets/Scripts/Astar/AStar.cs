using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();
        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));

        }
    }

    public static Stack<Node> GetPath(Point start, Point goal) {
        if (nodes == null) {
            CreateNodes();
        }
        HashSet<Node> openList = new HashSet<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> finalPath = new Stack<Node>();
       
          
        Node currentNode = nodes[start];

        // 1. Adds the start noide to the openList
        openList.Add(currentNode);

        while (openList.Count > 0) //Step 10
        {
            //2. Runs through all neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

                    if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].WalkAble && neighbourPos != currentNode.GridPosition)
                    {

                        // sets the initial value of g to zero
                        int gCost = 0;

                        if (Math.Abs(x - y) == 1) //checks if we need to score 10
                        {
                            gCost = 10;
                        }
                        else
                        {
                            if (!ConnectDiagonally(currentNode, nodes[neighbourPos]))
                            {
                                continue;
                            }    
                            gCost = 14;  //scores 14 if we are diagonal
                        }

                        // STEP 3.  Adds the neighbour to the open list
                        Node neighbour = nodes[neighbourPos];

                        if (openList.Contains(neighbour)) //Step 9.1
                        {
                            if (currentNode.G + gCost < neighbour.G)
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost); //Step 9.4

                            }
                        }

                        else if (!closedList.Contains(neighbour)) //Step 9.1
                        {
                            openList.Add(neighbour); //9.2
                            neighbour.CalcValues(currentNode, nodes[goal], gCost); //9.3
                        }

                    }

                }
            }
            //5 & 8 STEP    MOVES the current node from the current list to the closed list 
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0) //STEP 7
            {
                //Sorts the list by F value and selects the first on the list
                currentNode = openList.OrderBy(n => n.F).First();
            }

            if (currentNode==nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                
                break;
            }
        }
        return finalPath;
    }

    private static bool ConnectDiagonally(Node currentNode, Node neighbour)
    {
        Point direction = neighbour.GridPosition - currentNode.GridPosition;
        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y+ direction.Y);

        if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].WalkAble)
        {
            return false;
        }

        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].WalkAble)
        {
            return false;
        }
        return true;
             
    }
}


