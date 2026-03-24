using System.Collections.Generic;
using UnityEngine;
namespace AStar
{

    public class GridNode
    {
        public int x;
        public int y;

        public bool walkable;

        public int gCost;
        public int hCost;

        public int FCost => gCost + hCost;

        public GridNode parent;

        public GridNode(int x, int y, bool walkable)
        {
            this.x = x;
            this.y = y;
            this.walkable = walkable;
        }
    }
}