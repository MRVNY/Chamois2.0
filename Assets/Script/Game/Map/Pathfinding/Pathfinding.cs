﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding Instance;
    
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 15;

    private static int2 gridSize = new int2(300, 300);
    private PathNode[] PNArray = new PathNode[gridSize.x*gridSize.y];

    private static float cellSize = 2f;
    private static Vector3 originPosition = new Vector3(0, -600, 0);
    public TextAsset csvPath;

    public Task reading;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        reading = readWalkables();
        
        //Baking
        //     var s = "";
        //     for (int x = 0; x < gridSize.x; x++)
        //     {
        //         for (int y = 0; y < gridSize.y; y++)
        //         {
        //             var nodePos = GetWorldPosition(x, y);
        //             if (Physics2D.BoxCast(nodePos, Vector2.one*2, 0f, Vector2.zero, 0f, LayerMask.GetMask("Terrain")))
        //             {
        //                 s += "0,";
        //             }
        //             else s += "1,";
        //         }
        //         s += "\n";
        //     }
        //
        //     print(s);
    }
    
    async Task readWalkables()
    {
        string[][] walkables = new string[gridSize.y][];
        string[] lines = csvPath.text.Split('\n');
        for(int i = 0; i < lines.Length; i++)
        {
            walkables[i] = lines[i].Split(',');
        }

        //int[,] walkables = Walkables.walkables;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                PathNode pathNode = new PathNode();
                pathNode.x = x;
                pathNode.y = y;
                pathNode.gCost = 99999999;
                pathNode.index = FindPathJob.CalculateIndex(x, y, gridSize.x);
                pathNode.isWalkable = walkables[x][y] == "1";
                 
                // if (walkables[x][y] != "1")
                // {
                //     Vector3 nodePos = new Vector3(x, y, 0) * cellSize + originPosition;
                //     Debug.DrawLine(nodePos+Vector3.left, nodePos + Vector3.right, Color.red, 100f);
                //     Debug.DrawLine(nodePos + Vector3.up, nodePos + Vector3.down, Color.red, 100f);
                // }
         
                pathNode.cameFromNodeIndex = -1;
         
                PNArray[pathNode.index] = pathNode;
            }
        }
    }

    public async Task<List<Vector3>> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        if(reading!=null) await reading;
        NativeList<Vector3> path = new NativeList<Vector3>(Allocator.TempJob);
        NativeArray<PathNode> pathNodeNativeArray = new NativeArray<PathNode>(PNArray, Allocator.TempJob);

        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);
        
        //float startTime = Time.realtimeSinceStartup;

        FindPathJob findPathJob = new FindPathJob {
            startPosition = new int2(startX, startY),
            endPosition = new int2(endX, endY),
            gridSize = gridSize,
            
            pathNodeArray = pathNodeNativeArray,
            pathVectors = path,
        };
        findPathJob.Schedule().Complete();

        List<Vector3> pathList = new List<Vector3>();
        
        foreach (var p in findPathJob.pathVectors)
        {
            pathList.Add(GetWorldPosition((int)p.x, (int)p.y));
        }

        pathList.Reverse();
        
        pathNodeNativeArray.Dispose();
        path.Dispose();
        //print("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f)) ;

        
        if (pathList.Count > 0) return pathList;
        return null;
    }
    
    Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    
    void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    
    [BurstCompile]
    private struct FindPathJob : IJob {

        public int2 startPosition;
        public int2 endPosition;
        public int2 gridSize;
        public NativeList<Vector3> pathVectors;
        public NativeArray<PathNode> pathNodeArray;

        public void Execute() {
            
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down
            neighbourOffsetArray[4] = new int2(-1, -1); // Left Down
            neighbourOffsetArray[5] = new int2(-1, +1); // Left Up
            neighbourOffsetArray[6] = new int2(+1, -1); // Right Down
            neighbourOffsetArray[7] = new int2(+1, +1); // Right Up

            // for(int i = 0; i < pathNodeArray.Length; i++)
            // {
            //     PathNode pathNode = pathNodeArray[i];
            //     pathNode.gCost = 99999999;
            //     pathNode.CalculateFCost();
            //     pathNode.cameFromNodeIndex = -1;
            //     
            //     pathNodeArray[pathNode.index] = pathNode;
            // }
            
            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);
            
            int cpt = 0;
            
            while (openList.Length > 0 && cpt<1000)
            {
                cpt++;
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex) {
                    // Reached our destination!
                    break;
                }

                // Remove current node from Open List
                for (int i = 0; i < openList.Length; i++) {
                    if (openList[i] == currentNodeIndex) {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++) {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighbourPosition, gridSize)) {
                        // Neighbour not valid position
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);

                    if (closedList.Contains(neighbourNodeIndex)) {
                        // Already searched this node
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if (!neighbourNode.isWalkable) {
                        // Not walkable
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

	                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
	                if (tentativeGCost < neighbourNode.gCost) {
		                neighbourNode.cameFromNodeIndex = currentNodeIndex;
		                neighbourNode.gCost = tentativeGCost;
		                neighbourNode.CalculateFCost();
		                pathNodeArray[neighbourNodeIndex] = neighbourNode;

		                if (!openList.Contains(neighbourNode.index)) {
			                openList.Add(neighbourNode.index);
		                }
	                }

                }
            }
            
            //print(cpt);
            
            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1) {
                // Didn't find a path!
                //Debug.Log("Didn't find a path!");
            } else {
                // Found a path
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
                /*
                foreach (int2 pathPosition in path) {
                    Debug.Log(pathPosition);
                }
                */
                foreach (var p in path)
                {
                    pathVectors.Add(new Vector3(p.x,p.y,0));
                }
                
                path.Dispose();
            }

            //pathNodeArray.Dispose();
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }
        
        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode) {
            if (endNode.cameFromNodeIndex == -1) {
                // Couldn't find a path!
                return new NativeList<int2>(Allocator.Temp);
            } else {
                // Found a path
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));

                PathNode currentNode = endNode;
                while (currentNode.cameFromNodeIndex != -1) {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }

                return path;
            }
        }

        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize) {
            return
                gridPosition.x >= 0 && 
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        public static int CalculateIndex(int x, int y, int gridWidth) {
            return x + y * gridWidth;
        }

        public static int CalculateDistanceCost(int2 aPosition, int2 bPosition) {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

    
        private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray) {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];
            for (int i = 1; i < openList.Length; i++) {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if (testPathNode.fCost < lowestCostPathNode.fCost) {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }
    }
    
    public struct PathNode {
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;

        public int cameFromNodeIndex;

        public void CalculateFCost() {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool isWalkable) {
            this.isWalkable = isWalkable;
        }
    }
}
