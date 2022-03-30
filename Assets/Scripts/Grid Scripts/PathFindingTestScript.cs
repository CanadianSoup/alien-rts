using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTestScript : MonoBehaviour
{
    [SerializeField]
    private PathfindingVisual pathfindingVisual;

    private Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(10, 10);

        Debug.Log(pathfinding.GetGrid().ToString());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = MousePosition3D.Instance.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.black, 1f);
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = MousePosition3D.Instance.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
}
