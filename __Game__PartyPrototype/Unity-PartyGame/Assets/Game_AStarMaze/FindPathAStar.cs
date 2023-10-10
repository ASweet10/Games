using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class FindPathAStar : MonoBehaviour
{
    PathRequestManager requestManager;
    GridScript grid;
    public List<GameObject> activePlayers = new List<GameObject>();
    private void Awake() {
        requestManager = gameObject.GetComponent<PathRequestManager>();
        grid = gameObject.GetComponent<GridScript>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 endPos) {
        StartCoroutine(FindPath(startPos, endPos));
    }

    IEnumerator FindPath(Vector3 start, Vector3 end)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccessful = false;

        Node startNode = grid.NodeFromWorldPoint(start);
        Node endNode = grid.NodeFromWorldPoint(end);

        if(startNode.walkable && endNode.walkable){
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            //Hash set is like a dictionary but has only keys
            // Used to simply check if item exists or not
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while(openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if(currentNode == endNode)
                {

                    sw.Stop();
                    pathSuccessful = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if(!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if(newMovementCostToNeighbor < currentNode.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, endNode);
                        neighbor.parent = currentNode;

                        if(!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
        }

        yield return null;
        if(pathSuccessful){
            waypoints = RetracePath(startNode, endNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccessful);
    }

    Vector3[] RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path){
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++){
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if(directionNew != directionOld){
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        } else {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    //Consider moving this to game manager script
    // Observer pattern, when player takes trap damage
    // this function is called and this script is simply given a new target
    public Transform ChooseTargetByHealthPercentage()
    {
        float current = 100;
        Transform newTarget = null;
        for(int i = 0; i < activePlayers.Count; i++)
        {
            var health = activePlayers[i].GetComponent<PlayerHealth>().CurrentHealth;
            if(health < current)
            {
                current = health;
                newTarget = activePlayers[i].transform;
            }
        }

        return newTarget;
    }
}
