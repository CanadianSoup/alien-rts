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
        Vector3 originalPosition = transform.position;
        Vector3 positionWithOffset = BuildingSystem.GetMouseWorldPosition() + offset;
        Vector3 cellPos = BuildingSystem.current.SnapCoordinateToGrid(positionWithOffset);
        transform.position = cellPos;
        if(BuildingSystem.current.CanBePlaced())
        {
            return;
        }
        else
        {
            transform.position = originalPosition;
        }
        
    }
}
