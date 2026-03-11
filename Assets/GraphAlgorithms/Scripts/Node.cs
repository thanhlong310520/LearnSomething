using System.Collections.Generic;
using UnityEngine;

namespace GraphAlgorithms
{
    public class Node : MonoBehaviour
    {
        public List<Edge> neighbors = new List<Edge>();
        public void AddNeighbor(Edge neighbor)
        {
            if (!neighbors.Contains(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }
    }

    [System.Serializable] public struct Edge
    {
        public Node target;
        public float dis;
    }
}