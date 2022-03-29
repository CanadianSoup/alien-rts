using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private CustomGrid<PathNode> grid;
    public int x;
    public int y;

    // Walking cost from start node
    public int gCost;
    // Heuristic cost to reach end node
    public int hCost;
    // G + H
    public int fCost;

    public PathNode cameFromNode;
    public PathNode(CustomGrid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
