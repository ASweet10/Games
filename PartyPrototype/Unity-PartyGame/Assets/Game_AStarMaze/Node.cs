using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    
    public bool walkable;
    public Vector3 worldPosition;


    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;
    int heapIndex;

    public Node(bool _walkable, Vector3 worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get{ return hCost + gCost; }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node comparedNode)
    {
        int compare = fCost.CompareTo(comparedNode.fCost);  //If fcost is lower

        //If f costs are the same...
        if(compare == 0)
        {   
            //Compare h costs
            compare = hCost.CompareTo(comparedNode.hCost);
        }

        return -compare;
    }
}
