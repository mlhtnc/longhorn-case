using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [SerializeField]
    private Transform penTransformOnBoard;
    
    private StateMachine stateMachine;

    private void Start()
    {
        stateMachine = new StateMachine();

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var boardState  = new BoardState();
        var cupState    = new CupState();
        var plantState  = new PlantState();
        var binState    = new BinState();
        var doorState   = new DoorState();

        stateMachine.AddTransition(
            boardState,
            cupState,
            () => boardState.IsStateDone
        );

        stateMachine.AddTransition(
            cupState,
            plantState,
            () => cupState.IsStateDone
        );

        stateMachine.AddTransition(
            plantState,
            binState,
            () => plantState.IsStateDone
        );

        stateMachine.AddTransition(
            binState,
            doorState,
            () => binState.IsStateDone
        );

        stateMachine.SetState(boardState);
    }


    void Update()
    {
        stateMachine.Tick(Time.deltaTime);
    }
}
