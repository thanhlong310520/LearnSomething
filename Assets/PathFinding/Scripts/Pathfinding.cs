using AStar;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public static class Pathfinding
    {
        
        public static List<GridNode> AstarPath(GridMap grid, Vector2Int startPos, Vector2Int goalPos)
        {
            GridNode start = grid.grid[startPos.x, startPos.y];
            GridNode goal = grid.grid[goalPos.x, goalPos.y];

            List<GridNode> openSet = new List<GridNode>();
            HashSet<GridNode> closedSet = new HashSet<GridNode>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                GridNode current = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < current.FCost ||
                       openSet[i].FCost == current.FCost &&
                       openSet[i].hCost < current.hCost)
                    {
                        current = openSet[i];
                    }
                }

                openSet.Remove(current);
                closedSet.Add(current);

                if (current == goal)
                    return RetracePath(start, goal);

                foreach (var neighbor in grid.GetNeighbors(current))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;

                    int newCost = current.gCost + Distance(current, neighbor);

                    if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = Distance(neighbor, goal);
                        neighbor.parent = current;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return null;
        }

        static List<GridNode> RetracePath(GridNode start, GridNode end)
        {
            List<GridNode> path = new List<GridNode>();

            GridNode current = end;

            while (current != start)
            {
                path.Add(current);
                current = current.parent;
            }

            path.Reverse();
            return path;
        }

        static int Distance(GridNode a, GridNode b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }
}