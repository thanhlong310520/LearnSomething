using System.Collections.Generic;
using UnityEngine;

namespace GraphAlgorithms
{
    public class Node : MonoBehaviour
    {
        public List<Node> neighbors = new List<Node>();
        public void AddNeighbor(Node neighbor)
        {
            if (!neighbors.Contains(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }
    }
}