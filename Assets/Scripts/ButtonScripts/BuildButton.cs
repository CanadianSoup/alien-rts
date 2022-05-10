using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildButton : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameController gameController;

    IEnumerator Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(StartBuilding);

        if (gameController == null)
        {
            gameController = GameObject.FindObjectOfType<GameController>();
        }

        yield return new WaitUntil(() => gameController.buildingState != null);
        gameController.buildingState.DoneBuildingEvent += StopBuilding;

    }

    private void StartBuilding()
    {
        gameObject.GetComponent<Button>().interactable = false;
        gameController.gameStateMachine.ChangeState(gameController.buildingState);
        gameController.buildingState.InitializeWithObject(prefab);
    }

    private void StopBuilding()
    {
        gameObject.GetComponent<Button>().interactable = true;
        gameController.gameStateMachine.ChangeState(gameController.playingState);
    }
}
