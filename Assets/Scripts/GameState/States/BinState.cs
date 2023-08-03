using System;
using NotDecided.InputManagament;
using UnityEngine;

public class BinState : IState
{
    public bool IsStateDone { get; private set; }

    private GameStateController gameStateController;

    public int Id => (int) GameState.BinState;


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
        gameStateController.Cup.OnDragStopped += OnDragStopped;
    }

    public void OnExit()
    {
        gameStateController.Cup.OnDragStarted -= OnDragStarted;
        gameStateController.Cup.OnDragStopped -= OnDragStopped;
    }

    private void OnDragStarted()
    {
        gameStateController.Cup.GetComponent<Cup>().DisableRigidbody();
        gameStateController.Cup.EnableDrag();
    }

    private void OnDragStopped()
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
    }

    private void ThrowCup()
    {
        gameStateController.Cup.GetComponent<Cup>().ThrowCup();
    }
}
