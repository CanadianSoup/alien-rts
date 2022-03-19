using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private RectTransform SelectionBox;
    [SerializeField]
    private LayerMask UnitLayers;
    [SerializeField]
    private LayerMask FloorLayers;

    private Vector2 StartMousePosition;

    private void Update()
    {
        HandleSelectionInputs();
    }

    private void HandleSelectionInputs()
    {
        // Left mouse button clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // So we don't see any residual value from the previous time
            SelectionBox.sizeDelta = Vector2.zero;

            SelectionBox.gameObject.SetActive(true);
            StartMousePosition = Input.mousePosition;
        }
        // Left mouse button held down
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            ResizeSelectionBox();
        }
        // Left mouse button released
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(false);
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - StartMousePosition.x;
        float height = Input.mousePosition.y - StartMousePosition.y;

        SelectionBox.anchoredPosition = StartMousePosition + new Vector2(width / 2, height / 2);

        // Using Abs so if there's a negative position, we won't have a negative size
        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        /* Since mouse input comes from the bottom of the screen (see Input.mousePosition documentation),
         * whenever we setup our canvas with the selection box, we'll anchor the selection box to the 
         * bottom left. This way our input mouse position and our anchored position will be the same point.
         * Generally, whenever using Unity UI, the pivot point is the center of the image: meaning we need
         * to take half the size of it into account.
         * 
         * We'll have the canvas scaler set to Constant Pixel Size which makes it so the canvas keeps the 
         * same scale all the time.
         * 
         * If we do a canvas scaler that is not Constant Pixel Size, we'll need to account for the size of 
         * the canvas vs the screen size. */

        //Compare where units are in screen space compared to selection rectangle
        Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);

        for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
        {

            /* We are selecting the unit before the user releases the mouse. If we wanted to select the unit
             * after, we would need to manage a different list of which objects have been selected in this 
             * function and then on release (in the last condition for HandleMovementInputs) we would select
             * the units only after they've let go */

            /* WorldToScreenPoint converts a unit in world space to our screen space coordinates so we
             * can check if that falls within the bounds we created for our selection box. We will pass
             * in the world space coordinates for our unit*/
            if(UnitIsInSelectionBox(Camera.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))
            {
                SelectionManager.Instance.Select(SelectionManager.Instance.AvailableUnits[i]);
            }
            else
            {
                SelectionManager.Instance.Deselect(SelectionManager.Instance.AvailableUnits[i]);
            }
        }
    }

    private bool UnitIsInSelectionBox(Vector2 Position, Bounds Bounds)
    {
        return Position.x > Bounds.min.x
            && Position.x < Bounds.max.x
            && Position.y > Bounds.min.y
            && Position.y < Bounds.max.y;
    }
}
