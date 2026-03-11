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
            Node current;
            while (queue.Count > 0)
            {
                current = queue.Dequeue();

                ActionHandleNode?.Invoke(current);

                foreach (Edge neighbor in current.neighbors)
                {
                    if (!visited.Contains(neighbor.target))
                    {
                        visited.Add(neighbor.target);
                        queue.Enqueue(neighbor.target);
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
            Node current;
            while (queue.Count > 0)
            {
                current = queue.Dequeue();

                if (current == goal)
                    break;

                foreach (Edge neighbor in current.neighbors)
                {
                    if (!visited.Contains(neighbor.target))
                    {
                        visited.Add(neighbor.target);
                        parent[neighbor.target] = current;
                        queue.Enqueue(neighbor.target);
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
            Node current;

            while (stack.Count > 0)
            {
                current = stack.Pop();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                ActionHandleNode?.Invoke(current);


                foreach (Edge neighbor in current.neighbors)
                {
                    stack.Push(neighbor.target);
                }
            }
        }
        public static List<Node> DFSPath(Node start, Node goal)
        {
            Stack<Node> stack = new Stack<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            stack.Push(start);
            Node current;
            while (stack.Count > 0)
            {
                current = stack.Pop();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                if (current == goal)
                    break;

                foreach (Edge neighbor in current.neighbors)
                {
                    if (!visited.Contains(neighbor.target))
                    {
                        parent[neighbor.target] = current;
                        stack.Push(neighbor.target);
                    }
                }
            }

            return ReconstructPath(parent, start, goal);
        }


        public static Dictionary<Node, float> Dijkstra(Node start)
        {
            var distance = new Dictionary<Node, float>();
            PriorityQueue<Node> queue = new PriorityQueue<Node>();

            distance[start] = 0;
            queue.Enqueue(start, 0);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                foreach (var edge in current.neighbors)
                {
                    Node neighbor = edge.target;
                    float newDist = distance[current] + edge.dis;

                    if (!distance.ContainsKey(neighbor) || newDist < distance[neighbor])
                    {
                        distance[neighbor] = newDist;
                        queue.Enqueue(neighbor, newDist);
                    }
                }
            }

            return distance;
        }
        public static List<Node> DijkstraPath(Node start, Node goal)
        {
            var distance = new Dictionary<Node, float>();
            var parent = new Dictionary<Node, Node>();
            var queue = new PriorityQueue<Node>();
            bool foundPath = false;

            distance[start] = 0;
            queue.Enqueue(start, 0);
            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current == goal)
                {
                    foundPath = true;
                    break;
                }

                foreach (var edge in current.neighbors)
                {
                    Node neighbor = edge.target;

                    float newDist = distance[current] + edge.dis;

                    if (!distance.ContainsKey(neighbor) || newDist < distance[neighbor])
                    {
                        distance[neighbor] = newDist;
                        parent[neighbor] = current;

                        queue.Enqueue(neighbor, newDist);
                    }
                }
            }

            if (!foundPath) return null;

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

    public class PriorityQueue<T>
    {
        private List<(T item, float priority)> heap = new List<(T, float)>();

        public int Count => heap.Count;

        public void Enqueue(T item, float priority)
        {
            heap.Add((item, priority));
            HeapifyUp(heap.Count - 1);
        }

        public T Dequeue()
        {
            var root = heap[0].item;

            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);

            HeapifyDown(0);

            return root;
        }
        void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;

                if (heap[index].priority >= heap[parent].priority)
                    break;

                Swap(index, parent);

                index = parent;
            }
        }
        void HeapifyDown(int index)
        {
            while (true)
            {
                int left = index * 2 + 1;
                int right = index * 2 + 2;
                int smallest = index;

                if (left < heap.Count && heap[left].priority < heap[smallest].priority)
                    smallest = left;

                if (right < heap.Count && heap[right].priority < heap[smallest].priority)
                    smallest = right;

                if (smallest == index)
                    break;

                Swap(index, smallest);
                index = smallest;
            }
        }
        void Swap(int a, int b)
        {
            var temp = heap[a];
            heap[a] = heap[b];
            heap[b] = temp;
        }
    }
}