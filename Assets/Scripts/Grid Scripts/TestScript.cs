using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class TestScript : MonoBehaviour
{
    private CustomGrid grid;

    private void Start()
    {
        grid = new CustomGrid(4, 2, 3f, Vector3.zero);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(MousePosition3D.Instance.GetMouseWorldPosition(), 56);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(MousePosition3D.Instance.GetMouseWorldPosition()));
        }
    }

    private class HeatMapVisual
    {
        private CustomGrid grid;

        public HeatMapVisual(CustomGrid grid)
        {
            this.grid = grid;

            Vector3[] vertices;
            Vector2[] uv;
            int[] triangles;

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {

                }
            }
        }
    }
}
