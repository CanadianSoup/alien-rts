using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public float scrollSpeed = 100f;
    public float minY = 5f;
    public float maxY = 300f;

    void Update()
    {
        float xAxisValue = 0.0f;
        float yAxisValue = 0.0f;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            yAxisValue += panSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.y <= panBorderThickness)
        {
            yAxisValue -= panSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Vertical") != 0f)
        {
            yAxisValue = Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;
        }


        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
           xAxisValue += panSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x <= panBorderThickness)
        {
            xAxisValue -= panSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Horizontal") != 0f)
        {
            xAxisValue = Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
        }

        Camera.main.transform.Translate(new Vector3(xAxisValue, yAxisValue, 0.0f));

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float zoom = Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * 100f * scrollSpeed * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Clamp(zoom, minY, maxY);
        }
    }
}
