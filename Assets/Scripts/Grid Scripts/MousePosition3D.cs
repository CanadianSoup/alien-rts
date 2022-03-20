using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D
{
    private static MousePosition3D _instance;
    
    public static MousePosition3D Instance {
        get
        {
            if (_instance == null)
            {
                _instance = new MousePosition3D();
            }

            return _instance;
        }

        private set { _instance = value; }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance = 100f;
        float duration = 1f;
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green, duration);


        if (Physics.Raycast(ray, out RaycastHit hit)) {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
