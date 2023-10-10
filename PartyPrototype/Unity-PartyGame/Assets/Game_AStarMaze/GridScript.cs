using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public bool displayGridGizmos;
    public Vector2 gridSize;
    public float nodeRadius;
    public LayerMask obstacleMask;
    LayerMask walkableMask;

    public Node[,] grid;

    protected float nodeDiameter;
    protected int gridSizeX, gridSizeY;
    private void Awake() 
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;



        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //If there's a collision, set walkable false
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask));
                //Create new node
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    //Convert world point to coordinate on grid
    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridSize.x / 2) / gridSize.x;    //Get percentage of grid (X-axis)
        float percentY = (worldPos.z + gridSize.y / 2) / gridSize.y;    //Get percentage of grid (Z-axis)

        //If world position is outside of grid, avoid using index outside of array
        percentX = Mathf.Clamp01(percentX); //Clamp between 0 and 1
        percentY = Mathf.Clamp01(percentY); //Clamp between 0 and 1

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));
        
        if(grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                //Subtract 0.1 to show outline around node
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
