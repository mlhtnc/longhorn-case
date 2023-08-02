using System;
using NotDecided.InputManagament;

public class BinState : IState
{
    public int Id => (int) GameState.BinState;

    public bool IsStateDone { get; private set; }

    private GameStateController gameStateController;

    private bool isCupDragged;

    public BinState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
    }

    public void OnEnter()
    {
        if(gameStateController.Cup.IsDragging == false)
        {
            ThrowCup();
        }

        gameStateController.Cup.OnDragStarted += OnDragStarted;
        InputManager.OnAnyPointerUp += OnAnyPointerUp;
    }

    public void OnExit()
    {
        gameStateController.Cup.OnDragStarted -= OnDragStarted;
        InputManager.OnAnyPointerUp -= OnAnyPointerUp;
    }

    private void OnDragStarted()
    {
        isCupDragged = true;
    }

    private void OnAnyPointerUp()
    {
        if(isCupDragged)
        {
            var hit = gameStateController.Raycast(gameStateController.CupLayerMask);

            if(hit.collider != null)
            {
                var bin = hit.collider.GetComponent<Bin>();
                if(bin != null)
                {
                    // TODO: Cup goes into bin animation

                    IsStateDone = true;
                }
            }
        }
    }

    private void ThrowCup()
    {
        UnityEngine.Debug.Log("Throw Cup");
    }
}
