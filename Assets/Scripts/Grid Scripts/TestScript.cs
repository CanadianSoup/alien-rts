using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestScript : MonoBehaviour
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


public class HeatMapGridObject
{

    private const int MIN = 0;
    private const int MAX = 100;

    private CustomGrid<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value; 

    public HeatMapGridObject(CustomGrid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }
    public int GetValue()
    {
        return value;
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}