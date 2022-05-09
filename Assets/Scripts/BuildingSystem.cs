using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField]
    private Tilemap MainTilemap;
    [SerializeField]
    private TileBase whiteTile;
    [SerializeField]
    private GameObject floor;

    private PlaceableObject objectToPlace;

    public List<Vector3> availablePlaces;

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (!objectToPlace)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanBePlaced())
            {
                Debug.Log("Object can be placed");
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea();
            }
            else
            {
                Debug.Log("Object cannot be placed");
                Destroy(objectToPlace.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(objectToPlace.gameObject);
        }
    }

    #endregion

    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static GameObject GetClickedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        if(objectToPlace)
        {
            return new Vector3(position.x, VerticallySnapToFloor(), position.z);
        }
        else
        {
            return position;
        }
    }

    private float VerticallySnapToFloor()
    {
        Mesh objectMesh = objectToPlace.GetComponent<MeshFilter>().mesh;
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

        return objectToPlace.transform.position.y;

    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();

        // Make transparent when placing
        Color objectColor = obj.GetComponent<Renderer>().material.color;
        objectColor.a = 0.75f;
        obj.GetComponent<Renderer>().material.color = objectColor;

        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());
        obj.transform.position = position;
    }

    public bool CanBePlaced()
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = objectToPlace.Size;

        TileBase[] baseArray = MainTilemap.GetTilesBlock(area);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea()
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = objectToPlace.Size;

        TileBase[] tileArray = new TileBase[area.size.x * area.size.y];
        for (int index = 0; index < tileArray.Length; index++)
        {
            tileArray[index] = whiteTile;
        }
        MainTilemap.SetTilesBlock(area, tileArray);
    }

    public bool PlacingObject()
    {
        return objectToPlace != null && !objectToPlace.Placed;
    }

    #endregion
}
