using AStar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AStar
{
    public class GridMap
    {
        public int width;
        public int height;

        public GridNode[,] grid;

        public GridMap(int width, int height)
        {
            this.width = width;
            this.height = height;

            grid = new GridNode[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new GridNode(x, y, true);
                }
            }
        }

        public List<GridNode> GetNeighbors(GridNode node)
        {
            List<GridNode> neighbors = new List<GridNode>();

            int[,] dirs =
            {
            {0,1},
            {0,-1},
            {1,0},
            {-1,0}
        };

            for (int i = 0; i < 4; i++)
            {
                int nx = node.x + dirs[i, 0];
                int ny = node.y + dirs[i, 1];

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    neighbors.Add(grid[nx, ny]);
            }

            return neighbors;
        }
    }

}
