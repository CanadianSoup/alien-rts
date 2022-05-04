using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{

    public GameObject[] objects;
    public GameObject pendingObject;

    private Vector3 currentPosition;
    private Vector3 previousPosition;

    private RaycastHit hit;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    public GameObject floor;

    public float gridSize;

    [SerializeField]
    public float rotateAmount = 45;


    private void Update()
    {
        if(pendingObject != null)
        {
            Debug.Log("IsOverlappingAnotherObject() " + IsOverlappingAnotherObject());
            if (!IsOverlappingAnotherObject())
            {
                previousPosition = currentPosition;
                pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(currentPosition.x),
                    VerticallySnapToFloor(),
                    RoundToNearestGrid(currentPosition.z));
            }
            else
            {
                pendingObject.transform.position = previousPosition;
            }
           

            if(Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
        }
    }

    public void PlaceObject()
    {

        if (!IsOverlappingAnotherObject())
        {
            Debug.Log("Pending object is null");
            pendingObject = null;
        }
    }

    public void RotateObject()
    {
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            currentPosition = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        // transform.rotation will be 0,0,0 on the empty GameObject BuildingManager
        pendingObject = Instantiate(objects[index], currentPosition, transform.rotation);

    }

    private float RoundToNearestGrid(float pos)
    {
        // Get remainder of position/gridSize
        float xDiff = pos % gridSize;

        // Round the number down
        pos -= xDiff;

        // Round the number up in this case
        if(xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }

    private float VerticallySnapToFloor()
    {

        Mesh objectMesh = pendingObject.GetComponent<MeshFilter>().mesh;
        Mesh floorMesh = floor.GetComponent<MeshFilter>().mesh;

        Bounds objectBounds = objectMesh.bounds;
        Bounds floorBounds = floorMesh.bounds;

        float objectLowerBounds = objectBounds.min.y;
        float floorUpperBounds = floorBounds.max.y;

        if (objectLowerBounds < floorUpperBounds)
        {
            return floorUpperBounds - objectLowerBounds;
        }
        else if (objectLowerBounds > floorUpperBounds)
        {
            return objectLowerBounds - floorUpperBounds;
        }

        return pendingObject.transform.position.y;

    }

    private bool IsOverlappingAnotherObject()
    {
        Collider[] colliders = Physics.OverlapSphere(pendingObject.transform.position, 1)
            .Where(
            // Exclude the floor
            c => c.gameObject.layer != Mathf.RoundToInt(Mathf.Log(layerMask.value, 2))
            // Exclude itself
            && c.gameObject != pendingObject
            ).ToArray();

        return colliders.Length > 0;
    }
}


