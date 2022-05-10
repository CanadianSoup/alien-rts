using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Variables

    public GameStateMachine gameStateMachine;
    public BuildingState buildingState;
    public PlayingState playingState;

    [SerializeField]
    public RectTransform SelectionBox;
    [SerializeField]
    public LayerMask UnitLayers;
    [SerializeField]
    public LayerMask FloorLayers;
    [SerializeField]
    public GridLayout GridLayout;
    [SerializeField]
    public Tilemap MainTilemap;
    [SerializeField]
    public GameObject Floor;

    public Button Button;

    #endregion

    #region Methods


    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        gameStateMachine = new GameStateMachine();

        buildingState = new BuildingState(this, gameStateMachine);
        playingState = new PlayingState(this, gameStateMachine);

        gameStateMachine.Initialize(playingState);
    }

    private void Update()
    {
        gameStateMachine.CurrentState.HandleInput();

        gameStateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        gameStateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

}
