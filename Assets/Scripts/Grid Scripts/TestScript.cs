using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestScript : MonoBehaviour
{
    [SerializeField]
    private HeatMapVisual heatMapVisual;

    private CustomGrid grid;

    private void Start()
    {
        grid = new CustomGrid(50, 50, 1f, Vector3.zero);

        heatMapVisual.SetGrid(grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = MousePosition3D.Instance.GetMouseWorldPosition();
            grid.AddValue(mouseWorldPosition, 100, 5, 40);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(MousePosition3D.Instance.GetMouseWorldPosition()));
        }

        
    }
}
