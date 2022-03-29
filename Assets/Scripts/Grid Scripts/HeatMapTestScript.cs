using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeatMapTestScript : MonoBehaviour
{
    [SerializeField]
    private HeatMapVisual heatMapVisual;

    [SerializeField]
    private HeatMapBoolVisual heatMapBoolVisual;

    [SerializeField]
    private HeatMapGenericVisual heatMapGenericVisual;

    private CustomGrid<HeatMapGridObject> grid;


    private void Start()
    {
        grid = new CustomGrid<HeatMapGridObject>(10, 10, 1f, Vector3.zero, (CustomGrid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

        //heatMapVisual.SetGrid(grid);
        //heatMapBoolVisual.SetGrid(grid);
        heatMapGenericVisual.SetGrid(grid);


    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = MousePosition3D.Instance.GetMouseWorldPosition();
            //heatMapVisual.AddValue(grid, mouseWorldPosition, 100, 5, 40);
            //grid.SetValue(mouseWorldPosition, true);
            HeatMapGridObject he = grid.GetGridObject(mouseWorldPosition);
            if(he != null)
            {
                he.AddValue(1);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetGridObject(MousePosition3D.Instance.GetMouseWorldPosition()));
        }

        
    }
}