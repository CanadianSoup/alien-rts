using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    private Grid grid;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = BuildingSystem.GetMouseWorldPosition();
            PlaceCubeNear(pos);
        }
        if(Input.GetMouseButtonDown(1))
        {
            GameObject gameObject = BuildingSystem.GetClickedObject();
            if(gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void PlaceCubeNear(Vector3 clickPoint)
    {
        Vector3Int cellPos = grid.GetComponent<GridLayout>().WorldToCell(clickPoint);
        Vector3 position = grid.GetCellCenterWorld(cellPos);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.AddComponent<PlaceableObject>();

    }
}
