using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public delegate void DoneBuildingDelegate();

public class BuildingState : GameState
{
    #region Variables

    public static BuildingState current;

    public event DoneBuildingDelegate DoneBuildingEvent;

    public GridLayout GridLayout;
    private Grid Grid;
    private Tilemap MainTilemap;
    private TileBase WhiteTile;
    private GameObject Floor;
    private Button Button;

    private PlaceableObject ObjectToPlace;

    #endregion

    public BuildingState(GameController gameController, GameStateMachine gameStateMachine) : base(gameController, gameStateMachine)
    {
        current = this;
        GridLayout = gameController.GridLayout;
        Grid = GridLayout.gameObject.GetComponent<Grid>();
        MainTilemap = gameController.MainTilemap;
        WhiteTile = (TileBase) Resources.Load("white_tile");
        Floor = gameController.Floor;
        Button = gameController.Button;
    }

    #region State Methods


    public override void HandleInput()
    {
        base.HandleInput();

        if (!ObjectToPlace)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanBePlaced())
            {
                Debug.Log("Object can be placed");
                ObjectToPlace.Place();
                TakeArea();
                SendDoneBuildingEvent();
            }
            else
            {
                Debug.Log("Object cannot be placed");
                Object.Destroy(ObjectToPlace.gameObject);
                SendDoneBuildingEvent();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Object.Destroy(ObjectToPlace.gameObject);
            SendDoneBuildingEvent();
        }
    }

    #endregion

    #region Local Utils

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
        Vector3Int cellPos = GridLayout.WorldToCell(position);
        position = Grid.GetCellCenterWorld(cellPos);
        if (ObjectToPlace)
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
        Mesh objectMesh = ObjectToPlace.GetComponent<MeshFilter>().mesh;
        Mesh floorMesh = Floor.GetComponent<MeshFilter>().mesh;

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

        return ObjectToPlace.transform.position.y;

    }

    #endregion

    #region Local Methods

    public void InitializeWithObject(GameObject prefab)
    {
        GameObject obj = Object.Instantiate(prefab);
        ObjectToPlace = obj.GetComponent<PlaceableObject>();
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
        area.position = GridLayout.WorldToCell(ObjectToPlace.GetStartPosition());
        area.size = ObjectToPlace.Size;

        TileBase[] baseArray = MainTilemap.GetTilesBlock(area);

        foreach (var b in baseArray)
        {
            if (b == WhiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea()
    {
        BoundsInt area = new BoundsInt();
        area.position = GridLayout.WorldToCell(ObjectToPlace.GetStartPosition());
        area.size = ObjectToPlace.Size;

        TileBase[] tileArray = new TileBase[area.size.x * area.size.y];
        for (int index = 0; index < tileArray.Length; index++)
        {
            tileArray[index] = WhiteTile;
        }
        MainTilemap.SetTilesBlock(area, tileArray);
    }

    public void SendDoneBuildingEvent()
    {
        if (DoneBuildingEvent != null)
        {
            DoneBuildingEvent();
        }
        else
        {
            Debug.LogWarning("DoneBuildingEvent is NULL");
        }
    }

    #endregion
}
