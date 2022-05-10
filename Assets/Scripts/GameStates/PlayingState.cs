using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : GameState
{
    #region Variables

    private RectTransform SelectionBox;
    private LayerMask UnitLayers;
    private LayerMask FloorLayers;

    private float DragDelay = 0.1f;

    private float MouseDownTime;
    private Vector2 StartMousePosition;

    #endregion

    public PlayingState(GameController gameController, GameStateMachine gameStateMachine) : base(gameController, gameStateMachine)
    {
        this.SelectionBox = gameController.SelectionBox;
        this.UnitLayers = gameController.UnitLayers;
        this.FloorLayers = gameController.FloorLayers;
    }

    #region State Methods

    public override void HandleInput()
    {
        base.HandleInput();

        HandleSelectionInputs();
        HandleMovementInputs();
    }

    #endregion

    #region Local Methods

    private void HandleMovementInputs()
    {
        // If right click and we have something selected
        if (Input.GetKeyUp(KeyCode.Mouse1) && SelectionManager.Instance.SelectedUnits.Count > 0)
        {
            // If we clicked somewhere on the floor
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit Hit, this.gameController.FloorLayers))
            {
                foreach (SelectableUnit Unit in SelectionManager.Instance.SelectedUnits)
                {
                    Unit.MoveTo(Hit.point);
                }
            }
        }
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
            MouseDownTime = Time.time;
        }
        // Left mouse button held down
        else if (Input.GetKey(KeyCode.Mouse0) && MouseDownTime + DragDelay < Time.time)
        {
            ResizeSelectionBox();
        }
        // Left mouse button released
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(false);

            if (
                // If we hit something, we'll have a reference to a RaycastHit
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, UnitLayers)
                // Check if it has a selectable unit on it and get that reference
                && hit.collider.TryGetComponent<SelectableUnit>(out SelectableUnit unit))
            {
                // If we're holding shift, add to selected units
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.Instance.IsSelected(unit))
                    {
                        SelectionManager.Instance.Deselect(unit);
                    }
                    else
                    {
                        SelectionManager.Instance.Select(unit);
                    }
                }
                // If we're not holding shift, select that one unit
                else
                {
                    SelectionManager.Instance.DeselectAll();
                    SelectionManager.Instance.Select(unit);
                }
            }
            // Deselect all if not clicking SelectableUnit and not doing SelectionBox
            else if (MouseDownTime + DragDelay > Time.time)
            {
                SelectionManager.Instance.DeselectAll();
            }

            MouseDownTime = 0;
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
            if (UnitIsInSelectionBox(Camera.main.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))
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

    #endregion
}
