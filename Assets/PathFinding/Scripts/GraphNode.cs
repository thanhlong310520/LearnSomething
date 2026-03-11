using System.Collections.Generic;
using UnityEngine;
namespace AStar
{

    public class GraphNode
    {
        public Vector3 position;

        public List<GraphNode> neighbors = new List<GraphNode>();

        public float gCost;
        public float hCost;

        public float FCost => gCost + hCost;

        public GraphNode parent;

        public GraphNode(Vector3 pos)
        {
            position = pos;
        }
    }
}