using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        Vector3 cellPos = BuildingSystem.current.SnapCoordinateToGrid(pos);
        Debug.Log("Cell pos is " + cellPos);
        transform.position = cellPos;
    }
}
