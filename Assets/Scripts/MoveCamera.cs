using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField]
    private float speed = 0.05f;
    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal") * speed;
        float yAxisValue = Input.GetAxis("Vertical") * speed;
        if (Camera.current != null)
        {
            Camera.current.transform.Translate(new Vector3(xAxisValue, yAxisValue, 0.0f));
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * 100f >= 0)
            {
                Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 100f;
            }
            
        }
    }
}
