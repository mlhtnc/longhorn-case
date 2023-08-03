using NotDecided.InputManagament;
using UnityEngine;

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
        else
        {
            isCupDragged = true;
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
        gameStateController.Cup.GetComponent<Cup>().DisableRigidbody();
        gameStateController.Cup.EnableDrag();
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
                    var animTime = 0.4f;

                    var cupGo = gameStateController.Cup.gameObject;
                    
                    LeanTween.scale(cupGo, Vector3.one * 0.1f, animTime).setEaseInExpo();
                    LeanTween.moveY(cupGo, bin.transform.position.y, animTime).setEaseInBack();
                    LeanTween.moveX(cupGo, bin.transform.position.x, animTime);
                    LeanTween.moveZ(cupGo, bin.transform.position.z, animTime)
                    .setOnComplete(() => {
                        cupGo.SetActive(false);
                    });

                    IsStateDone = true;
                }
                else
                {
                    ThrowCup();
                }
            }

            isCupDragged = false;
        }
    }

    private void ThrowCup()
    {
        gameStateController.Cup.GetComponent<Cup>().ThrowCup();
    }
}
