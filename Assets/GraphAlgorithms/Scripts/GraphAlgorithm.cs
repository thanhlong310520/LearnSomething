using System.Collections.Generic;
using System;
using UnityEngine;

namespace GraphAlgorithms
{
    public static class GraphAlgorithm
    {
        public static void BFS(Node start, Action<Node> ActionHandleNode)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                ActionHandleNode?.Invoke(current);

                foreach (Node neighbor in current.neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        public static List<Node> BFSPath(Node start, Node goal)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current == goal)
                    break;

                foreach (Node neighbor in current.neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        parent[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return ReconstructPath(parent, start, goal);
        }


        public static void DFS(Node start, Action<Node> ActionHandleNode)
        {
            Stack<Node> stack = new Stack<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                ActionHandleNode?.Invoke(current);


                foreach (Node neighbor in current.neighbors)
                {
                    stack.Push(neighbor);
                }
            }
        }
        public static List<Node> DFSPath(Node start, Node goal)
        {
            Stack<Node> stack = new Stack<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                if (current == goal)
                    break;

                foreach (Node neighbor in current.neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        parent[neighbor] = current;
                        stack.Push(neighbor);
                    }
                }
            }

            return ReconstructPath(parent, start, goal);
        }



        static List<Node> ReconstructPath(Dictionary<Node, Node> parent, Node start, Node goal)
        {
            List<Node> path = new List<Node>();

            Node current = goal;

            while (current != start)
            {
                path.Add(current);
                current = parent[current];
            }

            path.Add(start);
            path.Reverse();

            return path;
        }
    }
}