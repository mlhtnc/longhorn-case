public class DoorState : IState
{
    public int Id => (int) GameState.DoorState;

    private GameStateController gameStateController;

    public DoorState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
    }

    public void OnEnter()
    {
        UIManager.Instance.StartDoorCircleAnimation();

        ClickableObject.OnAnyObjectClicked += OnAnyObjectClicked;
    }

    public void OnExit()
    {
        ClickableObject.OnAnyObjectClicked -= OnAnyObjectClicked;
    }

    private void OnAnyObjectClicked(ClickableObject obj)
    {
        var hit = gameStateController.Raycast(gameStateController.DoorLayerMask);

        if(hit.collider != null)
        {
            var door = hit.collider.GetComponent<Door>();
            if(door != null)
            {
                UIManager.Instance.OnLevelCompleted();
            }
        }
    }    
}
