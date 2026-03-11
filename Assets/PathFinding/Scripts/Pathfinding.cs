using AStar;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public static class Pathfinding
    {
        public static List<GraphNode> GraphAStarFindPath(GraphNode startNode, GraphNode targetNode)
        {
            List<GraphNode> openSet = new List<GraphNode>();
            HashSet<GraphNode> closedSet = new HashSet<GraphNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                GraphNode current = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < current.FCost ||
                       (openSet[i].FCost == current.FCost && openSet[i].hCost < current.hCost))
                    {
                        current = openSet[i];
                    }
                }

                openSet.Remove(current);
                closedSet.Add(current);

                if (current == targetNode)
                    return RetracePath(startNode, targetNode);

                foreach (var neighbor in current.neighbors)
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    float newCost = current.gCost + Distance(current, neighbor);

                    if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = Distance(neighbor, targetNode);
                        neighbor.parent = current;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return null;
        }

        static List<GraphNode> RetracePath(GraphNode start, GraphNode end)
        {
            List<GraphNode> path = new List<GraphNode>();
            GraphNode current = end;

            while (current != start)
            {
                path.Add(current);
                current = current.parent;
            }

            path.Reverse();
            return path;
        }

        static float Distance(GraphNode a, GraphNode b)
        {
            return Vector3.Distance(a.position, b.position);
        }
    }
}