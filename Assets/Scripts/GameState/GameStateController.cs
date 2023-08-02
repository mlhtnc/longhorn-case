using UnityEngine;

public class GameStateController : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private Transform penTransformOnBoard;
    
    [SerializeField]
    private Transform dropPenTransform;

    [SerializeField]
    private Transform dropCupTransform;

    [SerializeField]
    private Transform cupPlantTransform;

    [SerializeField]
    private DraggableObject cup;

    [SerializeField]
    private WaterDispenser waterDispenser;

    [SerializeField]
    private LayerMask cupLayerMask;

    #endregion

    #region Fields

    private StateMachine stateMachine;

    private Camera mainCamera;

    #endregion

    #region Properties

    public Transform PenTransformOnBoard => penTransformOnBoard;

    public Transform DropPenTransform => dropPenTransform;

    public DraggableObject Cup => cup;

    public LayerMask CupLayerMask => cupLayerMask;

    public Transform DropCupTransform => dropCupTransform;

    public Transform CupPlantTransform => cupPlantTransform;

    public WaterDispenser WaterDispenser => waterDispenser;

    public Vector3 InitialCupPos { get; private set; }

    #endregion

    private void Start()
    {
        stateMachine = new StateMachine();
        mainCamera   = Camera.main;

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        cup.DisableDrag();
        InitialCupPos = Cup.transform.position;

        var boardState  = new BoardState(this);
        var cupState    = new CupState(this);
        var plantState  = new PlantState(this);
        var binState    = new BinState(this);
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

    public RaycastHit Raycast(LayerMask layerMask)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var distance = mainCamera.farClipPlane - mainCamera.nearClipPlane;

        Physics.Raycast(
            ray,
            out RaycastHit hit,
            mainCamera.farClipPlane - mainCamera.nearClipPlane,
            layerMask
        );

        return hit;
    }
}
