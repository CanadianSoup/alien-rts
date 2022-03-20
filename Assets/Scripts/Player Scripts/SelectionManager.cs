using System.Collections.Generic;
using UnityEngine;

public class SelectionManager
{
    private static SelectionManager _instance;
    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SelectionManager();
            }

            return _instance;
        }

        private set { _instance = value; }
    }

    public HashSet<SelectableUnit> SelectedUnits = new HashSet<SelectableUnit>();
    public List<SelectableUnit> AvailableUnits = new List<SelectableUnit>();

    private SelectionManager() { }

    public void Select(SelectableUnit Unit)
    {
        SelectedUnits.Add(Unit);
        Unit.OnSelected();
        Debug.Log(Unit.name + " selected");
    }

    public void Deselect(SelectableUnit Unit)
    {
        Unit.OnDeselected();
        SelectedUnits.Remove(Unit);
        Debug.Log(Unit.name + " deselected");
    }

    public void DeselectAll()
    {
        foreach(SelectableUnit Unit in SelectedUnits)
        {
            Unit.OnDeselected();
        }
        SelectedUnits.Clear();
        Debug.Log("All units deselected");
    }

    public bool IsSelected(SelectableUnit Unit)
    {
        return SelectedUnits.Contains(Unit);
    }
}
