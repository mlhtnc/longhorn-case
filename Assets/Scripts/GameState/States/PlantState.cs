using System;
using NotDecided.InputManagament;
using UnityEngine;

public class PlantState : IState, IStateTickable
{
    public int Id => (int) GameState.PlantState;

    public bool IsStateDone { get; private set; }

    private GameStateController gameStateController;

    private const float PlantTime = 5f;

    private float plantProgress;

    private bool isCupDragged;


    public PlantState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
    }

    public void OnEnter()
    {
        gameStateController.Cup.OnDragStarted += OnDragStarted;
        InputManager.OnAnyPointerUp           += OnAnyPointerUp;
    }

    public void OnExit()
    {
        gameStateController.Cup.OnDragStarted -= OnDragStarted;
        InputManager.OnAnyPointerUp           -= OnAnyPointerUp;
    }

    public void Tick(float deltaTime)
    {
        if(isCupDragged == false)
            return;

        var hit = gameStateController.Raycast(gameStateController.CupLayerMask);

        if(hit.collider != null)
        {
            var plant = hit.collider.GetComponent<Plant>();
            if(plant != null)
            {
                if(LeanTween.isTweening(gameStateController.Cup.gameObject) == false &&
                    gameStateController.Cup.transform.eulerAngles != gameStateController.CupPlantTransform.eulerAngles)
                {
                    LeanTween.rotate(gameStateController.Cup.gameObject, gameStateController.CupPlantTransform.eulerAngles, 0.3f);
                }

                // Water plant particle loop start

                plantProgress += Time.deltaTime;
                if(plantProgress >= PlantTime)
                {
                    IsStateDone = true;
                }
                else
                {
                    var color = Color.Lerp(Color.blue, Color.white, plantProgress / PlantTime);
                    var renderer = gameStateController.Cup.GetComponent<Renderer>();
                    renderer.material.color = color;
                }
            }
            else
            {
                if(LeanTween.isTweening(gameStateController.Cup.gameObject) == false &&
                    gameStateController.Cup.transform.eulerAngles != gameStateController.DropCupTransform.eulerAngles)
                {
                    LeanTween.rotate(gameStateController.Cup.gameObject, gameStateController.DropCupTransform.eulerAngles, 0.3f);
                }

                // Water plant particle loop stop
            }
        }        
    }

    private void OnDragStarted()
    {
        // NOTE: We disabled collider of water dispenser because cup was inside of it and not clickable
        // Now, we need to enable collider back to make sure cup position updates are correct
        gameStateController.WaterDispenser.GetComponent<BoxCollider>().enabled = true;

        isCupDragged = true;
    }

    private void OnAnyPointerUp()
    {
        LeanTween.move(gameStateController.Cup.gameObject, gameStateController.InitialCupPos, 0.3f);
        LeanTween.rotate(gameStateController.Cup.gameObject, gameStateController.DropCupTransform.eulerAngles, 0.3f);
    }

}
