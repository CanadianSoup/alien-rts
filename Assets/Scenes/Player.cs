using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera _camera;
    private UnityEngine.AI.NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        if(_camera == null)
        {
            Debug.LogError("The Camera is null on the player controller");
        }

        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!Input.GetMouseButton(0)) return;

        RaycastHit hit;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            _agent.destination = hit.point;
        }
    }
}
