using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - BuildingState.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 originalPosition = transform.position;
        Vector3 positionWithOffset = BuildingState.GetMouseWorldPosition() + offset;
        Vector3 cellPos = BuildingState.current.SnapCoordinateToGrid(positionWithOffset);
        transform.position = cellPos;
        if(BuildingState.current.CanBePlaced())
        {
            return;
        }
        else
        {
            transform.position = originalPosition;
        }
        
    }
}
