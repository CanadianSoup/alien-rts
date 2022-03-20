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
        grid = new CustomGrid(4, 2, 3f);

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
}
