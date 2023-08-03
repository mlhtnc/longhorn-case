using System;
using NotDecided.InputManagament;
using UnityEngine;

public class CupState : IState
{
    public int Id => (int) GameState.CupState;

    public bool IsStateDone { get; private set; }

    private GameStateController gameStateController;

    private bool isCupDragged;

    private bool isCupPlacedIntoWaterDispenser;

    public CupState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
    }

    public void OnEnter()
    {
        this.gameStateController.Cup.EnableDrag();

        gameStateController.Cup.OnDragStarted += OnDragStarted;
        InputManager.OnAnyPointerUp += OnAnyPointerUp;
        ClickableObject.OnAnyObjectClicked += OnAnyObjClicked;
    }

    public void OnExit()
    {
        gameStateController.Cup.OnDragStarted -= OnDragStarted;
        InputManager.OnAnyPointerUp -= OnAnyPointerUp;
        ClickableObject.OnAnyObjectClicked -= OnAnyObjClicked;
    }

    private void OnDragStarted()
    {
        isCupDragged = true;
    }

    // CHANGE: Change this with OnDragStopped ?
    private void OnAnyPointerUp()
    {
        if(isCupDragged == false || isCupPlacedIntoWaterDispenser)
            return;

        var hit = gameStateController.Raycast(gameStateController.CupLayerMask);

        if(hit.collider != null)
        {
            var waterDispenser = hit.collider.GetComponent<WaterDispenser>();
            if(waterDispenser != null)
            {
                this.gameStateController.Cup.DisableDrag();
                LeanTween.move(gameStateController.Cup.gameObject, gameStateController.DropCupTransform.position, 0.2f);

                isCupPlacedIntoWaterDispenser = true;

                return;
            }            
        }

        LeanTween.move(gameStateController.Cup.gameObject, gameStateController.InitialCupPos, 0.3f);
    }
    
    private void OnAnyObjClicked(ClickableObject obj)
    {
        if(isCupPlacedIntoWaterDispenser == false)
            return;
        
        var waterDispenser = obj.GetComponent<WaterDispenser>();
        if(waterDispenser != null)
        {
            var seq = LeanTween.sequence();

            seq.append(() => {
                // Play particle here
            });
            seq.append(1f);

            seq.append(() => {
                var renderer = gameStateController.Cup.GetComponent<Renderer>();
                renderer.material.color = Color.blue;

                IsStateDone = true;

                this.gameStateController.Cup.EnableDrag();
                waterDispenser.GetComponent<BoxCollider>().enabled = false;
            });
        }
    }
}
